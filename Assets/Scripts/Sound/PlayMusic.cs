using System.Collections;
using UnityEngine;

public class PlayMusic : MonoBehaviour
{
    public string clipName;
    private void OnEnable()
    {
        StartCoroutine(DelayedStart());
    }

    private IEnumerator DelayedStart()
    {
        yield return null;
        SoundManager.Play(clipName);
    }
}
