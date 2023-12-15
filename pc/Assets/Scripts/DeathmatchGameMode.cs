using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathmatchGameMode : GameMode
{
    [SerializeField]
    private int startingLives;

    public GameEndStateObject gameEndStateObject;
    
    void Start()
    {
        mech1.GetComponent<MechState>().mechLives = startingLives;
        mech2.GetComponent<MechState>().mechLives = startingLives;

        mech1.GetComponent<MechState>().onMechDeath.AddListener(teamId => MechDestroyed(teamId));
        mech2.GetComponent<MechState>().onMechDeath.AddListener(teamId => MechDestroyed(teamId));


    }

    public void Update()
    {
        currentRoundTime += Time.deltaTime;
        if(maxRoundTime < currentRoundTime) {
            GameStateManager.Instance().EndGame();
        }
    }
    public void MechDestroyed(int mechId)
    {
        GameObject destroyedMech = mechId == mech1.GetComponent<MechState>().teamId ? mech1 : mech2;
        MechState destroyedMechState = destroyedMech.GetComponent<MechState>();
        if(--destroyedMechState.mechLives > 0)
        {
            destroyedMech.GetComponent<MechRespawn>().Respawn();
        }
        else
        {
            if (mechId == 0)
            {
                Debug.Log("Team 2 wins");
                gameEndStateObject.gameResult = GameEndStateObject.GameResult.TEAM_2_WIN;
            }
            else
            {
                Debug.Log("Team 1 wins");
                gameEndStateObject.gameResult = GameEndStateObject.GameResult.TEAM_1_WIN;
            }
            GameStateManager.Instance().EndGame();
        }
    }
}
