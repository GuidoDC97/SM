using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StaticStairLabelScript : MonoBehaviour
{
    /*
    public void Start()
    {
        this.GetComponent<TextMeshPro>().color = new Color(250, 255, 0);
    }

    */
    // Update is called once per frame
    void Update()
    {
        transform.rotation = Camera.main.transform.rotation;
    }
}
