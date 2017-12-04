using UnityEngine;
using UnityEditor;
using System.IO;

public class WriteDataToFile : MonoBehaviour
{
    public void WriteString(string str)
    {
        HUD hud = FindObjectOfType<HUD>();
        EventManager manager = this.GetComponent<EventManager>();

        string path = "Assets/Resources/DataFromPreviousRun.txt";

        //Write some text to the test.txt file
        StreamWriter writer = new StreamWriter(path, true);

        writer.WriteLine(System.DateTime.Now);
        writer.WriteLine(str);

        writer.Close();

        //Re-import the file to update the reference in the editor
        AssetDatabase.ImportAsset(path);
        TextAsset asset = (TextAsset) Resources.Load("DataFromPreviousRun");

        //Print the text from the file
        Debug.Log(asset.text);
    }

    //[MenuItem("Tools/Read file")]
    //static void ReadString()
    //{
    //    string path = "Assets/Resources/test.txt";

    //    //Read the text from directly from the test.txt file
    //    StreamReader reader = new StreamReader(path);
    //    Debug.Log(reader.ReadToEnd());
    //    reader.Close();
    //}
}