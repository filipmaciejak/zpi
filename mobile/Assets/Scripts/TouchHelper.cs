using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;

public class TouchHelper : MonoBehaviour
{
    public Canvas canvas;
    private void Awake()
    {
        canvas = GetComponent<Canvas>();
    }

    private void OnEnable()
    {
        EnhancedTouchSupport.Enable();
    }

    public Rect GetRectTransformCanvasPos(RectTransform rectTransform)
    {
        var relativePosition = rectTransform.anchoredPosition;
        
        var canvasRect = canvas.pixelRect;
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

        float scaleFactor = canvas.scaleFactor;
        
        resultPos.x /= scaleFactor;
        resultPos.y /= scaleFactor;
        
        return resultPos;
    }
}
