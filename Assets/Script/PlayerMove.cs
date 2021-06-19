using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMove : MonoBehaviour
{
    public float speed = 3;
    public float run = 0;
    public float runCoolTime = 0;

    public bool noRun = false;
    public bool isRun = false;
    public bool research = false;
    public bool breaking = false;
    public bool Push = false;
    public bool f_check = false;

    public List<GameObject> collisions = new List<GameObject>();

    public Animator ani;
    public Animator researchSliderAni;
    public GameObject target;
    public Slider staminaSlider;
    public Slider researchSlider;
    public Inventory inven;
    public GameObject grabbingObject, grabbingObjectParent;
    public GameObject Press_F;
    public Vector3 grabbingObjectPosition;

    public BoxCollider2D eastGrabCol, westGrabCol;

    void Start()
    {
        ani = gameObject.GetComponent<Animator>();
        researchSlider = GameObject.Find("ResearchSlider").GetComponent<Slider>();
        researchSliderAni = researchSlider.GetComponent<Animator>();
        staminaSlider = GameObject.Find("StaminaSlider").GetComponent<Slider>();
    }

    void Update()
    {
        float xMove = Input.GetAxis("Horizontal");
        float yMove = Input.GetAxis("Vertical");

        Vector2 move = new Vector2(xMove, yMove).normalized;

        RunCheck(move.x, move.y);

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
        {
            Move(move.x, move.y);
        }
        else
        {
            ani.SetBool("walking", false);
            ani.SetBool("breaking", false);
            breaking = false;
            this.GetComponent<Rigidbody2D>().velocity = new Vector3(0, 0, 0);
        }

        NowTarget();
        F_Check();

        if (Input.GetKeyDown(KeyCode.F))
        {
            if (Push == true)
            {
                Grab();
            }
            else
            {
                if (target != null)
                {
                    InteractionTarget();
                }
            }
        }

        AniControll();

        if(Push == true)
        {
            grabbingObject.transform.position = gameObject.transform.position + new Vector3(grabbingObjectPosition.x * ani.GetFloat("isWest"), grabbingObjectPosition.y, 0);
        }
    }

    private void NowTarget()
    {
        if (collisions.Count > 1)
        {
            for (int i = 0; i < collisions.Count - 1; i++)
            {
                Vector3 offset = transform.position - collisions[i].transform.position;
                Vector3 offset2 = transform.position - collisions[i + 1].transform.position;
                if (offset.sqrMagnitude < offset2.sqrMagnitude)
                {
                    target = collisions[i];
                }
                else if (offset.sqrMagnitude > offset2.sqrMagnitude)
                {
                    target = collisions[i + 1];
                }
            }
            f_check = true;
        }
        else if (collisions.Count == 1)
        {
            target = collisions[0];
            f_check = true;
        }
        else if (collisions.Count == 0)
        {
            target = null;
            f_check = false;
        }
    }

    private void F_Check()
    {
        string str = "";
        if (f_check == true)
        {
            if (target.tag == "Interactable"&&target.name=="Drawer")
            {
                if (target.GetComponent<Animator>().GetBool("Open") == true)
                {

                    str = "서랍 닫기";
                }
                else
                {

                    str = "아이템 탐색";
                }
            }
            else if (target.tag == "Interactable" && target.name=="Door")
            {
                if (target.GetComponent<Animator>().GetBool("Open") == true)
                {

                    str = "문 닫기";
                }
                else
                {

                    str = "문 열기";
                }
            }
            else if (target.tag == "Item")
            {
                str = "아이템 줍기";
            }
            else if (target.tag == "Interactable"&&target.name=="Grabable")
            {
                if (Push == false)
                {
                    str = "옮기기";
                }
                else
                {
                    str = "놓기";
                }
            }
            Press_F.transform.GetComponentInChildren<Text>().text = str;
            Press_F.SetActive(true);
        }else if (f_check == false)
        {
            Press_F.GetComponent<Animator>().SetTrigger("Finish");
        }
    }

    private void InteractionTarget()
    {
        if (target.tag == "Interactable" && target.name=="Drawer")
        {
            if (target.GetComponent<Animator>().GetBool("Open") == false)
            {
                research = true;
            }
            else if (target.GetComponent<Animator>().GetBool("Open") == true)
            {
                target.GetComponent<Animator>().SetBool("Open", false);
            }
        }
        else if (target.tag == "Interactable" && target.name== "Door")
        {
            Animator doorAni = target.GetComponent<Animator>();
            if (doorAni.GetBool("Open") == false)
            {
                doorAni.SetBool("Open", true);
            }
            else if (doorAni.GetBool("Open") == true)
            {
                doorAni.SetBool("Open", false);
            }
        }
        else if (target.tag == "Item")
        {
            inven.InvenCheck();
            if (inven.invenFull == false)
            {
                inven.TakeItem(target);
                Destroy(target);
            }
        }
        else if (target.tag == "Interactable"&&target.name== "Grabable")
        {
            Grab();
        }
    }

    private void Grab()
    {
        if(Push == true)
        {
            speed = 3f;
            ani.SetBool("Push", false);
            Push = false;
            grabbingObject = null;
            eastGrabCol.enabled = false;
            westGrabCol.enabled = false;

        }
        else
        {
            LadderGrapSystem _LGS;
            _LGS = target.gameObject.GetComponent<LadderGrapSystem>();

            if (_LGS.isWest == true)
            {
                ani.SetFloat("isWest", 1);
                westGrabCol.enabled = true;
            }
            else
            {
                ani.SetFloat("isWest", -1);
                eastGrabCol.enabled = true;
            }



            collisions.Clear();
            collisions.Add(_LGS.gameObject);

            grabbingObject = _LGS.MainObject;
            grabbingObjectParent = _LGS.Ledders;

            transform.position = grabbingObject.transform.position - new Vector3(grabbingObjectPosition.x * ani.GetFloat("isWest"), grabbingObjectPosition.y, 0);

            speed = 1f;
            ani.SetBool("Push", true);
            Push = true;
        }
    }

    private void AniControll()
    {
        if (research==true)
        {
            DrawerInven dInven = target.GetComponent<DrawerInven>();

            if (breaking == false)
            {
                if (dInven.GetOpen() == false)
                {
                    ani.SetBool("research", true);
                    researchSliderAni.SetBool("research", true);
                }

                if (researchSlider.value <= 0)
                {
                    dInven.ItemSpawn();
                    ani.SetBool("research", false);
                    researchSliderAni.SetBool("research", false);
                    dInven.SetOpen(true);
                    research = false;
                }
            }
            else if (breaking == true)
            {
                ani.SetBool("research", false);
                ani.SetBool("breaking", true);
                researchSliderAni.SetBool("research", false);
                research = false;
            }
        }
    }

    private void RunCheck(float xMove,float yMove)
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            if (noRun == false)
            {
                isRun = true;
            }
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            isRun = false;
        }

        if (noRun == false)
        {
            if ((isRun == true && (xMove != 0 || yMove != 0)) && staminaSlider.value > 0)
            {
                staminaSlider.value -= Time.deltaTime * 2;
            }
            else if (staminaSlider.value <= 0)
            {
                runCoolTime = 3.0f;
                noRun = true;
            }
            else if (isRun == false || (isRun == true && (xMove == 0 && yMove == 0)))
            {
                staminaSlider.value += Time.deltaTime * 0.5f;
            }
        }
        else if (noRun == true)
        {
            runCoolTime -= Time.deltaTime;
            if (runCoolTime <= 0)
            {
                staminaSlider.value = Time.deltaTime;
                noRun = false;
            }
        }

        if (isRun == true && noRun == false)
        {
            run = speed * 0.5f;
        }
        else if (isRun == false || noRun == true)
        {
            run = 0;
        }
    }

    private void Move(float xMove,float yMove)
    {
        ani.SetBool("walking", true);
        ani.SetBool("breaking", true);

        breaking = true;

        ani.SetFloat("x", xMove);
        ani.SetFloat("y", yMove);

        float x = xMove * (speed + run) * Time.deltaTime;
        float y = yMove * (speed + run) * Time.deltaTime;

        this.GetComponent<Rigidbody2D>().velocity = new Vector3(xMove * (speed + run), yMove * (speed + run), 0);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.name!="LayerSetting"&&collision.name!="Ladder" && collision.tag != "Obstacle" && Push == false)
        {
            if (collision.name== "Drawer" && collision.transform.position.z < gameObject.transform.position.z)
            {
                collisions.Remove(collision.gameObject);
            }
            else
            {
                if (!collisions.Contains(collision.gameObject))
                {
                    collisions.Add(collision.gameObject);
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        collisions.Remove(collision.gameObject);
    }

}
