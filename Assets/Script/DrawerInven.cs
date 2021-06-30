using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;

public class DrawerInven : MonoBehaviour
{
    public int itemsCode;
    public int persent;
    public float ItemsEndYPos;
    public bool isActiveNow = false;
    public int drawerNumbIndex;
    private ItemSpawner itemS;

    void Start()
    {
        itemS = GameObject.Find("ItemSpawner").GetComponent<ItemSpawner>();
    }

    public void ItemSpawn()
    {
        if (itemsCode != -1)
        {

            GameObject gameObject = Instantiate(itemS.items[itemsCode], this.gameObject.transform.position, Quaternion.identity);

            gameObject.GetComponent<Item>().lr = GameObject.Find("GameManager").GetComponent<GameManagerOnMap>().MasterPlayer.GetComponent<PlayerMoveForPhoton>().randomResultList[(itemS.ItemsCount * 2) + drawerNumbIndex];

            gameObject.GetComponent<Item>().endYPos = ItemsEndYPos;
            gameObject.GetComponent<Item>().Parentsdrawer = this.gameObject;
            itemsCode = -1;
        }
    }

    public void ItemIn(int code)
    {
        itemsCode = code;
    }

    public bool GetOpen()
    {
        return transform.GetComponent<Animator>().GetBool("Open");
    }

    public void SetOpen(bool open)
    {
        transform.GetComponent<Animator>().SetBool("Open", open);
    }

    public void IsActiveNouwEnded()
    {
        isActiveNow = false;
    }

    public void SettingItemDirection()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            int masterID = GameObject.Find("GameManager").GetComponent<GameManagerOnMap>().MasterPlayer.GetComponent<PhotonView>().ViewID;
            PhotonView.Find(masterID).RPC("RandomRangeInt", RpcTarget.AllBuffered, Random.Range(0, 2), (itemS.ItemsCount * 2) + drawerNumbIndex);
        }
    }
}
