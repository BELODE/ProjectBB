using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExpandableTextBox : MonoBehaviour
{
    public float minHeight;
    public Text tx;
    RectTransform BGImage;
    // Start is called before the first frame update
    void Start()
    {
        BGImage = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (tx.gameObject.GetComponent<RectTransform>().rect.height + 100f > minHeight)
        {
            BGImage.sizeDelta = new Vector2(BGImage.rect.width, tx.gameObject.GetComponent<RectTransform>().rect.height + 100f);
        }

        gameObject.transform.position = Input.mousePosition + new Vector3(1,1,0);
    }
}
