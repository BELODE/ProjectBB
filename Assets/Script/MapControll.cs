using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapControll : MonoBehaviour
{
    public SpriteRenderer nowPlace;
    public SpriteRenderer corridor;
    public SpriteRenderer door;
    public List<GameObject> candles;
    public bool check = false;
    public bool visible = true;

    void Start()
    {
        
    }

    void Update()
    {
        if (check == true)
        {
            if (visible==true)
            {
                Color color = nowPlace.color;
                color.a = color.a - 0.05f;
                nowPlace.color = color;
                corridor.color = color;
                if (color.a > 0.5f)
                {
                    door.color = color;
                }

                for (int i = 0; i < candles.Count; i++)
                {
                    candles[i].transform.GetChild(0).GetComponent<SpriteRenderer>().color = color;
                    candles[i].transform.GetChild(1).GetComponent<SpriteRenderer>().color = color;
                }

                if (color.a <= 0)
                {
                    door.sortingOrder = 13;
                    visible = false;
                    check = false;
                }
            }
            else if (visible==false)
            {
                Color color = nowPlace.color;
                color.a = color.a + 0.05f;
                nowPlace.color = color;
                corridor.color = color;
                door.color = color;
                for (int i = 0; i < candles.Count; i++)
                {
                    candles[i].transform.GetChild(0).GetComponent<SpriteRenderer>().color = color;
                    candles[i].transform.GetChild(1).GetComponent<SpriteRenderer>().color = color;
                }
                if (color.a >= 1f)
                {
                    door.sortingOrder = 0;
                    visible = true;
                    check = false;
                }
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (gameObject.transform.position.y < (collision.transform.position.y-0.47f) && visible == true)
        {
            check = true;
        }
        else if (gameObject.transform.position.y > (collision.transform.position.y-0.47f) && visible == false)
        {
            check = true;
        }
    }
}
