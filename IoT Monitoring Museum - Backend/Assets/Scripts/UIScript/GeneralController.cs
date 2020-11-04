using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.IO;

public class GeneralController : MonoBehaviour
{

    private AudioSource audio;

    private static float musicVolume = 1f;

    

    public void load() {
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            SceneManager.LoadScene(1);
        }
        else
        {
            SceneManager.LoadScene(0);
        }
    }

    public void exit() {
        Application.Quit();
    }

    public void setQual(int index)
    {

        QualitySettings.SetQualityLevel(index);


    }

    public void setFullScreen(bool isFullScreen)
    {

        Screen.fullScreen = isFullScreen;
    }

    // Start is called before the first frame update
    void Start()
    {
        audio = GetComponent<AudioSource>();
       
    }

    // Update is called once per frame
    void Update()
    {

        audio.volume = musicVolume;

    }

    public void setVol(float vol)
    {
        musicVolume = vol;
    }


}
