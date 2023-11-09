using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using ETouch = UnityEngine.InputSystem.EnhancedTouch;

namespace CannonModule
{
    public class Shell : MonoBehaviour
    {
        public AmmoBay parentAmmoBay;
        public GameObject cannon;
        private Cannon _cannonComponent;
        private RectTransform _chamberRT;

        private RectTransform _rectTransform;
    
        private TouchHelper _touchHelper;
        private Finger _finger;
    
        void Awake()
        {
            _touchHelper = GetComponentInParent<TouchHelper>();
        }

        void Start()
        {
            _rectTransform = GetComponent<RectTransform>();
            parentAmmoBay = GetComponentInParent<AmmoBay>();
            cannon = parentAmmoBay.cannon;
            _cannonComponent = cannon.GetComponent<Cannon>();
            _chamberRT = _cannonComponent.chamber.GetComponent<RectTransform>();
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
            if (_finger != null)
                return;
            
            if (!_touchHelper.FingerInsideRectTransform(_rectTransform, finger))
                return;

            if (parentAmmoBay.currentShellFinger != null)
                return;

            _finger = finger;
            parentAmmoBay.GrabShell(this.gameObject, finger);
        }

        private void OnFingerMove(Finger finger)
        {
            if (_finger != finger)
                return;

            Vector2 newPos;

            newPos = _touchHelper.ScaleScreenToCanvas(_finger.screenPosition);
            
            _rectTransform.anchoredPosition = newPos;
        }

        private void OnFingerUp(Finger finger)
        {
            if (finger != _finger)
                return;

            _finger = null;
            
            if (!_touchHelper.FingerInsideRectTransform(_chamberRT, finger))
            {
                parentAmmoBay.ReturnShell(this.gameObject);
                return;
            }

            if (_cannonComponent.loaded || !_cannonComponent.chamberOpen)
            {
                parentAmmoBay.ReturnShell(this.gameObject);
                return;
            }
            
            parentAmmoBay.LoadShell(this.gameObject);
            _cannonComponent.LoadShell();
        }
    }
}
