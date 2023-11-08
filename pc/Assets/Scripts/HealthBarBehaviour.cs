using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarBehaviour : MonoBehaviour
{
    private Slider healthBarSlider;
    // Start is called before the first frame update
    void Start()
    {
        healthBarSlider = GetComponent<Slider>();
    }

    public void SetHealthBarValue(float value)
    {
        healthBarSlider.value = value;
    }

    public float GetHealthBarValue()
    {
        return healthBarSlider.value;
    }
}
