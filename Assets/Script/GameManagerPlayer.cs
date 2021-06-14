using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManagerPlayer : MonoBehaviour
{
    public GameObject PlayerPrefab;
    public Text PingText;

    public GameObject chatBox;
    public GameObject chatBoxText;
    // Start is called before the first frame update
    void Start()
    {
        SpawnPlayer();
    }

    // Update is called once per frame
    void Update()
    {
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
        obj.GetComponent<Text>().text = player.name + " joined the game";
        obj.GetComponent<Text>().color = Color.green;
    }

    void OnPhotonPlayerDisconnected(PhotonPlayer player)
    {
        GameObject obj = Instantiate(chatBoxText, new Vector2(0, 0), Quaternion.identity);
        obj.transform.SetParent(chatBox.transform, false);
        obj.GetComponent<Text>().text = player.name + " left the game";
        obj.GetComponent<Text>().color = Color.red;
    }
}
