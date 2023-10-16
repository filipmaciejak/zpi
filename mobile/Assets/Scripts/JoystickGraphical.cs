using UnityEngine;

public class JoystickGraphical : MonoBehaviour
{
    public RectTransform rectTransform;
    public RectTransform knob;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }
}
