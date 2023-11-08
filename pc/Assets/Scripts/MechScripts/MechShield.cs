using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MechShield : MonoBehaviour
{
    [SerializeField]
    private int currentShield;

    [SerializeField]
    private int maxShield;

    [SerializeField]
    private StatBarBehaviour shieldBar;

    public int GetShield() { return currentShield; }
    public void SetShield(int value) {
        currentShield = value;
        shieldBar.SetStatBarLevel(currentShield);
    }

    public int GetMaxShield() { return  maxShield; }
}
