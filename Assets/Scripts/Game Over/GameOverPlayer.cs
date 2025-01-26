using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverPlayer : DialogPlayer
{
    public string gameOverActor = "game-over";

    public void TriggerGameOver()
    {
        var selectedEnding = DialogLibrary.Instance.GetDialog(gameOverActor);

        if (selectedEnding == null)
        {
            Debug.LogError("What the heck, broski?! How do we have 3 memories but no ending? Straighten up!");
        }

        Play(selectedEnding);
    }

    public override bool ShowNext()
    {
        var result = base.ShowNext();
        if (!result)
        {
            //Ok we're done!

            //Reload the main scene
            SceneManager.LoadScene("Title", LoadSceneMode.Single);
        }

        return result;
    }
}
