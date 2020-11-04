using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using TMPro;
using System.IO;

public class Fade : MonoBehaviour
{

    public Animator animator;

    private int indexLoad;

    public Image imm;

    public GameObject loadingScreen;
    public Slider slider;

    public TextMeshProUGUI m_Text;
    public TextMeshProUGUI percentText;



    public void Load(int i)
    {
        StartCoroutine(LoadAsync(i));
    }

    IEnumerator LoadAsync(int sceneIndex)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);

        loadingScreen.SetActive(true);

        //operation.allowSceneActivation = false;

        /*
        while (!operation.isDone)
        {
            //Output the current progress
    
            m_Text.text = "Loading progress: " + (operation.progress * 100) + "%";

            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            slider.value = progress;
            percentText.text = slider.value * 100 + "%"; 

            // Check if the load has finished
            if (operation.progress >= 0.9f)
            {
                //Change the Text to show the Scene is ready
                m_Text.text = "Press the space bar to continue";
                //Wait to you press the space key to activate the Scene
                if (Input.GetKeyDown(KeyCode.Space))
                    //Activate the Scene
                    operation.allowSceneActivation = true;
            }
         

            yield return null;
        }
        */
        
        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            slider.value = progress;
            percentText.text = slider.value * 100 + "%";

            //if(slider.value == 1)
            //{
            //    animator.SetTrigger("fadeout");
            //}

            yield return null;
        }
        
    }

    public void disable()
    {
        imm.enabled = false;
    }

    public void FadeScene()
    {
        imm.enabled = true;
        setIndexLoad();
        animator.SetTrigger("fadeout");

        //Load(indexLoad);

    }

    public void OnFadeComplete()
    {
        //SceneManager.LoadScene(indexLoad);
        //Load(indexLoad);
        StartCoroutine(LoadAsync(indexLoad));
    }

    void setIndexLoad() {
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            indexLoad = 1;
        }
        else {
            indexLoad = 0;
        }
    }

    

    
}

