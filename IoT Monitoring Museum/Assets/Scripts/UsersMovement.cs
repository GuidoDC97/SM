using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UsersMovement : MonoBehaviour
{
    public string path = "empty";

    public float speed = 1;

    private int i = 1;

    float r = 0.1f;

    public int numPoints = 50;


    // Update is called once per frame
    void Update()
    {
        if (string.Compare(path, "empty") != 0)
        {

            GameObject current = GameObject.Find(path[i].ToString());

            if (Vector3.Distance(current.transform.position, transform.position) < r)
            {
                i = i + 1;
            }


            transform.position = Vector3.MoveTowards(transform.position, current.transform.position, Time.deltaTime * speed);

            if (i >= path.Length)
            {
                path = "empty";
            }

        }
    } 

    public void SetPath(string p)
    {
        path = string.Copy(p);
    }

    public void SetSpeed(float s)
    {
        speed = s;
    }

    GameObject GetClosestLabel(GameObject[] labels, Transform fromThis)
    {
        GameObject bestTarget = null;

        float closestDistanceSqr = Mathf.Infinity;

        Vector3 currentPosition = fromThis.position;

        foreach (GameObject potentialTarget in labels)
        {
            Vector3 directionToTarget = potentialTarget.transform.position - currentPosition;

            float dSqrToTarget = directionToTarget.sqrMagnitude;

            if (dSqrToTarget < closestDistanceSqr)
            {
                closestDistanceSqr = dSqrToTarget;
                bestTarget = potentialTarget;
            }
        }

        if (bestTarget.transform.position == fromThis.position)
        {
            bestTarget = null;
        }

        return bestTarget;
    }    
}
