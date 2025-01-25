using UnityEngine;

public class MemoryListItem : MonoBehaviour
{
    public TMPro.TextMeshProUGUI memoryText;

    private MemoryEntry activeEntry;

    public void Init(MemoryEntry entry)
    {
        activeEntry = entry;
        memoryText.text = entry.MemoryText;
    }
}
