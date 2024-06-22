using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class DialogueManager {
    private static Dictionary<string, string[]> DialogueDict = new Dictionary<string, string[]>();

    public static void loadFromFile(string path) {
        if (DialogueDict.ContainsKey(path)) {
            return;
        }

        TextAsset textAsset = (TextAsset)Resources.Load(path);
        XmlDocument xmldoc = new XmlDocument();
        xmldoc.LoadXml(textAsset.text);
        XmlNodeList pages = xmldoc.GetElementsByTagName("page");

        string[] value = new string[pages.Count];
        for (int i = 0; i < pages.Count; i++)
        {
            value[i] = pages[i].InnerText;
        }

        DialogueDict.Add(path, value);
    }

    public static bool textToLoad(string path) {
        if(!DialogueDict.ContainsKey(path)) {
            return false;
        }

        DialogueTextManager.EnqueueTexts(DialogueDict.GetValueOrDefault(path));
        return true;
    }

}
