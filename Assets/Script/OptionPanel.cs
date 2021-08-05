using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionPanel : MonoBehaviour
{
    public Resolution[] resolutions;
    public Dropdown resolutionDropDown;
    public List<Resolution> resolutionIndex;
    public List<string> options;

    public Toggle fullScreenToggle;

    // Start is called before the first frame update
    void Start()
    {
        resolutionIndex = new List<Resolution>();

        resolutions = Screen.resolutions;

        resolutionDropDown.ClearOptions();

        options = new List<string>();

        int currentIndex = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            if ((float)resolutions[i].width / 16f == (float)resolutions[i].height / 9f)
            {
                string option = resolutions[i].width + "*" + resolutions[i].height;

                if (!options.Contains(option))
                {
                    options.Add(option);
                    resolutionIndex.Add(resolutions[i]);

                    if (resolutions[i].width == Screen.width && resolutions[i].height == Screen.height)
                    {
                        Debug.Log(Screen.width + "*" + Screen.height);
                        currentIndex = resolutionIndex.Count - 1;
                    }
                }
            }
        }

        resolutionDropDown.AddOptions(options);
        resolutionDropDown.value = currentIndex;
        resolutionDropDown.RefreshShownValue();
    }

    // Update is called once per frame
    void Update()
    {
        fullScreenToggle.isOn = Screen.fullScreen;
    }

    public void SetResolution(int resolutionIndexValue)
    {
        Resolution resolution = resolutionIndex[resolutionIndexValue];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void SetFullScreen(bool fullScreen)
    {
        Screen.fullScreen = fullScreen;
    }
}
