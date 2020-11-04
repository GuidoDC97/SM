using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PopUp : MonoBehaviour
{
    bool X = false;
    public GameObject pan;


    // Update is called once per frame


    public void click()
    {
        if (X == true)
        {
            //se èperto chiudi
            X = false;

        }
        else if (X == false)
        {
            //se èhiuso apri
            X = true;
        }
        pan.SetActive(X);


    }

}