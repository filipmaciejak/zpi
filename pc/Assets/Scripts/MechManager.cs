using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MechManager : MonoBehaviour
{
    List<Module> modules;
    public void Awake()
    {
        modules = new List<Module>();
    }
    private void FixedUpdate()
    {
        foreach (var module in modules)
        {
            module.Perform();
        }
    }


    public void AddModule(Module module)
    {
        modules.Add(module);
    }
}
