using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerOnMap : MonoBehaviour
{
    public GameObject[] Players, SpawnPoints;
    public GameObject PlayerParents, MasterPlayer;
    // Start is called before the first frame update
    void Start()
    {
        Players = GameObject.FindGameObjectsWithTag("Player");
        foreach(GameObject player in Players)
        {
            player.transform.parent = PlayerParents.transform;
            player.transform.position = SpawnPoints[Random.Range(0, SpawnPoints.Length)].transform.position;
        }
    }

    // Update is called once per frame
    void Update()
    {
        Players = GameObject.FindGameObjectsWithTag("Player");

        foreach (GameObject player in Players)
        {
            if(player.GetComponent<PlayerMoveForPhoton>().isMaster == true)
            {
                MasterPlayer = player;
            }
        }
    }
}
