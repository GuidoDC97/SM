using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TMPro;

public class FloorMovement : MonoBehaviour
{
    public GameObject Floorm1;
    public GameObject Floor0;
    public GameObject Floor1;
    public GameObject Floor2;

    private Vector3 targetm1;
    private Vector3 target0;
    private Vector3 target1;
    private Vector3 target2;

    float speed = 15.0f;
    public int visualization = 3;
    private int change = 0;
    public NavMeshSurface surface;

    private float r = 0.1f;

    public TMP_Dropdown viewDropdown;

    // Start is called before the first frame update
    private void Start()
    {
        //this.GetComponenet<SelectViewDrop>captionText.text="Tipo Visuale";
    }

    public void SetVisualization(int v)
     {
         visualization = v;
     }

    void Update()
    {

        if (visualization == 1) //Orizzontal View
        {
            targetm1 = new Vector3(0, 0, -20);
            target0 = new Vector3(0, 0, 0);
            target1 = new Vector3(-30, 0, 0);
            target2 = new Vector3(-30, -0.2f, -22);
        }
        else if (visualization == 3) //Vertical View
        {
            targetm1 = new Vector3(0, -15, 0);
            target0 = new Vector3(0, 0, 0);
            target1 = new Vector3(0, 15, 0);
            target2 = new Vector3(0, 30, 0);
        }
        else if (visualization == 2) //Obliqual View
        {
            targetm1 = new Vector3(0, -5, 15);
            target0 = new Vector3(0, 0, 0);
            target1 = new Vector3(0, 5, -15);
            target2 = new Vector3(0, 10, -30);
        }

        float step = speed * Time.deltaTime;

        if (change != visualization)
        {
            viewDropdown.interactable = false;
            surface.enabled = false;

            Floorm1.transform.position = Vector3.MoveTowards(Floorm1.transform.position, targetm1, step);
            Floor0.transform.position = Vector3.MoveTowards(Floor0.transform.position, target0, step);
            Floor1.transform.position = Vector3.MoveTowards(Floor1.transform.position, target1, step);
            Floor2.transform.position = Vector3.MoveTowards(Floor2.transform.position, target2, step);

            if (Vector3.Distance(Floorm1.transform.position, targetm1) < r && Vector3.Distance(Floor0.transform.position, target0) < r &&
                Vector3.Distance(Floor1.transform.position, target1) < r && Vector3.Distance(Floor2.transform.position, target2) < r)
            {
                surface.BuildNavMesh();
                surface.enabled = true;
                change = visualization;
                viewDropdown.interactable = true;
            }
        }
    }
}
