using UnityEngine;
using TMPro;
using System.Collections;

[RequireComponent(typeof(TMP_Text))]
public class SineWaveText : MonoBehaviour
{
    [Header("Wave Settings")]
    public float waveFrequency = 2f;      // Speed of the wave
    public float waveAmplitude = 10f;     // Height of the wave
    public float waveSpread = 0.5f;       // Horizontal spacing between characters
    public bool animate = true;           // Toggle animation
    
    private TMP_Text _textComponent;
    private TMP_TextInfo _textInfo;
    private bool _isAnimating;
    private Coroutine _animationCoroutine;

    private void Awake()
    {
        _textComponent = GetComponent<TMP_Text>();
    }

    private void OnEnable()
    {
        if (animate) StartAnimation();
    }

    private void OnDisable()
    {
        StopAnimation();
    }

    public void StartAnimation()
    {
        if (_isAnimating) return;
        _isAnimating = true;
        _animationCoroutine = StartCoroutine(AnimateTextWave());
    }

    public void StopAnimation()
    {
        if (!_isAnimating) return;
        _isAnimating = false;
        
        if (_animationCoroutine != null)
        {
            StopCoroutine(_animationCoroutine);
            ResetTextPositions(); // Reset to original positions
        }
    }

    private IEnumerator AnimateTextWave()
    {
        // Cache the text info
        _textInfo = _textComponent.textInfo;
        
        // Create copy of original vertices
        Vector3[][] originalVertices = new Vector3[_textInfo.characterCount][];
        
        while (_isAnimating)
        {
            if (_textComponent.havePropertiesChanged)
            {
                CacheOriginalVertices(ref originalVertices);
            }

            if (_textInfo.characterCount == 0) 
            {
                yield return null;
                continue;
            }

            for (int i = 0; i < _textInfo.characterCount; i++)
            {
                TMP_CharacterInfo charInfo = _textInfo.characterInfo[i];
                if (!charInfo.isVisible) continue;

                // Get the original vertices
                Vector3[] charVertices = originalVertices[i];
                if (charVertices == null) continue;

                // Apply sine wave offset
                float offsetY = Mathf.Sin(Time.time * waveFrequency + i * waveSpread) * waveAmplitude;
                
                // Modify vertices
                for (int j = 0; j < 4; j++) // Each character has 4 vertices (quad)
                {
                    Vector3 orig = charVertices[j];
                    _textInfo.meshInfo[charInfo.materialReferenceIndex].vertices[charInfo.vertexIndex + j] = 
                        orig + new Vector3(0, offsetY, 0);
                }
            }

            // Apply changes to all meshes
            for (int i = 0; i < _textInfo.meshInfo.Length; i++)
            {
                _textInfo.meshInfo[i].mesh.vertices = _textInfo.meshInfo[i].vertices;
                _textComponent.UpdateGeometry(_textInfo.meshInfo[i].mesh, i);
            }

            yield return null;
        }
    }

    private void CacheOriginalVertices(ref Vector3[][] originalVertices)
    {
        _textComponent.ForceMeshUpdate();
        _textInfo = _textComponent.textInfo;
        
        // Resize array if needed
        if (originalVertices.Length < _textInfo.characterCount)
        {
            System.Array.Resize(ref originalVertices, _textInfo.characterCount);
        }

        // Cache original vertex positions
        for (int i = 0; i < _textInfo.characterCount; i++)
        {
            TMP_CharacterInfo charInfo = _textInfo.characterInfo[i];
            if (!charInfo.isVisible) continue;

            int vertexIndex = charInfo.vertexIndex;
            int materialIndex = charInfo.materialReferenceIndex;
            
            originalVertices[i] = new Vector3[4];
            for (int j = 0; j < 4; j++)
            {
                originalVertices[i][j] = _textInfo.meshInfo[materialIndex].vertices[vertexIndex + j];
            }
        }
    }

    private void ResetTextPositions()
    {
        if (_textInfo == null) return;
        
        for (int i = 0; i < _textInfo.characterCount; i++)
        {
            TMP_CharacterInfo charInfo = _textInfo.characterInfo[i];
            if (!charInfo.isVisible) continue;

            // Get the original vertices from the current mesh
            for (int j = 0; j < 4; j++)
            {
                Vector3 orig = _textInfo.meshInfo[charInfo.materialReferenceIndex].vertices[charInfo.vertexIndex + j];
                _textInfo.meshInfo[charInfo.materialReferenceIndex].vertices[charInfo.vertexIndex + j] = orig;
            }
        }

        // Apply changes
        for (int i = 0; i < _textInfo.meshInfo.Length; i++)
        {
            _textInfo.meshInfo[i].mesh.vertices = _textInfo.meshInfo[i].vertices;
            _textComponent.UpdateGeometry(_textInfo.meshInfo[i].mesh, i);
        }
    }
}