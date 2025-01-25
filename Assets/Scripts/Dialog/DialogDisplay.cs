using UnityEngine;
using UnityEngine.InputSystem;

public class DialogDisplay : MonoBehaviour
{
    public static DialogDisplay Instance;
    public void Start()
    {
        if (Instance == null)
            Instance = this;
        else
            Debug.LogError("Two DialogDisplays are not allowed, jacknut");
    }

    private void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }

    public DialogPlayer DialogPlayer;
    public GameObject PromptArea;

    private bool activeDialog => DialogPlayer.gameObject.activeSelf;
    public void ShowDialog(Interactable interactable)
    {
        var entry = DialogLibrary.Instance.GetDialog(interactable.ActorName);
        if (entry == null)
        {
            Debug.LogError("Not sure what happened, ShowDialog couldn't retrieve a valid dialog. Ask Loren.");
        }        
        DialogPlayer.Play(entry);
    }

    private Interactable activePrompt = null;
    public void ShowPrompt(Interactable interactable)
    {
        PromptArea.SetActive(true);
        activePrompt = interactable;
    }

    public void HidePrompt(Interactable interactable)
    {
        if (activePrompt == interactable)
        {
            PromptArea.SetActive(false);
            activePrompt = null;
        }
    }

    #region Input Section
    public InputActionReference interactAction;
    private void OnEnable()
    {
        // Enable the action so we can read values from it
        if (interactAction != null)
        {
            interactAction.action.Enable();
        }
    }

    private void OnDisable()
    {
        // Disable the action when this script isn't active
        if (interactAction != null)
        {
            interactAction.action.Disable();
        }
    }

    private void Update()
    {
        if (activePrompt != null && interactAction.action.WasPressedThisFrame())
        {
            var selectedPrompt = activePrompt;
            HidePrompt(selectedPrompt);
            ShowDialog(selectedPrompt);
        } else if (activeDialog && interactAction.action.WasPressedThisFrame())
        {
            if (!DialogPlayer.ShowNext())
            {
                activePrompt = null;
            }
        }
    }
    #endregion
}
