using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class PlayerMoveForPhoton : MonoBehaviourPunCallbacks
{
    public float speed = 3;
    public float runSpeed = 1.5f;
    float speedIndex, runCoolDown = 3f;
    Animator ani;
    public PhotonView photonView;
    public GameObject PlayerLight, PlayerCamera, CustomizeCanvas, paletteCamera, GameCanvas, target, researchSlider, runSlider, interactionTextBox, alwaysOnCanvas;
    public Text playerNameText;
    public Inventory inven;
    public bool isGameStarted = false;
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

        if (photonView.IsMine)
        {
            if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "GameLobbyTest")
            { masterCanvas = GameObject.Find("MasterCanvas"); }

            NickNameCheck();
            ani = gameObject.GetComponent<Animator>();
            PlayerLight.SetActive(true);
            PlayerCamera.SetActive(true);
            CustomizeCanvas.SetActive(true);
            CustomizeCanvas.GetComponent<Canvas>().enabled = false;
            alwaysOnCanvas.SetActive(true);
            playerNameText.text = PhotonNetwork.NickName;
            playerNameText.color = Color.white;
        }
        else
        {
            playerNameText.text = photonView.Owner.NickName;
            playerNameText.color = Color.cyan;
        }
    }

    void Update()
    {
        photonView.RPC("MasterCheck", RpcTarget.AllBuffered);

        if (photonView.IsMine)
        {
            if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "GameLobbyTest")
            {
                if (PhotonNetwork.IsMasterClient)
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
                F_Check();

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
                        if (target.tag == "Interactable")
                        {
                            Interaction();
                        }
                        else if(target.tag == "Item")
                        {
                            ItemFarming();
                        }
                    }
                }
                if (isGameStarted == true)
                {
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

                    if (Input.GetKey(KeyCode.LeftShift) && isRunCoolDown == false && ani.GetBool("walking"))
                    {
                        runSlider.GetComponent<Slider>().value -= Time.deltaTime * 2;
                    }
                    else if (isRunCoolDown == false)
                    {
                        runSlider.GetComponent<Slider>().value += Time.deltaTime * 0.5f;
                    }
                }
            }

            if(collisions.Count > 0)
            {
                photonView.RPC("TargetSetting", RpcTarget.AllBuffered);
            }
            else
            {
                target = null;
            }
        }
        else
        {
            playerNameText.text = photonView.Owner.NickName;
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
        if (target.name == "Customizing")
        {
            if (CustomizeCanvas.GetComponent<Canvas>().enabled)
            {
                paletteCamera.SetActive(false);
                PlayerCamera.SetActive(true);
                CustomizeCanvas.GetComponent<Canvas>().enabled = false;
                lockInteraction = false;
                alwaysOnCanvas.SetActive(true);
                Canvas[] sceneCanvas = GameObject.Find("SceneCanvas").GetComponentsInChildren<Canvas>();
                foreach (Canvas canvas in sceneCanvas)
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
                alwaysOnCanvas.SetActive(false);
                ani.SetFloat("x", 1);
                ani.SetFloat("y", 0);
                Canvas[] sceneCanvas = GameObject.Find("SceneCanvas").GetComponentsInChildren<Canvas>();
                foreach (Canvas canvas in sceneCanvas)
                {
                    canvas.enabled = false;
                }
            }
        }

        else if (target.name == "Drawer")
        {
            photonView.RPC("DrawerItemSpawn", RpcTarget.AllBuffered, false);
        }
    }

    private void F_Check()
    {
        string str = "";
        if (target != null)
        {
            if (target.tag == "Interactable" && target.name == "Customizing")
            {
                str = "캐릭터 꾸미기";
            }
            else if (target.tag == "Interactable" && target.name == "Drawer")
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
            else if (target.tag == "Item")
            {
                str = "아이템 줍기";
            }
            interactionTextBox.transform.GetComponentInChildren<Text>().text = str;
            interactionTextBox.SetActive(true);
        }
        else
        {
            if (interactionTextBox.activeInHierarchy == true)
            {
                interactionTextBox.GetComponent<Animator>().SetTrigger("Finish");
            }
        }
    }

    public void ItemFarming()
    {
        inven.InvenCheck();
        if (inven.invenFull == false)
        {
            inven.TakeItem(target);
            photonView.RPC("ItemFarmingRPC", RpcTarget.AllBuffered);
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
        if (collision.tag == "Interactable" || collision.tag == "Item")
        {
            collisions.Add(collision.gameObject);  
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collisions.Contains(collision.gameObject))
        {
            if (photonView.IsMine)
            {
                collision.GetComponent<SpriteRenderer>().material.SetInt("_OutlineOn", 0);
            }
            collisions.Remove(collision.gameObject);
        }
    }

    private void OnLevelWasLoaded(int level)
    {
        if(level > 1)
        {
            GameCanvas.SetActive(true);
            isGameStarted = true;
        }
    }

    void NickNameCheck()
    {
        string beforeNickName = PhotonNetwork.NickName;
        foreach (Player _pp in PhotonNetwork.PlayerList)
        {
            if(PhotonNetwork.NickName == _pp.NickName)
            {
                if(hasSameNickname == true)
                {
                    PhotonNetwork.NickName = PhotonNetwork.NickName + "_1";
                }
                hasSameNickname = true;
            }
        }
        if(beforeNickName != PhotonNetwork.NickName)
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
                    if (photonView.IsMine)
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
    void ItemFarmingRPC()
    {
        Destroy(target);
    }

    [PunRPC]
    void ItemSpawn(string itemName)
    {
        Vector2 cPos = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y);
        GameObject obj = Instantiate(Resources.Load(itemName), cPos, Quaternion.identity) as GameObject;
        obj.GetComponent<Item>().firstDrop = false;
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

        if (photonView.IsMine)
        {
            target.GetComponent<SpriteRenderer>().material.SetInt("_OutlineOn", 1);
        }

        if (targetValueChangeTemp != null)
        {
            if (targetValueChangeTemp != target)
            {
                if (photonView.IsMine)
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
        if (photonView.Owner.IsMasterClient)
        {
            isMaster = true;
        }
        else
        {
            isMaster = false;
        }
    }
}
