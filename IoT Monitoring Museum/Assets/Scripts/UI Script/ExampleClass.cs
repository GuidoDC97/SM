using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExampleClass : MonoBehaviour
{
    private string url = "http://mannstatistics.000webhostapp.com/UnitsRepository.txt";

    void Start()
    {
        Debug.Log("hello");

        StartCoroutine(Start2());

    }

    IEnumerator Start2()
    {

        using (WWW www = new WWW(url))
        {
            yield return www;
            Debug.Log(www.text);
            Debug.Log("hello2");




        }
    }
}
