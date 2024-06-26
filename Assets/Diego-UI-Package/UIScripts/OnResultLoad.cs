using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Diego;
using System.Linq;

namespace Diego {
    public class OnResultLoad : MonoBehaviour {

        void FixedUpdate() {
            int maxNum = DropOff.GetMaxNum();
            List<Item> items = DropOff.GetItemsAndKill();
            ItemMentionsListView mentions = FindObjectOfType<ItemMentionsListView>();
            if (items.Count == 0) {
                mentions.SetScore(0);
                mentions.Show();
                Destroy(gameObject);
                return;
            }
            int countGood = 0;

            // randomize the items so that we get to see different mentions
            items = items.OrderBy(x => Random.Range(int.MinValue, int.MaxValue)).ToList();

            foreach (Item item in items) {
                Debug.Log(item.Quality);
                Debug.Log(item.ID);
                Debug.Log(item.Name);
                if (item.Quality.Equals(ItemQuality.Required)) {
                    countGood++;
                    mentions.AddGoodMention(item);
                }
                else {
                    mentions.AddBadMention(item);
                }
            }
            mentions.SetScore(countGood/(float)maxNum);
            mentions.Show();
            Destroy(gameObject);
        }
    }
}
