using UnityEngine;
using TMPro;
using System.Collections;

public class TextGenerator : MonoBehaviour
{
    public TextMeshProUGUI textComponent;
    public float wordDelay = 0.08f;

    public void ShowMessage(string message)
    {
        StartCoroutine(TypeMessage(message));
    }

    IEnumerator TypeMessage(string message)
    {
        textComponent.text = "";
        string[] words = message.Split(' ');

        foreach (string word in words)
        {
            textComponent.text += word + " ";
            yield return new WaitForSeconds(wordDelay);
        }
    }
}
