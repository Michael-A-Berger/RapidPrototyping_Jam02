using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnProperty : MonoBehaviour
{
    // Attributes
    public bool isCurrentTurn = false;
    public bool hasCompletedTurn = false;

    // CompleteTurn()
    public void CompleteTurn()
    {
        hasCompletedTurn = true;
    }
}
