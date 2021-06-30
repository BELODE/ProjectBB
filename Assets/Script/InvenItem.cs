using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Photon.Pun;

public class InvenItem : MonoBehaviour, IDropHandler,IDragHandler,IBeginDragHandler,IEndDragHandler
{
    public Items items = new Items();
    public int num;
    public GameObject player;
    public int exPanel_x_Pivot;
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
                Vector2 dir = player.GetComponent<PlayerMoveForPhoton>().PlayerCamera.GetComponent<Camera>().ScreenToWorldPoint(mos);

                RaycastHit2D hit = Physics2D.Raycast(dir, Vector2.zero, 0f);

                if (hit.collider != null && hit.transform.name == "Drawer" && player.GetComponent<PlayerMove>().target != null && player.GetComponent<PlayerMove>().target.name == "Drawer" && hit.transform.GetComponent<DrawerInven>().GetOpen() == true)
                {
                    hit.transform.GetComponent<DrawerInven>().ItemIn(transform.parent.parent.GetComponent<Inventory>().dragItem.GetComponent<InvenItem>().items.code);
                    hit.transform.GetComponent<DrawerInven>().SetOpen(false);
                }
                else
                {
                    player.GetComponent<PhotonView>().RPC("ItemSpawn", RpcTarget.AllBuffered, "Prefabs/" + transform.parent.parent.GetComponent<Inventory>().dragItem.GetComponent<InvenItem>().items.name + "_" + transform.parent.parent.GetComponent<Inventory>().dragItem.GetComponent<InvenItem>().items.type + "_" + transform.parent.parent.GetComponent<Inventory>().dragItem.GetComponent<InvenItem>().items.code + "_");
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
