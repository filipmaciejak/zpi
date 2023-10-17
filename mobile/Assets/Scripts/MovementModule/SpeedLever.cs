using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using ETouch = UnityEngine.InputSystem.EnhancedTouch;

namespace MovementModule
{
    public class SpeedLever : MonoBehaviour
    {
        public event Action<float> SpeedChanged;
        
        // Order of speeds DOES matter
        // First speed is full ahead
        // Last speed is full astern
        public List<RectTransform> speeds;
        public RectTransform lever;

        private float _speed;
        private int _currentSpeedSection;

        private TouchHelper _touchHelper;
        private RectTransform _mainRectTransform;
        private Finger _speedFinger;

        public void UpdateLeverPosition(float pos)
        {
            if (_speedFinger != null)
                return;
            SpeedSectionCheck(pos);
            SpeedLeverLock(_currentSpeedSection);
        }
        
        private void Awake()
        {
            _touchHelper = GetComponentInParent<TouchHelper>();
            _mainRectTransform = GetComponent<RectTransform>();
        }

        private void OnEnable()
        {
            EnhancedTouchSupport.Enable();
            ETouch.Touch.onFingerDown += OnFingerDown;
            ETouch.Touch.onFingerMove += OnFingerMove;
            ETouch.Touch.onFingerUp   += OnFingerUp;
        }

        private void OnDisable()
        {
            ETouch.Touch.onFingerDown -= OnFingerDown;
            ETouch.Touch.onFingerMove -= OnFingerMove;
            ETouch.Touch.onFingerUp   -= OnFingerUp;
        }

        private void OnFingerDown(Finger finger)
        {
            if (_speedFinger != null)
                return;
            
            if (!_touchHelper.FingerInsideRectTransform(_mainRectTransform, finger))
                return;

            _speedFinger = finger;
            int prevSpeedSection = _currentSpeedSection;
            float normLever = GetLeverPositionNorm(finger);
            float newY = normLever * (_mainRectTransform.rect.height / 2);
            lever.anchoredPosition = new Vector2(lever.anchoredPosition.x, newY);
            SpeedSectionCheck(normLever);
            
            if (prevSpeedSection != _currentSpeedSection)
                SpeedChanged?.Invoke(_speed);
        }
        
        private void OnFingerMove(Finger finger)
        {
            if (_speedFinger != finger)
                return;
            
            int prevSpeedSection = _currentSpeedSection;
            float normLever = GetLeverPositionNorm(finger);
            float newY = normLever * (_mainRectTransform.rect.height / 2);
            lever.anchoredPosition = new Vector2(lever.anchoredPosition.x, newY);
            SpeedSectionCheck(normLever);
            
            if (prevSpeedSection != _currentSpeedSection)
                SpeedChanged?.Invoke(_speed);
        }
        
        private void OnFingerUp(Finger finger)
        {
            if (_speedFinger != finger)
                return;
            _speedFinger = null;
            
            int prevSpeedSection = _currentSpeedSection;
            float normLever = GetLeverPositionNorm(finger);
            SpeedSectionCheck(normLever);
            SpeedLeverLock(_currentSpeedSection);
            
            if (prevSpeedSection != _currentSpeedSection)
                SpeedChanged?.Invoke(_speed);
        }

        private float GetLeverPositionNorm(Finger finger)
        {
            var fingerPos = _touchHelper.ScaleScreenToCanvas(finger.screenPosition);
            var rectCanvas = _touchHelper.GetRectTransformCanvasPos(_mainRectTransform);

            var height = rectCanvas.height;
            float normalizedUnclamped = (fingerPos.y - rectCanvas.y - (height / 2)) / height * 2;

            return Math.Clamp(normalizedUnclamped, -1f, 1f);
        }

        private void SpeedSectionCheck(float normPos)
        {
            float sectionHeight = 2.0f / speeds.Count;
            
            int speedNumber = (int) Math.Round(normPos / sectionHeight);
            
            int middleSpeed = speeds.Count / 2;
            int speedsInOneWay = middleSpeed;
            float oneSpeedIncrement = 1.0f / speedsInOneWay;

            _speed = oneSpeedIncrement * speedNumber;
            _currentSpeedSection = middleSpeed - speedNumber;
        }

        private void SpeedLeverLock(int sectionIndex)
        {
            float newY = speeds[sectionIndex].anchoredPosition.y;
            lever.anchoredPosition = new Vector2(lever.anchoredPosition.x, newY);
        }
    }
}
