using UnityEngine;

public class DialogueInitializer : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(DialogueManager.MeasureLoadSpeed("Sample"));
    }
}
