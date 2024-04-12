using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionScript : MonoBehaviour
{
    public void SwapBackgroundEvent()
    {
        DialogueManager.Instance.BackgroundSwap();
    }

    public void UnloadEvent()
    {
        this.gameObject.SetActive(false);
    }
}
