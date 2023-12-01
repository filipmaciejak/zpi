using System;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.UI;
using ETouch = UnityEngine.InputSystem.EnhancedTouch;

namespace CannonModule
{
    public class Cannon : MonoBehaviour
    {
        public event Action<bool> ChamberOpened;
        public event Action<bool> ChamberLoaded;
        
        public GameObject chamber;
        private RectTransform _chamberRT;
        public GameObject chamberCover;
        private RectTransform _chamberCoverRT;

        public float chamberCoverMargin;
        private float _chamberCoverRightSideLimit;
        private float _chamberCoverLeftSideLimit;

        private TouchHelper _touchHelper;
        private Finger _finger;

        public float defaultCooldown;
        private float _cooldownLeft;
        private float _lastShot;
        public Boolean loaded;
        public Boolean chamberOpen;

        private Rect _initRealCoverPos;
        private float _fingerInitPos;

        private void Awake()
        {
            _cooldownLeft = 0;
            _touchHelper = GetComponentInParent<TouchHelper>();
            _chamberCoverRT = chamberCover.GetComponent<RectTransform>();
            _chamberRT = chamber.GetComponent<RectTransform>();
            _chamberCoverRightSideLimit = _chamberRT.anchoredPosition.x
                                          + chamberCoverMargin;
            _chamberCoverLeftSideLimit = _chamberCoverRT.anchoredPosition.x;
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

        public void SetLoaded(bool loaded)
        {
            this.loaded = loaded;
            if(loaded)
                chamber.GetComponent<Image>().color = Color.green;
            else
                chamber.GetComponent<Image>().color = Color.red;
            ChamberLoaded?.Invoke(this.loaded); 
        }
        
        public void SetCooldownLeft(float cooldownLeft)
        {
            this._lastShot = Time.time;
            this._cooldownLeft = cooldownLeft;
        }

        public bool Fire()
        {
            if (!chamberOpen
                && loaded
                && _lastShot + _cooldownLeft <= Time.time)
            {
                SetCooldownLeft(defaultCooldown);
                SetLoaded(false);
                return true;
            }

            return false;
        }

        public void LoadShell()
        {
            SetLoaded(true);
        }

        private void OnFingerDown(Finger finger)
        {
            if (_finger != null)
                return;

            if (!_touchHelper.FingerInsideRectTransform(_chamberCoverRT, finger))
                return;

            _finger = finger;
            _fingerInitPos = _touchHelper.ScaleScreenToCanvas(finger.screenPosition).x;
            _initRealCoverPos = _touchHelper.GetRectTransformCanvasPos(_chamberCoverRT);
        }

        private void OnFingerMove(Finger finger)
        {
            if (finger != _finger)
                return;

            var fingerPos = _touchHelper.ScaleScreenToCanvas(finger.screenPosition);
            var posRelativeToStart = fingerPos.x - _fingerInitPos;
            var newAbsolutePos = _initRealCoverPos.x + posRelativeToStart;
            var newPivotPos = newAbsolutePos - _touchHelper.GetAnchorPosRelativeToCanvas(_chamberCoverRT).x;
            var newPos = newPivotPos + _chamberCoverRT.rect.width / 2;

            // clamp
            if (newPos > _chamberCoverRightSideLimit)
                newPos = _chamberCoverRightSideLimit;
            if (newPos < _chamberCoverLeftSideLimit)
                newPos = _chamberCoverLeftSideLimit;

            _chamberCoverRT.anchoredPosition = new Vector2(newPos, _chamberCoverRT.anchoredPosition.y);
        }

        private void OnFingerUp(Finger finger)
        {
            if (finger != _finger)
                return;

            _finger = null;

            chamberOpen = ChamberOpenCheck();
            SetChamberState(chamberOpen);
        }

        private Boolean ChamberOpenCheck()
        {
            Rect coverRect = _chamberCoverRT.rect;
            Vector2 coverAnchoredPos = _chamberCoverRT.anchoredPosition;
            Rect chamberRect = _chamberRT.rect;
            Vector2 chamberAnchoredPos = _chamberRT.anchoredPosition;

            return !(coverAnchoredPos.x + coverRect.width / 2 >= chamberAnchoredPos.x);
        }

        public void SetChamberState(Boolean chamberOpen)
        {
            this.chamberOpen = chamberOpen;
            float newPos;
            if (this.chamberOpen)
                newPos = _chamberCoverLeftSideLimit;
            else
                newPos = _chamberCoverRightSideLimit;
            _chamberCoverRT.anchoredPosition = new Vector2(newPos, _chamberCoverRT.anchoredPosition.y);
            ChamberOpened?.Invoke(this.chamberOpen);
        }
    }
}
