using System;
using System.Collections.Generic;
using UnityEngine;

namespace CannonModule
{
    public class CannonModuleManager : MonoBehaviour
    {
        private ClientManager _clientManager;

        public AimController aimController;

        public Cannon cannon;

        public AmmoBay ammoBay;
        
        public Button fireButton;
        public Button leaveButton;

        private void Awake()
        {
            _clientManager = GameManager.Instance.clientManager;
        }

        void Start()
        {
            var initData = GameManager.Instance.MinigameInitializationData;
            if (initData == null)
                return;

            OnMinigameUpdate(initData);
        }

        public void OnEnable()
        {
            _clientManager.UpdateMinigame += OnMinigameUpdate;
            
            aimController.AimPositionUpdated += OnAimPositionUpdate;
            cannon.ChamberOpened += OnChamberOpened;
            cannon.ChamberLoaded += OnChamberLoaded;
            fireButton.StartedPress  += OnStartedFire;
            leaveButton.StartedPress += OnLeave;
        }

        public void OnDisable()
        {
            _clientManager.UpdateMinigame -= OnMinigameUpdate;
            
            aimController.AimPositionUpdated -= OnAimPositionUpdate;
            cannon.ChamberLoaded -= OnChamberLoaded;
            fireButton.StartedPress  -= OnStartedFire;
            fireButton.StartedPress  -= OnStartedFire;
            leaveButton.StartedPress -= OnLeave;
        }

        public void OnMinigameUpdate(Dictionary<string, string> dictMessage)
        {
            if(dictMessage.TryGetValue("fire_cooldown", out var fireCooldown))
                cannon.SetCooldownLeft(float.Parse(fireCooldown));

            if (dictMessage.TryGetValue("ammo", out var ammo))
            {
                ammoBay.currentAmmo = int.Parse(ammo);
                ammoBay.UpdateShells();
            }

            if (dictMessage.TryGetValue("chamber_open", out var chamberOpen))
                cannon.SetChamberState(bool.Parse(chamberOpen));
                
            if(dictMessage.TryGetValue("chamber_loaded", out var chamberLoaded))
                cannon.SetLoaded(bool.Parse(chamberLoaded));
        }

        public void OnAimPositionUpdate(float newPosition)
        {
            Dictionary<string, string> sentDict = new Dictionary<string, string>
            {
                { "event", MessageEvent.UPDATE_MINIGAME.ToString() },
                { "minigame", MinigameType.CANNON_MODULE.ToString() },
                { "aim_pos", newPosition.ToString() }
            };
            
            _clientManager.SendDict(sentDict);
        }
        
        public void OnChamberOpened(bool opened)
        {
            Dictionary<string, string> sentDict = new Dictionary<string, string>
            {
                { "event", MessageEvent.UPDATE_MINIGAME.ToString() },
                { "minigame", MinigameType.CANNON_MODULE.ToString() },
                { "chamber_open", opened.ToString() }
            };
            
            _clientManager.SendDict(sentDict);
        }
        
        public void OnChamberLoaded(bool loaded)
        {
            Dictionary<string, string> sentDict = new Dictionary<string, string>
            {
                { "event", MessageEvent.UPDATE_MINIGAME.ToString() },
                { "minigame", MinigameType.CANNON_MODULE.ToString() },
                { "chamber_loaded", loaded.ToString() }
            };
            
            _clientManager.SendDict(sentDict);
        }
        public void OnStartedFire(string buttonName)
        {
            var succesfullyFired = cannon.Fire();
            if (!succesfullyFired)
                return;
            
            
            Dictionary<string, string> sentDict = new Dictionary<string, string>
            {
                { "event", MessageEvent.UPDATE_MINIGAME.ToString() },
                { "minigame", MinigameType.CANNON_MODULE.ToString() },
                { "fire_event", "fired" }
            };
            
            _clientManager.SendDict(sentDict);
        }

        public void OnLeave(string buttonName)
        {
            GameManager.Instance.AbortMinigame();
        }
    }
}
