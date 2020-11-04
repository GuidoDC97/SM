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

    public GameObject breturn; //bottore return 

    // Start is called before the first frame update
    void Start()
    {

       // popUpInsert.SetActive(false);
        
    }

    public void createTxt()
    {
        string path = Path.Combine(Application.persistentDataPath, "Resources");
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }

        path = Path.Combine(path, "FTPParameters.txt");
        

        //path = Application.dataPath + "/Resources/FTParameters.txt";
        //path = Path.Combine(Application.persistentDataPath, "Resources", "FTPParameters.txt");
        /*if (File.Exists(path)) {
            File.Delete(path);
        }
        File.WriteAllText(path, "\n");
        File.AppendAllText(path, url.text);
        File.AppendAllText(path, "\n" + user.text);
        File.AppendAllText(path, "\n" + pass.text);*/
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
            createTxt();
            popUpOk.SetActive(true);
            url.text = "";
            user.text = "";
            pass.text = "";
            breturn.SetActive(true);


        }
        else
        {
            popUpInsert.SetActive(true);
        }
    }

}
