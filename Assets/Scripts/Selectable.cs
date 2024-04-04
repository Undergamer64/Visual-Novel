using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ink.Runtime;

public class Selectable : MonoBehaviour
{
    public Choice element;
    public void Decide()
    {
        DialogueManager.SetDecision(element);
    }

}
