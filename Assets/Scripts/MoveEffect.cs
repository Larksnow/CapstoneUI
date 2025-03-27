using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(RectTransform))]
public class UIElementAnimator : MonoBehaviour
{
    [Header("Settings")]
    public float yOffset = 500f; // Positive = above screen, Negative = below
    public float moveDuration = 0.7f;
    public Ease moveInEase = Ease.OutExpo;
    public Ease moveOutEase = Ease.OutExpo;

    private RectTransform _rect;
    private Vector2 _originalPos;
    private Vector2 _offsetPos;

    private void Awake()
    {
        _rect = GetComponent<RectTransform>();
        _originalPos = _rect.anchoredPosition;
        _offsetPos = _originalPos + new Vector2(0, yOffset);
        
        // Start at offset position (hidden off-screen)
        _rect.anchoredPosition = _offsetPos;
    }

    // Call this to animate into view
    public void MoveIn()
    {
        _rect.DOAnchorPosY(_originalPos.y, moveDuration)
            .SetEase(moveInEase);
    }

    // Call this to animate out of view
    public void MoveOut()
    {
        _rect.DOAnchorPosY(_offsetPos.y, moveDuration)
            .SetEase(moveOutEase);
    }

    private void OnDestroy()
    {
        _rect.DOKill(); // Clean up tweens
    }
}