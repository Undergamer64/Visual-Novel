using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterScript : MonoBehaviour
{
    public void CharacterSwapEvent()
    {
        DialogueManager.Instance.CharacterSwap();
    }
}
