using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class StyleButtonGroup : MonoBehaviour
{
    public List<Button> buttons;
    public Color selectedColor = new Color32(100, 149, 255, 255); 
    public Color defaultColor = new Color32(255, 255, 255, 125);  // Semi-transparent white
    public string styleSelect = "";
    private Button currentSelected;

    void Start()
    {
        foreach (var btn in buttons)
        {
            btn.onClick.AddListener(() => OnButtonClicked(btn));
            SetButtonColor(btn, defaultColor);
        }
    }

    void OnButtonClicked(Button clicked)
    {
        if (currentSelected != null)
            SetButtonColor(currentSelected, defaultColor);

        SetButtonColor(clicked, selectedColor);
        currentSelected = clicked;

        // Update style prompt based on button name
        switch (clicked.name)
        {
            case "Casual":
                styleSelect = "casual";
                break;
            case "Formal":
                styleSelect = "formal";
                break;
            case "Poetry":
                styleSelect = "poetry";
                break;
            case "Custom":
                styleSelect = "custom";
                break;
            default:
                styleSelect = "custom";
                break;
        }

        Debug.Log($"Selected style: " + styleSelect);
    }

    void SetButtonColor(Button btn, Color color)
    {
        var colors = btn.colors;
        colors.normalColor = color;
        colors.highlightedColor = color;
        colors.selectedColor = color;
        colors.pressedColor = color;
        btn.colors = colors;
    }
}
