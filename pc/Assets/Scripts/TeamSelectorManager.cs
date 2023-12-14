using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TeamSelectorManager : MonoBehaviour
{
    public TextMeshProUGUI GeneralReadinessLabel;
    public TextMeshProUGUI Team1ReadinessLabel;
    public TextMeshProUGUI Team2ReadinessLabel;


    private List<List<int>> _teams;

    private const int MAX_PLAYERS_IN_TEAM = 2;
    private const int MAX_PLAYERS = 4;
    private int _readyPlayers = 0;

    // Start is called before the first frame update
    void Start()
    {
        _teams = new List<List<int>>();
        _teams.Add(new List<int>());
        _teams.Add(new List<int>());
    }

    public bool AddPlayerToTeam(int playerId, int teamId)
    {
        if (_teams[teamId].Contains(playerId)) { return true; }
        if (_teams[teamId].Count == MAX_PLAYERS_IN_TEAM) { return false; }

        _teams[teamId].Add(playerId);
        _readyPlayers++;
        UpdateShownInformation();
        return true;
    }

    public bool RemovePlayerFromTeam(int playerId) 
    {
        foreach (var team in _teams)
        {
            if(team.Contains(playerId))
            {
                team.Remove(playerId);
                _readyPlayers--;
                UpdateShownInformation();
                return true;
            }
        }
        return false;
    }
    void UpdateShownInformation()
    {
        GeneralReadinessLabel.text = _readyPlayers + "/" + MAX_PLAYERS + " graczy gotowych";
        Team1ReadinessLabel.text = _teams[0].Count + "/" + MAX_PLAYERS_IN_TEAM + " graczy";
        Team2ReadinessLabel.text = _teams[1].Count + "/" + MAX_PLAYERS_IN_TEAM + " graczy";
    }

    
    private void OnDestroy()
    {
        // only for testing
        if (_readyPlayers == MAX_PLAYERS) { return; }
        for (int teamId = 0; teamId < _teams.Count; teamId++)
        {
            foreach (var playerId in _teams[teamId])
            {
                ModuleEventManager.instance.idsToRequest[teamId].Add(playerId);
            }
        }
    }

    private void Update()
    {
        if(_readyPlayers == MAX_PLAYERS)
        {
            for(int teamId = 0; teamId < _teams.Count; teamId++)
            {
                foreach(var playerId in _teams[teamId])
                {
                    ModuleEventManager.instance.idsToRequest[teamId].Add(playerId);
                }
            }
            SceneManager.LoadScene("GamePrototype");
        }
    }
}
