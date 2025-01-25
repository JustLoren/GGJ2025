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
    public MemoryPlayer MemoryPlayer;
    public MemoryList MemoryList;
    public GameObject PromptArea;

    private bool activeDialog => DialogPlayer.gameObject.activeSelf;
    private bool activeMemory => MemoryPlayer.gameObject.activeSelf;
    private bool isGameOver => MemoryList.GameOverPlayer.gameObject.activeSelf;
    
    public void ShowDialog(Interactable interactable)
    {
        var entry = DialogLibrary.Instance.GetDialog(interactable.ActorName);
        if (entry == null)
        {
            Debug.LogError("Not sure what happened, ShowDialog couldn't retrieve a valid dialog. Ask Loren.");
        }        
        DialogPlayer.Play(entry);
    }

    public void ShowMemory()
    {
        var entry = MemoryLibrary.GetMemory();
        if (entry == null)
        {
            Debug.LogError("Wow, how did we get into ShowMemory with no Memory available? Probably your fault.");
        }
        MemoryPlayer.Play(entry);
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
    public InputActionReference cancelAction;
    private void OnEnable()
    {
        // Enable the action so we can read values from it
        if (interactAction != null)
        {
            interactAction.action.Enable();
            cancelAction.action.Enable();
        }
    }

    private void OnDisable()
    {
        // Disable the action when this script isn't active
        if (interactAction != null)
        {
            interactAction.action.Disable();
            cancelAction.action.Disable();
        }
    }
    #endregion

    private void Update()
    {
        if (activePrompt != null && interactAction.action.WasPressedThisFrame())
        {
            var selectedPrompt = activePrompt;
            HidePrompt(selectedPrompt);
            ShowDialog(selectedPrompt);
        } 
        else if (activeDialog && interactAction.action.WasPressedThisFrame())
        {
            if (!DialogPlayer.ShowNext())
            {
                //Dialog is over, but maybe we made a new memory
                if (MemoryLibrary.MemoryExists())
                {
                    ShowMemory();
                }

                activePrompt = null;
            }
        } 
        else if (activeMemory)
        {
            if (interactAction.action.WasPressedThisFrame())
            {
                MemoryList.Add(MemoryPlayer.Accept());
            } else if (cancelAction.action.WasPressedThisFrame())
            {
                MemoryPlayer.Deny();
            }
        }
        else if (isGameOver && interactAction.action.WasPressedThisFrame())
        {
            MemoryList.GameOverPlayer.ShowNext();
        }
    }
}
