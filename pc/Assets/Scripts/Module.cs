using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Module : MonoBehaviour
{
    public GameObject mech;
    public GameObject mechManager;
    public enum Type {ENERGY_MODULE, SHIELD_MODULE, GYROSCOPE_MODULE};
    [SerializeField] public Type type;
    public bool isBeingUsed = false;
    public abstract void Perform();
    public bool IsBeingUsed()
    {
        return isBeingUsed;
    }

    public void Start()
    {
        mechManager.GetComponent<MechManager>().AddModule(this);
    }
}
