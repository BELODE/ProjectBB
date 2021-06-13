using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManagerPlayer : MonoBehaviour
{
    public GameObject PlayerPrefab;
    public Text PingText;
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
}
