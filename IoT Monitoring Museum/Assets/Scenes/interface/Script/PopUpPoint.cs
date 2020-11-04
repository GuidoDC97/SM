using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PopUpPoint : MonoBehaviour
{
    bool X = false;
    public TextMeshProUGUI fieldrefpoint;


    public GameObject pan;
    public void click()
    {
        if (X == true)
        {
            //se è aperto chiudi
            X = false;
            fieldrefpoint.text = " ";

        }
        else if (X == false)
        {
            //se è chiuso apri
            X = true;
        }
        pan.SetActive(X);
    }
}
