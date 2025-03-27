using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class Communicator : MonoBehaviour
{
    [Header("Ollama Settings")]
    public string ollamaUrl = "http://localhost:11434/api/generate"; // Replace with your Ollama endpoint
    public string modelName = "hf.co/whj9068/Demo_1"; // Replace with your model name
    public TextGenerator textGenerator;
    [TextArea] public string prompt = "What's the weather today?";

    void Update()
    {
        // if (Input.GetKeyDown(KeyCode.Space))
        // {
        //     Debug.Log("Sending prompt to Ollama...");
        //     StartCoroutine(SendPromptToOllama(prompt));
        // }
    }


    public void StartSendingCoroutine(string prompt)
    {
        StartCoroutine(SendPromptToOllama(prompt));
    }

    private IEnumerator SendPromptToOllama(string prompt)
    {
        string jsonBody = JsonUtility.ToJson(new OllamaRequest
        {
            model = modelName,
            prompt = prompt,
            stream = false
        });

        using (UnityWebRequest request = new UnityWebRequest(ollamaUrl, "POST"))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonBody);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                string responseText = ExtractResponseText(request.downloadHandler.text);
                Debug.Log("Ollama Response: " + responseText);
                ChatSystem.Instance.AddLeftMessage(responseText);
            }
            else
            {
                Debug.LogError("Error: " + request.error);
            }
        }
    }

    [System.Serializable]
    public class OllamaResponse
    {
        public string response;
    }

    public string ExtractResponseText(string json)
    {
        try
        {
            OllamaResponse parsed = JsonUtility.FromJson<OllamaResponse>(json);
            return parsed.response;
        }
        catch (System.Exception e)
        {
            Debug.LogError("Failed to parse response: " + e.Message);
            return "";
        }
    }

    [System.Serializable]
    public class OllamaRequest
    {
        public string model;
        public string prompt;
        public bool stream;
    }
}
