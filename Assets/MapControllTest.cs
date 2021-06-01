using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapControllTest : MonoBehaviour
{
    public enum Maps
    {
        Library, LibraryHall
    }

    [Header("DoorMain")]
    public GameObject DoorMain;
    public MapControllTest otherMCT;
    public Maps HideMap, ShowMap;
    public float hideShowColorSpeed;

    [Header("MapObjects")]
    public GameObject library;
    public SpriteRenderer[] libraryRenderers;

    public GameObject libraryHall;
    public SpriteRenderer[] libraryHallRenderers;

    bool isEnter = false;
    SpriteRenderer[] hideRenderer, showRenderer;
    GameObject hideGroup, showGroup;
    // Start is called before the first frame update
    void Start()
    {
        libraryRenderers = library.GetComponentsInChildren<SpriteRenderer>();
        libraryHallRenderers = libraryHall.GetComponentsInChildren<SpriteRenderer>();

        if (HideMap == Maps.Library)
        {
            hideRenderer = libraryRenderers;
            hideGroup = library;
        }

        else if (HideMap == Maps.LibraryHall)
        {
            hideRenderer = libraryHallRenderers; 
            hideGroup = libraryHall;
        }

        if (ShowMap == Maps.Library)
        {
            showRenderer = libraryRenderers;
            showGroup = library;
        }

        else if (ShowMap == Maps.LibraryHall)
        {
            showRenderer = libraryHallRenderers;
            showGroup = libraryHall;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(isEnter == true)
        {
            HideAlpha(hideRenderer, hideGroup);
            ShowAlpha(showRenderer, showGroup);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (DoorMain.GetComponent<Animator>().GetBool("Open") == true && collision.tag == "Player")
        {
            otherMCT.enabled = false;
            isEnter = true;
        }        
    }

    private void OnDisable()
    {
        isEnter = false;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        otherMCT.enabled = true;
    }

    void HideAlpha(SpriteRenderer[] renderers, GameObject group)
    {
        foreach (SpriteRenderer _sp in renderers)
        {
            if (_sp.color.a > 0)
            {
                float alphaColor = _sp.color.a - hideShowColorSpeed;
                _sp.color = new Color(1, 1, 1, alphaColor);
            }
            else
            {
                group.SetActive(false);
            }
        }
    }

    void ShowAlpha(SpriteRenderer[] renderers, GameObject group)
    {
        group.SetActive(true);

        foreach (SpriteRenderer _sp in renderers)
        {
            if (_sp.color.a < 1)
            {
                float alphaColor = _sp.color.a + hideShowColorSpeed;
                _sp.color = new Color(1, 1, 1, alphaColor);
            }
        }
    }
}

