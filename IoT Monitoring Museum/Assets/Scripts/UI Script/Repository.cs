using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;

public class Repository : MonoBehaviour
{

    public TMP_InputField url;
    public TMP_InputField user;
    public TMP_InputField pass;
    public GameObject save; //bottone
    public GameObject popUpInsert;
    public GameObject popUpOk;
    string path;

    public GameObject bReturn;

    // Start is called before the first frame update
    void Start()
    {

        // popUpInsert.SetActive(false);

    }

    public void createTxt()
    {
        //path = Application.dataPath + "/Resources/FTParameters.txt";
        //path = Path.Combine(Application.persistentDataPath, "Resources", "FTPParameters.txt");
        path = Path.Combine(Application.persistentDataPath, "Resources");
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
        path = Path.Combine(path, "FTPParameters.txt");

        /*
        if (File.Exists(path)) {
            File.Delete(path);
        }
        File.WriteAllText(path, "\n");
        File.AppendAllText(path, url.text);
        File.AppendAllText(path, "\n" + user.text);
        File.AppendAllText(path, "\n" + pass.text);
        */

        if (File.Exists(path))
        {
            File.Delete(path);
        }

        var f = File.CreateText(path);

        //f.WriteLine("\n");
        f.WriteLine(url.text);
        f.WriteLine(user.text);
        f.WriteLine(pass.text);

        f.Close();

    }

    public void actionSave()
    {
        if (url.text != "" && user.text != "" && pass.text != "")
        {
            //MOD
            if (Application.platform == RuntimePlatform.WebGLPlayer)
            {
                FTPHolder.SetUrl(url.text);
                FTPHolder.SetUser(user.text);
                FTPHolder.SetPass(pass.text);

            }
            //fine MOD
            else
            {
                createTxt();
            }
            popUpOk.SetActive(true);
            url.text = "";
            user.text = "";
            pass.text = "";
            bReturn.SetActive(true);
        }
        else
        {
            popUpInsert.SetActive(true);
        }
    }

}
