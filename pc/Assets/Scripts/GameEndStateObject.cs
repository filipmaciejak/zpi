using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameEndStateObject", menuName = "ScriptableObjects/GameEndStateObject", order = 1)]
public class GameEndStateObject : ScriptableObject
{
    public enum GameResult
    {
        NOT_ENDED = -1,
        DRAW = 0,
        TEAM_1_WIN = 1,
        TEAM_2_WIN = 2,
    }

    public GameResult gameResult;
}
