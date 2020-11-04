using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LabelsScript : MonoBehaviour
{
    public uint visitors = 0;


    public void AddVisitor()
    {
        visitors++;
    }

    public uint GetVisitors()
    {
        return visitors;
    }

    public void ResetVisitors()
    {
        visitors = 0;
    }
}
