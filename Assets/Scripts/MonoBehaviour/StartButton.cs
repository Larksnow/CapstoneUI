using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StartButton : MonoBehaviour
{
    public TMP_InputField userInputField;            // Drag from Inspector
    public StyleButtonGroup styleSelector;           // Drag your StyleButtonGroup script here
    public Communicator communicator;
    public void OnButtonClicked()
    {
        string userInput = userInputField.text;
        if (userInput == "" || communicator.IsProcessing) return; // Check if the input field is empty
        PromptGenerator.Instance.GenerateStylePrompt(userInput, styleSelector.styleSelect);
        string finalPrompt = PromptGenerator.Instance.generatedPrompt;
        Debug.Log("Sending prompt: " + finalPrompt);
        communicator.StartSendingCoroutine(finalPrompt); // Adjust to match your method
        ChatSystem.Instance.AddRightMessage(userInput);
        userInputField.text = "";
        PromptGenerator.Instance.generatedPrompt = ""; // Reset the generated prompt
    }
}
