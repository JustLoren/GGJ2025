using System.IO;
using System.Collections.Generic;
using UnityEngine;

public static class FileLoader
{
    // Helper wrapper for top-level JSON arrays.
    [System.Serializable]
    private class DialogEntryArray
    {
        public DialogEntry[] Items;
    }

    [System.Serializable]
    private class MemoryEntryArray
    {
        public MemoryEntry[] Items;
    }

    /// <summary>
    /// Loads a list of DialogEntry objects from a JSON file on disk.
    /// The JSON is expected to be an array of DialogEntry at the top level.
    /// </summary>
    /// <param name="filePath">Full path to the JSON file.</param>
    /// <returns>A List of DialogEntry objects.</returns>
    public static List<DialogEntry> LoadDialogEntries(string filePath)
    {
        if (!File.Exists(filePath))
        {
            Debug.LogError($"DialogLoader: File not found at {filePath}");
            return null;
        }

        // Read raw JSON text
        string json = File.ReadAllText(filePath);

        // JsonUtility can’t directly parse top-level arrays, so wrap it
        // as if "Items" was the single property containing the array.
        // We prepend and append curly braces to simulate an object with a single field named "Items".
        string wrappedJson = $"{{\"Items\":{json}}}";

        // Deserialize using our wrapper
        DialogEntryArray data = JsonUtility.FromJson<DialogEntryArray>(wrappedJson);

        if (data == null || data.Items == null)
        {
            Debug.LogError("DialogLoader: Could not parse DialogEntry array from JSON.");
            return null;
        }

        return new List<DialogEntry>(data.Items);
    }

    /// <summary>
    /// Loads a list of DialogEntry objects from a JSON file on disk.
    /// The JSON is expected to be an array of DialogEntry at the top level.
    /// </summary>
    /// <param name="filePath">Full path to the JSON file.</param>
    /// <returns>A List of DialogEntry objects.</returns>
    public static List<MemoryEntry> LoadMemoryEntries(string filePath)
    {
        if (!File.Exists(filePath))
        {
            Debug.LogError($"DialogLoader: File not found at {filePath}");
            return null;
        }

        // Read raw JSON text
        string json = File.ReadAllText(filePath);

        // JsonUtility can’t directly parse top-level arrays, so wrap it
        // as if "Items" was the single property containing the array.
        // We prepend and append curly braces to simulate an object with a single field named "Items".
        string wrappedJson = $"{{\"Items\":{json}}}";

        // Deserialize using our wrapper
        var data = JsonUtility.FromJson<MemoryEntryArray>(wrappedJson);

        if (data == null || data.Items == null)
        {
            Debug.LogError("DialogLoader: Could not parse MemoryEntry array from JSON.");
            return null;
        }

        return new List<MemoryEntry>(data.Items);
    }
}
