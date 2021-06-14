using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChatTextDestroySetting : MonoBehaviour
{
    public float destroyTime = 4f;

    // Start is called before the first frame update
    void OnEnable()
    {
        Destroy(gameObject, destroyTime);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
