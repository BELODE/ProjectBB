using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public List<InvenItem> items;
    public int nowBlank = 0;

    public GameObject nowItem;
    public GameObject dragItem;

    public bool invenFull = false;

    void Start()
    {

    }

    void Update()
    {
        
    }

    public void TakeItem()
    {
        items[nowBlank].items.name = nowItem.GetComponent<Item>().items.name;
        items[nowBlank].items.type = nowItem.GetComponent<Item>().items.type;
        items[nowBlank].items.code = nowItem.GetComponent<Item>().items.code;
        items[nowBlank].gameObject.GetComponent<Image>().sprite = nowItem.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite;
        nowBlank++;
    }

    public void InvenCheck()
    {
        int c = 0;
        for(int i = 0; i < items.Count; i++)
        {
            if (items[i].items.name == null)
            {
                nowBlank = i;
                break;
            }
            c++;
        }
        if (c == items.Count)
        {
            invenFull = true;
        }else if (c != items.Count)
        {
            invenFull = false;
        }

    }
}
