using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FTPHolder : MonoBehaviour
{
    private static string url = "ftp://files.000webhost.com/public_html/UnitsRepository.txt";
    private static string user = "mannstatistics";
    private static string pass = "passpass2";

    public static void SetUrl(string s)
    {
        url = s;
    }

    public static void SetUser(string s)
    {
        user = s;
    }

    public static void SetPass(string s)
    {
        pass = s;
    }

    public static string GetUrl()
    {
        return url;
    }

    public static string GetUser()
    {
        return user;
    }

    public static string GetPass()
    {
        return pass;
    }
}
