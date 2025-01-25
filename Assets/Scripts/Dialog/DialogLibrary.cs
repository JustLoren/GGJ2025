using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

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
    private List<DialogEntry> visitedDialogs = new();

    [Inspectable]
    private HashSet<string> acquiredTags = new();

    public bool DialogExists(string actor) => knownDialogs.Any(d => IsValid(actor,d));

    public DialogEntry GetDialog(string actor)
    {
        var validDialogs = knownDialogs.Where(d => IsValid(actor, d));
        var sortedDialogs = validDialogs.OrderByDescending(d => d.Priority);

        var selectedDialog = sortedDialogs.FirstOrDefault();

        knownDialogs.Remove(selectedDialog);
        visitedDialogs.Add(selectedDialog);

        return selectedDialog;
    }

    public static void AddTags(string[] tags) => Instance.acquiredTags.AddRange(tags);

    public static void RemoveTag(string tag) => Instance.acquiredTags.Remove(tag);

    public static bool ValidTags(string[] requiredTags, string[] excludeTags)
    {
        foreach (var tag in requiredTags)
        {
            if (!Instance.acquiredTags.Contains(tag))
                return false;
        }

        foreach (var tag in excludeTags)
        {
            if (Instance.acquiredTags.Contains(tag))
                return false;
        }

        return true;
    }

    private bool IsValid(string actor, DialogEntry entry)
    {
        if (entry.Initiator != actor)
            return false;
        
        return ValidTags(entry.RequiredTags, entry.ExcludedByTags);
    }

    public void ResetState()
    {
        knownDialogs.AddRange(visitedDialogs);
        visitedDialogs.Clear();
        acquiredTags.Clear();
    }

#if UNITY_EDITOR
    [NaughtyAttributes.Button("Import from Json File")]
    private void ImportJson()
    {
        string path = Path.Combine(Application.dataPath, "dialog.json");
        List<DialogEntry> dialogEntries = FileLoader.LoadDialogEntries(path);

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