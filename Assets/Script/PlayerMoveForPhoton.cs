using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMoveForPhoton : Photon.MonoBehaviour
{
    public float speed = 3;
    Animator ani;
    public PhotonView photonView;
    public GameObject PlayerLight, PlayerCamera, playerCanvas, paletteCamera;
    public Text playerNameText;

    List<GameObject> collisions = new List<GameObject>();
    GameObject target, targetValueChangeTemp, masterCanvas;
    bool hasSameNickname = false;
    public bool lockInteraction = false;
    void Awake()
    {
        if (photonView.isMine)
        {
            NickNameCheck();
            ani = gameObject.GetComponent<Animator>();
            PlayerLight.SetActive(true);
            PlayerCamera.SetActive(true);
            playerCanvas.SetActive(true);
            playerCanvas.GetComponent<Canvas>().enabled = false;
            playerNameText.text = PhotonNetwork.playerName;
            playerNameText.color = Color.white;
            masterCanvas = GameObject.Find("MasterCanvas");
            masterCanvas.SetActive(false);
        }
        else
        {
            playerNameText.text = photonView.owner.NickName;
            playerNameText.color = Color.cyan;
        }

    }

    void Update()
    {
        if (photonView.isMine)
        {
            if (PhotonNetwork.isMasterClient)
            {
                masterCanvas.SetActive(true);
            }

            if (lockInteraction == false)
            {
                if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
                {
                    Move(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
                }
                else
                {
                    ani.SetBool("walking", false);
                    ani.SetBool("breaking", false);
                    this.GetComponent<Rigidbody2D>().velocity = new Vector3(0, 0, 0);
                }

                if (Input.GetKeyDown(KeyCode.F))
                {
                    if (target != null)
                    {
                        Interaction();
                    }
                }
            }

            if(collisions.Count > 0)
            {
                TargetSetting();
            }
            else
            {
                target = null;
            }
        }
        else
        {
            playerNameText.text = photonView.owner.NickName;
        }
    }

    void TargetSetting()
    {
        Vector3 distance = Vector3.zero;

        for (int i = 0; i < collisions.Count; i++)
        {
            if(i == 0)
            {
                distance = gameObject.transform.position - collisions[i].transform.position;
                target = collisions[i];
            }
            else
            {
                if(Vector3.SqrMagnitude(gameObject.transform.position - collisions[i].transform.position) < distance.sqrMagnitude)
                {
                    distance = gameObject.transform.position - collisions[i].transform.position;
                    target = collisions[i];
                }
            }
        }

        target.GetComponent<SpriteRenderer>().material.SetInt("_OutlineOn", 1);
        if (targetValueChangeTemp != null)
        {
            if (targetValueChangeTemp != target)
            {
                targetValueChangeTemp.GetComponent<SpriteRenderer>().material.SetInt("_OutlineOn", 0);
                targetValueChangeTemp = target;
            }
        }
        else
        {
            targetValueChangeTemp = target;
        }
    }

    public void Interaction()
    {
        if(target.name == "Customizing")
        {
            if (playerCanvas.GetComponent<Canvas>().enabled)
            {
                paletteCamera.SetActive(false);
                PlayerCamera.SetActive(true);
                playerCanvas.GetComponent<Canvas>().enabled = false;
                lockInteraction = false;
                Canvas[] sceneCanvas = GameObject.Find("SceneCanvas").GetComponentsInChildren<Canvas>();
                foreach(Canvas canvas in sceneCanvas)
                {
                    canvas.enabled = true;
                }
            }
            else
            {
                paletteCamera.SetActive(true);
                PlayerCamera.SetActive(false);
                playerCanvas.GetComponent<Canvas>().enabled = true;
                lockInteraction = true;
                ani.SetFloat("x", 1);
                ani.SetFloat("y", 0);
                Canvas[] sceneCanvas = GameObject.Find("SceneCanvas").GetComponentsInChildren<Canvas>();
                foreach (Canvas canvas in sceneCanvas)
                {
                    canvas.enabled = false;
                }
            }
        }
    }


    private void Move(float xMove,float yMove)
    {
        ani.SetBool("walking", true);
        ani.SetBool("breaking", true);

        ani.SetFloat("x", xMove);
        ani.SetFloat("y", yMove);

        float x = xMove * (speed) * Time.deltaTime;
        float y = yMove * (speed) * Time.deltaTime;

        this.transform.Translate(new Vector3(x, y, 0));
        //this.GetComponent<Rigidbody2D>().velocity = new Vector3(xMove * (speed), yMove * (speed), 0);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Interactable")
        {
            collisions.Add(collision.gameObject);  
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collisions.Contains(collision.gameObject))
        {
            collision.GetComponent<SpriteRenderer>().material.SetInt("_OutlineOn", 0);
            collisions.Remove(collision.gameObject);
        }
    }

    void NickNameCheck()
    {
        string beforeNickName = PhotonNetwork.playerName;
        foreach (PhotonPlayer _pp in PhotonNetwork.playerList)
        {
            if(PhotonNetwork.playerName == _pp.NickName)
            {
                if(hasSameNickname == true)
                {
                    PhotonNetwork.playerName = PhotonNetwork.playerName + "_1";
                }
                hasSameNickname = true;
            }
        }
        if(beforeNickName != PhotonNetwork.playerName)
        {
            hasSameNickname = false;
            NickNameCheck();
        }
    }
}
