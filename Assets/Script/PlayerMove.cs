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

    public Animator ani;
    public Animator researchSliderAni;
    public GameObject target;
    public Slider staminaSlider;
    public Slider researchSlider;
    public Inventory inven;

    public MaterialManager materialManager;

    void Start()
    {
        ani = gameObject.GetComponent<Animator>();
        researchSlider = GameObject.Find("ResearchSlider").GetComponent<Slider>();
        researchSliderAni = researchSlider.GetComponent<Animator>();
        staminaSlider = GameObject.Find("StaminaSlider").GetComponent<Slider>();
        materialManager = GameObject.Find("materialManager").GetComponent<MaterialManager>();
    }

    void Update()
    {
        RunCheck(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        if (Input.GetKey(KeyCode.W)||Input.GetKey(KeyCode.A)|| Input.GetKey(KeyCode.S)|| Input.GetKey(KeyCode.D)|| Input.GetKey(KeyCode.LeftArrow)|| Input.GetKey(KeyCode.RightArrow)|| Input.GetKey(KeyCode.UpArrow)|| Input.GetKey(KeyCode.DownArrow))
        {
            Move(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        }
        else
        {
            ani.SetBool("walking", false);
            ani.SetBool("breaking", false);
            breaking = false;
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            inven.InvenCheck();
            if (GameObject.Find("Inven").GetComponent<Inventory>().nowItem != null && inven.invenFull == false)
            {
                GameObject.Find("Inven").GetComponent<Inventory>().TakeItem();
                Destroy(target);
            }
        }

        Research();
        if (target != null && target.name == "Door")
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (target.GetComponent<Animator>().GetBool("Open") == false)
                {
                    target.GetComponent<Animator>().SetBool("Open", true);
                }
                else if (target.GetComponent<Animator>().GetBool("Open") == true)
                {
                    target.GetComponent<Animator>().SetBool("Open", false);
                }
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

        this.transform.Translate(new Vector3(x, y, 0));
    }

    private void Research()
    {
        if (target != null && target.tag == "Obstacle")
        {
            if (research == true)
            {
                if (breaking == false)
                {
                    if (target.GetComponent<Animator>().GetBool("Open") == false && Input.GetKey(KeyCode.Space)&&target.GetComponent<ObstacleInven>().itemsCode.Count!=0)
                    {
                        ani.SetBool("research", research);
                        researchSliderAni.SetBool("research", research);
                    }

                    if (researchSlider.value <= 0)
                    {
                        target.GetComponent<ObstacleInven>().ItemSpawn();
                        ani.SetBool("research", false);
                        researchSliderAni.SetBool("research", false);
                        target.GetComponent<Animator>().SetBool("Open", true);
                        target.GetComponent<Animator>().GetComponent<SpriteRenderer>().material = materialManager.origin;
                    }
                }
                else if (breaking == true)
                {
                    ani.SetBool("research", false);
                    ani.SetBool("breaking", true);
                    researchSliderAni.SetBool("research", false);
                }
            }
            else if (research == false)
            {
                ani.SetBool("research", research);
                researchSliderAni.SetBool("research", research);
                if (target != null)
                {
                    target.GetComponent<Animator>().SetBool("Open", false);
                    target = null;
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Obstacle")
        {
            collision.GetComponent<SpriteRenderer>().material = materialManager.change;
            target = collision.gameObject;
            research = true;
        }
        if(collision.tag == "Door")
        {
            target = collision.gameObject;
        }
        if (collision.tag == "Item")
        {
            target = collision.gameObject;
            inven.nowItem = collision.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Obstacle")
        {
            collision.GetComponent<SpriteRenderer>().material = materialManager.origin;
            research = false;
        }
        if (collision.tag == "Item")
        {
            inven.nowItem = null;
        }
    }

}
