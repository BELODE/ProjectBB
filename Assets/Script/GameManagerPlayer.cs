using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManagerPlayer : Photon.MonoBehaviour
{
    public GameObject PlayerPrefab;
    public Text PingText, roomNameText, playerCountText;
    public GameObject chatBox, playerListContentsBox;
    public GameObject chatBoxText, playerListContents;
    public List<GameObject> playerListPrefabs;
    public string masterName;
    PhotonPlayer[] players;
    // Start is called before the first frame update
    void Start()
    {
        SpawnPlayer();
        roomNameText.text = PhotonNetwork.room.Name;
        masterName = PhotonNetwork.masterClient.NickName;
        players = PhotonNetwork.playerList;
    }

    // Update is called once per frame
    void Update()
    {
        if(PhotonNetwork.room == null)
        {
            PhotonNetwork.LoadLevel("CreateRoomTest");
        }

        if(players != PhotonNetwork.playerList)
        {
            SettingKickPlayerBox();
            players = PhotonNetwork.playerList;
        }
        playerCountText.text = PhotonNetwork.room.PlayerCount.ToString() + "/" + PhotonNetwork.room.MaxPlayers.ToString();
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

    void OnPhotonPlayerConnected(PhotonPlayer player)
    {

        GameObject obj = Instantiate(chatBoxText, new Vector2(0, 0), Quaternion.identity);
        obj.transform.SetParent(chatBox.transform, false);
        obj.GetComponent<Text>().text = player.NickName + " joined the game";
        obj.GetComponent<Text>().color = Color.green;
    }

    void OnPhotonPlayerDisconnected(PhotonPlayer player)
    {
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
        }
        foreach (PhotonPlayer _pp in PhotonNetwork.playerList)
        {
            if (_pp != PhotonNetwork.masterClient)
            {
                GameObject _go = Instantiate(playerListContents, playerListContentsBox.transform);
                playerListPrefabs.Add(_go);
                _go.GetComponent<KickPlayer>()._PP = _pp;
            }
        }
        
    }

    public void KickPlayer(PhotonPlayer kickPlayer)
    {
        PhotonNetwork.CloseConnection(kickPlayer);
    }
}
