using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UpdateDateField : MonoBehaviour
{
    public TextMeshProUGUI field1;
    public TextMeshProUGUI field2;
    public TextMeshProUGUI field3;
    public TextMeshProUGUI fieldref1; //reference to TextField of Calendar1
    public TextMeshProUGUI fieldref2; //reference to TextField of Calendar2
    public TextMeshProUGUI fieldpoint; //reference to InputField
    private void Update()
    {

        field1.text = fieldref1.text;
        field2.text = fieldref2.text;
        field3.text = fieldpoint.text;

    }
}
