using UnityEngine;
using TMPro;
using DG.Tweening;

public class ProcessingIndicator : MonoBehaviour
{
    [Header("References")]
    public TMP_Text processingText;
    public Communicator communicator;
    
    [Header("Animation Settings")]
    public float fadeDuration = 1f;
    public float minAlpha = 0.3f;
    public float maxAlpha = 0.8f;
    
    private Sequence _breathSequence;
    private bool _isVisible;

    private void Awake()
    {
        // Initialize hidden
        processingText.alpha = 0f;
        processingText.gameObject.SetActive(false);
        
        // Setup DOTween sequence for breath effect
        _breathSequence = DOTween.Sequence()
            .Append(processingText.DOFade(maxAlpha, fadeDuration))
            .Append(processingText.DOFade(minAlpha, fadeDuration))
            .SetLoops(-1, LoopType.Restart)
            .Pause();
        
        // Subscribe to communicator events
        communicator.OnProcessingStarted += ShowIndicator;
        communicator.OnProcessingFinished += HideIndicator;
    }

    private void OnDestroy()
    {
        _breathSequence?.Kill();
        communicator.OnProcessingStarted -= ShowIndicator;
        communicator.OnProcessingFinished -= HideIndicator;
    }

    private void ShowIndicator()
    {
        if (_isVisible) return;
        
        _isVisible = true;
        processingText.gameObject.SetActive(true);
        processingText.alpha = 0f;
        
        // Start the breath animation
        _breathSequence.Restart();
    }

    private void HideIndicator()
    {
        if (!_isVisible) return;
        
        _isVisible = false;
        
        // Fade out completely before hiding
        processingText.DOFade(0f, fadeDuration / 2f)
            .OnComplete(() => {
                processingText.gameObject.SetActive(false);
                _breathSequence.Pause();
            });
    }
}