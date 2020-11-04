 using UnityEngine;
 using UnityEngine.UI;
 using System.Collections;
 
public class SetDateOnText : MonoBehaviour
{
    public DatePickerControl date1;
    Text txt;

    // Use this for initialization
    void Start()
    {
       txt = gameObject.GetComponent<Text>();
        txt.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        txt.text = "Data Inizio: ";
        txt.text= date1.inputFieldDate.day.text+"/"+ date1.inputFieldDate.month.text + "/"+ date1.inputFieldDate.year.text + "/" + date1.inputFieldTime.hour.text;


    }
}