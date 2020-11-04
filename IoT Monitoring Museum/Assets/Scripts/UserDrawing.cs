using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class UserDrawing : MonoBehaviour
{
    public const float INC_PATH = 0.0005f;
    private const float MINWIDTH = 0.01f;
    private const float MAXWIDTH = 0.5f;

    public NavMeshAgent agent;
    public GameObject user;

    public string path = "empty";

    public float speed = 1f;
    public float changingSpeed = 10f;

    private int index;
    private int i = 1;

    private float rLabel = 0.4f;
    //private float rStair = 0.1f;
    private float rStair = 0.4f;
    private float rPoint = 0.2f;


    private float cont = 0.01f;

    public Slider slider;

    //public int numPoints = 50;

    private bool change = false;
    public bool changing = false;
    private bool reached = false;

    private Vector3[][] positions;
    private List<Vector3> listPositions = new List<Vector3>();
    GameObject[] last3Points = new GameObject[3];
    string pointName;

    //05 06
    GameObject mainPath;
    int numPoints =10;
    private const float MINOFF = 10.0f;
    private const float MAXOFF = 20.0f;

    // Start is called before the first frame update
    void Start()
    {
        mainPath = new GameObject();
        mainPath.name = user.name;
        mainPath.tag = "Paths";
    }

    // Update is called once per frame
    void Update()
    {
        //agent.speed = slider.value;
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

            // controllo per casi eccezionali: floor0 e floor1
            if (string.Compare(current.transform.parent.name, "Floor0") == 0 &&
                (string.Compare(next.transform.parent.name, "Floor1") == 0 || string.Compare(next.transform.parent.name, "Floor2") == 0))
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




            // controllo della propria destinazione
            if (changing == false && current.transform.parent == next.transform.parent)
            {
                agent.SetDestination(next.transform.position);
                pointName = current.transform.parent.name;

                /*
                // controllo del point più vicino
                GameObject[] points = GameObject.FindGameObjectsWithTag("Points"+next.transform.parent.name);

                bool check = false;
                for(int j=0 ; j<points.Length && check == false; j++)
                {
                    if (Vector3.Distance(transform.position, points[j].transform.position) < rPoint && points[j] != lastPoint)
                    { 
                        listPositions.Add(points[j].transform.position);
                        lastPoint = points[j];
                        check = true;
                    }
                }
                */
            }
            else if (changing == false && current.transform.parent != next.transform.parent)
            {
                if (reached == false)
                {
                    change = true;

                    //agent.stoppingDistance = rStair;

                    agent.SetDestination(currentStairs[0].transform.position);
                    pointName = currentStairs[0].transform.parent.name;

                    /*
                    // controllo del point più vicino
                    GameObject[] points = GameObject.FindGameObjectsWithTag("Points" + next.transform.parent.name);

                    bool check = false;
                    for (int j = 0; j < points.Length && check == false; j++)
                    {
                        if (Vector3.Distance(transform.position, points[j].transform.position) < rPoint && points[j] != lastPoint)
                        {
                            listPositions.Add(points[j].transform.position);
                            lastPoint = points[j];
                            check = true;
                        }
                    }
                    */
                }
                else if (reached == true)
                {
                    //agent.stoppingDistance = rLabel;

                    agent.SetDestination(next.transform.position);
                    pointName = next.transform.parent.name;

                    /*
                    // controllo del point più vicino
                    GameObject[] points = GameObject.FindGameObjectsWithTag("Points" + next.transform.parent.name);

                    bool check = false;
                    for (int j = 0; j < points.Length && check == false; j++)
                    {
                        if (Vector3.Distance(transform.position, points[j].transform.position) < rPoint && points[j] != lastPoint)
                        {
                            listPositions.Add(points[j].transform.position);
                            lastPoint = points[j];
                            check = true;
                        }
                    }
                    */
                }
            }
            else if (changing == true)
            {
                transform.position = Vector3.MoveTowards(transform.position, nextStairs[0].transform.position, changingSpeed * Time.deltaTime);
                
            }

            if (changing == false)
            {
                // controllo del point più vicino
                GameObject[] points = GameObject.FindGameObjectsWithTag("Points" + pointName);

                bool check = false;
                for (int j = 0; j < points.Length && check == false; j++)
                {
                    if (Vector3.Distance(transform.position, points[j].transform.position) < rPoint && points[j] != last3Points[2])
                    {
                        
                        if (points[j] != last3Points[1])
                        {
                            last3Points[0] = last3Points[1];
                            last3Points[1] = last3Points[2];
                            last3Points[2] = points[j];
                            listPositions.Add(points[j].transform.position);


                        }
                        
                        /*
                        listPositions.Add(points[j].transform.position);
                        last3Points[2] = points[j];
                        */

                        check = true;
                    }
                }
            }

            if (((change == false && changing == false) || (reached == true)) && Vector3.Distance(next.transform.position, transform.position) < rLabel)
            {
                /*
                // aggiornamento delle posizioni
                positions[i-1] = new Vector3[listPositions.Count + 2];
                Debug.Log("posizioni "+ (i-1) + ": "+ positions[i - 1].Length);

                positions[i-1][0] = current.transform.position;
                for (int j=0; j<listPositions.Count; j++)
                {
                    positions[i-1][j+1] = new Vector3(listPositions[j].x, listPositions[j].y, listPositions[j].z);
                }
                positions[i-1][(positions[i-1].Length) - 1] = new Vector3 (next.transform.position.x, 0.1f, next.transform.position.z);

                listPositions.Clear();
                last3Points[2] = null;
                */
                i = i + 1;

                
                // 0506
                listPositions.Insert(0, current.transform.position);
                listPositions.Add(next.transform.position);
                Draw(i-1, positions.Length);    //utilizza listPositions
                last3Points[2] = null;
                listPositions.Clear();
                

                if (reached)
                {
                    reached = false;
                }
            }
            else if (change == true && Vector3.Distance(currentStairs[0].transform.position, transform.position) < rStair)
            {
                change = false;
                changing = true;

                listPositions.Add(currentStairs[0].transform.position);

                float offset = Random.Range(MINOFF, MAXOFF);
                float xoffset = Random.Range(-1, 1);

                Vector3 middlePosition = GetMiddlePoint(currentStairs[0].transform.position, nextStairs[0].transform.position, offset);
                middlePosition = new Vector3(middlePosition.x , middlePosition.y, middlePosition.z + xoffset);

                for (int k = 0; k < numPoints - 1; k++)
                {
                    float t = (k / (float)numPoints);

                    Vector3 pos = CalculateCuadraticBezierPoint(t, currentStairs[0].transform.position, middlePosition, nextStairs[0].transform.position);
                   //if (k != 0)
                   // {
                   //     pos.z = pos.z + zoff;
                   // }
                    listPositions.Add(pos);
                }


                //agent.transform.position = new Vector3(agent.transform.position.x, agent.transform.position.y + 1, agent.transform.position.z);
                //agent.transform.position = new Vector3(agent.transform.position.x, currentStairs[0].transform.position.y, agent.transform.position.z);

                agent.enabled = false;
                user.layer = 12;
            }
            else if (changing == true && Vector3.Distance(nextStairs[0].transform.position, transform.position) < rStair)
            {
                changing = false;   
                reached = true;

                listPositions.Add(nextStairs[0].transform.position);

                agent.transform.position = new Vector3(agent.transform.position.x, nextStairs[0].transform.position.y, agent.transform.position.z);

                agent.enabled = true;
                user.layer = 14;
            }

            if (i >= path.Length)
            {
                path = "empty";
                //Draw();
            }

        }
    }

    public void SetPath(string p)
    {
        path = string.Copy(p);

        positions = new Vector3[path.Length - 1][];
    }

    public void SetSpeed(float s)
    {
        speed = s;

    }

    public void SetAgent(NavMeshAgent a)
    {
        agent = a;
        agent.speed = 0.5f;
        agent.acceleration = 0.5f;
        agent.obstacleAvoidanceType = ObstacleAvoidanceType.LowQualityObstacleAvoidance;
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
        cont = cont + INC_PATH;
        cont = Mathf.Clamp(cont, MINWIDTH, MAXWIDTH);

    }

    public float GetCont()
    {
        return cont;
    }

    public void SetSlider(Slider s)
    {
        slider = s;
    }

    public void Draw()
    {


        for (int k = 0; k < positions.Length; k++)
        {
            GameObject pathLine = new GameObject();

            pathLine.transform.parent = mainPath.transform;
            pathLine.transform.position = new Vector3(0, 0, 0);
            pathLine.name = string.Concat("Path", index.ToString(), ".", k.ToString());
            pathLine.AddComponent<LineRenderer>();

            Color color = GetComponent<Renderer>().material.color;

            LineRenderer pathRenderer = pathLine.GetComponent<LineRenderer>();

            pathRenderer.material = new Material(Shader.Find("Mobile/Particles/Additive"));
            pathRenderer.startColor = color;
            pathRenderer.endColor = color;
            pathRenderer.startWidth = cont;
            pathRenderer.endWidth = cont;
            pathRenderer.positionCount = 3;
            pathRenderer.positionCount = positions[k].Length;
            pathRenderer.numCapVertices = 10;
            pathRenderer.numCornerVertices = 10;

            pathRenderer.SetPositions(positions[k]);
        }

        GameObject.Destroy(user);
    }

    public void Draw(int k, int dimPath)
    {
        Vector3[] p = new Vector3[listPositions.Count];
        
        for (int j = 0; j < listPositions.Count; j++)
        {
            p[j] = new Vector3(listPositions[j].x, listPositions[j].y, listPositions[j].z);
        }


        GameObject pathLine = new GameObject();

        pathLine.transform.parent = mainPath.transform;
        pathLine.transform.position = new Vector3(0, 0, 0);
        pathLine.name = string.Concat("Path", index.ToString(), ".", k.ToString());
        pathLine.AddComponent<LineRenderer>();

        Color color = GetComponent<Renderer>().material.color;

        LineRenderer pathRenderer = pathLine.GetComponent<LineRenderer>();

        pathRenderer.material = new Material(Shader.Find("Mobile/Particles/Additive"));
        pathRenderer.startColor = color;
        pathRenderer.endColor = color;
        pathRenderer.startWidth = cont;
        pathRenderer.endWidth = cont;
        pathRenderer.positionCount = 3;
        pathRenderer.positionCount = p.Length;
        pathRenderer.numCapVertices = 10;
        pathRenderer.numCornerVertices = 10;

        pathRenderer.SetPositions(p);

        // if dimpath -2

        
    }

    Vector3 CalculateCuadraticBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2)
    {
        float u = 1 - t;
        float uu = u * u;
        float tt = t * t;

        return uu * p0 + 2 * u * t * p1 + tt * p2;
    }

    Vector3 GetMiddlePoint(Vector3 p0, Vector3 p1, float offset)
    {
        Vector3 dir = (p0 - p1).normalized;

        Vector3 perpDir = Vector3.Cross(dir, Vector3.right);

        Vector3 midPoint = (p0 + p1) / 2f;
        midPoint.y = midPoint.y + offset;

        return midPoint;
    }
}
