using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFOV : MonoBehaviour
{
    public float viewDistance = 2f;
    public LayerMask layerMask;
    public LayerMask targetMask;

    public Collider2D[] targetCollider;

    private void Start()
    {
        targetCollider = Physics2D.OverlapCircleAll(transform.position, Mathf.Infinity, targetMask);
    }

    void LateUpdate()
    {
        DrawFOV();
    }

    void DrawFOV()
    {
        /*for(int i = 0; i < targetCollider.Length; i++)
        {
            Transform target = targetCollider[i].transform;
            Vector2 dirTarget = (target.position - transform.position).normalized;

            float dstTarget = Vector2.Distance(transform.position, target.position);
            if (dstTarget < viewDistance)
            {
                if (!Physics2D.Raycast(transform.position, dirTarget, dstTarget, layerMask))
                {
                    if (target.GetComponent<PlayerMove>().life == true)
                        target.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
                }
                else
                {
                    target.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
                }
            }
            else
            {
                target.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
            }
        }*/
    }
}
