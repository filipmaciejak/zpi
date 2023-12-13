using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class PlayerRepresentation : MonoBehaviour
{
    [SerializeField]
    private int _id;

    public TeamSelectorManager TeamSelectorManager;

    private Vector3 _startingPosition;
    private const float DISTANCE_TO_TEAM = 5f;
    private const float DISTANCE_SELECTION_LIMIT = 0.8f;
    private float _lastInput = 0;
    private bool _isTeamChosen = false;

    // Start is called before the first frame update
    void Start()
    {
        _startingPosition = GetComponent<RectTransform>().position;

        CrewmateEventManager.instance.onCrewmateMoveInputUpdate.AddListener(
            (id, inputX, inputY) => {
                if (id != _id) return;
                UpdateMoveInput(inputX);
            }
        );

        CrewmateEventManager.instance.onCrewmateButtonAPushed.AddListener(
            (id) => {
                if (id != _id) return;
                if(_isTeamChosen)
                {
                    _isTeamChosen = false;
                    GetComponent<RectTransform>().position = _startingPosition;
                    TeamSelectorManager.RemovePlayerFromTeam(_id);
                }
                else if (_lastInput > DISTANCE_SELECTION_LIMIT)
                {
                    if(TeamSelectorManager.AddPlayerToTeam(_id, 1))
                    {
                        _isTeamChosen = true;
                        GetComponent<RectTransform>().position = new Vector3(_startingPosition.x + DISTANCE_TO_TEAM, _startingPosition.y, _startingPosition.z);
                    }                    
                }
                else if (_lastInput < -DISTANCE_SELECTION_LIMIT)
                {
                    if(TeamSelectorManager.AddPlayerToTeam(_id, 0))
                    {
                        _isTeamChosen = true;
                        GetComponent<RectTransform>().position = new Vector3(_startingPosition.x - DISTANCE_TO_TEAM, _startingPosition.y, _startingPosition.z);
                    }                    
                }
            }
        );
    }

    private void UpdateMoveInput(float inputX)
    {
        if (!_isTeamChosen)
        {
            GetComponent<RectTransform>().position = new Vector3(_startingPosition.x + inputX * DISTANCE_TO_TEAM, _startingPosition.y, _startingPosition.z);
            _lastInput = inputX;
        }
    }
}
