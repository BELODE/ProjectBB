﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public Items items = new Items();

    float speed = 0.0002f;
    float xSpeed = 0f;
    public float endYPos;

    public Vector3 upPos;
    public Vector3 endPos;
    public Vector3 upLrPos;

    public bool up = false;
    public bool floor = false;
    public bool firstDrop = true;

    public GameObject Parentsdrawer;
    public float lr = -1;

    void Start()
    {

        string[] str = gameObject.name.Split('_');
        items.name = str[0];
        items.type = str[1];
        items.code = int.Parse(str[2]);
        if (firstDrop == true)
        {
            upPos = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 0.25f, -2f);
            endPos = new Vector3(gameObject.transform.position.x, Parentsdrawer.transform.parent.transform.position.y - endYPos, 0);

            if (lr == 0)
            {
                xSpeed = -0.005f;
                upLrPos = new Vector3(gameObject.transform.position.x + -0.6f / 2, gameObject.transform.position.y, 0);
            }
            else if(lr == 1)
            {
                xSpeed = 0.005f;
                upLrPos = new Vector3(gameObject.transform.position.x + 0.6f / 2, gameObject.transform.position.y, 0);
            }
        }
    }

    void Update()
    {
        if (firstDrop == true)
        {
            if (up == false && floor == false)
            {
                Vector3 del = new Vector3(upLrPos.x, upPos.y, -2f);
                this.transform.position = Vector3.Lerp(gameObject.transform.position, del, 0.05f);
                if (this.transform.position.y + 0.1f >= upPos.y)
                {
                    up = true;
                }
            }
            else if (up == true && floor == false)
            {
                this.transform.Translate(new Vector2(xSpeed, -1f * (speed += 0.001f)));
                if (this.transform.position.y <= endPos.y)
                {
                    floor = true;
                }
            }
            else
            {
                this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, 0);
            }
        }
    }
}
