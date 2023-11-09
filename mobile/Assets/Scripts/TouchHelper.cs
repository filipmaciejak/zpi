using System;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;

public class TouchHelper : MonoBehaviour
{
    private Canvas _canvas;
    private RectTransform _rectTransform;
    private void Awake()
    {
        _canvas = GetComponent<Canvas>();
        _rectTransform = GetComponent<RectTransform>();
    }

    private void OnEnable()
    {
        EnhancedTouchSupport.Enable();
    }

    // THIS ONLY WORKS WHEN THE RECCTRANSFORM IS A DIRECT
    // DESCENDANT OF A CANVAS OR A RECTTRANSFORM THAT IS THE EXACT SIZE OF THE SCREEN
    // it's bad, but it's unity
    public Rect GetRectTransformCanvasPos(RectTransform rectTransform)
    {
        var relativePosition = rectTransform.anchoredPosition;
        var canvasRect = _rectTransform.rect;
        var rectTransformAnchor = rectTransform.anchorMin;
        var anchorPosition = new Vector2(rectTransformAnchor.x * canvasRect.width, rectTransformAnchor.y * canvasRect.height);
        
        var rectTransformRect = rectTransform.rect;
        var size = rectTransformRect.size;
        
        var truePosition = anchorPosition + relativePosition + rectTransformRect.position;
        
        return new Rect(truePosition, size);
    }
    
    // THIS ONLY WORKS WHEN THE RECCTRANSFORM IS A DIRECT
    // DESCENDANT OF A CANVAS OR A RECTTRANSFORM THAT IS THE EXACT SIZE OF THE SCREEN
    public Vector2 GetAnchorPosRelativeToCanvas(RectTransform rectTransform)
    {
        var RTAnchor = rectTransform.anchorMin;
        var canvasRect = _rectTransform.rect;

        return new Vector2(RTAnchor.x * canvasRect.width, RTAnchor.y * canvasRect.height);
    }
    
    public Vector2 ScaleScreenToCanvas(Vector2 screenPos)
    {
        Vector2 resultPos = screenPos;

        float scaleFactor = _canvas.scaleFactor;
        
        resultPos.x /= scaleFactor;
        resultPos.y /= scaleFactor;
        
        return resultPos;
    }

    public Boolean FingerInsideRectTransform(RectTransform rectTransform, Finger finger)
    {
        var rect = GetRectTransformCanvasPos(rectTransform);
        var fingerPos = ScaleScreenToCanvas(finger.screenPosition);
        return rect.Contains(fingerPos);
    }
}
