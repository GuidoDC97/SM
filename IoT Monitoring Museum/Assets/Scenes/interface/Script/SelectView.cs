using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;



public class SelectView : MonoBehaviour
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
    public NavMeshSurface surface;
    private float r = 0.1f;
    float step = 0;
    public Dropdown m_Dropdown;
    int m_DropdownValue;



    void  Update()
    {
         step = speed * Time.deltaTime;
        switch (m_Dropdown.value)
        {
            case 1:
                MoveOrizzontal();
                break;
            case 2:
                MoveVertical();
                break;
            case 3:
                MoveObliquic();
                break;
            default:
                MoveOrizzontal();
                break;
        }

    }

        void MoveOrizzontal()
        {
            Debug.Log("SCELTA 0");
            targetm1 = new Vector3(0, 0, -20);
            target0 = new Vector3(0, 0, 0);
            target1 = new Vector3(-30, 0, 0);
            target2 = new Vector3(-30, 0, -22);

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
            }
        }

        void MoveVertical()
        {
            Debug.Log("SCELTA 1");

            targetm1 = new Vector3(0, -15, 0);
            target0 = new Vector3(0, 0, 0);
            target1 = new Vector3(0, 15, 0);
            target2 = new Vector3(0, 30, 0);

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
            }
        }

        void MoveObliquic()
        {
            Debug.Log("SCELTA 2");
            targetm1 = new Vector3(0, -5, 15);
            target0 = new Vector3(0, 0, 0);
            target1 = new Vector3(0, 5, -15);
            target2 = new Vector3(0, 10, -30);

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
            }
        }

       

    }

