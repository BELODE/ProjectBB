using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LadderGrapSystem : MonoBehaviour
{
    public GameObject MainObject, Ledders;
    public bool isWest = true;
    public SpriteRenderer _SpRenderer;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame


    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            PlayerMove player = collision.gameObject.GetComponent<PlayerMove>();
            MaterialManager mManager = GameObject.Find("MaterialManager").GetComponent<MaterialManager>();

            if (player.target == gameObject)
            {
                _SpRenderer.material = mManager.change;
            }
            else
            {
                _SpRenderer.material = mManager.origin;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            MaterialManager mManager = GameObject.Find("MaterialManager").GetComponent<MaterialManager>();

            _SpRenderer.material = mManager.origin;
        }
    }


}
