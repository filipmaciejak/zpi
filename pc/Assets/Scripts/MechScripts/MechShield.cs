using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MechShield : MonoBehaviour
{
    [SerializeField]
    private int currentShield;

    [SerializeField]
    private int maxShield;

    public int GetShield() { return currentShield; }
    public void SetShield(int value) { currentShield = value; }

    public int GetMaxShield() { return  maxShield; }
}
