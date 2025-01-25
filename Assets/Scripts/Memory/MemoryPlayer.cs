using UnityEngine;

public class MemoryPlayer : MonoBehaviour
{
    public TMPro.TextMeshProUGUI memoryText;

    private MemoryEntry activeEntry = null;
    public void Play(MemoryEntry entry)
    {        
        activeEntry = entry;
        memoryText.text = entry.MemoryText;
        Toggle(true);
    }

    public MemoryEntry Accept()
    {
        DialogLibrary.AddTags(activeEntry.MemoryAcceptedTags);

        var acceptedMemory = activeEntry;

        Toggle(false);

        return acceptedMemory;
    }

    public void Deny()
    {
        DialogLibrary.AddTags(activeEntry.MemoryDeniedTags);
        Toggle(false);
    }

    private void Toggle(bool visible)
    {
        this.gameObject.SetActive(visible);

        if (!visible) activeEntry = null;
    }
}
