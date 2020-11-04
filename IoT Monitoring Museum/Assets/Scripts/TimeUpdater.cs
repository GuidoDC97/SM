using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TimeUpdater : MonoBehaviour
{
    public Slider maxh;
    public Slider maxm;
    public Slider minh;
    public Slider minm;

    public TextMeshProUGUI maxDuration;
    public TextMeshProUGUI minDuration;

    public TextMeshProUGUI TMPmaxh;
    public TextMeshProUGUI TMPmaxm;
    public TextMeshProUGUI TMPminh;
    public TextMeshProUGUI TMPminm;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        maxDuration.SetText(maxh.value + " : " + maxm.value);
        minDuration.SetText(minh.value + " : " + minm.value);
        TMPmaxh.SetText(maxh.value.ToString());
        TMPmaxm.SetText(maxm.value.ToString());
        TMPminh.SetText(minh.value.ToString());
        TMPminm.SetText(minm.value.ToString());


    }
}
