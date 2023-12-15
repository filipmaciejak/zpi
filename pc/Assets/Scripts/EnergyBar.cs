using UnityEngine;
using UnityEngine.UI;

public class EnergyBar : MonoBehaviour
{
    public Slider energyBar;

    [SerializeField]
    private MechMainEnergy mechEnergy;

    private void Update()
    {
        UpdateEnergyLevel();
    }
    public void UpdateEnergyLevel()
    {
        energyBar.value = 1 - mechEnergy.GetCurrentEnergy() / mechEnergy.GetMaxEnergy(); ;
    }
}
