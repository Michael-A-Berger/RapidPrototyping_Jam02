using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnPooperty : MonoBehaviour
{
    // Attributes
    public bool isCurrentTurn = false;
    public bool hasCompletedTurn = false;

    /// <summary>
    /// CompleteTurn() - A helper method to definitively declare than
    /// the object with this TurnProperty script has finished its turn.
    /// The same effect can be achieved with "hasCompletedTurn = true".
    /// </summary>
    public void CompleteTurn()
    {
        hasCompletedTurn = true;
    }
}
