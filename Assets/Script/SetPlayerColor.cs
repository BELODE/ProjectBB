using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class SetPlayerColor : MonoBehaviour
{
    public int partSetting = 0;
    public Toggle shadowColorToggle;
    public GameObject paletteCursor;
    public PhotonView photonView;
    public Vector2[] cursorPosition = new Vector2[4];
    Material mat;

    // Start is called before the first frame update
    void Start()
    {
        
        mat = GetComponent<SpriteRenderer>().material;
        SettingPart(0);
    }

    // Update is called once per frame
    void Update()
    {
        cursorPosition[partSetting] = paletteCursor.transform.position;
    }



    public void PUNSetColor(Color color)
    {
        Vector3 colorTemp = new Vector3(color.r, color.g, color.b);
        photonView.RPC("SetColor", RpcTarget.AllBuffered, colorTemp);
    }
    public void PUNSetShadow(bool color)
    {
        photonView.RPC("SetShadow", RpcTarget.AllBuffered, color);
    }

    public void PUNPartSetting(int index)
    {
        photonView.RPC("SettingPart", RpcTarget.AllBuffered, index);
    }

    [PunRPC]
    void SetColor(Vector3 colorTemp)
    {
        Color color = new Color(colorTemp.x, colorTemp.y, colorTemp.z, 1);

        if (partSetting == 0)
        {
            mat.SetColor("_HairColor", color);
        }
        else if (partSetting == 1)
        {
            mat.SetColor("_ShirtColor", color);
        }
        else if(partSetting == 2)
        {
            mat.SetColor("_PantsColor", color);
        }
        else if (partSetting == 3)
        {
            mat.SetColor("_EyesColor", color);
        }
    }

    [PunRPC]
    void SettingPart(int index)
    {
        partSetting = index;
        paletteCursor.transform.position = cursorPosition[index];

        if (index == 0)
        {
            shadowColorToggle.interactable = true;

            if (mat.GetInt("_HairColorCheck") == 1)
                shadowColorToggle.isOn = true;
            else
                shadowColorToggle.isOn = false;
        }
        if (index == 1)
        {
            shadowColorToggle.interactable = true;

            if (mat.GetInt("_ShirtColorCheck") == 1)
                shadowColorToggle.isOn = true;
            else
                shadowColorToggle.isOn = false;
        }
        if (index == 2)
        {
            shadowColorToggle.interactable = true;

            if (mat.GetInt("_PantsColorCheck") == 1)
                shadowColorToggle.isOn = true;
            else
                shadowColorToggle.isOn = false;
        }
        if (index == 3)
        {
            shadowColorToggle.isOn = false;
            shadowColorToggle.interactable = false;
        }
    }

    [PunRPC]
    void SetShadow(bool color)
    {
        if (partSetting == 0)
        {
            if (color == true)
            {
                mat.SetInt("_HairColorCheck", 1);
            }
            else
            {
                mat.SetInt("_HairColorCheck", 0);
            }
        }
        else if (partSetting == 1)
        {
            if (color == true)
            {
                mat.SetInt("_ShirtColorCheck", 1);
            }
            else
            {
                mat.SetInt("_ShirtColorCheck", 0);
            }
        }
        else if (partSetting == 2)
        {
            if (color == true)
            {
                mat.SetInt("_PantsColorCheck", 1);
            }
            else
            {
                mat.SetInt("_PantsColorCheck", 0);
            }
        }
        else if (partSetting == 3)
        {
            return;
        }
    }
}
