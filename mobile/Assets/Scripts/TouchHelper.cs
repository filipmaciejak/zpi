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

    public Boolean FingerInsideBounds(Bounds bounds, Finger finger)
    {
        var worldFinger = Camera.main.ScreenToWorldPoint(finger.screenPosition);
        worldFinger = new Vector3(worldFinger.x, worldFinger.y, bounds.center.z);
        return bounds.Contains(worldFinger);
    }
}
