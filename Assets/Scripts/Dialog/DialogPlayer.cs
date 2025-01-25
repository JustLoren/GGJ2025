using UnityEngine;
using UnityEngine.UI;

public class DialogPlayer : MonoBehaviour
{
    public Image profilePic;
    public TMPro.TextMeshProUGUI dialogText;
    public GameObject nextIndicator, doneIndicator;

    private int textIndex = 0;
    private DialogEntry activeEntry = null;
    public void Play(DialogEntry entry)
    {
        //Turn ourselves on
        this.gameObject.SetActive(true);
        textIndex = 0;
        activeEntry = entry;
        ShowNext();
    }

    public bool ShowNext()
    {
        if (textIndex >= activeEntry.TextBlocks.Length)
        {
            DialogLibrary.Instance.AddTags(activeEntry.GrantedTags);
            //Dialog complete!
            this.gameObject.SetActive(false);
            return false;
        }

        dialogText.text = activeEntry.TextBlocks[textIndex++];

        var isDone = textIndex >= activeEntry.TextBlocks.Length;
        
        doneIndicator.SetActive(isDone);
        nextIndicator.SetActive(!isDone);

        return true;
    }
}
