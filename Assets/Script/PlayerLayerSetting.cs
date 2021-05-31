using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLayerSetting : MonoBehaviour
{
    public int settingLayer;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            SpriteRenderer _playerRenderer;
            _playerRenderer = collision.gameObject.GetComponent<SpriteRenderer>();
            _playerRenderer.sortingOrder = settingLayer;
        }
    }
}
