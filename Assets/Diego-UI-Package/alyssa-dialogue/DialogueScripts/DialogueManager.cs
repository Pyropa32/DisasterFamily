using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using System.Threading;


// Next up:
// Include Actor data
// Actor Expression Data as well
public class Page
{
    public string dialogue;
    public string avatar;
    public string name;
    public Page(string dialogue, string avatar, string name){
        this.dialogue = dialogue;
        this.avatar = avatar;
        this.name = name;
    }
}

public class DialogueManager {
    // load dialogue from XML into dict
    private static Dictionary<string, Page[]> DialogueDict = new Dictionary<string, Page[]>();
    private static Dictionary<string, Sprite> SpriteDict = new Dictionary<string, Sprite>();

    public static void loadFromFile(string path)
    {
        if (DialogueDict.ContainsKey(path))
        {
            return;
        }
        TextAsset textAsset = (TextAsset)Resources.Load(path);
        string asset = textAsset.text;
        Thread IOthread = new Thread(() => loadFromFileThread(path, asset));
        IOthread.Start();
    }

    public static Sprite getSprite(string path)
    {
        Sprite returnVal;
        if (SpriteDict.ContainsKey(path)) {
            SpriteDict.TryGetValue(path, out returnVal);
            return returnVal;
        }
        returnVal = Resources.Load<Sprite>(path);
        SpriteDict.Add(path, returnVal);
        return returnVal;
    }

    // thread for performance 
    public static void loadFromFileThread(string path, string textAsset) {
        XmlDocument xmldoc = new XmlDocument();
        xmldoc.LoadXml(textAsset);
        XmlNodeList dialogue = xmldoc.GetElementsByTagName("dialogue");

        //should only be 1 dialogue tag in XML
        if (dialogue.Count != 1) {
            Debug.LogError("There should only be 1 dialogue tag! Check " + path);
            return;
        }
        // follows XML format : dialogue > character name > interaction id > page
        XmlNodeList characters = dialogue[0].ChildNodes;
        for (int i = 0; i < characters.Count; i++) {
            if (characters[i].Attributes.Count != 2)
            {
                Debug.LogError("We only want two attribute tag per character -- the name and the sprite!");
                continue;
            }
            string name = characters[i].Attributes[0].Value;
            string defaultAvatar = "AvatarSprites/" + name + "/" + characters[i].Attributes[1].Value;

            XmlNodeList interactions = characters[i].ChildNodes;
            for (int j = 0; j < interactions.Count; j++) {
                //changes dict key to be specific per character interaction
                string key = path;
                key += "." + name;

                if (interactions[j].Attributes.Count != 1 || interactions[j].Attributes[0].Name != "id") {
                    Debug.LogError("We only want one attribute tag per interactions -- the id!");
                    continue;
                }

                key += "." + interactions[j].Attributes[0].Value;

                XmlNodeList pages = interactions[j].ChildNodes;
                Page[] sequence = new Page[pages.Count];
                for (int k = 0; k < pages.Count; k++) {
                    Page currPage = new Page(pages[k].InnerText, defaultAvatar, name);
                    if (pages[k].Attributes.Count == 1)
                    {
                        string currAvatar = "AvatarSprites/" + name + "/" + pages[k].Attributes[0].Value;
                        currPage.avatar = currAvatar;
                    }
                    sequence[k] = currPage;
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

