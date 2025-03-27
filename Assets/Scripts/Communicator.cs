using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using System;

public class Communicator : MonoBehaviour
{
    [Header("Ollama Settings")]
    public string ollamaUrl = "http://localhost:11434/api/generate";
    public string modelName = "hf.co/whj9068/Demo_1";
    public TextGenerator textGenerator;
    public event Action OnProcessingStarted;
    public event Action OnProcessingFinished;
    [TextArea] public string prompt = "What's the weather today?";
    
    [Header("Request Settings")]
    public float timeoutSeconds = 40f;
    
    public bool IsProcessing { get; private set; }
    public event Action<string> OnResponseReceived;
    public event Action<string> OnRequestFailed;

    public void StartSendingCoroutine(string prompt)
    {
        if (IsProcessing)
        {
            Debug.LogWarning("Request already in progress");
            return;
        }
        StartCoroutine(SendPromptToOllama(prompt));
    }

    private IEnumerator SendPromptToOllama(string prompt)
    {
        IsProcessing = true;
        OnProcessingStarted?.Invoke();
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

            // Start the request and timeout timer
            UnityWebRequestAsyncOperation asyncOp = request.SendWebRequest();
            float startTime = Time.time;
            bool timedOut = false;

            // Wait for request to complete or timeout
            while (!asyncOp.isDone)
            {
                if (Time.time - startTime > timeoutSeconds)
                {
                    timedOut = true;
                    request.Abort();
                    break;
                }
                yield return null;
            }

            if (timedOut)
            {
                string timeoutMessage = "Timeout: Try again later";
                Debug.LogError(timeoutMessage);
                OnRequestFailed?.Invoke(timeoutMessage);
                ChatSystem.Instance.AddLeftMessage(timeoutMessage);
            }
            else if (request.result == UnityWebRequest.Result.Success)
            {
                string responseText = ExtractResponseText(request.downloadHandler.text);
                Debug.Log("Ollama Response: " + responseText);
                OnResponseReceived?.Invoke(responseText);
                ChatSystem.Instance.AddLeftMessage(responseText);
            }
            else
            {
                string errorMessage = "Error: " + request.error;
                Debug.LogError(errorMessage);
                OnRequestFailed?.Invoke(errorMessage);
                ChatSystem.Instance.AddLeftMessage(errorMessage);
            }
        }

        IsProcessing = false;
        OnProcessingFinished?.Invoke();
        yield break;
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
            return "Error: Could not parse response";
        }
    }

    [System.Serializable]
    public class OllamaResponse
    {
        public string response;
    }

    [System.Serializable]
    public class OllamaRequest
    {
        public string model;
        public string prompt;
        public bool stream;
    }
}