using UnityEngine;
using UnityEditor;
using System;
using Newtonsoft.Json;
using UnityEngine.UI;

public class GetColorCode : EditorWindow
{
    string myString = "{r: 0.5017824, g: 0.3547971, b: 0.6320754, a: 0.4039216}";
    Color matColor = Color.white;

    // Add menu item named "My Window" to the Window menu
    [MenuItem("Window/Get Color Code")]
    public static void ShowWindow()
    {
        EditorWindow window = EditorWindow.GetWindow(typeof(GetColorCode));
        window.Show();
    }

    void OnGUI()
    {
        try
        {
            GUILayout.Label("Base Settings", EditorStyles.boldLabel);
            myString = EditorGUILayout.TextField("Color Json", myString);
            matColor = ConvertToColor1(JsonConvert.DeserializeObject<GetColor>(myString));
            matColor = EditorGUILayout.ColorField("New Color", matColor);

            if (GUI.Button(new Rect(140, 60, 150, 20), "Set Color Of Image"))
                ChangeColorsOfImage(matColor);

            if (GUI.Button(new Rect(300, 60, 150, 20), "Set Color Of Text"))
                ChangeColorsOfText(matColor);

            if (GUI.Button(new Rect(10, 60, 110, 20), "Get Color Code"))
                PrintColorCode();
        }
        catch (Exception ec)
        {

        }
    }

    private void PrintColorCode()
    {
        matColor = ConvertToColor255(JsonConvert.DeserializeObject<GetColor>(myString));
        Debug.Log("R: " + matColor.r.ToString() + " G: " + matColor.g.ToString() + " B: " + matColor.b.ToString() + " A: " + matColor.a.ToString());
    }
    private void ChangeColorsOfImage(Color color)
    {
        matColor = EditorGUILayout.ColorField("New Color", color);
        if (Selection.activeGameObject) ;

        foreach (GameObject t in Selection.gameObjects)
        {
            Image image = t.GetComponent<Image>();

            if (image != null)
            {
                image.color = color;
                Debug.Log("Done.!");
            }
            else
            {
                Debug.LogError("Not found image component.!");
            }
        }

    }
    private void ChangeColorsOfText(Color color)
    {
        matColor = EditorGUILayout.ColorField("New Color", color);
        if (!Selection.activeGameObject)

            foreach (GameObject t in Selection.gameObjects)
            {
                Text text = t.GetComponent<Text>();

                if (text != null)
                {
                    text.color = color;
                    Debug.Log("Done.!");
                }
                else
                {
                    Debug.LogError("Not found text component.!");
                }
            }

    }


    public static Color ConvertToColor1(GetColor colorParsed)
    {
        Color color = Color.white;
        try
        {
            color = new Color(colorParsed.r, colorParsed.g, colorParsed.b, colorParsed.a);
        }
        catch (Exception)
        {

        }
        return color;
    }

    public static Color ConvertToColor255(GetColor colorParsed)
    {
        colorParsed.r *= 255;
        colorParsed.g *= 255;
        colorParsed.b *= 255;
        colorParsed.a *= 255;

        return new Color(colorParsed.r, colorParsed.g, colorParsed.b, colorParsed.a);
    }
}

[System.Serializable]
public class GetColor
{
    public float r { get; set; }
    public float g { get; set; }
    public float b { get; set; }
    public float a { get; set; }
}