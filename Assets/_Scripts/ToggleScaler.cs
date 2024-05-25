using UnityEngine;
using UnityEngine.UI;

public class ToggleScaler : MonoBehaviour
{
    public Toggle toggle;
    public Transform buttonTransform;
    public Vector3 toggledScale = new Vector3(2.5f, 2.5f, 2.5f); 
    private Vector3 originalScale;
    public Toggle otherToggle;

    void Start()
    {
        originalScale = buttonTransform.localScale; 
        toggle.onValueChanged.AddListener(OnToggleChanged); 
    }

    void OnToggleChanged(bool isOn)
    {
        if (isOn)
        {
            buttonTransform.localScale = toggledScale;
            if (otherToggle != null && otherToggle.isOn) // Check if otherToggle is not null and is on
            {
                otherToggle.isOn = false; // Turn the other toggle off
            }
        }
        else
        {
            buttonTransform.localScale = originalScale; 
        }
    }

    void OnDestroy()
    {
        toggle.onValueChanged.RemoveListener(OnToggleChanged); 
    }
}
