using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    // Attributes
    public List<TurnProperty> objectList;
    public int currentIndex = 0;
    public CameraMove cameraScript;
    public Vector3 relativeCameraPos;
    public Vector3 relativeCameraRot;
    public bool debug;
    bool special = false;

    // Properties
    private bool attemptingRound = false;

    // StartRound()
    public void StartRound()
    {
        if (objectList.Count < 1)
        {
            Debug.LogError("'objectList' in TurnManager is not defined!");
        }
        else
        {
            attemptingRound = true;
            currentIndex = 0;
            objectList[0].isCurrentTurn = true;
            MoveCamera();
            if (debug)
            {
                Debug.Log("Start of Round!");
                Debug_CurrentTurn();
            }
        }
    }

    // MoveCamera()
    public void MoveCamera()
    {
        // IF the camera script exists...
        if (cameraScript != null)
        {
            // Telling the camera to move to the new position + rotation
            TurnProperty current = objectList[currentIndex];
            Transform currentT = current.gameObject.transform;
            Vector3 newLoc = currentT.position + currentT.rotation * relativeCameraPos;
            Vector3 newRot = currentT.forward - relativeCameraRot;
            cameraScript.targetPos = newLoc;
            cameraScript.targetDir.SetLookRotation(newRot);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // IF the Turn Manager is attempting to complete a round...
        if (attemptingRound)
        {
            // IF the current turn object is not the last object...
            if (currentIndex < objectList.Count)
            {

                ///
                HandlePlayer();
                HandleAI();
                ///

                // IF the current turn object has completed its action...
                if (objectList[currentIndex].hasCompletedTurn)
                {
                    // Telling the current object that it's turn is over
                    objectList[currentIndex].isCurrentTurn = false;
                    currentIndex++;

                    // IF the previous object was not the last object...
                    if (currentIndex < objectList.Count)
                    {
                        // Telling the next object that it is the current turn
                        objectList[currentIndex].isCurrentTurn = true;
                        MoveCamera();

                        if (debug) Debug_CurrentTurn();
                    }
                }
            }
            else
            {
                // End the round
                attemptingRound = false;
                foreach (TurnProperty prop in objectList)
                {
                    prop.hasCompletedTurn = false;
                    prop.isCurrentTurn = false;
                }
                if (debug) Debug.Log("End of Round!");
            }
        }
    }

    // LateUpdate()
    private void LateUpdate()
    {
        // IF debug is enabled...
        if (debug)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                StartRound();
            }
            for (int keyCode = 49; keyCode < 58; keyCode++)
            {
                if (Input.GetKeyDown((KeyCode)keyCode))
                {
                    if (keyCode - 49 < objectList.Count)
                    {
                        objectList[keyCode - 49].hasCompletedTurn = true;
                    }
                    else
                    {
                        Debug.Log("Object #" + (keyCode - 48) + " is not defined");
                    }
                }
            }
        }
    }

    // Debug_CurrentTurn()
    private void Debug_CurrentTurn()
    {
        Debug.Log("Object #" + (currentIndex + 1) + "'s turn...");
    }


    ///
    void HandlePlayer()
    {
        /// <summary>
        /// Attacking Functionality
        /// Test Code for checking if the Attacking(int) function works for players.
        /// </summary>
        if (Input.GetKeyUp(KeyCode.A))
        {
            // Grabs the player script from the current objects turn and 
            objectList[currentIndex].gameObject.GetComponent<CharacterBase>().Attacking(0);
            //Debug.Log(objectList[currentIndex].transform.gameObject.GetComponent<CharacterBase>().currentEnemy.GetComponent<CharacterBase>().health);

            // Completes the players turn once Attacking(int) has finished
            objectList[currentIndex].hasCompletedTurn = true;
        }
        // Activates boolean to begin special functionality for players
        else if (Input.GetKeyDown(KeyCode.S))
        {
            special = true;
            Debug.Log("SPECIAL STARTED...");
        }

        /// <summary>
        /// Special Funcitonality
        /// When the special boolean is true, it checks if the currentTarget object of the player is true.
        /// If not, call the SelectTarget function of the player which checks for a mouse click on a gameObject.
        /// If currentTarget recieves a valid element, activate the UseSpecial function, which activates the
        /// players' special ability. Once finished; UseSpecial is called, special boolean changes back to false,
        /// and the players' turn ends.
        /// </summary>
        if (special)
        {
            // Checks if currentTarget is null
            if (objectList[currentIndex].gameObject.GetComponent<CharacterBase>().currentTarget == null)
            {
                // Waits for user to click on a valid object
                objectList[currentIndex].gameObject.GetComponent<CharacterBase>().SelectTarget();
            }
            else
            {
                // Uses special ability and resets values to prepare for next players' turn
                objectList[currentIndex].gameObject.GetComponent<CharacterBase>().UseSpecial();
                Debug.Log("SPECIAL ENDED...");
                special = false;
                objectList[currentIndex].hasCompletedTurn = true;
            }
        }
    }

    void HandleAI()
    {
       
    }

    void EndGame()
    {
        int downedPlayers = 0;
        for (int x = 0; x < objectList.Count; x++)
        {
            if (objectList[x].gameObject.GetComponent<CharacterBase>().isDowned)
            {
                downedPlayers++;
            }

        }

        if (downedPlayers == objectList.Count - 1 && objectList[objectList.Count - 1].gameObject.GetComponent<CharacterBase>().isDowned == true)
        {
            // END GAME PLAYERS LOSE
        }
    }
    ///
}
