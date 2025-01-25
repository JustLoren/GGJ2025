using UnityEngine;
using UnityEngine.InputSystem;

public class Interactable : MonoBehaviour
{
    public string ActorName;
    bool _isPrompted = false;
    private void OnTriggerEnter(Collider other)
    {
        if (DialogLibrary.Instance.DialogExists(ActorName))
        {
            DialogDisplay.Instance.ShowPrompt(this);
            _isPrompted = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (_isPrompted)
        {
            DialogDisplay.Instance.HidePrompt(this);
            _isPrompted = false;
        }
    }
}
