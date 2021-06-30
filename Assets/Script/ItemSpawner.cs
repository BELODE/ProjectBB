using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class ItemSpawner : MonoBehaviour
{
    public GameObject[] items;
    public List<GameObject> drawers;
    public int ItemsCount;
    private int[] drawerCount;
    public bool settingComplete = false;

    int[] randomResult;
    void Start()
    { 
        drawerCount = new int[ItemsCount];

    }

    void Update()
    {

        if(settingComplete == false && GameObject.Find("GameManager").GetComponent<GameManagerOnMap>().MasterPlayer != null)
        {
            settingComplete = true;

            for (int i = 0; i < drawers.Count; i++)
            {
                drawers[i].GetComponent<DrawerInven>().itemsCode = -1;
            }

            RandomValueBeNotSame(0, drawers.Count, ItemsCount);
            for (int i = 0; i < ItemsCount; i++)
            {
                int masterID = GameObject.Find("GameManager").GetComponent<GameManagerOnMap>().MasterPlayer.GetComponent<PhotonView>().ViewID;

                if (PhotonNetwork.IsMasterClient)
                {
                    PhotonView.Find(masterID).RPC("RandomRangeInt", RpcTarget.AllBuffered, randomResult[i], i);
                    drawerCount[i] = GameObject.Find("GameManager").GetComponent<GameManagerOnMap>().MasterPlayer.GetComponent<PlayerMoveForPhoton>().randomResultList[i];
                }
                else
                {
                    drawerCount[i] = GameObject.Find("GameManager").GetComponent<GameManagerOnMap>().MasterPlayer.GetComponent<PlayerMoveForPhoton>().randomResultList[i];
                }
            }

            RandomValueBeNotSame(0, items.Length, ItemsCount);
            for (int i = 0; i < ItemsCount; i++)
            {
                int masterID = GameObject.Find("GameManager").GetComponent<GameManagerOnMap>().MasterPlayer.GetComponent<PhotonView>().ViewID;

                if (PhotonNetwork.IsMasterClient)
                {
                    PhotonView.Find(masterID).RPC("RandomRangeInt", RpcTarget.AllBuffered, randomResult[i], i + ItemsCount);
                    drawers[drawerCount[i]].GetComponent<DrawerInven>().itemsCode = GameObject.Find("GameManager").GetComponent<GameManagerOnMap>().MasterPlayer.GetComponent<PlayerMoveForPhoton>().randomResultList[i + ItemsCount];
                }
                else
                {
                    drawers[drawerCount[i]].GetComponent<DrawerInven>().itemsCode = GameObject.Find("GameManager").GetComponent<GameManagerOnMap>().MasterPlayer.GetComponent<PlayerMoveForPhoton>().randomResultList[i + ItemsCount];
                }
            }
        }
    }

    void RandomValueBeNotSame(int min, int max, int numb)
    {
        var randomvalue = new List<int>();

        for (int i = 0; i < max - min; i++)
        {
            randomvalue.Add(min + i);
        }

        randomResult = new int[numb];

        for (int i = 0; i < numb; i++)
        {
            var index = Random.Range(0, randomvalue.Count);
            randomResult[i] = randomvalue[index];
            randomvalue.Remove(randomResult[i]);
        }

    }
}
