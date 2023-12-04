using System;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using ETouch = UnityEngine.InputSystem.EnhancedTouch;

namespace MovementModule
{
    public class SteeringSlider : MonoBehaviour
    {
        
        public event Action<float> StartedControlling;
        public event Action<float> MovedControls;
        public event Action<float> StoppedControlling;
        
        private RectTransform _rectTransform;
        private TouchHelper _touchHelper;
        private Finger _sliderFinger;

        public float sliderNeutralLock;

        public RectTransform sliderRectTransform;

        [NonSerialized]
        public float SliderPosition;
        
        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
            _touchHelper = GetComponentInParent<TouchHelper>();
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

        public void UpdateSliderPosition(float pos)
        {
            if (_sliderFinger != null)
                return;
            SetSliderPosition(pos);
        }
        
        public void SetSliderPosition(float pos)
        {
            SliderPosition = pos;
            var newX = SliderPosition * (_rectTransform.rect.width / 2);
            sliderRectTransform.anchoredPosition = new Vector2(newX,
                sliderRectTransform.anchoredPosition.y);
        }

        private void OnFingerDown(Finger finger)
        {
            if (_sliderFinger != null)
                return;

            if (!_touchHelper.FingerInsideRectTransform(_rectTransform, finger))
                return;

            _sliderFinger = finger;
            
            OnFingerMove(finger);
            StartedControlling?.Invoke(SliderPosition);
        }

        private void OnFingerMove(Finger finger)
        {
            if (_sliderFinger != finger)
                return;

            float newPos = GetNormalizedSliderPosition(finger);
            SetSliderPosition(newPos);
            MovedControls?.Invoke(SliderPosition);
        }

        private void OnFingerUp(Finger finger)
        {
            if (finger != _sliderFinger)
                return;

            _sliderFinger = null;
            if (NeutralLockCheck())
                SetSliderPosition(0);
            
            StoppedControlling?.Invoke(SliderPosition);
        }

        private float GetNormalizedSliderPosition(Finger finger)
        {
            var rectCanvas = _touchHelper.GetRectTransformCanvasPos(_rectTransform);
            var fingerPos = _touchHelper.ScaleScreenToCanvas(finger.screenPosition);
            
            var width = rectCanvas.width;
            float normalizedUnclamped = (fingerPos.x - rectCanvas.x - (width / 2)) / width*2;

            float result = Math.Clamp(normalizedUnclamped, -1f, 1f);

            return result;
        }

        private bool NeutralLockCheck()
        {
            if (Math.Abs(SliderPosition) <= sliderNeutralLock)
                return true;
            return false;
        }
    }
}
