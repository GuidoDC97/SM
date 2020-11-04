
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Fade : MonoBehaviour
{

    public Animator animator;

    private int indexLoad;

    public Image imm;

    // Update is called once per frame
    /*void Update()
    {
        if (Input.GetMouseButton(0))
        {
            FadeScene(1);
        }

    }*/

    public void disable()
    {
        imm.enabled = false;
    }

    public void FadeScene()
    {
        imm.enabled = true;
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            indexLoad = 1;
        }
        else {
            indexLoad = 0;
        }
        animator.SetTrigger("fadeout");
    }

    public void OnFadeComplete()
    {
        SceneManager.LoadScene(indexLoad);
    }
}
