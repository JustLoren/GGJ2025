using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class MemoryLibrary : MonoBehaviour
{
    #region Singleton stuff
    public static MemoryLibrary Instance;
    public void Start()
    {
        if (Instance == null)
            Instance = this;
        else
            Debug.LogError("Two MemoryLibraries are not allowed, crackhead");
    }

    private void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }
    #endregion

    public List<MemoryEntry> availableMemories = new();
    public List<MemoryEntry> usedMemories = new();
    public List<MemoryEntry> acceptedMemories = new();

    public static bool MemoryExists() => Instance.availableMemories.Any(m => DialogLibrary.ValidTags(m.RequiredTags, m.ExcludedByTags));

    public static MemoryEntry GetMemory()
    {
        var validMemories = Instance.availableMemories.Where(d => DialogLibrary.ValidTags(d.RequiredTags, d.ExcludedByTags));

        var selectedMemory = validMemories.FirstOrDefault();

        Instance.availableMemories.Remove(selectedMemory);
        Instance.usedMemories.Add(selectedMemory);

        return selectedMemory;
    }

    public static void AcceptMemory(MemoryEntry memory)
    {
        Instance.acceptedMemories.Add(memory);
    }


    #region Importer stuff

#if UNITY_EDITOR
    [NaughtyAttributes.Button("Import from Json File")]
    private void ImportJson()
    {
        string path = Path.Combine(Application.dataPath, "memory.json");
        var memoryEntries = FileLoader.LoadMemoryEntries(path);

        if (memoryEntries != null)
        {
            availableMemories = memoryEntries;
        }
    }
#endif

    #endregion
}
