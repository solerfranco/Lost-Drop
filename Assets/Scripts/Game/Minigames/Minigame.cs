using System;
using System.Collections;
using UnityEngine;

public class Minigame : MonoBehaviour
{
    public Action<bool> OnGameFinished;

    public virtual void ResetAndRestart()
    {
        Reset();
        StartCoroutine(Play());
    }

    public virtual void Reset()
    {
        
    }

    public virtual IEnumerator Play()
    {
        yield return null;
    }
}
