using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMoveForPhoton : Photon.MonoBehaviour
{
    public float speed = 3;
    public float runSpeed = 1.5f;
    float speedIndex, runCoolDown = 3f;
    Animator ani;
    public PhotonView photonView;
    public GameObject PlayerLight, PlayerCamera, CustomizeCanvas, paletteCamera, GameCanvas, target, researchSlider, runSlider;
    public Text playerNameText;

    List<GameObject> collisions = new List<GameObject>();
    GameObject targetValueChangeTemp, masterCanvas;
    bool hasSameNickname = false; 
    public bool lockInteraction = false, isRunCoolDown = false;

    public bool isMaster = false;
    public int[] randomResultList;

    void Awake()
    {
        speedIndex = speed;
        randomResultList = new int[100];

        for(int i = 0; i<randomResultList.Length; i++)
        {
            randomResultList[i] = -1;
        }

        if (photonView.isMine)
        {
            if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "GameLobbyTest")
            { masterCanvas = GameObject.Find("MasterCanvas"); }

            NickNameCheck();
            ani = gameObject.GetComponent<Animator>();
            PlayerLight.SetActive(true);
            PlayerCamera.SetActive(true);
            CustomizeCanvas.SetActive(true);
            CustomizeCanvas.GetComponent<Canvas>().enabled = false;
            playerNameText.text = PhotonNetwork.playerName;
            playerNameText.color = Color.white;
            GameCanvas.SetActive(true);
        }
        else
        {
            playerNameText.text = photonView.owner.NickName;
            playerNameText.color = Color.cyan;
        }
    }

    void Update()
    {
        photonView.RPC("MasterCheck", PhotonTargets.AllBuffered);

        if (photonView.isMine)
        {
            if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "GameLobbyTest")
            {
                if (PhotonNetwork.isMasterClient)
                {
                    masterCanvas.SetActive(true);
                }
                else
                {
                    masterCanvas.SetActive(false);
                }
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

                if (Input.GetKeyDown(KeyCode.LeftShift) && ani.GetBool("walking") && runSlider.GetComponent<Slider>().value > 0 && isRunCoolDown == false)
                {
                    speed = speedIndex * runSpeed;
                }
                else if ((Input.GetKeyUp(KeyCode.LeftShift) || runSlider.GetComponent<Slider>().value <= 0) && isRunCoolDown == false)
                {
                    if (runSlider.GetComponent<Slider>().value <= 0)
                    {
                        StartCoroutine(RunCoolTimeSet());
                    }
                    speed = speedIndex;
                }

                if (Input.GetKey(KeyCode.LeftShift) && isRunCoolDown == false)
                {
                    runSlider.GetComponent<Slider>().value -= Time.deltaTime * 2;
                }
                else if(isRunCoolDown == false)
                {
                    runSlider.GetComponent<Slider>().value += Time.deltaTime * 0.5f;
                }


            }

            if(collisions.Count > 0)
            {
                photonView.RPC("TargetSetting", PhotonTargets.AllBuffered);
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

    IEnumerator RunCoolTimeSet()
    {
        isRunCoolDown = true;
        yield return new WaitForSeconds(runCoolDown);
        runSlider.GetComponent<Slider>().value = 0.1f;
        isRunCoolDown = false;
    }

    public void Interaction()
    {
        if(target.name == "Customizing")
        {
            if (CustomizeCanvas.GetComponent<Canvas>().enabled)
            {
                paletteCamera.SetActive(false);
                PlayerCamera.SetActive(true);
                CustomizeCanvas.GetComponent<Canvas>().enabled = false;
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
                CustomizeCanvas.GetComponent<Canvas>().enabled = true;
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

        else if(target.name == "Drawer")
        {
            photonView.RPC("DrawerItemSpawn", PhotonTargets.AllBuffered,false);
        }
    }


    private void Move(float xMove,float yMove)
    {
        Vector2 move = new Vector2(xMove, yMove).normalized;

        ani.SetBool("walking", true);
        ani.SetBool("breaking", true);

        ani.SetFloat("x", move.x);
        ani.SetFloat("y", move.y);

        this.GetComponent<Rigidbody2D>().velocity = new Vector3(move.x * (speed), move.y * (speed), 0);
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
            if (photonView.isMine)
            {
                collision.GetComponent<SpriteRenderer>().material.SetInt("_OutlineOn", 0);
            }
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



    [PunRPC]
    void StartButtonPUN()
    {
        GameObject[] _GOs = GameObject.FindGameObjectsWithTag("Player");
        foreach(GameObject go in _GOs)
        {
            DontDestroyOnLoad(go);
        }
        PhotonNetwork.LoadLevel("Map_01");
    }

    [PunRPC]
    void DrawerItemSpawn(bool isResearchSlider)
    {
        if (isResearchSlider == false)
        {
            if (target.GetComponent<DrawerInven>().isActiveNow == false)
            {
                target.GetComponent<DrawerInven>().isActiveNow = true;

                if (target.GetComponent<Animator>().GetBool("Open"))
                {
                    target.GetComponent<Animator>().SetBool("Open", false);
                }
                else
                {
                    if (photonView.isMine)
                    {
                        researchSlider.GetComponent<Animator>().SetBool("research", true);
                        ani.SetBool("research", true);
                    }
                    target.GetComponent<DrawerInven>().SettingItemDirection();
                }
            }
        }
        else
        {
            target.GetComponent<Animator>().SetBool("Open", true);
            target.GetComponent<DrawerInven>().ItemSpawn();
        }
    }

    [PunRPC]
    void DrawerBreak()
    {
        target.GetComponent<DrawerInven>().IsActiveNouwEnded();
    }


    [PunRPC]
    void TargetSetting()
    {
        Vector3 distance = Vector3.zero;

        for (int i = 0; i < collisions.Count; i++)
        {
            if (i == 0)
            {
                distance = gameObject.transform.position - collisions[i].transform.position;
                target = collisions[i];
            }
            else
            {
                if (Vector3.SqrMagnitude(gameObject.transform.position - collisions[i].transform.position) < distance.sqrMagnitude)
                {
                    distance = gameObject.transform.position - collisions[i].transform.position;
                    target = collisions[i];
                }
            }
        }

        if (photonView.isMine)
        {
            target.GetComponent<SpriteRenderer>().material.SetInt("_OutlineOn", 1);
        }

        if (targetValueChangeTemp != null)
        {
            if (targetValueChangeTemp != target)
            {
                if (photonView.isMine)
                {
                    targetValueChangeTemp.GetComponent<SpriteRenderer>().material.SetInt("_OutlineOn", 0);
                }
                targetValueChangeTemp = target;
            }
        }

        else
        {
            targetValueChangeTemp = target;
        }
    }

    [PunRPC]
    void RandomRangeInt(int randomValue, int index)
    {
        randomResultList[index] = randomValue;
        Debug.Log(randomValue);
    }

    [PunRPC]
    void MasterCheck()
    {
        if (photonView.owner.IsMasterClient)
        {
            isMaster = true;
        }
        else
        {
            isMaster = false;
        }
    }
}
