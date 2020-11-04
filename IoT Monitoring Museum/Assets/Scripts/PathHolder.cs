using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathHolder : MonoBehaviour
{
    static List<string> paths = new List<string>();

    public static void AddPath(string p)
    {
        paths.Add(p);
    }

    public static List<string> GetPath()
    {
        return paths;
    }
}
