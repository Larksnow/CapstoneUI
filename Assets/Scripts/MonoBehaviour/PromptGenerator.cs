using UnityEngine;
using System.Text;
using System.Collections.Generic;

public class PromptGenerator : MonoBehaviour
{
    public static PromptGenerator Instance { get; private set; }
    
    [System.Serializable]
    public class StyleDescription
    {
        public string styleprompt;
    }

    [Header("Output")]
    [TextArea(10, 20)]
    public string generatedPrompt = "";
    
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
    }
    
    private readonly Dictionary<string, StyleDescription> styleDictionary = new Dictionary<string, StyleDescription>()
    {
        {
            "poetry", new StyleDescription()
            {
                styleprompt = "You are an expert in analyzing and adapting writing styles.\nTransform the content text to poetry writing style while maintaining the original"
            }
        },
        {
            "casual", new StyleDescription()
            {
                styleprompt = "You are an expert in analyzing and adapting writing styles.\nTransform the content text to casual writing style while maintaining the original"
            }
        },
        {
            "formal", new StyleDescription()
            {
                styleprompt = "You are an expert in analyzing and adapting writing styles.\nTransform the content text to formal writing style while maintaining the original"
            }
        }
    };

    public void GenerateStylePrompt(string userInput, string selectedStyle)
    {
        if (!styleDictionary.ContainsKey(selectedStyle))
        {
            Debug.Log("Use custom style");
            generatedPrompt = userInput;
            return;
        }
        
        StyleDescription style = styleDictionary[selectedStyle];
        
        StringBuilder prompt = new StringBuilder();
        prompt.AppendLine(style.styleprompt);
        prompt.AppendLine();
        prompt.AppendLine("Text to transform:");
        prompt.AppendLine(userInput);

        generatedPrompt = prompt.ToString();
    }
}