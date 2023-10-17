using System.Collections.Generic;
using UnityEngine;

namespace MovementModule
{
    public class MovementModuleManager : MonoBehaviour
    {
        private ClientManager _clientManager;

        public SteeringSlider steeringSlider;
        public SpeedLever speedLever;
        
        public void Awake()
        {
            _clientManager = GameManager.Instance.clientManager;
        }

        public void Start()
        {
            var initData = GameManager.Instance.MinigameInitializationData;
            if (initData == null)
                return;
            
            OnMinigameUpdate(initData);
        }

        public void OnEnable()
        {
            _clientManager.UpdateMinigame += OnMinigameUpdate;
            
            steeringSlider.StartedControlling += OnStartedSlider;
            steeringSlider.MovedControls      += OnMovedSlider;
            steeringSlider.StoppedControlling += OnEndedSlider;

            speedLever.SpeedChanged += OnSpeedChange;
        }

        public void OnDisable()
        {
            _clientManager.UpdateMinigame -= OnMinigameUpdate;
            
            steeringSlider.StartedControlling -= OnStartedSlider;
            steeringSlider.MovedControls      -= OnMovedSlider;
            steeringSlider.StoppedControlling -= OnEndedSlider;
            
            speedLever.SpeedChanged -= OnSpeedChange;
        }

        private void OnMinigameUpdate(Dictionary<string, string> dictMessage)
        {
            if(dictMessage.TryGetValue("steering_pos", out var steeringPos))
                steeringSlider.UpdateSliderPosition(float.Parse(steeringPos));
            if(dictMessage.TryGetValue("speed_pos", out var speedPos))
                speedLever.UpdateLeverPosition(float.Parse(speedPos));
        }
        
        private void OnStartedSlider(float pos)
        {
            Dictionary<string, string> sentDict = new Dictionary<string, string>
            {
                { "event", MessageEvent.UPDATE_MINIGAME.ToString() },
                { "minigame", MinigameType.MOVEMENT_MODULE.ToString() },
                { "steering_event", "started" },
                { "steering_pos", pos.ToString() }
            };
            
            _clientManager.SendDict(sentDict);
        }

        private void OnMovedSlider(float pos)
        {
            Dictionary<string, string> sentDict = new Dictionary<string, string>
            {
                { "event", MessageEvent.UPDATE_MINIGAME.ToString() },
                { "minigame", MinigameType.MOVEMENT_MODULE.ToString() },
                { "steering_event", "moved" },
                { "steering_pos", pos.ToString() }
            };
            
            _clientManager.SendDict(sentDict);
        }

        private void OnEndedSlider(float pos)
        {
            Dictionary<string, string> sentDict = new Dictionary<string, string>
            {
                { "event", MessageEvent.UPDATE_MINIGAME.ToString() },
                { "minigame", MinigameType.MOVEMENT_MODULE.ToString() },
                { "steering_event", "ended" },
                { "steering_pos", pos.ToString() }
            };
            
            _clientManager.SendDict(sentDict);
        }

        private void OnSpeedChange(float speed)
        {
            Dictionary<string, string> sentDict = new Dictionary<string, string>
            {
                { "event", MessageEvent.UPDATE_MINIGAME.ToString() },
                { "minigame", MinigameType.MOVEMENT_MODULE.ToString() },
                { "speed_pos", speed.ToString() }
            };
            
            _clientManager.SendDict(sentDict);
        }
        
    }
}
