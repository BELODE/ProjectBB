using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LadderGrapSystem : MonoBehaviour
{
    public bool isEnter = false;
    public PlayerMove PM;
    public GameObject MainObject, Ledders;
    public bool isWest = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isEnter == true)
            {
                if (PM != null)
                {
                    MainObject.transform.parent = PM.gameObject.transform;
                    PM.speed = 2f;
                    PM.gameObject.GetComponent<Animator>().SetBool("Push", true);
                }
            }
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            if (PM != null)
            {
                MainObject.transform.parent = Ledders.transform;
                PM.speed = 3f;
                PM.gameObject.GetComponent<Animator>().SetBool("Push", false);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            PM = collision.gameObject.GetComponent<PlayerMove>();
            isEnter = true;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            isEnter = false;
            if (isWest == true)
            {
                PM.gameObject.GetComponent<Animator>().SetFloat("isWest", 1f);
            }
            else
            {
                PM.gameObject.GetComponent<Animator>().SetFloat("isWest", -1f);
            }
        }
    }
}
