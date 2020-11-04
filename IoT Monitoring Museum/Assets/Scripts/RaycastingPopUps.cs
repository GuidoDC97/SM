using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RaycastingPopUps : MonoBehaviour
{
    private const float DISTANCE = 500.0f;
    public TextMeshPro floatingText;
    private bool on = false;
    private TextMeshPro instance;
    private const float TXTOFF = 5.0f;
    private const float TXTOFF_USERS = 2.0f;

    // Update is called once per frame
    private void Update()
    {
        if (instance)
        {
            instance.transform.rotation = Camera.main.transform.rotation;
        }
    }


    void OnMouseDown()
    {

            RaycastHit target;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Debug.Log("Arrivo qua:  Camera.main.ScreenPointToRay(Input.mousePosition)");
            if (Physics.Raycast(ray, out target, DISTANCE))
            {
                Debug.Log("Arrivo qua:Physics.Raycast(ray, out target, DISTANCE))");
                if (target.transform)
                {
                    Debug.Log("Arrivo qua:(target.transform)");
                    int layer = target.transform.gameObject.layer;
                    Debug.Log(layer);
                    if (layer == 8)
                    {
                        
                        GameObject controlUnit = target.transform.gameObject;

                        if (floatingText && !on)
                        {
                            ShowFloatingText(controlUnit);
                            on = true;
                        }
                        else if (floatingText && on)
                        {
                            HideFloatingText();
                            on = false;
                        }
                    }
                    else if (layer == 11)
                    {
                        GameObject agent = target.transform.gameObject;

                        if (floatingText && !on)
                        {
                            ShowFloatingTextAgent(agent);
                            on = true;
                        }
                        else if (floatingText && on)
                        {
                            HideFloatingText();
                            on = false;
                        }
                    }
                }
            }
        

    }
    
    private void ShowFloatingText(GameObject cu)
    {
        instance = Instantiate(floatingText, new Vector3(cu.transform.position.x, cu.transform.position.y + TXTOFF, cu.transform.position.z), 
                                    Camera.main.transform.rotation, cu.transform);
        /*
        instance.GetComponent<TextMeshPro>().SetText("Nome centralina:" + cu.name + "\nUtenti passati:" 
                                                + cu.gameObject.GetComponent<LabelsScript>().GetVisitors().ToString());
                                                */
        instance.GetComponent<TextMeshPro>().SetText("Label's name:" + cu.name + "\nUsers detected:"
                                                + cu.gameObject.GetComponent<LabelsScript>().GetVisitors().ToString());
        instance.GetComponent<TextMeshPro>().color = Color.red;

    }

    private void ShowFloatingTextAgent(GameObject ag)
    {
        instance = Instantiate(floatingText, new Vector3(ag.transform.position.x, ag.transform.position.y + TXTOFF_USERS, ag.transform.position.z),
                                Camera.main.transform.rotation, ag.transform);
        instance.GetComponent<TextMeshPro>().SetText("Users represented:" + ag.gameObject.GetComponent<UserController>().GetCont() + "\nPath:"
                                            + ag.gameObject.GetComponent<UserController>().path);
        instance.GetComponent<TextMeshPro>().color = Color.blue;
    }

    private void HideFloatingText()
    {
        GameObject.Destroy(instance);
    }

}
