using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;


// Next up:
// Include Actor data
// Actor Expression Data as well

public class DialogueManager {
    // load dialogue from XML into dict
    private static Dictionary<string, string[]> DialogueDict = new Dictionary<string, string[]>();

    public static void loadFromFile(string path) {
        if (DialogueDict.ContainsKey(path)) {
            return;
        }

        TextAsset textAsset = (TextAsset)Resources.Load(path);
        XmlDocument xmldoc = new XmlDocument();
        xmldoc.LoadXml(textAsset.text);
        XmlNodeList dialogue = xmldoc.GetElementsByTagName("dialogue");

        //should only be 1 dialogue tag in XML
        if (dialogue.Count != 1) {
            Debug.LogError("There should only be 1 dialogue tag! Check " + path);
            return;
        }
        // follows XML format : dialogue > character name > interaction id > page
        XmlNodeList characters = dialogue[0].ChildNodes;
        for (int i = 0; i < characters.Count; i++) {
            XmlNodeList interactions = characters[i].ChildNodes;
            for (int j = 0; j < interactions.Count; j++) {
                //changes dict key to be specific per character interaction
                string key = path;
                if (characters[i].Attributes.Count != 1 || characters[i].Attributes[0].Name != "name") {
                    Debug.LogError("We only want one attribute tag per character -- the name!");
                    continue;
                }

                key += "." + characters[i].Attributes[0].Value;

                if (interactions[j].Attributes.Count != 1 || interactions[j].Attributes[0].Name != "id") {
                    Debug.LogError("We only want one attribute tag per interactions -- the id!");
                    continue;
                }

                key += "." + interactions[j].Attributes[0].Value;

                XmlNodeList pages = interactions[j].ChildNodes;
                string[] sequence = new string[pages.Count];
                for (int k = 0; k < pages.Count; k++) {
                    sequence[k] = pages[k].InnerText;
                }
                DialogueDict.Add(key, sequence);
            }
        }

    }

    public static bool textToLoad(string path) {
        if (!DialogueDict.ContainsKey(path)) {
            return false;
        }

        DialogueTextManager.EnqueueTexts(DialogueDict.GetValueOrDefault(path));
        return true;
    }

}

// to use : call textToLoad("FileName.characterName.interactionID")
// need to know which character & which interaction to use >> see XML