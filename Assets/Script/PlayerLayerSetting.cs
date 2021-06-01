using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLayerSetting : MonoBehaviour
{
    public int settingLayer;
    public bool isStatic = true, isFront;
    public SpriteRenderer ladderRenderer;
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

            if (isStatic == true)
            {

                _playerRenderer.sortingOrder = settingLayer;
            }
            else
            {
                if(isFront == true)
                {
                    _playerRenderer.sortingOrder = ladderRenderer.sortingOrder + 1;
                }
                else
                {
                    _playerRenderer.sortingOrder = ladderRenderer.sortingOrder - 1;
                }
            }
        }
        if (collision.tag == "Ladder")
        {
            SpriteRenderer _ladderRenderer;
            _ladderRenderer = collision.gameObject.GetComponent<SpriteRenderer>();
            _ladderRenderer.sortingOrder = settingLayer + 1;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            if (isStatic == false)
            {
                SpriteRenderer _playerRenderer;
                _playerRenderer = collision.gameObject.GetComponent<SpriteRenderer>();

                if (isFront == true)
                {
                    _playerRenderer.sortingOrder = ladderRenderer.sortingOrder + 1;
                }
                else
                {
                    _playerRenderer.sortingOrder = ladderRenderer.sortingOrder - 1;
                }
            }
        }
    }
}
