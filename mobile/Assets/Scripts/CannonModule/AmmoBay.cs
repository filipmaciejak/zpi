using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using ETouch = UnityEngine.InputSystem.EnhancedTouch;

namespace CannonModule
{
    public class AmmoBay : MonoBehaviour
    {
        public float ammoHeadroom;
        public float ammoSpaceBetween;
        public int ammoCapacity;
        public int currentAmmo;

        public GameObject background;
        private RectTransform _backgroundRT;

        public GameObject cannon;

        public GameObject shellPrefab;
        private RectTransform _shellPrefabRT;
        private List<GameObject> _shells;

        public Finger currentShellFinger;
        private TouchHelper _touchHelper;

        public void Awake()
        {
            _shells = new List<GameObject>();
            _touchHelper = GetComponentInParent<TouchHelper>();
            _backgroundRT = background.GetComponent<RectTransform>();
            _shellPrefabRT = shellPrefab.GetComponent<RectTransform>();
        }

        public void Start()
        {
            UpdateShells();
        }

        public void GrabShell(GameObject shell, Finger finger)
        {
            if (currentShellFinger != null)
                return;

            _shells.Remove(shell);

            currentAmmo -= 1;
            currentShellFinger = finger;
            UpdateShells();
        }

        public void ReturnShell(GameObject shell)
        {
            Destroy(shell);
            currentShellFinger = null;
            currentAmmo += 1;
            UpdateShells();
        }

        public void LoadShell(GameObject shell)
        {
            Destroy(shell);
            currentShellFinger = null;
            UpdateShells();
        }

        public void UpdateShells()
        {
            var shellsToAdd = currentAmmo - _shells.Count;

            for (int i = 0; i < shellsToAdd; i++)
            {
                var newShell = Instantiate<GameObject>(shellPrefab, this.transform);
                _shells.Add(newShell);
            }
            
            var bgRect = _touchHelper.GetRectTransformCanvasPos(_backgroundRT);

            var xToSpawn = bgRect.x + bgRect.width / 2;
            var yToSpawn = bgRect.y + bgRect.height - _shellPrefabRT.rect.height/2 - ammoHeadroom;
            
            var yDifference = _shellPrefabRT.rect.height + ammoSpaceBetween;

            foreach (var shell in _shells)
            {
                var shellRT = shell.GetComponent<RectTransform>();
                shellRT.anchoredPosition = new Vector2(
                    xToSpawn,
                    yToSpawn
                );

                yToSpawn -= yDifference;
            }
        }
    }
}
