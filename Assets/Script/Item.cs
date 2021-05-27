using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public Items items = new Items();

    float speed = 0.0002f;

    public Vector2 upPos;
    public Vector2 endPos;
    public Vector2 upLrPos;
    public Vector2 endLrPos;

    public bool up = false;
    public bool floor = false;
    public bool firstDrop = true;

    void Start()
    {
        string[] str = gameObject.name.Split('_');
        items.name = str[0];
        items.type = str[1];
        items.code = int.Parse(str[2]);
        if (firstDrop == true)
        {
            float lr = Random.Range(0.4f, 0.6f);
            upPos = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y + 0.5f);
            endPos = new Vector2(gameObject.transform.position.x, GameObject.Find("Player").GetComponent<PlayerMove>().target.transform.parent.transform.position.y - 1.5f);
            upLrPos = new Vector2(gameObject.transform.position.x + lr / 2, gameObject.transform.position.y);
            endLrPos = new Vector2(gameObject.transform.position.x + lr, gameObject.transform.position.y);
        }
    }

    void Update()
    {
        if (firstDrop == true)
        {
            if (up == false && floor == false)
            {
                Vector2 del = new Vector2(upLrPos.x, upPos.y);
                this.transform.position = Vector3.Lerp(gameObject.transform.position, del, 0.05f);
                if (this.transform.position.y + 0.1f >= upPos.y)
                {
                    up = true;
                }
            }
            else if (up == true && floor == false)
            {
                this.transform.Translate(new Vector2(0.005f, endPos.y * (speed += 0.001f)));
                if (this.transform.position.y <= endPos.y)
                {
                    floor = true;
                }
            }
        }
    }
}
