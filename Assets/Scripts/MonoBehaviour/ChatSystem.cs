using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class ChatSystem : MonoBehaviour
{
    public static ChatSystem Instance { get; private set; }
    [Header("References")]
    public ScrollRect scrollRect;
    public RectTransform content;
    public GameObject leftChatBoxPrefab;  // Pivot: Top-Center
    public GameObject rightChatBoxPrefab; // Pivot: Top-Center
    public float spacing = 20f;

    [Header("Positioning")]
    public float leftMargin = 50f;
    public float rightMargin = 50f;
    
    private float contentHeight = 0;
    private bool wasScrolledToBottom = true;
    public float wordDelay = 0.08f;

    void OnEnable() => scrollRect.onValueChanged.AddListener(OnScrollChanged);
    void OnDisable() => scrollRect.onValueChanged.RemoveListener(OnScrollChanged);

    public void AddLeftMessage(string message) => StartCoroutine(AddMessageCoroutine(leftChatBoxPrefab, message, false));
    public void AddRightMessage(string message) => StartCoroutine(AddMessageCoroutine(rightChatBoxPrefab, message, true));

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
    IEnumerator AddMessageCoroutine(GameObject prefab, string message, bool isRight)
    {
        // Create new chat box
        GameObject newChatBox = Instantiate(prefab, content);
        // if (isRight)
        // {
        //     SetTextToChatBox(newChatBox, message);
        // }else
        // {
        //     StartCoroutine(TypeMessage(message, newChatBox.GetComponentInChildren<TextMeshProUGUI>()));
        // }
        
        SetTextToChatBox(newChatBox, message);
        // Wait for layout calculation (2 frames)
        yield return null; 
        yield return null;
        
        // Position the chat box
        PositionChatBox(newChatBox.GetComponent<RectTransform>(), isRight);
        
        // Update scroll
        UpdateScrollView();
    }

    void SetTextToChatBox(GameObject chatBox, string message)
    {
        // Adjust this based on your chatbox structure
        var textComponent = chatBox.GetComponentInChildren<TMPro.TMP_Text>();
        if(textComponent) textComponent.text = message;
    }

    void PositionChatBox(RectTransform rt, bool isRight)
    {
        float messageHeight = rt.rect.height;
        float xPos = isRight ? -rightMargin : leftMargin;
        
        rt.anchoredPosition = new Vector2(
            xPos,
            -contentHeight - (contentHeight > 0 ? spacing : 0)
        );
        
        contentHeight += messageHeight + (contentHeight > 0 ? spacing : 0);
        content.sizeDelta = new Vector2(content.sizeDelta.x, contentHeight);
    }

    void UpdateScrollView()
    {
        if(wasScrolledToBottom)
        {
            Canvas.ForceUpdateCanvases();
            scrollRect.verticalNormalizedPosition = 0;
            Canvas.ForceUpdateCanvases();
        }
    }

    IEnumerator TypeMessage(string message, TextMeshProUGUI textComponent)
    {
        textComponent.text = "";
        string[] words = message.Split(' ');

        foreach (string word in words)
        {
            textComponent.text += word + " ";
            yield return new WaitForSeconds(wordDelay);
        }
    }

    void OnScrollChanged(Vector2 pos) => wasScrolledToBottom = pos.y <= 0.01f;
}