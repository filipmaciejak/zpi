using System;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using ETouch = UnityEngine.InputSystem.EnhancedTouch;

public class JoystickController : MonoBehaviour
{
    public event Action<Vector2> StartedControlling;
    public event Action<Vector2> MovedControls;
    public event Action<Vector2> StoppedControlling;
    
    
    [SerializeField] private JoystickGraphical joystickGraphical;
    [SerializeField] private Vector2 joystickSize;
    [SerializeField] private RectTransform legalJoystickSpawn;

    private TouchHelper _touchHelper;
    private Vector2 _joystickSize;
    private Finger _movementFinger;
    private Vector2 _movementAmount;

    private void Awake()
    {
        _touchHelper = GetComponentInParent<TouchHelper>();
        _joystickSize = joystickSize;
        _movementFinger = null;
        _movementAmount = Vector2.zero;
    }

    private void OnEnable()
    {
        EnhancedTouchSupport.Enable();
        ETouch.Touch.onFingerDown += OnFingerDown;
        ETouch.Touch.onFingerMove += OnFingerMove;
        ETouch.Touch.onFingerUp += OnFingerUp;
    }

    // Start is called before the first frame update
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
    }

    private void OnDisable()
    {
        ETouch.Touch.onFingerDown -= OnFingerDown;
        ETouch.Touch.onFingerMove -= OnFingerMove;
        ETouch.Touch.onFingerUp -= OnFingerUp;
    }

    private void OnFingerDown(Finger finger)
    {
        if (_movementFinger != null)
            return;

        var fingerPos = _touchHelper.ScaleScreenToCanvas(finger.screenPosition);

        if (!_touchHelper.GetRectTransformCanvasPos(legalJoystickSpawn).Contains(fingerPos))
            return;

        _movementFinger = finger;
        _movementAmount = Vector2.zero;

        joystickGraphical.gameObject.SetActive(true);
        joystickGraphical.rectTransform.sizeDelta = _joystickSize;
        joystickGraphical.rectTransform.anchoredPosition = ClampStartPos(_touchHelper.ScaleScreenToCanvas(finger.screenPosition));
        UpdateKnobPos();
        StartedControlling?.Invoke(_movementAmount);
    }

    private void OnFingerMove(Finger finger)
    {
        if (_movementFinger != finger)
            return;

        UpdateKnobPos();
        MovedControls?.Invoke(_movementAmount);
    }

    private void OnFingerUp(Finger finger)
    {
        if (_movementFinger == null)
            return;

        if (_movementFinger != finger)
            return;

        joystickGraphical.gameObject.SetActive(false);

        _movementFinger = null;
        _movementAmount = Vector2.zero;
        StoppedControlling?.Invoke(_movementAmount);
    }

    private void UpdateKnobPos()
    {
        var joystickPos = joystickGraphical.rectTransform.anchoredPosition;
        var maxRange = _joystickSize.x / 2f;
        var touchPosition = _touchHelper.ScaleScreenToCanvas(_movementFinger.screenPosition);

        Vector2 knobPos;
        if (Vector2.Distance(touchPosition, joystickPos) > maxRange)
            knobPos = (touchPosition - joystickPos).normalized * maxRange;
        else
            knobPos = touchPosition - joystickPos;

        joystickGraphical.knob.anchoredPosition = knobPos;
        _movementAmount = knobPos / maxRange;
    }

    private Vector2 ClampStartPos(Vector2 pos)
    {
        if (pos.x < _joystickSize.x / 2f)
            pos.x = _joystickSize.x / 2f;

        if (pos.y < _joystickSize.y / 2f)
            pos.y = _joystickSize.y / 2f;
        else if (pos.y > 1080 - _joystickSize.y / 2f)
            pos.y = 1080 - _joystickSize.y / 2f;

        return pos;
    }

}
