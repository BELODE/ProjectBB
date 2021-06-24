using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    public GameObject[] items;
    public List<GameObject> drawers;
    public int ItemsCount;
    private int[] drawerCount;

    void Start()
    {
        GameObject[] gameobjectTemp = GameObject.FindGameObjectsWithTag("Interactable");
        foreach(GameObject go in gameobjectTemp)
        {
            if (go.name == "Drawer")
            {
                drawers.Add(go);
            }
        }

        drawerCount = new int[ItemsCount];

        for(int i = 0; i < drawers.Count; i++)
        {
            drawers[i].GetComponent<DrawerInven>().itemsCode = -1;
        }

        for(int i = 0; i < ItemsCount; i++)
        {
            drawerCount[i] = Random.Range(0, drawers.Count);
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
                    drawerCount[i] = Random.Range(0, drawers.Count);
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
