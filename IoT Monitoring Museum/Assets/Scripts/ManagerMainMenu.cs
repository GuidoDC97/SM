using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class ManagerMainMenu : MonoBehaviour
{
   

    static GameObject start;

    public GameObject menuIn;
    public GameObject menuChangeRep;
    public GameObject bReturn; //bottone
    public GameObject popUpInfo;


    // Start is called before the first frame update
    void Start()
    {

        start = GameObject.Find("ButtonStart");
        start.SetActive(false);

        //Debug.Log(Application.persistentDataPath);

         checkFile();
         Debug.Log(Application.persistentDataPath);
       
    }

    public void exit()
    {
        Application.Quit();
    }

    public static GameObject getB() {
        return start;
    }

    public void checkFile()
    {
        string path = Path.Combine(Application.persistentDataPath, "Resources", "FTPParameters.txt");
        if (!File.Exists(path))
        {
            //menuChangeRep.SetActive(true);
            popUpInfo.SetActive(true);
            //bReturn.SetActive(false);
            //menuIn.SetActive(false);
        }
    }

    public void openChangeRep() {
        menuChangeRep.SetActive(true);
        bReturn.SetActive(false);
        menuIn.SetActive(false);
    }
}
