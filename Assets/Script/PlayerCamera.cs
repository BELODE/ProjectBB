using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public Transform player;
    public float yPositionTemp;
    void FixedUpdate()
    {
        this.transform.position = new Vector3(player.position.x, player.position.y + yPositionTemp, player.position.z - 10);
    }
}
