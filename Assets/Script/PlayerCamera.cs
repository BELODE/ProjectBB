﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public Transform player;

    void FixedUpdate()
    {
        this.transform.position = new Vector3(player.position.x, player.position.y, player.position.z - 10);
    }
}
