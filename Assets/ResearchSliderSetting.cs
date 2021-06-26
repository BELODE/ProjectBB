using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResearchSliderSetting : MonoBehaviour
{
    public GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (player.GetComponent<Animator>().GetBool("walking") && gameObject.GetComponent<Animator>().GetBool("research"))
        {
            gameObject.GetComponent<Animator>().SetBool("research", false);
            player.GetComponent<Animator>().SetBool("research", false);
            player.GetComponent<PhotonView>().RPC("DrawerBreak", PhotonTargets.AllBuffered);
        }
    }

    public void SliderEndPoint()
    {
        player.GetComponent<PhotonView>().RPC("DrawerItemSpawn", PhotonTargets.AllBuffered,true);
        gameObject.GetComponent<Animator>().SetBool("research", false);
        player.GetComponent<Animator>().SetBool("research", false);
    }
}
