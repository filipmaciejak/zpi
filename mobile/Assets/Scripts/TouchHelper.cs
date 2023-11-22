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
        //Debug.Log(rect.position.x + " " + rect.position.y + " " + rect.size.x + " " + rect.size.y);
        var fingerPos = ScaleScreenToCanvas(finger.screenPosition);
        //Debug.Log(fingerPos);
        return rect.Contains(fingerPos);
    }

    public Boolean FingerInsideBounds(Bounds bounds, Finger finger)
    {
        var rect = BoundsToScreenRect(bounds);
        //Debug.Log(rect.position.x + " " + rect.position.y + " " + rect.size.x + " " + rect.size.y);
        //Debug.Log(finger.screenPosition);
        return rect.Contains(finger.screenPosition);
    }

    public Rect BoundsToScreenRect(Bounds bounds)
    {
        // Get mesh origin and farthest extent (this works best with simple convex meshes)
        Vector3 origin = Camera.main.WorldToScreenPoint(new Vector3(bounds.min.x, bounds.max.y, 0f));
        Vector3 extent = Camera.main.WorldToScreenPoint(new Vector3(bounds.max.x, bounds.min.y, 0f));

        // Create rect in screen space and return - does not account for camera perspective
        return new Rect(origin.x, Screen.height - origin.y, extent.x - origin.x, origin.y - extent.y);
    }
}
