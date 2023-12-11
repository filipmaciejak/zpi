using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem.EnhancedTouch;
using ETouch = UnityEngine.InputSystem.EnhancedTouch;

public class Button : MonoBehaviour
{
    public event Action<string> StartedPress;
    public event Action<string> EndedPress;

    public Sprite unpressedSprite;
    public Sprite pressedSprite;
    
    private Image _buttonImage;
    
    private TouchHelper _touchHelper;

    private RectTransform _thisRectTransform;
    private Finger _buttonFinger;

    [NonSerialized]
    public Boolean IsPressed;

    private void Awake()
    {
        _touchHelper = GetComponentInParent<TouchHelper>();
        _thisRectTransform = GetComponent<RectTransform>();
	_buttonImage = GetComponent<Image>();
    }

    private void OnEnable()
    {
        EnhancedTouchSupport.Enable();
        ETouch.Touch.onFingerDown += OnFingerDown;
        ETouch.Touch.onFingerMove += OnFingerMove;
        ETouch.Touch.onFingerUp += OnFingerUp;
    }

    private void OnDisable()
    {
        ETouch.Touch.onFingerDown -= OnFingerDown;
        ETouch.Touch.onFingerMove -= OnFingerMove;
        ETouch.Touch.onFingerUp -= OnFingerUp;
    }

    private void OnFingerDown(Finger finger)
    {
        if (_buttonFinger != null)
            return;
        
        if (!FingerInside(finger))
            return;

        _buttonFinger = finger;

        IsPressed = true;
	_buttonImage.sprite = pressedSprite;
        StartedPress?.Invoke(this.name);
    }

    private void OnFingerMove(Finger finger)
    {
        if (finger == null)
            return;
        
        if (finger != _buttonFinger)
            return;

        if (FingerInside(_buttonFinger))
            return;
        
        OnFingerUp(finger);
    }

    private void OnFingerUp(Finger finger)
    {
        if (finger == null)
            return;
        
        if (finger != _buttonFinger)
            return;

        _buttonFinger = null;
        IsPressed = false;
	_buttonImage.sprite = unpressedSprite;
        EndedPress?.Invoke(this.name);
    }

    private Vector2 GetFingerPos(Finger finger)
    {
        return _touchHelper.ScaleScreenToCanvas(finger.screenPosition);
    }

    private Boolean FingerInside(Finger finger)
    {
        var rect = _touchHelper.GetRectTransformCanvasPos(_thisRectTransform);
        
        return rect.Contains(GetFingerPos(finger));
    }
}
