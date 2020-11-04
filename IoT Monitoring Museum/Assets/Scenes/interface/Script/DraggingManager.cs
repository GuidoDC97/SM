using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class DraggingManager : EventTrigger
{
    private bool dragging;
    private GameObject panel;

    // Start is called before the first frame update
    void Start()
    {
        panel = GameObject.Find("MainPanel");
    }

    // Update is called once per frame
    void Update()
    {
        if (dragging)
        {
            var posx = Input.mousePosition.x;
            var posy = Input.mousePosition.y;
            var positionx = Mathf.Clamp(posx, 0.01f, Screen.width);
            var positiony = Mathf.Clamp(posy, 0.01f, Screen.height);
            panel.transform.position = new Vector2(positionx, positiony);
        }
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        dragging = true;
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        dragging = false;
    }
 

}
