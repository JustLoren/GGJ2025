using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DialogPlayer : MonoBehaviour
{
    [System.Serializable]
    public class ProfilePic
    {
        public string Name;
        public Sprite Sprite;
    }

    public List<ProfilePic> knownProfiles = new();
    public Image profilePic;
    public TMPro.TextMeshProUGUI dialogText;
    public GameObject nextIndicator, doneIndicator;

    private int textIndex = 0;
    private DialogEntry activeEntry = null;
    public void Play(DialogEntry entry)
    {
        //Turn the player off
        SimpleCharacterController.AllowMovement(false);

        //Turn ourselves on
        this.gameObject.SetActive(true);

        textIndex = 0;
        activeEntry = entry;

        var entryPic = knownProfiles.FirstOrDefault(p => p.Name == entry.ProfilePic);

        if (entryPic != null)
        {
            profilePic.sprite = entryPic.Sprite;
            profilePic.gameObject.SetActive(true);
        } 
        else
        {
            profilePic.gameObject.SetActive(false);
        }
        ShowNext();
    }

    public bool ShowNext()
    {
        if (textIndex >= activeEntry.TextBlocks.Length)
        {
            DialogLibrary.AddTags(activeEntry.GrantedTags);
            
            //Dialog complete!
            this.gameObject.SetActive(false);

            //Turn the player back on
            SimpleCharacterController.AllowMovement(true);

            return false;
        }

        dialogText.text = activeEntry.TextBlocks[textIndex++];

        var isDone = textIndex >= activeEntry.TextBlocks.Length;
        
        doneIndicator.SetActive(isDone);
        nextIndicator.SetActive(!isDone);

        return true;
    }
}
