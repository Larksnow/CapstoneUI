using UnityEngine;
using System.Text;
using System.Collections.Generic;

public class PromptGenerator : MonoBehaviour
{
     public static PromptGenerator Instance { get; private set; }
    [System.Serializable]
    public class StyleDescription
    {
        public string Word_Length;
        public string Syllabic_Word;
        public string Emotion;
        public string Rhetoric;
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
                Word_Length = "The text carefully selects words for their sonic qualities, connotative richness, and imagistic power. It employs precise, evocative terms that create layers of meaning beyond their literal definitions.",
                Syllabic_Word = "The text arranges words with deliberate attention to rhythm, stress patterns, and syllabic balance. It creates musicality through controlled meter, strategic pauses, and sound devices like assonance and consonance.",
                Emotion = "The text distills emotional experiences into concentrated imagery and sensory impressions. It approaches feelings obliquely through metaphor and symbolism rather than direct statement, creating resonance through suggestion.",
                Rhetoric = "The text employs heightened language with compressed syntax and strategic line breaks. It uses literary devices including metaphor, imagery, alliteration, and repetition, while creating meaning through form as well as content."
            }
        },
        {
            "casual", new StyleDescription()
            {
                Word_Length = "The text uses simple, everyday language with common words and phrases. It favors short, familiar terms over specialized vocabulary and avoids unnecessarily complex expressions.",
                Syllabic_Word = "The text employs primarily short, single or double-syllable words that create an accessible, conversational flow. It uses natural speech patterns and rhythms that mirror everyday conversation.",
                Emotion = "The text maintains a relaxed, friendly tone that conveys approachability and authenticity. It expresses emotions directly and naturally, occasionally using humor, enthusiasm, or candid observations.",
                Rhetoric = "The text employs informal structure with contractions, colloquialisms, and occasional sentence fragments. It uses direct address, personal anecdotes, and a conversational approach with simplified explanations."
            }
        },
        {
            "formal", new StyleDescription()
            {
                Word_Length = "The text predominantly uses precise, sophisticated vocabulary to convey authority and expertise. It employs field-appropriate terminology and avoids colloquialisms, slang, and imprecise language.",
                Syllabic_Word = "The text favors multisyllabic, specialized terms that demonstrate erudition and command of the subject matter. It maintains lexical sophistication while ensuring clarity for the intended audience.",
                Emotion = "The text maintains an objective, detached tone that prioritizes logical argumentation over emotional appeal. It presents information dispassionately, focusing on evidence and reasoning rather than sentiment.",
                Rhetoric = "The text employs complex sentence structures with proper subordination and coordination. It avoids contractions and casual expressions while using formal transitions, third-person perspective, and precise syntax."
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
        prompt.AppendLine("You are an expert in analyzing and adapting writing styles.");
        prompt.AppendLine("Your process for style transfer:");
        prompt.AppendLine("1. Consider provided style descriptions");
        prompt.AppendLine("2. Combine analyses to identify key style elements");
        prompt.AppendLine("3. Transform the text while preserving core meaning and applying style patterns");
        prompt.AppendLine("Transform the input text to match the style demonstrated in the examples while maintaining the original meaning and ensuring natural flow.");
        prompt.AppendLine();
        prompt.AppendLine($"Context text to be transformed:");
        prompt.AppendLine(userInput);
        prompt.AppendLine();
        prompt.AppendLine("Provided style descriptions:");
        prompt.AppendLine($"Word_Length: {style.Word_Length}");
        prompt.AppendLine($"Syllabic_Word: {style.Syllabic_Word}");
        prompt.AppendLine($"Emotion: {style.Emotion}");
        prompt.AppendLine($"Rhetoric: {style.Rhetoric}");

        generatedPrompt = prompt.ToString();
    }
}