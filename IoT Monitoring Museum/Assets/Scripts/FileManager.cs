using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SimpleFileBrowser;
#if UNITY_EDITOR
using UnityEditor;
#endif

using TMPro;

public class FileManager : MonoBehaviour
{

    //static string path;
    static List<string> paths = new List<string>();
    static string path;
    

    private static int selectedIndex = -1;

    public static GameObject content;

    public static GameObject item;

    public static GameObject save; //bottone

    public static GameObject delete; //bottone

    public static  GameObject deleteitem; //bottone ( public Button deleteitem; )

    public GameObject PopUpDeleteItem; //menu
    public GameObject PopUpDeleteAll; //menu
    public GameObject PopUpOk; //menu

    public static List<string> options = new List<string>();

    //public Dropdown.DropdownEvent onValueChanged;

    //colori
    public static Color selectedColor = Color.green; //no

    public static Color selectedTextColor = Color.green;

    public static Color normalColor = Color.white; //no

    public static Color normalTextColor = Color.black;





    void Start()
    {

        paths = new List<string>();

        content = GameObject.Find("ContentS");
        item = GameObject.Find("ButtonItemS");
        save = GameObject.Find("ButtonSaveS");
        delete = GameObject.Find("ButtonDeleteAllS");
        deleteitem = GameObject.Find("ButtonDeleteItemS");
        

        save.SetActive(false);  //vedere se disattivare quando rimuovo tutta la lista

        delete.SetActive(false);

        deleteitem.SetActive(false);
       

    }

    /*public void OpenExplorer()
    {
        //path = EditorUtility.OpenFilePanel("Seleziona il dataset da caricare","./","txt");
        //path = EditorUtility.OpenFilePanel("Seleziona il dataset da caricare", "", "txt");
        
        #if UNITY_EDITOR
                path = EditorUtility.OpenFilePanel("Select dataset to load", "", "txt");
        #endif

        //ERROREEEEEEEEEEEEEEEEEEEEEE

        updateText(path);
        checkElement(); //da posizionare meglio,controllo nel caso in cui apro per selezionare ma non lo faccio
    }*/

    public void activeStart() {
        ManagerMainMenu.getB().SetActive(true); //abilitare se parto dal menu principale    
        PopUpOk.SetActive(true);
    }

    public static List<string> getPath()  //da modificare nel caso di più testi
    {
        return paths;
    }

    public static void updateText(string p)
    {

        if (path != "")
        {
     
            add(p);

            checkElement();
        }

    }

    //da qua codice modificato

    public static void add(string p)
    {

        var copy = Instantiate(item);
        copy.transform.SetParent(content.transform);
        copy.transform.localPosition = Vector3.zero;
        copy.transform.localScale = Vector3.one;

        copy.GetComponentInChildren<Text>().text = p;

        // Add the event handler (per quando clicco il bottone)
        copy.GetComponent<Button>().onClick.AddListener(
            () => { OnItemSelected(FindIndex(copy)); }
        );

        // Add the option to the list
        options.Add(p);

    }

    public void RemoveSelected()
    {
   
        RemoveAt(selectedIndex);
        deleteitem.SetActive(false);
        checkElement();

        //devo chiudere PopUp
        PopUpDeleteItem.SetActive(false);

    }

    public void RemoveAt(int index)
    {
        if (index < 0 || index >= options.Count) return;

        // Remove UI component
        Transform t = content.transform.GetChild(index);
        Destroy(t.gameObject);

        // Remove logical component
        options.RemoveAt(index);
    }

    public void ClearOptions()
    {
        // Remove the UI objects
        foreach (Transform t in content.transform)
        {
            Destroy(t.gameObject);
        }

        // Remove the underlying data
        options.Clear();

        checkElement();

        //devo chiudere PopUp
        PopUpDeleteAll.SetActive(false);
    }


    private static void OnItemSelected(int index)
    {
        if(deleteitem.activeSelf==false) deleteitem.SetActive(true);
        print(index);
        ClearItem(selectedIndex);
        selectedIndex = index;
        SetItem(index);
    }


    private static void SetItem(int index)
    {
        if (index < 0 || index >= options.Count) return;
        SetButtonColor(index,selectedTextColor);
    }

    private static void ClearItem(int index)
    {
        if (index < 0 || index >= options.Count) return;
        SetButtonColor(index,normalTextColor);
    }

    private static void SetButtonColor(int index, Color text)
    {
        
        var button = content.transform.GetChild(index).GetComponent<Button>();

        ColorBlock cb = new ColorBlock()
        {
            normalColor = button.colors.normalColor,
            colorMultiplier = button.colors.colorMultiplier,
            disabledColor = button.colors.disabledColor,
            fadeDuration = button.colors.fadeDuration,
            highlightedColor = button.colors.highlightedColor,
            pressedColor = button.colors.pressedColor
        };

        button.colors = cb;
        button.GetComponentInChildren<Text>().color = text;

    }

    private static int FindIndex(GameObject copy)
    {
        for (int i = 0; i < content.transform.childCount; i++)
        {
            Transform t = content.transform.GetChild(i);
            if (t.gameObject == copy)
            {
                return i;
            }
        }

        return -1;
    }

    private static void checkElement(){
        if (options.Count == 0)
        {
            //deleteitem.SetActive(false); non ci sta bisogno visto che ad ogni removeselected() lo disattivo
            delete.SetActive(false);
            save.SetActive(false);
            //ManagerMainMenu.getB().SetActive(false);
        }
        else if(options.Count > 0)
        {
            delete.SetActive(true);
            save.SetActive(true);
            //ManagerMainMenu.getB().SetActive(true);
        }
    }

    public static void AddPath(string p)
    {
        paths.Add(p);

    }

}

