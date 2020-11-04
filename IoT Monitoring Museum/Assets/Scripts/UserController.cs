using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class UserController : MonoBehaviour
{

    public NavMeshAgent agent;
    public GameObject user;

    public string path = "empty";

    public float speed = 1f;
    public float changingSpeed = 5f;

    private int index;
    private int i = 1;

    private float rLabel = 0.4f;
    //private float rStair = 0.1f;
    private float rStair = 0.4f;


    private int cont = 1;

    public Slider slider;

    //public int numPoints = 50;

    private bool change = false;
    public bool changing = false;
    private bool reached = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        agent.speed = slider.value;
        //Debug.Log(slider.value);

        if (string.Compare(path, "empty") != 0)
        {
            GameObject current = GameObject.Find(path[i - 1].ToString());
            GameObject next = GameObject.Find(path[i].ToString());

            GameObject[] currentStairs = GameObject.FindGameObjectsWithTag(current.transform.parent.name);
            GameObject[] nextStairs = GameObject.FindGameObjectsWithTag(next.transform.parent.name);

            /*
            if (currentStairs.Length != 1)
            {
                if (string.Compare(next.transform.parent.name, current.transform.parent.name) < 0 || string.Compare(next.transform.parent.name, "Floor-1") == 0)
                {
                    currentStairs[0] = GameObject.Find("Stair1");
                } else if (string.Compare(next.transform.parent.name, current.transform.parent.name) > 0)
                {
                    currentStairs[0] = GameObject.Find("Stair0");
                }
            }
            */

            if (string.Compare(current.transform.parent.name, "Floor0") == 0 && 
                (string.Compare(next.transform.parent.name, "Floor1") == 0 || string.Compare(next.transform.parent.name, "Floor2") == 0) )
            {
                currentStairs[0] = currentStairs[0];
            }
            if (string.Compare(current.transform.parent.name, "Floor0") == 0 && string.Compare(next.transform.parent.name, "Floor-1") == 0)
            {
                currentStairs[0] = currentStairs[1];
            }


            if (string.Compare(current.transform.parent.name, "Floor-1") == 0 && string.Compare(next.transform.parent.name, "Floor0") == 0)
            {
                nextStairs[0] = nextStairs[1];
            }

            if (changing == false && current.transform.parent == next.transform.parent)
            {
                //agent.stoppingDistance = rLabel;

                agent.SetDestination(next.transform.position);
            }
            else if(changing == false && current.transform.parent != next.transform.parent )
            {
                if (reached == false)
                {
                    change = true;

                    //agent.stoppingDistance = rStair;

                    agent.SetDestination(currentStairs[0].transform.position);
                }
                else if(reached == true)
                {
                    //agent.stoppingDistance = rLabel;

                    agent.SetDestination(next.transform.position);
                }
            }
            else if(changing == true)
            {
                transform.position = Vector3.MoveTowards(transform.position, nextStairs[0].transform.position, changingSpeed * Time.deltaTime);
            }

            if (((change == false && changing == false) || (reached == true)) && Vector3.Distance(next.transform.position, transform.position) < rLabel)
            {
                i = i + 1;

                if (reached)
                {
                    reached = false;
                }
            }
            else if (change == true && Vector3.Distance(currentStairs[0].transform.position, transform.position) < rStair)
            {
                change = false;
                changing = true;

                //agent.transform.position = new Vector3(agent.transform.position.x, agent.transform.position.y + 1, agent.transform.position.z);
                //agent.transform.position = new Vector3(agent.transform.position.x, currentStairs[0].transform.position.y, agent.transform.position.z);

                agent.enabled = false;
                user.layer = 12;
            }
            else if (changing == true && Vector3.Distance(nextStairs[0].transform.position, transform.position) < rStair )
            {
                changing = false;
                reached = true;

                agent.transform.position = new Vector3(agent.transform.position.x, nextStairs[0].transform.position.y, agent.transform.position.z);

                agent.enabled = true;
                user.layer = 11;
            }

            if (i >= path.Length)
            {
                path = "empty";
                GameObject.Destroy(user);
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

    public void SetAgent(NavMeshAgent a)
    {
        agent = a;
        agent.speed = speed;
        agent.acceleration = 0.5f;
        agent.obstacleAvoidanceType = ObstacleAvoidanceType.LowQualityObstacleAvoidance;
        agent.avoidancePriority = 99;

    }

    public void IncPriority()
    {

        agent.avoidancePriority = Mathf.Clamp(agent.avoidancePriority - 1, 0, 99);
    }

    public void SetIndex(int ind)
    {
        index = ind;
    }

    public void SetUser(GameObject u)
    {
        user = u;
    }

    public void SetScale(Vector3 s)
    {
        transform.localScale = s;
    }

    public void incCont()
    {
        cont = cont + 1;
    }

    public int GetCont()
    {
        return cont;
    }

    public void SetSlider(Slider s)
    {
        slider = s;
    }

    public void SetRadius(float r)
    {
        agent.radius = r;
    }

    public float GetRadius()
    {
        return agent.radius;
    }

}
