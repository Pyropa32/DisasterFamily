using UnityEngine;
using Diego;
using System;

public class ItemsUniverseInitializer : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(ItemsUniverse.MeasureLoadSpeed("ItemList"));
    }
}
