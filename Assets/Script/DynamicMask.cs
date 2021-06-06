using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicMask : MonoBehaviour
{
    SpriteRenderer _sr;
    SpriteMask _sm;
    // Start is called before the first frame update
    void Start()
    {
        _sr = GetComponent<SpriteRenderer>();
        _sm = GetComponent<SpriteMask>();
    }

    // Update is called once per frame
    void Update()
    {
        _sm.sprite = _sr.sprite;
    }
}
