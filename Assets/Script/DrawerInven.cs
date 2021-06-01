using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawerInven : MonoBehaviour
{
    public List<int> itemsCode = new List<int>();
    static public int itemSortingOrder;

    private MaterialManager mManager;
    private PlayerMove player;
    private ItemSpawner itemS;

    void Start()
    {
        mManager = GameObject.Find("MaterialManager").GetComponent<MaterialManager>();
        player = GameObject.Find("Player").GetComponent<PlayerMove>();
        itemS = GameObject.Find("ItemSpawner").GetComponent<ItemSpawner>();
        int rand = Random.Range(1, 5);
        for (int i = 0; i < rand; i++)
        {
            itemsCode.Add(Random.Range(0, GameObject.Find("ItemSpawner").GetComponent<ItemSpawner>().items.Length));
        }
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
        if (itemsCode.Count > 0)
        {
            int rand = Random.Range(0, itemsCode.Count);
            itemSortingOrder = this.gameObject.GetComponent<SpriteRenderer>().sortingOrder;
            GameObject gameObject = Instantiate(itemS.items[itemsCode[rand]], GameObject.Find("Player").GetComponent<PlayerMove>().target.transform.position, Quaternion.identity) as GameObject;
            itemsCode.RemoveAt(rand);
        }
    }

    public void ItemIn(int code)
    {
        itemsCode.Add(code);
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
