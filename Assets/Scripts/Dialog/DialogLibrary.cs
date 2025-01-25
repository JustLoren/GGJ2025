using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class DialogLibrary : MonoBehaviour
{
    #region Singleton shenanigans
    public static DialogLibrary Instance;
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
    #endregion

    [SerializeField]
    private List<DialogEntry> knownDialogs = new();
    [Inspectable]
    private HashSet<string> acquiredTags = new();

    public bool DialogExists(string actor) => knownDialogs.Any(d => IsValid(actor,d));

    public DialogEntry GetDialog(string actor)
    {
        var validDialogs = knownDialogs.Where(d => IsValid(actor, d));
        var sortedDialogs = validDialogs.OrderByDescending(d => d.Priority);

        var selectedDialog = sortedDialogs.FirstOrDefault();

        knownDialogs.Remove(selectedDialog);

        return selectedDialog;
    }

    public void AddTags(string[] tags) => acquiredTags.AddRange(tags);

    public void RemoveTag(string tag) => acquiredTags.Remove(tag);

    private bool IsValid(string actor, DialogEntry entry)
    {
        if (entry.Initiator != actor)
            return false;

        foreach(var tag in entry.RequiredTags)
        {
            if (!acquiredTags.Contains(tag))
                return false;
        }

        foreach(var tag in entry.ExcludedByTags)
        {
            if (acquiredTags.Contains(tag)) 
                return false;
        }

        return true;
    }

#if UNITY_EDITOR
    [NaughtyAttributes.Button("Import from Json File")]
    private void ImportJson()
    {
        string path = Path.Combine(Application.dataPath, "dialog.json");
        List<DialogEntry> dialogEntries = DialogLoader.LoadDialogEntries(path);

        if (dialogEntries != null)
        {
            var tagsProvided = new HashSet<string>();
            var tagsNeeded = new HashSet<string>();

            foreach (var entry in dialogEntries)
            {
                Debug.Log($"Loaded dialog from {entry.Initiator} with priority {entry.Priority}.");
                tagsProvided.AddRange(entry.GrantedTags);
                tagsNeeded.AddRange(entry.RequiredTags);
            }

            foreach(var neededTag in tagsNeeded)
            {
                if (!tagsProvided.Contains(neededTag))
                {
                    Debug.LogError($"Uh oh! We need {neededTag} but it doesn't seem to be Granted by any dialog options!");
                }
            }

            knownDialogs = dialogEntries;
        }
    }
#endif
}