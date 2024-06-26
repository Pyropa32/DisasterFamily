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
    GameObject nameUI;
    [SerializeField]
    GameObject descriptionUI;
    [SerializeField]
    TextMeshProUGUI nameTMP;
    [SerializeField]
    TextMeshProUGUI descriptionTMP;
    [SerializeField]
    Image gradientBG;
    [SerializeField]
    Image icon;

    //private TextMeshProUGUI nameTMP;
    //private TextMeshProUGUI descriptionTMP;

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
        nameTMP = nameUI.GetComponent<TextMeshProUGUI>();
        descriptionTMP = descriptionUI.GetComponent<TextMeshProUGUI>();
    }

    public void Setup(Item data, ItemMentionsQuality quality)
    {
        nameTMP.text = data.Name;
        descriptionTMP.text = data.Remarks;
        icon.sprite = data.Sprite;
        
        if (quality == ItemMentionsQuality.Useful)
        {
            //add useful item
            descriptionTMP.color = descriptionGoodColor;
            gradientBG.color = gradientGoodColor;
        }
        else
        {
            descriptionTMP.color = descriptionBadColor;
            gradientBG.color = gradientBadColor;
        }
        // populate my children
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
