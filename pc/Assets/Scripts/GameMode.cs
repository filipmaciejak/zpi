using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameMode : MonoBehaviour
{
    [SerializeField]
    protected GameObject mech1;

    [SerializeField]
    protected GameObject mech2;

    [SerializeField]
    protected int maxRoundTime;

    protected float currentRoundTime;
}
