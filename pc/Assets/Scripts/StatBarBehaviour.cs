using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatBarBehaviour : MonoBehaviour
{
    [SerializeField]
    private Slider healthBarSlider;
    public void SetStatBarLevel(float value)
    {
        healthBarSlider.value = value;
    }

    public float GetStatBarLevel()
    {
        return healthBarSlider.value;
    }
}
