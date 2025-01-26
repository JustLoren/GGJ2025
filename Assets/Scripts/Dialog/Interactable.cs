using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Interactable : MonoBehaviour
{    
    private void Start()
    {
        StartCoroutine(Subscribe());
    }

    private IEnumerator Subscribe()
    {
        yield return null;
        DialogLibrary.Instance.OnTagsChanged += DialogLibrary_OnTagsChanged;
        ToggleEffects();
    }

    private void OnDestroy()
    {
        DialogLibrary.Instance.OnTagsChanged -= DialogLibrary_OnTagsChanged;
    }

    private void DialogLibrary_OnTagsChanged(object sender, System.EventArgs e)
    {
        ToggleEffects();
    }

    private void ToggleEffects()
    {
        var visible = DialogLibrary.Instance.DialogExists(ActorName);

        if (visible)
            visualFx.Play(true);
        else
            visualFx.Stop();

        spriteObj.SetActive(visible);
    }

    public string ActorName;
    public ParticleSystem visualFx;
    public GameObject spriteObj;
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
