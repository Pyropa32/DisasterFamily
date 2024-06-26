using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Diego;
using TMPro;
using Unity.Mathematics;
using UnityEngine;

public class ItemMentionsListView : MonoBehaviour
{
    [SerializeField]
    public GameObject listItemPrefab;
    [SerializeField]
    public GameObject andMorePrefab;

    [SerializeField]
    public GameObject goodItemListUI;

    [SerializeField]
    public GameObject badItemListUI;
    [SerializeField]
    public GameObject scoreUI;

    [SerializeField]
    public uint maxItemDetailsDisplayed; 

    private List<Item> goodItemData = new List<Item>();
    private List<Item> badItemData = new List<Item>();

    private readonly Color minScoreColor = Color.red;
    private readonly Color maxScoreColor = Color.green;
    private readonly Color midScoreColor = Color.white;

    private TextMeshProUGUI scoreText;

    // Start is called before the first frame update
    void Start()
    {
        scoreText = scoreUI.GetComponent<TextMeshProUGUI>();        
    }
    /// <summary>
    /// Once you add everything, make sure to call Show() to cause items to appear
    /// </summary>
    /// <param name="mentionedItem"></param>
    /// <exception cref="ArgumentException"></exception>
    public void AddBadMention(Item mentionedItem)
    {
        if (mentionedItem.Quality != ItemQuality.Useless)
        {
            throw new ArgumentException("Cannot add non-useless item " + mentionedItem.Name + " to Bad Mentions");
        }
        badItemData.Add(mentionedItem);
    }

    /// <summary>
    /// Once you add everything, make sure to call Show() to cause items to appear
    /// </summary>
    /// <param name="mentionedItem"></param>
    /// <exception cref="ArgumentException"></exception>
    public void AddGoodMention(Item mentionedItem)
    {
        if (mentionedItem.Quality != ItemQuality.Required)
        {
            throw new ArgumentException("Cannot add non-required item " + mentionedItem.Name + " to Good Mentions");
        }
        goodItemData.Add(mentionedItem);
    }

    /// <summary>
    /// sets the player score (100% - 0%) based on the items you collected.
    /// </summary>
    /// <param name="value"></param>
    public void SetScore(float value)
    {
        var scoreColor = Color.black;
        if (value > 0.5f)
        {
            scoreColor = Color.Lerp(midScoreColor, maxScoreColor, (value - 0.5f) * 2f);
        }
        else
        {
            scoreColor = Color.Lerp(minScoreColor, midScoreColor, value * 2f);
        }
        scoreText.color = scoreColor;

        var scoreString = String.Format("Survival Rate: {0:0}%", value * 100f);
        scoreText.text = scoreString;
    }

    public void Show()
    {
        // actually does the shit
        
        // add good items
        var goodItemsDataSize = Math.Min(goodItemData.Count, maxItemDetailsDisplayed);
        
        for (int i = 0; i < goodItemsDataSize; i++)
        {
            var goodItem = goodItemData[i];

            var listItem = Instantiate(listItemPrefab, goodItemListUI.transform);
            var listItemView = listItem.GetComponent<ItemMentionView>();
            listItemView.Setup(goodItem, ItemMentionsQuality.Useful);
        }

        var goodItemsDataLeftoverCount = goodItemData.Count - goodItemsDataSize;
        var goodAndMoreUI = Instantiate(andMorePrefab, goodItemListUI.transform);

        var andMoreText = String.Format("+ {0} more items...", goodItemsDataLeftoverCount);
        goodAndMoreUI.GetComponentInChildren<TextMeshProUGUI>().text = andMoreText;
        goodAndMoreUI.GetComponent<UnityEngine.UI.Image>().color = ItemMentionView.gradientGoodColor;

        var badItemsDataSize = Math.Min(badItemData.Count, maxItemDetailsDisplayed); 
        
        for (int i = 0; i < badItemsDataSize; i++)
        {
            var badItem = badItemData[i];

            var listItem = Instantiate(listItemPrefab, badItemListUI.transform);
            var listItemView = listItem.GetComponent<ItemMentionView>();
            listItemView.Setup(badItem, ItemMentionsQuality.Useless);
        }

        // add mention of rest of the items.
        var badItemsDataLeftoverCount = badItemData.Count - badItemsDataSize;
        var badAndMoreUI = Instantiate(andMorePrefab, badItemListUI.transform);
        
        andMoreText = String.Format("+ {0} more items...", badItemsDataLeftoverCount);
        badAndMoreUI.transform.GetChild(0).GetChild(0).GetComponent<TMP_Text>().text = andMoreText;
        badAndMoreUI.GetComponent<UnityEngine.UI.Image>().color = ItemMentionView.gradientBadColor;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
