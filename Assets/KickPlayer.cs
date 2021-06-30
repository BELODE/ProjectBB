using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Realtime;
public class KickPlayer : MonoBehaviour
{
    public Text playerName;
    public Player _PP;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        playerName.text = _PP.NickName;
    }

    public void KickPlayerButton()
    {
        GameObject.Find("GameManager").GetComponent<GameManagerPlayer>().KickPlayer(_PP);
    }
}
