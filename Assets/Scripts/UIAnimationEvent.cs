using UnityEngine;
using System.Collections.Generic;
using TMPro;
using DG.Tweening;

public class UIAnimationEvents : MonoBehaviour
{
    public static UIAnimationEvents Instance { get; private set; }

    [Header("Animation Targets")]
    public List<UIElementAnimator> targets = new List<UIElementAnimator>();
    public TextMeshProUGUI mainpage;
    private void Awake()
    {
        // Singleton pattern
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        
        Instance = this;
    }

    private void Update()
    {
        // Check if Ctrl is pressed along with I or O
        bool ctrlPressed = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift); 

        if (ctrlPressed && Input.GetKeyDown(KeyCode.I))
        {
            mainpage.DOFade(0, 0.8f).SetEase(Ease.OutExpo).OnComplete(() => 
            {
                TriggerMoveIn();
                mainpage.gameObject.SetActive(false);
            });
        }
        else if (ctrlPressed && Input.GetKeyDown(KeyCode.O))
        {
            TriggerMoveOut();
            mainpage.gameObject.SetActive(true);
            mainpage.DOFade(1, 0.8f).SetEase(Ease.InExpo);
        }
    }

    public void TriggerMoveIn()
    {
        foreach (var animator in targets)
        {
            if (animator != null)
            {
                animator.MoveIn();
            }
        }
    }

    public void TriggerMoveOut()
    {
        foreach (var animator in targets)
        {
            if (animator != null)
            {
                animator.MoveOut();
            }
        }
    }

    // Editor helper to auto-find all UIElementAnimators
    [ContextMenu("Find All Animators")]
    public void FindAllAnimators()
    {
        targets.Clear();
        targets.AddRange(FindObjectsOfType<UIElementAnimator>());
    }
}