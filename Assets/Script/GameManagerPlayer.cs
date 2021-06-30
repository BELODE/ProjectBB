using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class GameManagerPlayer : MonoBehaviourPunCallbacks
{
    public GameObject PlayerPrefab;
    public Text PingText, roomNameText, playerCountText;
    public GameObject chatBox, playerListContentsBox;
    public GameObject chatBoxText, playerListContents;
    public List<GameObject> playerListPrefabs;
    // Start is called before the first frame update
    void Start()
    {
        SpawnPlayer();
        if (PhotonNetwork.InRoom)
        {
            roomNameText.text = PhotonNetwork.CurrentRoom.Name;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(PhotonNetwork.CurrentRoom == null)
        {
            PhotonNetwork.LoadLevel("CreateRoomTest");
        }
        if (PhotonNetwork.InRoom)
        {
            playerCountText.text = PhotonNetwork.CurrentRoom.PlayerCount.ToString() + "/" + PhotonNetwork.CurrentRoom.MaxPlayers.ToString();
        }
        PingText.text = PhotonNetwork.GetPing().ToString();
    }

    void SpawnPlayer()
    {
        PhotonNetwork.Instantiate(PlayerPrefab.name, new Vector2(this.transform.position.x, this.transform.position.y), Quaternion.identity, 0);
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.LoadLevel("CreateRoomTest");
    }

    public override void OnPlayerEnteredRoom(Player player)
    {
        SettingKickPlayerBox();
        GameObject obj = Instantiate(chatBoxText, new Vector2(0, 0), Quaternion.identity);
        obj.transform.SetParent(chatBox.transform, false);
        obj.GetComponent<Text>().text = player.NickName + " joined the game";
        obj.GetComponent<Text>().color = Color.green;
    }

    public override void OnPlayerLeftRoom(Player player)
    {
        SettingKickPlayerBox();
        GameObject obj = Instantiate(chatBoxText, new Vector2(0, 0), Quaternion.identity);
        obj.transform.SetParent(chatBox.transform, false);
        obj.GetComponent<Text>().text = player.NickName + " left the game";
        obj.GetComponent<Text>().color = Color.red;
    }

    void SettingKickPlayerBox()
    {
        if (playerListPrefabs != null)
        {
            foreach (GameObject _go in playerListPrefabs)
            {
                Destroy(_go);
            }
            playerListPrefabs.Clear();
        }
        foreach (Player _pp in PhotonNetwork.PlayerList)
        {
            if (_pp != PhotonNetwork.MasterClient)
            {
                GameObject _go = Instantiate(playerListContents, playerListContentsBox.transform);
                playerListPrefabs.Add(_go);
                _go.GetComponent<KickPlayer>()._PP = _pp;
            }
        }
        
    }

    public void KickPlayer(Player kickPlayer)
    {
        PhotonNetwork.CloseConnection(kickPlayer);
    }

    public void GameStartButton()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.CurrentRoom.IsOpen = false;
        }
        Debug.Log(PhotonNetwork.CurrentRoom.IsOpen);
        PhotonView _PV = GameObject.FindGameObjectWithTag("Player").GetComponent<PhotonView>();
        _PV.RPC("StartButtonPUN", RpcTarget.AllBuffered);
    }
}
