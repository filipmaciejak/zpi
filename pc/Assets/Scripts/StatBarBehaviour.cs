using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatBarBehaviour : MonoBehaviour
{
    private Slider healthBarSlider;
    // Start is called before the first frame update
    void Start()
    {
        healthBarSlider = GetComponent<Slider>();
    }

    public void SetStatBarLevel(float value)
    {
        healthBarSlider.value = value;
    }

    public float GetStatBarLevel()
    {
        return healthBarSlider.value;
    }
}
