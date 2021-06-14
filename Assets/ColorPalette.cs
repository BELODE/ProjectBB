using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System;

[Serializable]
public class ColorEvent : UnityEvent<Color> { }
public class ColorPalette : Photon.MonoBehaviour
{
    public ColorEvent onColorPreview;
    public ColorEvent onColorSelect;
    public PhotonView photonView;
    public GameObject Cursor;
    RectTransform rect;
    Texture2D colorTexture;
    Color color;
    // Start is called before the first frame update
    void Start()
    {
        rect = GetComponent<RectTransform>();

        colorTexture = GetComponent<Image>().mainTexture as Texture2D;
    }

    // Update is called once per frame
    void Update()
    {
        if (photonView.isMine)
        {
            if (RectTransformUtility.RectangleContainsScreenPoint(rect, Input.mousePosition))
            {
                ColorPaletteSetting();

                onColorPreview?.Invoke(color);

                if (Input.GetMouseButton(0))
                {
                    Cursor.transform.position = Input.mousePosition;
                    onColorSelect?.Invoke(color);
                }
            }
        }
    }

    void ColorPaletteSetting()
    {
        Vector2 delta;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rect, Input.mousePosition, null, out delta);

        float width = rect.rect.width;
        float height = rect.rect.height;

        delta += new Vector2(width * 0.5f, height * 0.5f);

        float x = Mathf.Clamp(delta.x / width, 0f, 1f);
        float y = Mathf.Clamp(delta.y / height, 0f, 1f);

        int texX = Mathf.RoundToInt(x * colorTexture.width);
        int texY = Mathf.RoundToInt(y * colorTexture.height);

        color = colorTexture.GetPixel(texX, texY);
    }
}
