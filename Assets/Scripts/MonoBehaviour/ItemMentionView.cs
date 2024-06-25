using System.Collections;
using System.Collections.Generic;
using Diego;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemMentionView : MonoBehaviour
{
    // // Start is called before the first frame update
    // private string text;
    // private string desc;
    // private Sprite sprite;
    // private ItemMentionsQuality quality;
    [SerializeField]
    TextMeshPro name;
    [SerializeField]
    TextMeshPro description;
    [SerializeField]
    Image gradientBG;
    [SerializeField]
    Image icon;

    public static readonly Color descriptionBadColor = new Color(
        0.8784313725490196f,
        0.4235294117647059f,
        0.45098039215686275f
    );

    public static readonly Color descriptionGoodColor = new Color(
        0.4235294117647059f,
        0.8784313725490196f,
        0.6431372549019608f
    );

    public static readonly Color gradientBadColor = new Color(
        0.403921568627451f,
        0.1843137254901961f,
        0.2f
    );
    public static readonly Color gradientGoodColor = new Color(
        0.3137254901960784f,
        0.403921568627451f,
        0.1843137254901961f  
    );

    void Start()
    {
        
    }

    public void Setup(Item data, ItemMentionsQuality quality)
    {
        name.text = data.Name;
        description.text = data.Remarks;
        icon.sprite = data.Sprite;
        
        if (quality == ItemMentionsQuality.Useful)
        {
            //add useful item
            description.color = descriptionGoodColor;
            gradientBG.color = gradientGoodColor;
        }
        else
        {
            description.color = descriptionBadColor;
            gradientBG.color = gradientBadColor;
        }
        // populate my children
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
