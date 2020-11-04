using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PopUpScript : MonoBehaviour
{
    bool X = false;
    public TextMeshProUGUI fieldref1;
    public TextMeshProUGUI fieldref2;

    public GameObject pan;
    public void click()
    {
        if (X == true)
        {
            //se è aperto chiudi
            X = false;
            fieldref1.text = " ";
            fieldref2.text = " ";


        }
        else if (X == false)
        {
            //se è chiuso apri
            X = true;
        }
        pan.SetActive(X);
    }
}