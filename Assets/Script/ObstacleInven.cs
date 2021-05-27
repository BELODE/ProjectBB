using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleInven : MonoBehaviour
{
    public List<int> itemsCode = new List<int>();

    void Start()
    {
        int rand = Random.Range(1, 5);
        for(int i = 0; i < rand; i++)
        {
            itemsCode.Add(Random.Range(0, GameObject.Find("ItemSpawner").GetComponent<ItemSpawner>().items.Length));
        }
    }

    void Update()
    {
        
    }

    public void ItemSpawn()
    {
        ItemSpawner itemS = GameObject.Find("ItemSpawner").GetComponent<ItemSpawner>();
        int rand = Random.Range(0, itemsCode.Count);
        GameObject gameObject = Instantiate(itemS.items[itemsCode[rand]], GameObject.Find("Player").GetComponent<PlayerMove>().target.transform.position, Quaternion.identity) as GameObject;
        itemsCode.RemoveAt(rand);
    }

    public void ItemIn(int code)
    {
        itemsCode.Add(code);
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Item")
        {
            if (collision.GetComponent<Item>().firstDrop == false)
            {
                
            }
        }
    }
}
