using System;
using System.Collections;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    public void DoOnNextFrame(Action action)
    {
        StartCoroutine(NextFrameCoroutine(action));
    }

    public void DoOnEndOfFrame(Action action)
    {
        StartCoroutine(EndOfFrameCoroutine(action));
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private IEnumerator NextFrameCoroutine(Action action)
    {
        yield return null;
        action();
    }

    private IEnumerator EndOfFrameCoroutine(Action action)
    {
        yield return new WaitForEndOfFrame();
        action();
    }
}
