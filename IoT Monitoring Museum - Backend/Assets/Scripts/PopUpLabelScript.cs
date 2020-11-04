using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PopUpLabelScript : MonoBehaviour
{

    private const float DISTANCE = 500.0f;
    public TextMeshPro floatingText;
    private bool on = false;
    private TextMeshPro instance;
    private const float TXTOFF = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        instance = Instantiate(floatingText, new Vector3(transform.position.x, transform.position.y + TXTOFF, transform.position.z),
                               Camera.main.transform.rotation, transform);
        instance.GetComponent<TextMeshPro>().SetText(this.transform.name);
        instance.GetComponent<TextMeshPro>().color = Color.cyan;
    }



    // Update is called once per frame
    void Update()
    {


        if (instance)
        {
            instance.transform.rotation = Camera.main.transform.rotation;
        }

    }
    /*
    private void OnMouseDown()
    {
        RaycastHit target;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out target, DISTANCE))
        {
            if (target.transform)
            {
                int layer = target.transform.gameObject.layer;
                if (layer == 8)
                {

                    GameObject controlUnit = target.transform.gameObject;

                    if (floatingText && !on)
                    {
                        Debug.Log("chiamo show floatingtext");
                        ShowFloatingText(controlUnit);
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
        Debug.Log("eseguo show floatingtext");
        instance = Instantiate(floatingText, new Vector3(cu.transform.position.x, cu.transform.position.y + TXTOFF, cu.transform.position.z),
                                Camera.main.transform.rotation, cu.transform);
        instance.GetComponent<TextMeshPro>().SetText("Nome centralina:" + cu.name + "\nUtenti passati:"
                                            + cu.gameObject.GetComponent<LabelsScript>().GetVisitors().ToString());
        instance.GetComponent<TextMeshPro>().color = Color.cyan;

    }

    private void HideFloatingText()
    {
        GameObject.Destroy(instance);
    }
    */

}
