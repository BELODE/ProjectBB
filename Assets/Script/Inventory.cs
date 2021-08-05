using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public List<InvenItem> items;
    public ExplainationBoardSetting _EBS;
    public int nowBlank = 0;

    public GameObject dragItem;

    public bool invenFull = false;

    public GameObject[] selectedImage;

    public string selectedItemName, selectedItemType;

    void Start()
    {

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SelectItem(0);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SelectItem(1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SelectItem(2);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            SelectItem(3);
        }
    }

    public void TakeItem(GameObject Item)
    {
        Item item = Item.GetComponent<Item>();
        items[nowBlank].items.name = item.items.name;
        items[nowBlank].items.type = item.items.type;
        items[nowBlank].items.code = item.items.code;
        items[nowBlank].gameObject.GetComponent<Image>().sprite = item.GetComponent<SpriteRenderer>().sprite;
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

    void SelectItem(int index)
    {
        if (selectedImage[index].activeInHierarchy == false)
        {
            for (int i = 0; i < selectedImage.Length; i++)
            {
                selectedImage[i].SetActive(false);
            }

            selectedImage[index].SetActive(true);
            GetSelectedItemFromCSV(index);

            if (selectedItemType == "무기")
            {
                gameObject.GetComponentInParent<PlayerMoveForPhoton>().attackable = true;
                if (selectedItemName == "칼")
                {
                    gameObject.GetComponentInParent<PlayerMoveForPhoton>().ChangeAniLayer(2);
                }
            }
            else
            {
                gameObject.GetComponentInParent<PlayerMoveForPhoton>().attackable = false;
                gameObject.GetComponentInParent<PlayerMoveForPhoton>().ChangeAniLayer(0);
            }
        }
        else
        {
            selectedImage[index].SetActive(false);

            gameObject.GetComponentInParent<PlayerMoveForPhoton>().attackable = false;
            gameObject.GetComponentInParent<PlayerMoveForPhoton>().ChangeAniLayer(0);
        }
    }

    void GetSelectedItemFromCSV(int index)
    {
        if (_EBS.ImageName.Contains(items[index].gameObject.GetComponent<Image>().sprite.name))
        {
            int temp = _EBS.ImageName.IndexOf(items[index].gameObject.GetComponent<Image>().sprite.name);
            selectedItemName = _EBS.Name[temp];
            selectedItemType = _EBS.Category[temp];
        }
        else
        {
            selectedItemName = null;
            selectedItemType = null;
        }
    }
}
