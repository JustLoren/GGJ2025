using UnityEngine;

public class ToggleText : MonoBehaviour
{
    public string onText, offText;
    public TMPro.TextMeshProUGUI textBox;
    public UnityEngine.UI.Toggle toggle;

    private void Start()
    {        
        UpdateText(toggle.isOn);
        toggle.onValueChanged.AddListener(UpdateText);
    }

    private void UpdateText(bool value)
    {
        if (value)
            textBox.text = onText;
        else 
            textBox.text = offText;
    }
}
