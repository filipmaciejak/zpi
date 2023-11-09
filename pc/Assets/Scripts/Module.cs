using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Module : MonoBehaviour
{
    public GameObject mech;
    public enum Type {Walking, Shield, Gun};
    [SerializeField] public Type type;
    public bool isBeingUsed = false;
}
