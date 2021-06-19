using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InvenItem : MonoBehaviour, IDropHandler,IDragHandler,IBeginDragHandler,IEndDragHandler
{
    public Items items = new Items();
    public int num;

    void Start()
    {

    }

    void Update()
    {

    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (num != 10)
        {
            transform.parent.parent.GetComponent<Inventory>().dragItem.GetComponent<InvenItem>().items.name = items.name;
            transform.parent.parent.GetComponent<Inventory>().dragItem.GetComponent<InvenItem>().items.type = items.type;
            transform.parent.parent.GetComponent<Inventory>().dragItem.GetComponent<InvenItem>().items.code = items.code;
            transform.parent.parent.GetComponent<Inventory>().dragItem.GetComponent<Image>().sprite = gameObject.GetComponent<Image>().sprite;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (num != 10)
        {
            Vector2 mousePos = Input.mousePosition;
            transform.parent.parent.GetComponent<Inventory>().dragItem.transform.position = mousePos;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (num != 10)
        {
            Sprite[] spr = Resources.LoadAll<Sprite>("Items/item");
            if (eventData.pointerEnter == null && items.name != null)
            {
                Vector2 mos = Input.mousePosition;
                Vector2 dir = Camera.main.ScreenToWorldPoint(mos);

                RaycastHit2D hit = Physics2D.Raycast(dir, Vector2.zero, 0f, 1 << LayerMask.NameToLayer("Drawer"));

                if (hit.collider != null && hit.transform.GetComponent<DrawerInven>().GetOpen() == true && GameObject.Find("Player").GetComponent<PlayerMove>().target != null && GameObject.Find("Player").GetComponent<PlayerMove>().target.name=="Drawer")
                {
                    hit.transform.GetComponent<DrawerInven>().ItemIn(transform.parent.parent.GetComponent<Inventory>().dragItem.GetComponent<InvenItem>().items.code);
                    hit.transform.GetComponent<DrawerInven>().SetOpen(false);
                }
                else if (hit.collider == null || hit.transform.GetComponent<DrawerInven>().GetOpen() == false || (hit.collider != null && GameObject.Find("Player").GetComponent<PlayerMove>().target == null) || (hit.collider != null && GameObject.Find("Player").GetComponent<PlayerMove>().target != null && GameObject.Find("Player").GetComponent<PlayerMove>().target.name != "Drawer"))
                {
                    Vector2 cPos = new Vector2(GameObject.Find("Player").transform.position.x, GameObject.Find("Player").transform.position.y);
                    GameObject obj = Instantiate(Resources.Load("Prefabs/" + transform.parent.parent.GetComponent<Inventory>().dragItem.GetComponent<InvenItem>().items.name + "_" + transform.parent.parent.GetComponent<Inventory>().dragItem.GetComponent<InvenItem>().items.type + "_" + transform.parent.parent.GetComponent<Inventory>().dragItem.GetComponent<InvenItem>().items.code + "_"), cPos, Quaternion.identity) as GameObject;
                    obj.GetComponent<Item>().firstDrop = false;
                }

                items.name = null;
                items.type = null;
                items.code = 0;
                gameObject.GetComponent<Image>().sprite = spr[21];
            }
            transform.parent.parent.GetComponent<Inventory>().dragItem.GetComponent<InvenItem>().items.name = null;
            transform.parent.parent.GetComponent<Inventory>().dragItem.GetComponent<InvenItem>().items.type = null;
            transform.parent.parent.GetComponent<Inventory>().dragItem.GetComponent<InvenItem>().items.code = 0;
            transform.parent.parent.GetComponent<Inventory>().dragItem.GetComponent<Image>().sprite = spr[21];
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (num != 10)
        {
            if (num != eventData.pointerPress.GetComponent<InvenItem>().num)
            {
                string n = eventData.pointerPress.GetComponent<InvenItem>().items.name;
                string t = eventData.pointerPress.GetComponent<InvenItem>().items.type;
                int c = eventData.pointerPress.GetComponent<InvenItem>().items.code;
                Sprite s = eventData.pointerPress.GetComponent<Image>().sprite;
                eventData.pointerPress.GetComponent<InvenItem>().items.name = items.name;
                eventData.pointerPress.GetComponent<InvenItem>().items.type = items.type;
                eventData.pointerPress.GetComponent<InvenItem>().items.code = items.code;
                eventData.pointerPress.GetComponent<Image>().sprite = gameObject.GetComponent<Image>().sprite;
                items.name = n;
                items.type = t;
                items.code = c;
                gameObject.GetComponent<Image>().sprite = s;
            }
        }
    }

}
