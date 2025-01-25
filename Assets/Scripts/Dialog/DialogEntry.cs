

using UnityEngine;

[System.Serializable]
public class DialogEntry
{
    [Header("Selection Criteria")]
    public string Initiator;
    public int Priority;
    public string[] RequiredTags;
    public string[] ExcludedByTags;

    [Header("Display Items")]
    public string ProfilePic;
    public string[] TextBlocks;

    [Header("Dialog Rewards")]
    public string[] GrantedTags;
}
