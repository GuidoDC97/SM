using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawPath : MonoBehaviour
{
    private const float MINOFF = 5.0f;
    private const float MAXOFF = 20.0f;

    private const float MINWIDTH = 0.01f;
    private const float MAXWIDTH = 0.5f;

    private string path = "empty";

    private Color color;

    private int numPoints = 10;
    private int index = 0;  // path index
    private float width = MINWIDTH;

    // Start is called before the first frame update
    void Start()
    {

        //while (string.Compare(path, "empty") == 0); // busy wait

        //Draw();
        //StartCoroutine(DrawAnimation());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetPath(string p)
    {
        path = string.Copy(p);
    }

    public void SetColor(Color c)
    {
        color = c;
    }

    public void SetIndex(int i)
    {
        index = i;
    }

    public void SetWidth(float w)
    {
        width = width + w;
        width = Mathf.Clamp(width, MINWIDTH, MAXWIDTH);
    }

    public float GetWidth()
    {
        return width;
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

    public void Draw()
    {
        for (int j = 0; j < path.Length-1; j++)
        {
            GameObject pathLine = new GameObject();

            pathLine.transform.parent = transform;
            pathLine.transform.position = new Vector3(0, 0, 0);
            pathLine.name = string.Concat("Path", index.ToString(), ".", j.ToString());
            pathLine.AddComponent<LineRenderer>();

            LineRenderer pathRenderer = pathLine.GetComponent<LineRenderer>();

            pathRenderer.material = new Material(Shader.Find("Mobile/Particles/Additive"));
            pathRenderer.startColor = color;
            pathRenderer.endColor = color;
            pathRenderer.startWidth = width;
            pathRenderer.endWidth = width;
            pathRenderer.positionCount = numPoints;
            pathRenderer.numCapVertices = 10;
            pathRenderer.numCornerVertices = 10;

            GameObject current = GameObject.Find(path[j].ToString());
            GameObject next = GameObject.Find(path[j+1].ToString());

            Vector3[] positions = new Vector3[numPoints];

            float offset = Random.Range(MINOFF, MAXOFF);
            float xoffset = Random.Range(-1, 1);
            Vector3 middlePosition = GetMiddlePoint(current.transform.position, next.transform.position, offset);
            middlePosition = new Vector3 (middlePosition.x + xoffset, middlePosition.y, middlePosition.z );

            for (int k = 0; k < numPoints - 1; k++)
            {
                float t = (k / (float)numPoints);

                positions[k] = CalculateCuadraticBezierPoint(t, current.transform.position, middlePosition, next.transform.position);
            }
            //for(int i=1; i<numPoints; i++)
            //{
            //    positions[i] = new Vector3(positions[i].x, positions[i].y, positions[i].z + xoffset);
            //}
            
            positions[numPoints - 1] = next.transform.position; 

            pathRenderer.SetPositions(positions);
        }
    }

    //IEnumerator DrawAnimation()
    //{
    //    for (int j = 1; j < path.Length; j++)
    //    {
    //        GameObject pathLine = new GameObject();

    //        pathLine.transform.parent = transform;
    //        pathLine.transform.position = new Vector3(0, 0, 0);
    //        pathLine.name = string.Concat("Path", index.ToString(), ".", j.ToString());
    //        pathLine.AddComponent<LineRenderer>();

    //        LineRenderer pathRenderer = pathLine.GetComponent<LineRenderer>();

    //        pathRenderer.material = new Material(Shader.Find("Mobile/Particles/Additive"));
    //        pathRenderer.startColor = color;
    //        pathRenderer.endColor = color;
    //        pathRenderer.startWidth = width;
    //        pathRenderer.endWidth = width;
    //        pathRenderer.positionCount = numPoints;
    //        pathRenderer.numCapVertices = 10;
    //        pathRenderer.numCornerVertices = 10;

    //        GameObject current = GameObject.Find(path[j - 1].ToString());
    //        GameObject next = GameObject.Find(path[j].ToString());
    
    //        Vector3[] positions = new Vector3[numPoints];

    //        float offset = Random.Range(MINOFF, MAXOFF);

    //        for (int k = 0; k < numPoints; k++)
    //        {
    //            float t = (k / (float)numPoints);

    //            Vector3 middlePosition = GetMiddlePoint(current.transform.position, next.transform.position, offset);

    //            positions[k] = CalculateCuadraticBezierPoint(t, current.transform.position, middlePosition, next.transform.position);
    //        }

    //        pathRenderer.SetPositions(positions);

    //        yield return new WaitForSeconds(1f);
    //    }
    //}

}
