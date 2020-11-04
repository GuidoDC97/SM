using System;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.AI;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using TMPro;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
#region attributes
    public static int STATIC_MODE = 1;
    public static int ANIMATION_MODE = 2;
    public static int HYBRID_MODE= 3;

    private int mode = -1;

    private DateTime date1;
    private DateTime date2;

    private string label = "";

    public const float INC_PATH = 0.0005f;
    public const float INC_USER = 0.0005f;

    private bool classFilter = true;
#endregion

    public GameObject labelPrefab;
    public GameObject userPrefab;

    public GameObject userDrawingPrefab;
    //public GameObject popupURI;

    //public string url = "ftp://files.000webhost.com/public_html/";

    public List<string> filePath;
    //private string fileSetupPath = "./Assets/Resources/SetupTecnico/UnitsRepository.txt";
    private string fileSetupPath;


    int dimFile = 0;
    int dimFileSetup = 0;

    string[] labels;
    string[] times;
    int[] hh;
    int[] mm;
    int[] ss;
    string[] dates;
    string[] hours;
    int[] classes;
    int hpermH;
    int mpermH;
    int hpermL;
    int mpermL;
    public Slider hHField;
    public Slider mHField;
    public Slider hLField;
    public Slider mLField;

    public Toggle permToggle;
    private int MIN_PERM = 0;
    private int MAX_PERM = 99;
    private string ftpPath;

    HashSet<char> floor0Labels = new HashSet<char>();
    HashSet<char> floor1Labels = new HashSet<char>();
    HashSet<char> floor2Labels = new HashSet<char>();
    HashSet<char> floorm1Labels = new HashSet<char>();
    HashSet<char> pathSet = new HashSet<char>();
    Dictionary<int, int> classesDictionary = new Dictionary<int, int>();
    Dictionary<int, Color> classColors = new Dictionary<int, Color>();
    Dictionary<string, DrawPath> pathDictionary = new Dictionary<string, DrawPath>();
    Dictionary<string, UserController> userDictionary = new Dictionary<string, UserController>();

    Dictionary<string, UserDrawing> userDrawingDictonary = new Dictionary<string, UserDrawing>();


    //public int numPaths = 1;
    //public int numUsers = 1;

    private const float MIN_SIZE= 0.01f;
    private const float MAX_SIZE = 0.4f;


    public int month = 1;

    private HashSet<char> labelsTec = new HashSet<char>();

    private string ltec;

    public float userSpeed = 1;

    public GameObject floorNameM1;
    public GameObject floorName0;
    public GameObject floorName1;
    public GameObject floorName2;

#region UI
    public GameObject popUpConnection;
    public GameObject popUpFile;
    public Toggle datesToggle;
    public Toggle d1Toggle;
    public Toggle d2Toggle;
    public TextMeshProUGUI d1;
    public TextMeshProUGUI d2;
    public TMP_Dropdown viewDropdown;
    public TMP_Dropdown modeDropdown;
    public Button startButton;
    public Button pauseButton;
    public Button stopButton;
    public TextMeshProUGUI labelAllert;

    public GameObject loadingScreen;
    public Slider slider;

    public GameObject speed;
    public Slider speedSlider;

    public GameObject popUpStatistics;
    public Button buttonStats;



    #endregion

    #region statistics
    int[] visitorsPerMonths = new int[12];
    //int[] classVisitors = new int[3];
    #endregion

    //0506
    private bool sampling = false;
    private static int GUARD = 5000;
     

    // Start is called before the first frame update
    void Start()
    {


        GameObject[] pointsm1 = GameObject.FindGameObjectsWithTag("PointsFloor-1");
        GameObject floorm1 = GameObject.Find("Floor-1");
        for(int i=0; i < pointsm1.Length; i++)
        {
            pointsm1[i].gameObject.transform.SetParent(floorm1.transform);
        }

        GameObject[] points0 = GameObject.FindGameObjectsWithTag("PointsFloor0");
        GameObject floor0 = GameObject.Find("Floor0");
        for (int i = 0; i < points0.Length; i++)
        {
            points0[i].gameObject.transform.SetParent(floor0.transform);
        }

        GameObject[] points1 = GameObject.FindGameObjectsWithTag("PointsFloor1");
        GameObject floor1 = GameObject.Find("Floor1");
        for (int i = 0; i < points1.Length; i++)
        {
            points1[i].gameObject.transform.SetParent(floor1.transform);
        }

        GameObject[] points2 = GameObject.FindGameObjectsWithTag("PointsFloor2");
        GameObject floor2 = GameObject.Find("Floor2");
        for (int i = 0; i < points2.Length; i++)
        {
            points2[i].gameObject.transform.SetParent(floor2.transform);
        }

        Debug.Log("Metodo Start");

        filePath = new List<string>();

        fileSetupPath = Path.Combine(Application.persistentDataPath, "Resources", "UnitsRepository.txt");
        //Debug.Log(fileSetupPath);
        ftpPath = Path.Combine(Application.persistentDataPath, "Resources", "FTPParameters.txt");

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



        filePath = FileManager.getPath();
        //filePath = PathHolder.GetPath();

        //SetPath(FileManager.getPath());
        if(LoadData(filePath, fileSetupPath))
        {
            
        }
        
            //GenerateStatistics();

            //CreatePaths(numPaths, date1, date2, label);
            //CreatePaths(numPaths, 04);
            //CreatePaths(numPaths, new DateTime(2019, 06, 11));
            //CreatePaths(numPaths, "[Q]");

            //CreateUsers(numUsers, date1, date2, label); 

            //CreateUsers2(numUsers, date1, date2, label);

            //CreateUsers(numUsers, 04);
            //CreateUsers(numUsers, new DateTime(2019, 06, 11));
            //CreateUsers(numUsers, "[Q]");
        
        
    }

    void Update()
    {
        if(viewDropdown.value != 0 && modeDropdown.value != 0 && !stopButton.interactable && viewDropdown.IsInteractable())
        {

            startButton.interactable = true;
        }
        else
        {
            startButton.interactable = false;

        }
    }

#region Getter&Setter

    public void SetSampling(bool s)
    {
        sampling = s;
    }

    public void SetMode(int m)
    {
        mode = m;

        if(mode == ANIMATION_MODE)
        {
            //Debug.Log("animata");
            speed.SetActive(true);
        } else
        {
            speed.SetActive(false);
            //Debug.Log("altro");

        }

        if (mode == ANIMATION_MODE)
        {
            viewDropdown.ClearOptions();
            viewDropdown.AddOptions(new List<string> { "Select perspective:", "Horizontal", "Oblique", "Vertical"});
        }
        else if(mode == STATIC_MODE || mode == HYBRID_MODE)
        {
            viewDropdown.ClearOptions();
            viewDropdown.AddOptions(new List<string> { "Select perspective:", "Horizontal", "Oblique" });
        }
    }

    public void SetDate1(string d1)
    {
        string[] date = d1.Split(new char[] { '/' });

        int day = int.Parse(date[0].Trim());
        int month = int.Parse(date[1].Trim());

        string[] year_hour = date[2].Split(new char[] { ':' });

        int year = int.Parse(year_hour[0]);
        int hour = int.Parse(year_hour[1]);

        DateTime d = new DateTime(year, month, day, hour, 0, 0);

        date1 = d;
    }

    public void SetDate2(string d2)
    {
        string[] date = d2.Split(new char[] { '/' });

        int day = int.Parse(date[0].Trim());
        int month = int.Parse(date[1].Trim());

        string[] year_hour = date[2].Split(new char[] { ':' });

        int year = int.Parse(year_hour[0]);
        int hour = int.Parse(year_hour[1]);

        DateTime d = new DateTime(year, month, day, hour, 0, 0);

        date2 = d;
    }

    public void SetLabel(string s)
    {
        //Debug.Log("set");
        s = s.ToUpper();

        bool check = true;
        for (int i = 0; i < s.Length && check; i++)
        {
            if (!labelsTec.Contains<char>(s[i]))
            {
                check = false;

            }
        }

        if (check)
        {
            label = "[" + s + "]";
            labelAllert.color = Color.blue;
            labelAllert.text = "";
        }
        else
        {
            label = "";
            labelAllert.color = Color.red;
            //labelAllert.text = "Attenzione, inserire una label valida!";
            labelAllert.text = "WARNING, insert a valid label!";
        }
        //Debug.Log("label: " + label);
    }

    public void SetClassFilter(bool c)
    {
        classFilter = c;
    }
    /*
    public void SetPath(string p)
    {
        filePath = p;
    }
    */

    public void SetUserSpeed(float s)
    {
        userSpeed = s;
    }
#endregion

    public bool LoadData(List<string> filePath, string fileSetupPath)
    {
        //caricamento dataset utente
        LoadDataset(filePath);

        //caricamento file setup
        try
        {
            //MOD
            if (Application.platform == RuntimePlatform.WebGLPlayer)
            {
                string url = "ftp://mannstatistics.000webhostapp.com/UnitsRepository.txt";

                WWW ftprequest = new WWW(url);

                Debug.Log(ftprequest.text);
            }
            //fine MOD
            else
            {
                LoadSetup(fileSetupPath);
            }
        }
        catch (UriFormatException e)
        {
            //popupURI.SetActive(true);

            if (File.Exists(fileSetupPath))
            {
                popUpConnection.GetComponentInChildren<TextMeshProUGUI>().SetText("Warning! Invalid URI format, the latest downloaded setup version will be loaded. Please check URI in Settings->Modify Repository.");
                popUpConnection.transform.Find("ButtonOkFile").gameObject.SetActive(true);
                popUpConnection.SetActive(true);
                LoadSetup();
            }
            else
            {
                popUpConnection.GetComponentInChildren<TextMeshProUGUI>().SetText("Warning! Invalid URI format, no setup file found.  Please check URI in Settings->Modify Repository.");
                popUpConnection.transform.Find("ButtonOkNoFile").gameObject.SetActive(true);
                popUpConnection.SetActive(true);
                return false;
            }

        }
        catch (WebException e)
        {
            if (File.Exists(fileSetupPath))
            {
                popUpConnection.GetComponentInChildren<TextMeshProUGUI>().SetText("Warning! Connection problems, the latest downloaded setup version will be loaded.");
                popUpConnection.transform.Find("ButtonOkFile").gameObject.SetActive(true);
                popUpConnection.SetActive(true);
                LoadSetup();
            }
            else
            {
                popUpConnection.GetComponentInChildren<TextMeshProUGUI>().SetText("Warning! Connection problems, no setup file found.");
                popUpConnection.transform.Find("ButtonOkNoFile").gameObject.SetActive(true);
                popUpConnection.SetActive(true);
                return false;
            }
        }
        catch (Exception) {
            if (File.Exists(fileSetupPath))
            {
                popUpConnection.GetComponentInChildren<TextMeshProUGUI>().SetText("Warning!! Connection problems, the latest downloaded setup version will be loaded.");
                popUpConnection.transform.Find("ButtonOkFile").gameObject.SetActive(true);
                popUpConnection.SetActive(true);
                LoadSetup();
            }
            else
            {
                popUpConnection.GetComponentInChildren<TextMeshProUGUI>().SetText("Warning!! Connection problems, no setup file found.");
                popUpConnection.transform.Find("ButtonOkNoFile").gameObject.SetActive(true);
                popUpConnection.SetActive(true);
                return false;
            }
        }

        //verifica sulla coerenza dei files
        bool okFiles = CheckLabels();
        if (!okFiles)
        {
            //Debug.Log("ERRORE FILES");
            popUpFile.SetActive(true);
            return false;
        }

        //Aggiornamento info su ogni centralina Attenzione, qua conta tutti gli utenti passati senza nessun filtro.
        //UpdateLabelsInfo();

        //Aggiornamento info per ogni piano.
        //UpdateFloorsInfo();

        return true;
    }

    //Carica il setup tecnico
    public void LoadSetup()
    {
        string[] lines = File.ReadAllLines(fileSetupPath);
        dimFileSetup = lines.Length;

        ltec = lines[1];
            
        for (int i = 0; i < lines[1].Length; i++)
        {
            labelsTec.Add(lines[1][i]);
        }

        for (int i = 2; i < dimFileSetup; i++)
        {
            string[] s = lines[i].Split(new char[] { ';' });

            string labelName = s[0].Trim();
            string floorName = s[1].Trim();
            float xCoord = float.Parse(s[2].Trim());
            float yCoord = float.Parse(s[3].Trim());
            float zCoord = float.Parse(s[4].Trim());

            switch (floorName)
            {
                case "Floor-1":
                    if (!floorm1Labels.Contains<char>(char.Parse(labelName)))
                    {
                        floorm1Labels.Add(char.Parse(labelName));
                    }
                    break;
                case "Floor0":
                    if (!floor0Labels.Contains<char>(char.Parse(labelName)))
                    {
                        floor0Labels.Add(char.Parse(labelName));
                    }
                    break;
                case "Floor1":
                    if (!floor1Labels.Contains<char>(char.Parse(labelName)))
                    {
                        floor1Labels.Add(char.Parse(labelName));
                    }
                    break;
                case "Floor2":
                    if (!floor2Labels.Contains<char>(char.Parse(labelName)))
                    {
                        floor2Labels.Add(char.Parse(labelName));
                    }
                    break;
            }

            if (labelPrefab)
            {
                GameObject controlUnit = Instantiate(labelPrefab, new Vector3(xCoord, yCoord, zCoord), Quaternion.identity);
                controlUnit.transform.name = labelName;
                controlUnit.transform.parent = GameObject.Find(floorName).transform;
            }
        }
    }

    //Carica il setup tecnico
    private void LoadSetup(string p)
    {
        string[] ftpParametrs = File.ReadAllLines(ftpPath);

        // FTP
        // Get the object used to communicate with the server.
        FtpWebRequest request = (FtpWebRequest)WebRequest.Create(ftpParametrs[0]);
        request.Method = WebRequestMethods.Ftp.DownloadFile;

        // This example assumes the FTP site uses anonymous logon.
        request.Credentials = new NetworkCredential(ftpParametrs[1], ftpParametrs[2]);

        FtpWebResponse response = (FtpWebResponse)request.GetResponse();

        Stream responseStream = response.GetResponseStream();
        StreamReader reader = new StreamReader(responseStream);
        //Console.WriteLine(reader.ReadToEnd());

        //Console.WriteLine($"Download Complete, status {response.StatusDescription}");

        // END FTP

        using (StreamWriter offlineWriter = File.CreateText(fileSetupPath))
        {
            string info = reader.ReadLine();
            offlineWriter.WriteLine(info);

            ltec = reader.ReadLine();
            //Debug.Log("first" + ltec);
            offlineWriter.WriteLine(ltec);

            for (int i = 0; i < ltec.Length; i++)
            {
                labelsTec.Add(ltec[i]);
            }

            string l = "";
            while ((l = reader.ReadLine()) != null)
            {
                offlineWriter.WriteLine(l);

                string[] s = l.Split(new char[] { ';' });

                string labelName = s[0].Trim();
                // Debug.Log(labelName);
                string floorName = s[1].Trim();
                // Debug.Log(floorName);
                float xCoord = float.Parse(s[2].Trim());
                // Debug.Log(xCoord);
                float yCoord = float.Parse(s[3].Trim());
                //Debug.Log(yCoord);
                float zCoord = float.Parse(s[4].Trim());
                //Debug.Log(zCoord);

                /*
                if (!floor0Labels.Contains<char>(char.Parse(labelName)))
                {
                    floor0Labels.add
                }
                */

                switch (floorName)
                {
                    case "Floor-1":
                        if (!floorm1Labels.Contains<char>(char.Parse(labelName)))
                        {
                            floorm1Labels.Add(char.Parse(labelName));
                        }
                        break;
                    case "Floor0":
                        if (!floor0Labels.Contains<char>(char.Parse(labelName)))
                        {
                            floor0Labels.Add(char.Parse(labelName));
                        }
                        break;
                    case "Floor1":
                        if (!floor1Labels.Contains<char>(char.Parse(labelName)))
                        {
                            floor1Labels.Add(char.Parse(labelName));
                        }
                        break;
                    case "Floor2":
                        if (!floor2Labels.Contains<char>(char.Parse(labelName)))
                        {
                            floor2Labels.Add(char.Parse(labelName));
                        }
                        break;
                }

                if (labelPrefab)
                {
                    GameObject controlUnit = Instantiate(labelPrefab, new Vector3(xCoord, yCoord, zCoord), Quaternion.identity);
                    controlUnit.transform.name = labelName;
                    controlUnit.transform.parent = GameObject.Find(floorName).transform;
                }
            }
        }



        responseStream.Close();
        reader.Close();
        response.Close();
    }

   

    //Carica il dataset utente
    public void LoadDataset(List<string> p)
    {
        loadingScreen.SetActive(true);
        float progress = 0;
        slider.value = 0;
        
        int dim = 0;
        int dim_riciclo = 0;

        for (int index = 0; index < p.Count; index++)
        {
            string[] lines = File.ReadAllLines(p[index]);
            dimFile += lines.Length; //del vettore di file
        }

        labels = new string[dimFile];
        times = new string[dimFile];
        hh = new int[dimFile];
        mm = new int[dimFile];
        ss = new int[dimFile];
        dates = new string[dimFile];
        hours = new string[dimFile];
        classes = new int[dimFile];

        for (int index = 0; index < p.Count; index++)
        {

            string[] lines = File.ReadAllLines(p[index]);
            dim = lines.Length; //del singolo file

            // test tutti gli utenti
            // SetNumPaths(dimFile);

            int k = 0;
            for (int i = dim_riciclo; i < dim + dim_riciclo; i++)
            {

                progress = progress + 1 / (float) dimFile;
                Mathf.Clamp01(progress);
                //Debug.Log(progress);
                slider.value = progress;

                string[] s = lines[k].Split(new char[] { ',' });
                k++;

                labels[i] = s[0].Trim();  //trim leva gli spazi a dx e sx
                times[i] = s[1].Trim();

                string[] hhmm = s[1].Split(':');
                hh[i] = int.Parse(hhmm[0]);
                mm[i] = int.Parse(hhmm[1]);
                ss[i] = int.Parse(hhmm[2]);

                string[] t = s[2].Split(new char[] { ' ' });
                dates[i] = t[0].Trim();
                hours[i] = t[1].Trim();

                if (classFilter)
                {
                    if (s.Length > 3)
                    {
                        int cl = Int32.Parse(s[3]);
                        classes[i] = cl;
                        if (!classesDictionary.ContainsKey(cl))
                        {
                            classesDictionary.Add(cl, 1);
                        }
                        else
                        {
                            classesDictionary[cl] = classesDictionary[cl] + 1;
                        }

                    }
                    else
                    {
                        classFilter = false;
                    }

                }

                string[] date = dates[i].Split(new char[] { '/' });
                int month = int.Parse(date[1]);
                visitorsPerMonths[month - 1] = visitorsPerMonths[month - 1] + 1;

                //devo far ripartire il ciclo interno dalla dim dell'ultimo file

            }
            dim_riciclo += dim;
        }

        if (classFilter)
        {
            foreach (KeyValuePair<int, int> entry in classesDictionary)
            {
                classColors.Add(entry.Key, new Color(Random.Range(1.0f, 0.0f), Random.Range(1.0f, 0.0f), Random.Range(1.0f, 0.0f), 1.0f));
            }
        }

        loadingScreen.SetActive(false);
    }

    //verifica presenza di tutte le centraline nel file caricato dall'utente.
    private bool CheckLabels()
    {
        bool check = true;
        for (int i = 0; i < dimFile && check; i++)
        {
            for (int j = 0; j < labels[i].Length && check; j++)
            {
                if (!labelsTec.Contains<char>(labels[i][j]))
                {
                    check = false;
                }
            }

        }

        return check;
    }

    //Aggiorna info pop-up sulle centraline
    private void UpdateLabelsInfo()
    {
        foreach (char c in labelsTec)
        {
            GameObject lab = GameObject.Find(c.ToString());
            LabelsScript label = lab.GetComponent<LabelsScript>();
            for (int i = 0; i < dimFile; i++)
            {
                if (labels[i].Contains(label.name))
                {
                    label.AddVisitor();
                }

            }
        }

    }

    private void UpdateLabelsInfo(string path)
    {
        foreach (char c in labelsTec)
        {
            GameObject lab = GameObject.Find(c.ToString());
            LabelsScript label = lab.GetComponent<LabelsScript>();

            if (path.Contains(label.name))
            {
                label.AddVisitor();
            }

        }

    }

    public void ResetInfo()
    {
        foreach (char c in labelsTec)
        {
            GameObject lab = GameObject.Find(c.ToString());
            LabelsScript label = lab.GetComponent<LabelsScript>();

            label.ResetVisitors();
        }

        floorName0.GetComponent<FloorDataSctipt>().ResetVisitors();
        floorName1.GetComponent<FloorDataSctipt>().ResetVisitors();
        floorName2.GetComponent<FloorDataSctipt>().ResetVisitors();
        floorNameM1.GetComponent<FloorDataSctipt>().ResetVisitors();
    }

    //Aggiorna info pop-up sui piani
    private void UpdateFloorsInfo()
    {

        for (int i = 0; i < labels.Length; i++)
        {
            bool checkm1 = false;
            bool check0 = false;
            bool check1 = false;
            bool check2 = false;

            for (int j = 0; j < labels[i].Length; j++)
            {
                if (floorm1Labels.Contains<char>(labels[i][j]))
                {
                    if (!checkm1)
                    {
                        floorNameM1.GetComponent<FloorDataSctipt>().addVisitor();
                        checkm1 = true;
                    }

                }
                else if (floor0Labels.Contains<char>(labels[i][j]))
                {
                    if (!check0)
                    {
                        floorName0.GetComponent<FloorDataSctipt>().addVisitor();
                        check0 = true;
                    }

                }
                else if (floor1Labels.Contains<char>(labels[i][j]))
                {
                    if (!check1)
                    {
                        floorName1.GetComponent<FloorDataSctipt>().addVisitor();
                        check1 = true;
                    }

                }
                else
                {
                    if (!check2)
                    {
                        floorName2.GetComponent<FloorDataSctipt>().addVisitor();
                        check2 = true;
                    }
                }
            }
        }

        floorNameM1.GetComponent<FloorDataSctipt>().SetFloorInfo();
        floorName0.GetComponent<FloorDataSctipt>().SetFloorInfo();
        floorName1.GetComponent<FloorDataSctipt>().SetFloorInfo();
        floorName2.GetComponent<FloorDataSctipt>().SetFloorInfo();
    }

    private void UpdateFloorsInfo(string path)
    {
            bool checkm1 = false;
            bool check0 = false;
            bool check1 = false;
            bool check2 = false;

            for (int j = 0; j < path.Length; j++)
            {
                if (floorm1Labels.Contains<char>(path[j]))
                {
                    if (!checkm1)
                    {
                        floorNameM1.GetComponent<FloorDataSctipt>().addVisitor();
                        checkm1 = true;
                    }

                }
                else if (floor0Labels.Contains<char>(path[j]))
                {
                    if (!check0)
                    {
                        floorName0.GetComponent<FloorDataSctipt>().addVisitor();
                        check0 = true;
                    }

                }
                else if (floor1Labels.Contains<char>(path[j]))
                {
                    if (!check1)
                    {
                        floorName1.GetComponent<FloorDataSctipt>().addVisitor();
                        check1 = true;
                    }

                }
                else
                {
                    if (!check2)
                    {
                        floorName2.GetComponent<FloorDataSctipt>().addVisitor();
                        check2 = true;
                    }
                }
            }
    }

#region visualization

    public void Visualize()
    {
        //Debug.Log("toggle date: " + datesToggle.isOn.ToString());
        if (datesToggle.isOn)
        {
            if (d1Toggle.isOn)
            {
                SetDate1(d1.text);
                //Debug.Log("d1: " + date1.ToString());

            }
            else
            {
                date1 = new DateTime(1900, 01, 01);

            }

            if (d2Toggle.isOn)
            {
                SetDate2(d2.text);
                //Debug.Log("d2 " + date2.ToString());

            }
            else
            {
                date2 = new DateTime(2100, 12, 31);
            }

        }
        else
        {
            date1 = new DateTime(1900, 01, 01);
            date2 = new DateTime(2100, 12, 31);
        }

        //Debug.Log("date1: " + date1.ToString());
        //Debug.Log("date2: " + date2.ToString());

        if (permToggle.isOn)
        {
            hpermH = (int)hHField.value;
            mpermH = (int)mHField.value;
            hpermL = (int)hLField.value;
            mpermL = (int)mLField.value;

            if (hpermH == 0)
            {
                hpermH = MAX_PERM;
            }
        }
        else
        {
            hpermH = MAX_PERM;
            mpermH = MAX_PERM;
            hpermL = MIN_PERM;
            mpermL = MIN_PERM;
        }

        //Debug.Log(hpermH);
        //Debug.Log(mpermH);
        //Debug.Log(hpermL);
        //Debug.Log(mpermL);


        if (string.Compare(label, "") == 0 || string.Compare(label, "[]") == 0)
        {
            label = "[" + ltec + "]";
        }

        if (mode == STATIC_MODE)
        {
            CreatePaths();
        }
        else if (mode == ANIMATION_MODE)
        {
            pauseButton.interactable = true;
            CreateUsers();
        } else if(mode == HYBRID_MODE)
        {
            pauseButton.interactable = true;
            CreateUsersDrawing();
        }

        buttonStats.interactable = true;

    }



    //Creazione percorsi statici con archi
    public void CreatePaths()
    {
        ResetInfo();

        //Debug.Log(label);
        loadingScreen.SetActive(true);
        float progress = 0;
        slider.value = 0;

        for (int i = 0; i < dimFile; i++)
        {
            progress = progress + 1 / (float)dimFile;
            Mathf.Clamp01(progress);
            slider.value = progress;

            string path = labels[i];

            Match match = Regex.Match(path, label);

            if (match.Success)
            { 
                string[] date = dates[i].Split(new char[] { '/' });

                int day = int.Parse(date[0]);
                int month = int.Parse(date[1]);
                int year = 2000 + int.Parse(date[2]);

                DateTime d = new DateTime(year, month, day);

                if ((DateTime.Compare(d, date1) >= 0) && (DateTime.Compare(d, date2) <= 0))
                {
                    int totUser = hh[i] * 60 + mm[i];

                    int totMax = hpermH * 60 + mpermH;
                    int totMin = hpermL * 60 + mpermL;

                    if ((totMin <= totUser && totUser <= totMax))
                    {
                        UpdateFloorsInfo(path);
                        UpdateLabelsInfo(path);

                        if (!pathDictionary.ContainsKey(path))
                        {
                            Color randomColor = new Color(Random.Range(1.0f, 0.0f), Random.Range(1.0f, 0.0f), Random.Range(1.0f, 0.0f), 1.0f);

                            if (classFilter)
                            {
                                //Debug.Log("Filtro per classe");
                                //Debug.Log("classe: " + classes[i].ToString());

                                randomColor = classColors[classes[i]];
                            }

                            if (sampling && (i % (int)(dimFile * 20 / 100)) == 0)
                            {
                                GameObject mainPathRenderer = new GameObject();

                                mainPathRenderer.name = string.Concat("Path", i.ToString());
                                mainPathRenderer.tag = "Paths";


                                mainPathRenderer.AddComponent<DrawPath>();

                                DrawPath drawPath = mainPathRenderer.GetComponent<DrawPath>();
                                drawPath.SetColor(randomColor);
                                drawPath.SetIndex(i);
                                drawPath.SetPath(path);

                                pathDictionary.Add(path, drawPath);
                            } else if (!sampling)
                            {
                                GameObject mainPathRenderer = new GameObject();

                                mainPathRenderer.name = string.Concat("Path", i.ToString());
                                mainPathRenderer.tag = "Paths";


                                mainPathRenderer.AddComponent<DrawPath>();

                                DrawPath drawPath = mainPathRenderer.GetComponent<DrawPath>();
                                drawPath.SetColor(randomColor);
                                drawPath.SetIndex(i);
                                drawPath.SetPath(path);

                                pathDictionary.Add(path, drawPath);
                            }
                        }
                        else
                        {
                            DrawPath outDrawPath;
                            pathDictionary.TryGetValue(path, out outDrawPath);
                            float width = outDrawPath.GetWidth() + INC_PATH;
                            outDrawPath.SetWidth(width);
                        }
                    }
                }
            }
        }

        floorNameM1.GetComponent<FloorDataSctipt>().SetFloorInfo();
        floorName0.GetComponent<FloorDataSctipt>().SetFloorInfo();
        floorName1.GetComponent<FloorDataSctipt>().SetFloorInfo();
        floorName2.GetComponent<FloorDataSctipt>().SetFloorInfo();

        foreach (DrawPath p in pathDictionary.Values)
        {
            p.Draw();
        }

        loadingScreen.SetActive(false);
    }

    //Creazione percorsi dinamici con Agenti
    void CreateUsers()
    {
        ResetInfo();

        loadingScreen.SetActive(true);
        float progress = 0;
        slider.value = 0;

        for (int i = 0; i < dimFile; i++)
        {
            string path = labels[i];

            Match match = Regex.Match(path, label);

            if (match.Success)
            {
                string[] date = dates[i].Split(new char[] { '/' });

                int hour = int.Parse(hours[i].Split(new char[] { ':' })[0]);    //corretto
                int day = int.Parse(date[0]);
                int month = int.Parse(date[1]);
                int year = 2000 + int.Parse(date[2]);

                DateTime d = new DateTime(year, month, day, hour, 0, 0);

                //Debug.Log("data: " + d.ToString());


                if ((DateTime.Compare(d, date1) >= 0) && (DateTime.Compare(d, date2) <= 0))
                {
                    //Debug.Log("ci sono anch io");

                    //Debug.Log(hh[i]);
                    //Debug.Log(mm[i]);

                    int totUser = hh[i] * 60 + mm[i];
                    int totMax = hpermH * 60 + mpermH;
                    int totMin = hpermL * 60 + mpermL;

                    if ((totMin <= totUser && totUser <= totMax))
                    {
                        UpdateFloorsInfo(path);
                        UpdateLabelsInfo(path);

                        if (!userDictionary.ContainsKey(path))
                        {
                            float speed = userSpeed;

                            Color randomColor = new Color(Random.Range(1.0f, 0.0f), Random.Range(1.0f, 0.0f), Random.Range(1.0f, 0.0f), 0.5f);

                            if (classFilter)
                            {
                                //Debug.Log("Filtro per classe");
                                //Debug.Log("classe: " + classes[i].ToString());

                                randomColor = classColors[classes[i]];
                            }

                            if (sampling && (i % (int)(dimFile * 20 / 100)) == 0)
                            {
                                GameObject user = Instantiate(userPrefab);

                                user.name = string.Concat("User", i.ToString());

                                GameObject firstLabel = GameObject.Find(path[0].ToString());

                                user.transform.position = new Vector3(firstLabel.transform.position.x, 0.1f, firstLabel.transform.position.z);
                                //user.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
                                user.layer = 11;
                                user.tag = "Users";

                                Renderer usersRenderer = user.GetComponent<Renderer>();
                                usersRenderer.material = new Material(Shader.Find("Standard"));
                                usersRenderer.material.color = randomColor;

                                user.AddComponent<NavMeshAgent>();
                                NavMeshAgent agent = user.GetComponent<NavMeshAgent>();

                                //user.AddComponent<UserController>();

                                UserController usersController = user.GetComponent<UserController>();
                                usersController.SetSlider(speedSlider);
                                usersController.SetPath(path);
                                usersController.SetSpeed(speed);
                                usersController.SetAgent(agent);
                                usersController.SetIndex(i);
                                usersController.SetUser(user);

                                userDictionary.Add(path, usersController);
                            }
                            else if (!sampling)
                            {
                                GameObject user = Instantiate(userPrefab);

                                user.name = string.Concat("User", i.ToString());

                                GameObject firstLabel = GameObject.Find(path[0].ToString());

                                user.transform.position = new Vector3(firstLabel.transform.position.x, 0.1f, firstLabel.transform.position.z);
                                //user.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
                                user.layer = 11;
                                user.tag = "Users";

                                Renderer usersRenderer = user.GetComponent<Renderer>();
                                usersRenderer.material = new Material(Shader.Find("Standard"));
                                usersRenderer.material.color = randomColor;

                                user.AddComponent<NavMeshAgent>();
                                NavMeshAgent agent = user.GetComponent<NavMeshAgent>();

                                //user.AddComponent<UserController>();

                                UserController usersController = user.GetComponent<UserController>();
                                usersController.SetSlider(speedSlider);
                                usersController.SetPath(path);
                                usersController.SetSpeed(speed);
                                usersController.SetAgent(agent);
                                usersController.SetIndex(i);
                                usersController.SetUser(user);

                                userDictionary.Add(path, usersController);
                            }
                        }
                        else
                        {
                            UserController outUserController;
                            userDictionary.TryGetValue(path, out outUserController);

                            Vector3 scale = outUserController.transform.localScale + new Vector3(INC_USER, INC_USER, INC_USER);

                            outUserController.SetScale(new Vector3(Mathf.Clamp(scale.x, MIN_SIZE, MAX_SIZE),
                                Mathf.Clamp(scale.y, MIN_SIZE, MAX_SIZE), Mathf.Clamp(scale.z, MIN_SIZE, MAX_SIZE)));


                            //outUserController.SetRadius(Mathf.Clamp(outUserController.GetRadius() + INC_USER, MIN_SIZE, MAX_SIZE));
                            //outUserController.transform.gameObject.GetComponent<SphereCollider>().radius = Mathf.Clamp(outUserController.GetRadius() + INC_USER, MIN_SIZE, MAX_SIZE);
                            outUserController.incCont();
                            outUserController.IncPriority();
                        }
                    }
                }
            }

            progress = progress + 1 / (float)dimFile;
            Mathf.Clamp01(progress);
            slider.value = progress;
        }

        floorNameM1.GetComponent<FloorDataSctipt>().SetFloorInfo();
        floorName0.GetComponent<FloorDataSctipt>().SetFloorInfo();
        floorName1.GetComponent<FloorDataSctipt>().SetFloorInfo();
        floorName2.GetComponent<FloorDataSctipt>().SetFloorInfo();

        loadingScreen.SetActive(false);
    }

    public void StopVisualize()
    {
        GameObject[] gameObjects = null;

        if (mode == STATIC_MODE)
        {
            gameObjects = GameObject.FindGameObjectsWithTag("Paths");
            pathDictionary.Clear();

            for (int i = 0; i < gameObjects.Length; i++)
            {
                GameObject.Destroy(gameObjects[i]);
            }
        }
        else if (mode == ANIMATION_MODE)
        {
            //Debug.Log("stop animata");
            gameObjects = GameObject.FindGameObjectsWithTag("Users");
            userDictionary.Clear();

            for (int i = 0; i < gameObjects.Length; i++)
            {
                GameObject.Destroy(gameObjects[i]);
            }
        } else if(mode == HYBRID_MODE)
        {
            gameObjects = GameObject.FindGameObjectsWithTag("UsersDrawing");
            userDrawingDictonary.Clear();

            for (int i = 0; i < gameObjects.Length; i++)
            {
                GameObject.Destroy(gameObjects[i]);
            }

            gameObjects = GameObject.FindGameObjectsWithTag("Paths");
            //pathDictionary.Clear();   NON necessaria

            for (int i = 0; i < gameObjects.Length; i++)
            {
                GameObject.Destroy(gameObjects[i]);
            }
        }
    }

    public void PauseVisualize()
    {
        GameObject[] gameObjects = null;

        if (mode == ANIMATION_MODE)
        {
            //Debug.Log("pausa animata");
            gameObjects = GameObject.FindGameObjectsWithTag("Users");

            for (int i = 0; i < gameObjects.Length; i++)
            {
                UserController controller = gameObjects[i].GetComponent<UserController>();
                controller.enabled = false;

                NavMeshAgent agent = gameObjects[i].GetComponent<NavMeshAgent>();
                agent.enabled = false;
            }
        } else if(mode == HYBRID_MODE)
        {
            gameObjects = GameObject.FindGameObjectsWithTag("UsersDrawing");

            Debug.Log("pausa ibrida");

            for (int i = 0; i < gameObjects.Length; i++)
            {
                UserDrawing controller = gameObjects[i].GetComponent<UserDrawing>();
                controller.enabled = false;

                NavMeshAgent agent = gameObjects[i].GetComponent<NavMeshAgent>();
                agent.enabled = false;
            }
        }


    }

    public void ResumeVisualize()
    {
        GameObject[] gameObjects = null;

        if (mode == ANIMATION_MODE)
        {
            //Debug.Log("riprendi animata");
            gameObjects = GameObject.FindGameObjectsWithTag("Users");

            for (int i = 0; i < gameObjects.Length; i++)
            {
                NavMeshAgent agent = gameObjects[i].GetComponent<NavMeshAgent>();
                UserController controller = gameObjects[i].GetComponent<UserController>();

                if (!controller.changing)
                {
                    agent.enabled = true;
                }
                controller.enabled = true;
            }
        }  else if(mode == HYBRID_MODE)
        {
            Debug.Log("riprendi ibrida");
            gameObjects = GameObject.FindGameObjectsWithTag("UsersDrawing");

            for (int i = 0; i < gameObjects.Length; i++)
            {
                NavMeshAgent agent = gameObjects[i].GetComponent<NavMeshAgent>();
                UserDrawing controller = gameObjects[i].GetComponent<UserDrawing>();

                if (!controller.changing)
                {
                    agent.enabled = true;
                }
                controller.enabled = true;
            }
        }
    }

#endregion

    // Generazione statistiche
    public void GenerateStatistics()
    {
        string filePath = Path.Combine(Application.persistentDataPath, "Resources", "Statistics.txt");

        try
        {
            // Check if file already exists. If yes, delete it.     
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            // Create a new file     
            using (StreamWriter sw = File.CreateText(filePath))
            {
                sw.WriteLine("# {0}", DateTime.Now.ToLongDateString());

                sw.WriteLine("Total Visitors: {0}", dimFile);
                sw.WriteLine("Labels: {0}", labels);

                sw.WriteLine("Visitors per month:");
                sw.WriteLine("\tJanuary: {0}", visitorsPerMonths[0]);
                sw.WriteLine("\tFebruary: {0}", visitorsPerMonths[1]);
                sw.WriteLine("\tMarch: {0}", visitorsPerMonths[2]);
                sw.WriteLine("\tApril: {0}", visitorsPerMonths[3]);
                sw.WriteLine("\tMay: {0}", visitorsPerMonths[4]);
                sw.WriteLine("\tJune: {0}", visitorsPerMonths[5]);
                sw.WriteLine("\tJuly: {0}", visitorsPerMonths[6]);
                sw.WriteLine("\tAugust: {0}", visitorsPerMonths[7]);
                sw.WriteLine("\tSeptember: {0}", visitorsPerMonths[8]);
                sw.WriteLine("\tOctober: {0}", visitorsPerMonths[9]);
                sw.WriteLine("\tNovember: {0}", visitorsPerMonths[10]);
                sw.WriteLine("\tDicember: {0}", visitorsPerMonths[11]);

                sw.WriteLine();
                sw.WriteLine("Visitors per floor:");
                sw.WriteLine("\tFloor -1: {0}", floorNameM1.GetComponent<FloorDataSctipt>().getVisitors());
                sw.WriteLine("\tFloor 0: {0}", floorName0.GetComponent<FloorDataSctipt>().getVisitors());
                sw.WriteLine("\tFloor 1: {0}", floorName1.GetComponent<FloorDataSctipt>().getVisitors());
                sw.WriteLine("\tFloor 2: {0}", floorName2.GetComponent<FloorDataSctipt>().getVisitors());

                sw.WriteLine();
                sw.WriteLine("Visitors per label:");
                foreach (char c in labelsTec)
                {
                    GameObject lab = GameObject.Find(c.ToString());
                    LabelsScript label = lab.GetComponent<LabelsScript>();
                    sw.WriteLine("\t{0}: {1}", c, label.GetVisitors());
                }

                if (classFilter)
                {
                    sw.WriteLine();
                    sw.WriteLine("Visitors per class:");
                    foreach (KeyValuePair<int, int> entry in classesDictionary)
                    {
                        sw.WriteLine("\tClass " + entry.Key + ": " + entry.Value);
                    }
                }

                sw.WriteLine();
                sw.WriteLine("Time statistics:");
                int maxPermH = hh[0];
                int maxPermM = mm[0];
                int maxPermS = ss[0];
                int minPermH = hh[0];
                int minPermM = mm[0];
                int minPermS = ss[0];
                double medPermH = 0;
                double medPermM = 0;
                double medPermS = 0;
                for (int i = 1; i < dimFile; i++)
                {
                    if (hh[i] > maxPermH && mm[i] > maxPermM && ss[i] > maxPermS)
                    {
                        maxPermH = hh[i];
                        maxPermM = mm[i];
                        maxPermS = ss[i];
                    }
                    else if (hh[i] < minPermH && mm[i] < minPermM && ss[i] < minPermS)
                    {
                        minPermH = hh[i];
                        minPermM = mm[i];
                        minPermS = ss[i];
                    }
                    medPermH += hh[i];
                    medPermM += mm[i];
                    medPermS += ss[i];
                }
                medPermH = medPermH / dimFile;
                medPermM = medPermM / dimFile;
                medPermS = medPermS / dimFile;

                sw.WriteLine("\tMax Time: " + (int)maxPermH + ":" + (int)maxPermM + ":" + (int)maxPermS);
                sw.WriteLine("\tMin Time: " + (int)minPermH + ":" + (int)minPermM + ":" + (int)minPermS);
                sw.WriteLine("\tAvg Time: " + (int)medPermH + ":" + (int)medPermM + ":" + (int)medPermS);
            }

        }
        catch (Exception Ex)
        {
            Debug.Log(Ex.ToString());
        }

        //chiamo popUp
        popUpStatistics.SetActive(true);
    }


    void CreateUsersDrawing()
    {
        ResetInfo();

        loadingScreen.SetActive(true);
        float progress = 0;
        slider.value = 0;

        for (int i = 0; i < dimFile ; i++)
        {
            string path = labels[i];

            Match match = Regex.Match(path, label);

            if (match.Success)
            {
                string[] date = dates[i].Split(new char[] { '/' });

                int hour = int.Parse(hours[i].Split(new char[] { ':' })[0]);    //corretto
                int day = int.Parse(date[0]);
                int month = int.Parse(date[1]);
                int year = 2000 + int.Parse(date[2]);

                DateTime d = new DateTime(year, month, day, hour, 0, 0);

                //Debug.Log("data: " + d.ToString());


                if ((DateTime.Compare(d, date1) >= 0) && (DateTime.Compare(d, date2) <= 0))
                {
                    //Debug.Log(hh[i]);
                    //Debug.Log(mm[i]);
                     

                    int totUser = hh[i] * 60 + mm[i];

                    int totMax = hpermH * 60 + mpermH;
                    int totMin = hpermL * 60 + mpermL;

                    if ((totMin <= totUser && totUser <= totMax))
                    {
                        //Debug.Log("ci sono anch io");

                        UpdateFloorsInfo(path);
                        UpdateLabelsInfo(path);

                        if (!userDrawingDictonary.ContainsKey(path))
                        { 

                            float speed = userSpeed;

                            Color randomColor = new Color(Random.Range(1.0f, 0.0f), Random.Range(1.0f, 0.0f), Random.Range(1.0f, 0.0f), 0.5f);

                            if (classFilter)
                            {
                                //Debug.Log("Filtro per classe");
                                //Debug.Log("classe: " + classes[i].ToString());

                                randomColor = classColors[classes[i]];
                            }

                            /*
                            GameObject user = Instantiate(userDrawingPrefab);

                            user.name = string.Concat("User", i.ToString());

                            GameObject firstLabel = GameObject.Find(path[0].ToString());

                            user.transform.position = new Vector3(firstLabel.transform.position.x, 0.1f, firstLabel.transform.position.z);
                            // modificare il layer (non visibile)
                            //user.layer = 11;

                            Renderer usersRenderer = user.GetComponent<Renderer>();
                            usersRenderer.material = new Material(Shader.Find("Standard"));
                            usersRenderer.material.color = randomColor;

                            user.AddComponent<NavMeshAgent>();
                            NavMeshAgent agent = user.GetComponent<NavMeshAgent>();

                            //user.AddComponent<UserDrawing>();


                            UserDrawing usersDrawing = user.GetComponent<UserDrawing>();
                            usersDrawing.SetSlider(speedSlider);
                            usersDrawing.SetPath(path);
                            usersDrawing.SetSpeed(speed);
                            usersDrawing.SetAgent(agent);
                            usersDrawing.SetIndex(i);
                            usersDrawing.SetUser(user);

                            userDrawingDictonary.Add(path, usersDrawing);
                            */

                            if (sampling && (i % (int)(dimFile*20/100)) == 0)
                            {
                                GameObject user = Instantiate(userDrawingPrefab);

                                user.name = string.Concat("User", i.ToString());

                                GameObject firstLabel = GameObject.Find(path[0].ToString());

                                user.transform.position = new Vector3(firstLabel.transform.position.x, 0.1f, firstLabel.transform.position.z);
                                // modificare il layer (non visibile)
                                //user.layer = 11;

                                Renderer usersRenderer = user.GetComponent<Renderer>();
                                usersRenderer.material = new Material(Shader.Find("Standard"));
                                usersRenderer.material.color = randomColor;

                                user.AddComponent<NavMeshAgent>();
                                NavMeshAgent agent = user.GetComponent<NavMeshAgent>();

                                //user.AddComponent<UserDrawing>();


                                UserDrawing usersDrawing = user.GetComponent<UserDrawing>();
                                usersDrawing.SetSlider(speedSlider);
                                usersDrawing.SetPath(path);
                                usersDrawing.SetSpeed(speed);
                                usersDrawing.SetAgent(agent);
                                usersDrawing.SetIndex(i);
                                usersDrawing.SetUser(user);

                                userDrawingDictonary.Add(path, usersDrawing);
                            } else if (!sampling)
                            {
                                GameObject user = Instantiate(userDrawingPrefab);

                                user.name = string.Concat("User", i.ToString());

                                GameObject firstLabel = GameObject.Find(path[0].ToString());

                                user.transform.position = new Vector3(firstLabel.transform.position.x, 0.1f, firstLabel.transform.position.z);
                                // modificare il layer (non visibile)
                                //user.layer = 11;

                                Renderer usersRenderer = user.GetComponent<Renderer>();
                                usersRenderer.material = new Material(Shader.Find("Standard"));
                                usersRenderer.material.color = randomColor;

                                user.AddComponent<NavMeshAgent>();
                                NavMeshAgent agent = user.GetComponent<NavMeshAgent>();

                                //user.AddComponent<UserDrawing>();


                                UserDrawing usersDrawing = user.GetComponent<UserDrawing>();
                                usersDrawing.SetSlider(speedSlider);
                                usersDrawing.SetPath(path);
                                usersDrawing.SetSpeed(speed);
                                usersDrawing.SetAgent(agent);
                                usersDrawing.SetIndex(i);
                                usersDrawing.SetUser(user);

                                userDrawingDictonary.Add(path, usersDrawing);
                            }
                        }
                        else
                        {
                            UserDrawing outUser;
                            userDrawingDictonary.TryGetValue(path, out outUser);

                            //Vector3 scale = outUser.transform.localScale + new Vector3(INC_USER, INC_USER, INC_USER);

                            //outUser.SetScale(new Vector3(Mathf.Clamp(scale.x, MIN_SIZE, MAX_SIZE),
                            //Mathf.Clamp(scale.y, MIN_SIZE, MAX_SIZE), Mathf.Clamp(scale.z, MIN_SIZE, MAX_SIZE)));


                            //outUserController.SetRadius(Mathf.Clamp(outUserController.GetRadius() + INC_USER, MIN_SIZE, MAX_SIZE));
                            //outUserController.transform.gameObject.GetComponent<SphereCollider>().radius = Mathf.Clamp(outUserController.GetRadius() + INC_USER, MIN_SIZE, MAX_SIZE);
                            outUser.incCont();
                            //outUserController.IncPriority();
                        }
                    }

                }
            }

            progress = progress + 1 / (float)dimFile;
            Mathf.Clamp01(progress);
            slider.value = progress;
        }

        floorNameM1.GetComponent<FloorDataSctipt>().SetFloorInfo();
        floorName0.GetComponent<FloorDataSctipt>().SetFloorInfo();
        floorName1.GetComponent<FloorDataSctipt>().SetFloorInfo();
        floorName2.GetComponent<FloorDataSctipt>().SetFloorInfo();

        loadingScreen.SetActive(false);
    }

}