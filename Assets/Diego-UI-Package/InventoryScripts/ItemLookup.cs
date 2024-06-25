using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
namespace Diego
{
    public class ItemLookup
    {
        public static Item GetItemFromID(int id)
        {
            return InventoryManager.GetItemFromID(id);
        }

        public static Item GetItemFromSprite(Sprite sprite)
        {
            for (int i = 0; i < InventoryManager.SingletonInstance.sprites.Length; i++)
            {
                if (InventoryManager.SingletonInstance.sprites[i] == sprite)
                {
                    return InventoryManager.GetItemFromID(i);
                }
            }
            return Item.Empty;
        }

        public static Item GetItemFromName(string name)
        {
            // change to dictionary in future and load at start
            // read from xml file to load item from given name
            TextAsset textAsset = (TextAsset)Resources.Load("ItemList");
            XmlDocument xmldoc = new XmlDocument();
            xmldoc.LoadXml(textAsset.text);
            XmlNodeList items = xmldoc.GetElementsByTagName("item");

            string[] value = new string[items.Count];
            for (int i = 0; i < items.Count; i++)
            {
                XmlNodeList item = items[i].ChildNodes;
                if (item[1].InnerText.Equals(name))
                {
                    return GetItemFromID(int.Parse(item[0].InnerText));
                }
            }
            return Item.Empty;
        }
    }
}
