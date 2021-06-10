using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ExplainationBoardSetting : MonoBehaviour
{
    public GameObject ExplanationBar;
    public Text itemName, itemType, itemEx;
    public Canvas _canvas;

    GraphicRaycaster graphicRaycaster;
    PointerEventData pED;

    public List<string> ImageName, Code, Name, Category, Ex;


    // Start is called before the first frame update
    void Start()
    {
        ReadItemCSV();
        graphicRaycaster = _canvas.GetComponent<GraphicRaycaster>();
        pED = new PointerEventData(null);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ItemCheck(float xPivot)
    {
        pED.position = Input.mousePosition;
        List<RaycastResult> result = new List<RaycastResult>();
        graphicRaycaster.Raycast(pED, result);

        if(result.Count != 0)
        {
            if (ImageName.Contains(result[0].gameObject.GetComponent<Image>().sprite.name))
            {
                ExplanationBar.transform.position = Input.mousePosition + new Vector3(1, 1, 0);
                ExplanationBar.GetComponent<RectTransform>().pivot = new Vector2(xPivot, 0);
                ExplanationBar.SetActive(true);
                int index = ImageName.IndexOf(result[0].gameObject.GetComponent<Image>().sprite.name);
                itemName.text = Name[index].ToString();
                itemType.text = Category[index].ToString();
                itemEx.text = Ex[index].ToString();
            }

        }
    }

    public void EndCheck()
    {
        ExplanationBar.SetActive(false);
    }

    void ReadItemCSV()
    {
        List<Dictionary<string, object>> data = CSVReader.Read("Documents/CSV_ItemCode");

        for (var i = 0; i < data.Count; i++)
        {
            ImageName.Add(data[i]["ImageName"].ToString());
            Code.Add(data[i]["Code"].ToString());
            Name.Add(data[i]["Name"].ToString());
            Category.Add(data[i]["Category"].ToString());
            Ex.Add(data[i]["Explanation"].ToString());

        }

    }
}
