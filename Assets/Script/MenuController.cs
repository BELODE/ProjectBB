using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class MenuController : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject userNameMenu, connectPanel, startButton;
    [SerializeField] private InputField userNameInput, createGameInput, joinGameInput;
    public List<string> roomList;
    public GameObject joinListButton,joinListButtonParents;
    public List<string> roomNames;
    // Start is called before the first frame update
    void Awake()
    {
        PhotonNetwork.ConnectUsingSettings();
        PhotonNetwork.AutomaticallySyncScene = false;
    }

    // Update is called once per frame
    void Start()
    {
        userNameMenu.SetActive(true);
    }

    private void Update()
    {

    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach (RoomInfo ri in roomList)
        {
            roomNames.Add(ri.Name);
        }

        MakeRoomList(roomList);
    }

    public void MakeRoomList(List<RoomInfo> roomInfo)
    {
        roomList.Clear();
        Button[] buttons = joinListButtonParents.GetComponentsInChildren<Button>();

        foreach (Button bts in buttons)
        {
            Destroy(bts.gameObject);
        }

        foreach (RoomInfo ri in roomInfo)
        {
            if (ri.IsOpen)
            {
                if (!roomList.Contains(ri.Name))
                {
                    roomList.Add(ri.Name);
                    GameObject JoinButton = Instantiate(joinListButton, joinListButtonParents.transform);
                    Text[] texts = JoinButton.GetComponentsInChildren<Text>();
                    foreach (Text tx in texts)
                    {
                        if (tx.name == "RoomTitle")
                        {
                            tx.text = ri.Name;
                        }
                        else
                        {
                            tx.text = ri.PlayerCount.ToString() + "/" + ri.MaxPlayers;
                        }
                    }
                }
            }
        }
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby(TypedLobby.Default);
        Debug.Log("Connected");
    }

    public void ChangeUserNameInput()
    {
        if(userNameInput.text.Length >= 3)
        {
            startButton.GetComponent<Button>().interactable = true;
        }
        else
        {
            startButton.GetComponent<Button>().interactable = false;
        }
    }

    public void CreateRoomNameSetting(Button button)
    {
        if (createGameInput.text.Length >= 4)
        {
            button.interactable=true;
        }
        else
        {
            button.interactable=false;
        }
    }

    public void JoinRoomNameSetting(Button button)
    {
        if (joinGameInput.text.Length >= 4)
        {
            button.interactable = true;
        }
        else
        {
            button.interactable = false;
        }
    }

    public void SetUserName()
    {
        userNameMenu.SetActive(false);
        PhotonNetwork.NickName = userNameInput.text;
    }

    public void CreateGame()
    {
        PhotonNetwork.CreateRoom(createGameInput.text, new RoomOptions() { MaxPlayers = 10 }, null);
    }

    public void JoinGame()
    {
        PhotonNetwork.JoinRoom(joinGameInput.text);
    }

    public void JoinGameToString(string text)
    {
        PhotonNetwork.JoinRoom(text);
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("GameLobbyTest");
    }
}
