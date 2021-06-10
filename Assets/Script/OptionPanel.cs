using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionPanel : MonoBehaviour
{
    public Resolution[] resolutions;
    public Dropdown resolutionDropDown;
    // Start is called before the first frame update
    void Start()
    {
        resolutions = Screen.resolutions;

        resolutionDropDown.ClearOptions();

        List<string> options = new List<string>();

        for(int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + "*" + resolutions[i].height;
            options.Add(option);
        }

        resolutionDropDown.AddOptions(options);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
