using UnityEngine;
using UnityEngine.UI;

public class ToggleScaler : MonoBehaviour
{
    public Toggle toggle;
    public Transform buttonTransform;
    public Vector3 toggledScale = new Vector3(1.2f, 1.2f, 1.2f); 
    private Vector3 originalScale; 

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
