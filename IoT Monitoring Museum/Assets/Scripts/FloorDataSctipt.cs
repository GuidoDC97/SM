using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FloorDataSctipt : MonoBehaviour
{

    private uint numVisitors = 0;

    public void Update()
    {
        this.transform.rotation = Camera.main.transform.rotation;
    }

    public void SetFloorInfo()
    {
        //Debug.Log("setto il testo");
        TextMeshPro textComponent = this.GetComponent<TextMeshPro>();
        textComponent.text = this.transform.parent.name + "\nVisitors: " + getVisitors();
    }

    public uint getVisitors()
    {
        return numVisitors;
    }

    public void addVisitor()
    {
        this.numVisitors++;
    }

    public void ResetVisitors()
    {
        numVisitors = 0;
    }
}
