using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    [SerializeField] private string versionName = "0.1";
    [SerializeField] private GameObject userNameMenu, connectPanel, startButton;
    [SerializeField] private InputField userNameInput, createGameInput, joinGameInput;


    // Start is called before the first frame update
    void Awake()
    {
        PhotonNetwork.ConnectUsingSettings(versionName);
    }

    // Update is called once per frame
    void Start()
    {
        userNameMenu.SetActive(true);
    }

    void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby(TypedLobby.Default);
        Debug.Log("Connected");
    }

    public void ChangeUserNameInput()
    {
        if(userNameInput.text.Length >= 3)
        {
            startButton.SetActive(true);
        }
        else
        {
            startButton.SetActive(false);
        }
    }

    public void SetUserName()
    {
        userNameMenu.SetActive(false);
        PhotonNetwork.playerName = userNameInput.text;
    }

    public void CreateGame()
    {
        PhotonNetwork.CreateRoom(createGameInput.text, new RoomOptions() { maxPlayers = 8 }, null);
    }

    public void JoinGame()
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.maxPlayers = 8;
        PhotonNetwork.JoinOrCreateRoom(joinGameInput.text, roomOptions, TypedLobby.Default);
    }

    void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("GameLobbyTest");
    }
}
