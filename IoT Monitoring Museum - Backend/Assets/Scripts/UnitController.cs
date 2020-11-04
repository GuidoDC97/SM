using UnityEngine;
using System.Collections;
using TMPro;

public class UnitController : MonoBehaviour
{
    private Vector3 screenPoint;
    private Vector3 offset;

    private float y = 0.2f;

    private bool toCheck = false;
    private bool enable = false;

    public TextMeshProUGUI textAllert;

    private void Update()
    {
        if(toCheck == true)
        {
            if (!CheckPosition())
            {
                transform.localPosition = new Vector3(-5.0f, transform.localPosition.y, -6.0f);
                textAllert.color = Color.red;
                textAllert.text = "ATTENZIONE: trascinare la centralina in una posizione valida!";
            }
            else
            {
                textAllert.color = Color.blue;
                textAllert.text = "Centralina posizionata correttamente!";
            }

            toCheck = false;
        }
    }

    void OnMouseDown()
    {
        if(enable == true)
        {
            screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);

            offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
        }
    }

    void OnMouseDrag()
    {
        if(enable == true)
        {
            Vector3 cursorScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
            Vector3 cursorPosition = Camera.main.ScreenToWorldPoint(cursorScreenPoint) + offset;
            transform.position = new Vector3(cursorPosition.x, transform.position.y, cursorPosition.z);
        }
    }

    private void OnMouseUp()
    {
        if (enable == true)
        {
            toCheck = true;
        }
    }

    public void SetEnable(bool en)
    {
        enable = en;
    }

    public void SetTextAllert(TextMeshProUGUI t)
    {
        textAllert = t;
    }

    public bool CheckPosition()
    {
        bool esito = false;

        RaycastHit target;
        Ray ray = new Ray(transform.position, new Vector3(0, -1, 0));

        if (Physics.Raycast(ray, out target, 100))
        {
            Debug.Log(target.transform.gameObject.layer.ToString());

            if (target.transform.gameObject.layer != 8 || target.transform.parent != transform.parent)
            {
                Debug.Log("target: " + target.transform.parent.name + "     parent: " + transform.parent.name);
                esito = false;
            }
            else
            {
                esito = true;
            }
        }

        return esito;
    }
}
