using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    public GameObject[] items;
    public GameObject[] drawers;
    public int ItemsCount;
    private int[] drawerCount;

    void Start()
    {
        drawerCount = new int[ItemsCount];

        for(int i = 0; i < drawers.Length; i++)
        {
            drawers[i].GetComponent<DrawerInven>().itemsCode = -1;
        }

        for(int i = 0; i < ItemsCount; i++)
        {
            drawerCount[i] = Random.Range(0, drawers.Length);
        }

        NotSame();

        for(int i = 0; i < ItemsCount; i++)
        {
            drawers[drawerCount[i]].GetComponent<DrawerInven>().itemsCode = Random.Range(0, items.Length);
        }
    }

    private void NotSame()
    {
        bool same = true;
        while (same == true)
        {
            for (int i = 0; i < drawerCount.Length - 1; i++)
            {
                if (drawerCount[i] == drawerCount[i + 1])
                {
                    drawerCount[i] = Random.Range(0, drawers.Length);
                    same = true;
                }
                else
                {
                    same = false;
                }
            }
        }
    }

    void Update()
    {

    }

}
