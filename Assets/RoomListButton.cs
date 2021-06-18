using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomListButton : MonoBehaviour
{
    MenuController _MC;
    // Start is called before the first frame update
    void Start()
    {
        _MC = GameObject.Find("MenuController").GetComponent<MenuController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void JoinRoomToButton()
    {
        _MC.JoinGameToString(GetComponentInChildren<Text>().text);
    }
}
