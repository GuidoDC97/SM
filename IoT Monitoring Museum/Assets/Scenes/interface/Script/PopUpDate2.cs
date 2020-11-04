using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class PopUpDate2 : MonoBehaviour
{
    bool X = false;
    public TextMeshProUGUI fieldref;

    public GameObject pan;
    public void click()
    {
        if (X == true)
        {
            //se è aperto chiudi
            X = false;
            fieldref.text ="";

        }
        else if (X == false)
        {
            //se è chiuso apri
            X = true;
        }
        pan.SetActive(X);
    }
}