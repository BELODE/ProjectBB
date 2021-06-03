using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawerInven : MonoBehaviour
{
    public int itemsCode;
    public int persent;
    static public int itemSortingOrder;

    private MaterialManager mManager;
    private PlayerMove player;
    private ItemSpawner itemS;

    void Start()
    {
        mManager = GameObject.Find("MaterialManager").GetComponent<MaterialManager>();
        player = GameObject.Find("Player").GetComponent<PlayerMove>();
        itemS = GameObject.Find("ItemSpawner").GetComponent<ItemSpawner>();
        itemsCode = Random.Range(0, GameObject.Find("ItemSpawner").GetComponent<ItemSpawner>().items.Length);
    }

    void Update()
    {
        MaterialChange();
    }

    private void MaterialChange()
    {
        if (player.target == gameObject)
        {
            transform.GetComponent<SpriteRenderer>().material = mManager.change;
        }
        else
        {
            transform.GetComponent<SpriteRenderer>().material = mManager.origin;
        }
    }

    public void ItemSpawn()
    {
        if (itemsCode != -1)
        {
            itemSortingOrder = this.gameObject.GetComponent<SpriteRenderer>().sortingOrder;
            GameObject gameObject = Instantiate(itemS.items[itemsCode], GameObject.Find("Player").GetComponent<PlayerMove>().target.transform.position, Quaternion.identity) as GameObject;
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
}
