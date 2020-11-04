using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenManager : MonoBehaviour
{
    public void setFullScreen(bool isFullScreen)
    {

        Screen.fullScreen = isFullScreen;
    }

    public void setQual(int index)
    {

        QualitySettings.SetQualityLevel(index);


    }
}
