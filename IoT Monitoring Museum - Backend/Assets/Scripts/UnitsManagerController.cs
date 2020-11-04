using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UnitsManagerController : MonoBehaviour
{

    private string ftpPath;
    //private string fileSetupPath;
    public static int ADD_ALLERT = 0;
    public static int MOV_ALLERT = 1;
    public static int DES_ALLERT = 2;

    public static int MOV_DROPDOWN = 0;
    public static int DES_DROPDOWN = 1;

    public GameObject unitPrefab;

    List<GameObject> units = new List<GameObject>();
    List<string> unitsNames = new List<string>();

    public string unitName = "";
    public string floorName = "";

    //public string url = "ftp://files.000webhost.com/public_html/UnitsRepository.txt";
    //public string pathRepository;
    public string pathPreviousSetup;
    private int dimFileSetup;

    #region UI
    public GameObject popUpConnectionPanel;
    public GameObject mainPanel;

    public TextMeshProUGUI[] textAllert = new TextMeshProUGUI[3];
    public TMP_Dropdown[] dropdown = new TMP_Dropdown[2];
    public TMP_InputField addInputField;
    public TMP_Dropdown floorDropdowm;
    public Button addButton;
    public Button finButton;
    public Button saveButton;
    public Button remButton;


    public GameObject popUpURI;
    public GameObject popSavWithConn;

    public Toggle addToggle;
    public Toggle moveToggle;
    public Toggle remToggle;

    

    #endregion


    // Start is called before the first frame update
    void Start()
    {
        // creazione cartelle
        ftpPath = Path.Combine(Application.persistentDataPath, "Resources", "FTPParameters.txt");

        //pathRepository = Path.Combine(Application.persistentDataPath, "Resources", "UnitsRepository.txt");

        pathPreviousSetup = Path.Combine(Application.persistentDataPath, "Resources", "UnitsRepositoryOLD.txt");
        /*
        try
        {
            string pathR = Path.Combine(Application.persistentDataPath, "Resources");
            if (!Directory.Exists(pathR))
            {
                Directory.CreateDirectory(pathR);
            }

        }
        catch (IOException ex)
        {
            Console.WriteLine(ex.Message);
        }
        */

        if (File.Exists(pathPreviousSetup))
        {
            Debug.Log("setupOLD trovato");
            LoadSetup();
           
        }
        else
        {
            Debug.Log("nessun SetupOLD trovato");
        }



        /*FABIO
        GameObject[] u = GameObject.FindGameObjectsWithTag("ControlUnit");

        for (int i = 0; i < u.Length; i++)
        {
            units.Add(u[i]);
        }

        for (int i = 0; i < units.Count; i++)
        {
            unitsNames.Add(units[i].name);
        }

        for(int i = 0; i < dropdown.Length; i++)
        {
            dropdown[i].AddOptions(unitsNames);
        }
        */

    }

    private void Update()
    {
        if(addToggle.isOn || moveToggle.isOn || remToggle.isOn)
        {
            saveButton.interactable = false;
        } else
        {
            saveButton.interactable = true;
        }

        if(dropdown[DES_DROPDOWN].value == 0)
        {
            remButton.interactable = false;
        }
        else
        {
            remButton.interactable = true;

        }
    }


    public void LoadSetup()
    {
        string[] lines = File.ReadAllLines(pathPreviousSetup);
        dimFileSetup = lines.Length;

        for (int i = 2; i < dimFileSetup; i++)
        {
            string[] s = lines[i].Split(new char[] { ';' });

            string labelName = s[0].Trim();
            string floorName = s[1].Trim();
            float xCoord = float.Parse(s[2].Trim());
            float yCoord = float.Parse(s[3].Trim());
            float zCoord = float.Parse(s[4].Trim());

            if (unitPrefab)
            {
                GameObject controlUnit = Instantiate(unitPrefab, new Vector3(xCoord, yCoord, zCoord), Quaternion.identity);
                controlUnit.transform.name = labelName;
                controlUnit.transform.parent = GameObject.Find(floorName).transform;
                units.Add(controlUnit);
            }

            
        }

        for (int i = 0; i < units.Count; i++)
        {
            unitsNames.Add(units[i].name);
        }

        for (int i = 0; i < dropdown.Length; i++)
        {
            dropdown[i].AddOptions(unitsNames);
        }

    }


    public void SetUnitName(string n)
    {
        unitName = n.ToUpper();
    }

    public void SetFloorName(int i)
    {
        switch (i)
        {
            case 0:
                floorName = "";
                break;

            case 1:
                floorName = "Floor-1";
                break;

            case 2:
                floorName = "Floor0";
                break;

            case 3:
                floorName = "Floor1";
                break;

            case 4:
                floorName = "Floor2";
                break;
        }
    }

    public void SetUnitName(int i)
    {
        if(i != 0)
        {
            unitName = unitsNames[i-1];
        }
        else
        {
            unitName = "";
        }
        Debug.Log(unitName);
    }

    public void CreateUnit()
    {
        if (unitName.Length>1 || !(string.Compare(unitName,"A")>0 && string.Compare(unitName, "Z")<0))
        {
            textAllert[ADD_ALLERT].color = Color.red;
            textAllert[ADD_ALLERT].text = "WARNING: insert a valid label's name.";

            Debug.Log("ERROR");
        }
        else
        {
            if (string.Compare(unitName, "") != 0 && string.Compare(floorName, "") != 0 && !unitsNames.Contains(unitName))
            {
                addToggle.interactable = false;

                textAllert[ADD_ALLERT].color = Color.blue;
                //textAllert[ADD_ALLERT].text = "Centralina aggiunta! Posizionare la centralina.";
                textAllert[ADD_ALLERT].text = "Label added! Place it.";
                addButton.interactable = false;
                finButton.interactable = true;
                addInputField.interactable = false;
                floorDropdowm.interactable = false;

                unitsNames.Add(unitName);

                for (int i = 0; i < dropdown.Length; i++)
                {
                    dropdown[i].ClearOptions();
                    dropdown[i].AddOptions(new List<string>() { "Select label" });
                    dropdown[i].AddOptions(unitsNames);
                }

                GameObject floor = GameObject.Find(floorName);
                GameObject unit = GameObject.Instantiate(unitPrefab, new Vector3(0, 0.2f, 0), new Quaternion(0, 0, 0, 0));

                units.Add(unit);

                unit.name = unitName;

                unit.transform.parent = floor.transform;
                unit.transform.localPosition = new Vector3(-5.0f, unit.transform.localPosition.y, -6.0f);

                unit.AddComponent<UnitController>();
                UnitController unitController = unit.GetComponent<UnitController>();
                unitController.SetTextAllert(textAllert[ADD_ALLERT]);
                unitController.SetEnable(true);
            }
            else if (unitsNames.Contains(unitName))
            {
                textAllert[ADD_ALLERT].color = Color.red;
                textAllert[ADD_ALLERT].text = "WARNING: this label already exist.";

                Debug.Log("ERROR");
            }
            else if (string.Compare(unitName, "") == 0)
            {
                textAllert[ADD_ALLERT].color = Color.red;
                textAllert[ADD_ALLERT].text = "WARNING: insert label's name.";
            }
            else if (string.Compare(floorName, "") == 0)
            {
                textAllert[ADD_ALLERT].color = Color.red;
                textAllert[ADD_ALLERT].text = "WARNING: select floor.";
            }
        }
        
    }

    public void DestroyUnit()
    {
        if (string.Compare(unitName, "") != 0 && unitsNames.Contains(unitName))
        {
            Debug.Log("Destroy: " + unitName);
            GameObject unit = GameObject.Find(unitName);

            GameObject.Destroy(unit);

            unitsNames.Remove(unitName);

            for(int i = 0; i < dropdown.Length; i++)
            {
                dropdown[i].ClearOptions();
                dropdown[i].AddOptions(new List<string>() { "Select label" });
                dropdown[i].AddOptions(unitsNames);
            }
            textAllert[ADD_ALLERT].color = Color.blue;
            textAllert[DES_ALLERT].text = "Label deleted.";
        }
    }

    public void MoveUnit()
    {
        for (int i = 0; i<units.Count; i++)
        {
            UnitController CurrentUnitController = units[i].GetComponent<UnitController>();
            CurrentUnitController.SetEnable(false);
        }

        if (string.Compare(unitName, "") != 0 && unitsNames.Contains(unitName))
        {
            GameObject NextUnit = GameObject.Find(unitName);
            UnitController NextUnitController = NextUnit.GetComponent<UnitController>();
            NextUnitController.SetTextAllert(textAllert[MOV_ALLERT]);
            NextUnitController.SetEnable(true);

            textAllert[ADD_ALLERT].color = Color.blue;
            textAllert[MOV_ALLERT].text = "Now you can move the label " + unitName + "!"; ;
        }
    }

    public void DiasableUnitMovement()
    {
        if (string.Compare(unitName, "") != 0 && unitsNames.Contains(unitName))
        {
            GameObject unit = GameObject.Find(unitName);
            UnitController unitController = unit.GetComponent<UnitController>();
            unitController.SetEnable(false);
        }
    }

    /*
    public void SaveData()
    {
        string timeStamp = DateTime.Now.ToLongDateString();

        string fileName = string.Concat("./Assets/Repository/UnitsRepository_", timeStamp, ".txt");

        try
        {
            // Check if file already exists. If yes, delete it.     
            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }

            // Create a new file     
            using (StreamWriter sw = File.CreateText(fileName))
            {
                sw.WriteLine("# {0}", DateTime.Now.ToLongDateString());

                string labels = "";
                for (int i = 0; i < unitsNames.Count; i++)
                {
                    labels = string.Concat(labels, unitsNames[i]);
                }
                sw.WriteLine(labels);

                for (int i = 0; i < units.Count; i++)
                {
                    string line = string.Concat(units[i].name, ";", units[i].transform.parent.name, ";",
                        units[i].transform.position.x.ToString(), ";", units[i].transform.position.y.ToString(), ";",
                        units[i].transform.position.z.ToString());
                    sw.WriteLine(line);
                }
            }

            Debug.Log("Saved");

        }
        catch (Exception Ex)
        {
            Debug.Log(Ex.ToString());
        }
    }
    */

    public void SaveData()
    {
        string timeStamp = DateTime.Now.ToLongDateString();

        try
        {
            /*
            // Check if file already exists. If yes, delete it.     
            if (File.Exists(pathRepository))
            {
                File.Delete(pathRepository);
            }

            // Create a new file     
            using (StreamWriter sw = File.CreateText(pathPreviousSetup))
            {
                sw.WriteLine("# {0}", DateTime.Now.ToLongDateString());

                string labels = "";
                for (int i = 0; i < unitsNames.Count; i++)
                {
                    labels = string.Concat(labels, unitsNames[i]);
                }
                sw.WriteLine(labels);

                for (int i = 0; i < units.Count; i++)
                {
                    string line = string.Concat(units[i].name, ";", units[i].transform.parent.name, ";",
                        units[i].transform.position.x.ToString(), ";", units[i].transform.position.y.ToString(), ";",
                        units[i].transform.position.z.ToString());
                    sw.WriteLine(line);
                }
            }
            */

            if (File.Exists(pathPreviousSetup))
            {
                File.Delete(pathPreviousSetup);
            }

            // Create a new file     
            using (StreamWriter sw = File.CreateText(pathPreviousSetup))
            {
                sw.WriteLine("# {0}", DateTime.Now.ToLongDateString());

                string labels = "";
                for (int i = 0; i < unitsNames.Count; i++)
                {
                    labels = string.Concat(labels, unitsNames[i]);
                }
                sw.WriteLine(labels);

                for (int i = 0; i < units.Count; i++)
                {
                    string line = string.Concat(units[i].name, ";", units[i].transform.parent.name, ";",
                        units[i].transform.position.x.ToString(), ";", units[i].transform.position.y.ToString(), ";",
                        units[i].transform.position.z.ToString());
                    sw.WriteLine(line);
                }
            }

            Debug.Log("Saved");

        }
        catch (Exception Ex)
        {
            Debug.Log(Ex.ToString());
        }


        string[] ftpParametrs = File.ReadAllLines(ftpPath);

        try
        {
            // FTP
            // Get the object used to communicate with the server.
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(ftpParametrs[0]);
            request.Method = WebRequestMethods.Ftp.UploadFile;

            // This example assumes the FTP site uses anonymous logon.
            request.Credentials = new NetworkCredential(ftpParametrs[1], ftpParametrs[2]);
            /*
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(url);
            request.Method = WebRequestMethods.Ftp.UploadFile;

            request.Credentials = new NetworkCredential("mannstatistics", "passpass2");
            */
            byte[] fileContents;
            using (StreamReader sourceStream = new StreamReader(pathPreviousSetup))
            {
                fileContents = Encoding.UTF8.GetBytes(sourceStream.ReadToEnd());
            }

            request.ContentLength = fileContents.Length;

            using (Stream requestStream = request.GetRequestStream())
            {
                requestStream.Write(fileContents, 0, fileContents.Length);
            }

            using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
            {
            }

            popSavWithConn.SetActive(true);

        }
        catch (UriFormatException e)
        {
            Debug.Log(e.ToString());
            popUpURI.SetActive(true);

        }
        catch (WebException e)
        {
            Debug.Log("No connection");
            popUpConnectionPanel.SetActive(true);
            mainPanel.SetActive(false);
        }catch(Exception e)
        {
            Debug.Log("No connection");
            popUpConnectionPanel.SetActive(true);
            mainPanel.SetActive(false);
        }



    }
}
