using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LadderExitSytem : MonoBehaviour
{
    public float pow;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            collision.transform.position = new Vector3(collision.transform.position.x, collision.transform.position.y, 0);
            collision.gameObject.layer = LayerMask.NameToLayer("Player");
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player")
            collision.transform.position = new Vector3(collision.transform.position.x, collision.transform.position.y, (gameObject.transform.position.y - collision.transform.position.y) * pow - 0.3f);

    }
}
