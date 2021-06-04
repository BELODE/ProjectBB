using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LadderSystem : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Ladder"), LayerMask.NameToLayer("Player"), true);
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Ladder"), LayerMask.NameToLayer("Default"), true);
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("PlayerLadderOn"), LayerMask.NameToLayer("Default"), true);
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("PlayerLadderOn"), LayerMask.NameToLayer("LadderDefault"), true);
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("GrabedPlayer"), LayerMask.NameToLayer("LadderDefault"), true);
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("GrabedPlayer"), LayerMask.NameToLayer("Ladder"), true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (collision.GetComponent<PlayerMove>().Push == false)
            {
                collision.gameObject.layer = LayerMask.NameToLayer("PlayerLadderOn");
            }
        }
    }
}
