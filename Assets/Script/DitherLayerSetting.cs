using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DitherLayerSetting : MonoBehaviour
{
    public SpriteMask[] Mask;
    public SpriteRenderer MainSprite;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        foreach (SpriteMask _sm in Mask)
        {
            if(_sm.GetComponent<SpriteRenderer>().sortingOrder < MainSprite.sortingOrder)
            {
                _sm.enabled = false;
            }
            else
            {
                _sm.enabled = true;
            }
           
        }
    }

}
