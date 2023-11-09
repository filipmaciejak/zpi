using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

enum MinigameType
{
    MOVEMENT_MODULE,
    ENERGY_MODULE,
    SHIELD_MODULE
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public ClientManager clientManager;
    
    // This should be grabbed by a new section instantiated by the game manager
    // before the first update.
    [NonSerialized]
    public Dictionary<string, string> MinigameInitializationData;
    public event Action<Dictionary<string, string>> UpdateMinigameData;
    
    void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;
        DontDestroyOnLoad(gameObject);
        clientManager = GetComponentInChildren<ClientManager>();
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }

    private void OnEnable()
    {
        clientManager.StartMinigame += OnStartMinigame;
        clientManager.UpdateMinigame += OnUpdateMinigame;
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    private void OnDisable()
    {
        clientManager.StartMinigame -= OnStartMinigame;
        clientManager.UpdateMinigame -= OnUpdateMinigame;
    }

    public void OnStartMinigame(Dictionary<string,string> data)
    {
        Boolean conversionSuccess = Enum.TryParse(data["minigame"], out MinigameType minigame);
        if (!conversionSuccess)
        {
            Debug.Log("This minigame does not exist!: " + data["minigame"]);
            return;
        }
        
        MinigameInitializationData = data;
        SceneManager.LoadScene(data["minigame"]);
    }

    public void OnUpdateMinigame(Dictionary<string, string> data)
    {
        UpdateMinigameData?.Invoke(data);
    }
}
