using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapControllTest : MonoBehaviour
{
    public enum Maps
    {
        Library, LibraryHall, ExhibitionHall
    }

    [Header("DoorMain")]
    public GameObject DoorMain;
    public MapControllTest otherMCT;
    public Maps HideMap, ShowMap;
    public float hideShowColorSpeed;

    [Header("MapObjects")]
    public GameObject library;
    SpriteRenderer[] libraryRenderers;
    Tilemap[] libraryTileRenderers;

    public GameObject libraryHall;
    SpriteRenderer[] libraryHallRenderers;
    Tilemap[] libraryHallTileRenderers;

    public GameObject ExhibitionHall;
    SpriteRenderer[] ExhibitionHallRenderers;
    Tilemap[] ExhibitionHallTileRenderers;

    bool isEnter = false;
    SpriteRenderer[] hideRenderer, showRenderer;
    Tilemap[] hideTileRenderer, showTileRenderer;
    GameObject hideGroup, showGroup;
    // Start is called before the first frame update
    void Start()
    {
        libraryRenderers = library.GetComponentsInChildren<SpriteRenderer>();
        libraryHallRenderers = libraryHall.GetComponentsInChildren<SpriteRenderer>();
        ExhibitionHallRenderers = ExhibitionHall.GetComponentsInChildren<SpriteRenderer>();

        libraryTileRenderers = library.GetComponentsInChildren<Tilemap>();
        libraryHallTileRenderers = libraryHall.GetComponentsInChildren<Tilemap>();
        ExhibitionHallTileRenderers = ExhibitionHall.GetComponentsInChildren<Tilemap>();


        if (HideMap == Maps.Library)
        {
            hideRenderer = libraryRenderers;
            hideGroup = library;
            hideTileRenderer = libraryTileRenderers;
        }

        else if (HideMap == Maps.LibraryHall)
        {
            hideRenderer = libraryHallRenderers; 
            hideGroup = libraryHall;
            hideTileRenderer = libraryHallTileRenderers;
        }

        else if (HideMap == Maps.ExhibitionHall)
        {
            hideRenderer = ExhibitionHallRenderers;
            hideGroup = ExhibitionHall;
            hideTileRenderer = ExhibitionHallTileRenderers;
        }

        if (ShowMap == Maps.Library)
        {
            showRenderer = libraryRenderers;
            showGroup = library;
            showTileRenderer = libraryTileRenderers;
        }

        else if (ShowMap == Maps.LibraryHall)
        {
            showRenderer = libraryHallRenderers;
            showGroup = libraryHall;
            showTileRenderer = libraryHallTileRenderers;
        }

        else if (ShowMap == Maps.ExhibitionHall)
        {
            showRenderer = ExhibitionHallRenderers;
            showGroup = ExhibitionHall;
            showTileRenderer = ExhibitionHallTileRenderers;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(isEnter == true)
        {
            HideAlpha(hideRenderer, hideGroup, hideTileRenderer);
            ShowAlpha(showRenderer, showGroup, showTileRenderer);
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

    void HideAlpha(SpriteRenderer[] renderers, GameObject group, Tilemap[] tiles)
    {
        foreach (Tilemap _sp in tiles)
        {
            if (_sp.color.a > 0)
            {
                float alphaColor = _sp.color.a - hideShowColorSpeed;
                _sp.color = new Color(1, 1, 1, alphaColor);
            }
        }

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

    void ShowAlpha(SpriteRenderer[] renderers, GameObject group, Tilemap[] tiles)
    {

        group.SetActive(true);

        foreach (Tilemap _sp in tiles)
        {
            if (_sp.color.a < 1)
            {
                float alphaColor = _sp.color.a + hideShowColorSpeed;
                _sp.color = new Color(1, 1, 1, alphaColor);
            }
        }

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

