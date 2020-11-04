using UnityEngine;

public class GUI_Window_Date : MonoBehaviour
{
    Rect windowRect = new Rect(200, 20, 120, 50);

    void OnGUI()
    {
        // Register the window. Here we instruct the layout system to
        // make the window 100 pixels wide no matter what.
        windowRect = GUILayout.Window(0, windowRect, DoMyWindow, "My Window", GUILayout.Width(100));
    }

    // Make the contents of the window
    void DoMyWindow(int windowID)
    {
        // This button is too large to fit the window
        // Normally, the window would have been expanded to fit the button, but due to
        // the GUILayout.Width call above the window will only ever be 100 pixels wide
        if (GUILayout.Button("Please click me a lot"))
        {
            print("Got a click");
        }
    }
}
