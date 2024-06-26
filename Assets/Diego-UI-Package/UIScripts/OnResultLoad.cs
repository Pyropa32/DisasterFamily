using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Diego;

namespace Diego {
    public class OnResultLoad : MonoBehaviour {
        void FixedUpdate() {
            List<Item> items = DropOff.GetItemsAndKill();
            ItemMentionsListView mentions = FindObjectOfType<ItemMentionsListView>();
            if (items.Count == 0) {
                mentions.SetScore(0);
                mentions.Show();
                Destroy(gameObject);
                return;
            }
            int countGood = 0;
            foreach (Item item in items) {
                if (item.Quality == ItemQuality.Required) {
                    countGood++;
                    mentions.AddGoodMention(item);
                }
                else {
                    mentions.AddBadMention(item);
                }
            }
            mentions.SetScore(countGood/(float)items.Count);
            mentions.Show();
            Destroy(gameObject);
        }
    }
}
