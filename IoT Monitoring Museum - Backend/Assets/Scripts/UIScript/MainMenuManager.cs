using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class MainMenuManager : MonoBehaviour
{

    public GameObject menuIn;
    public GameObject menuChangeRep;
    public GameObject bReturn; //bottone
    public GameObject popUpInfo;

    public static bool go; //settato dal popUP di URIinvalid


    // Start is called before the first frame update
    void Start()
    {
       if (go == true) {
            menuIn.SetActive(false);
            menuChangeRep.SetActive(true);
        }
        checkFile();
        Debug.Log(Application.persistentDataPath);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void checkFile()
    {
        string path = "";

        if(Application.platform == RuntimePlatform.OSXPlayer)
        {
            path = Application.persistentDataPath + "/Resources/FTPParameters.txt";
        }
        else
        {
            path = Path.Combine(Application.persistentDataPath, "Resources", "FTPParameters.txt");
        }
        //string path = Path.Combine(Application.persistentDataPath, "Resources", "FTPParameters.txt"); //crea path indipendente dal sistema operativo
        if (!File.Exists(path))
        {
            //menuChangeRep.SetActive(true);
            popUpInfo.SetActive(true);
            //bReturn.SetActive(false);
            //menuIn.SetActive(false);
        }
    }

    public void openChangeRep() {
        menuIn.SetActive(false);
        menuChangeRep.SetActive(true);
        bReturn.SetActive(false);
    }
}
