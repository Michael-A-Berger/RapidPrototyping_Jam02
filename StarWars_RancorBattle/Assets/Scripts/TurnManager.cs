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
    public bool gameOver = false;

    // Properties
    private bool attemptingRound = false;
    private bool special = false;
    private bool victory = false;
    

    // StartRound()
    public void StartRound()
    {
        if (objectList.Count < 1)
        {
            Debug.LogError("'objectList' in TurnManager is not defined!");
        }
        else
        {
            if (!gameOver)
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
            else
            {
                Debug.Log("Game has ended");
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
        EndGame();

        // IF the Turn Manager is attempting to complete a round...
        if (attemptingRound)
        {
            // IF the current turn object is not the last object...
            if (currentIndex < objectList.Count)
            {

                ///
                
                if (currentIndex != objectList.Count - 1)
                {
                    if (objectList[currentIndex].GetComponent<CharacterBase>().isDowned)
                        objectList[currentIndex].hasCompletedTurn = true;
                    else
                        HandlePlayer(); 
                }
                else
                { 
                    HandleAI(objectList[objectList.Count - 1].GetComponent<CharacterBase>()); 
                    Debug.Log("AI TURN");
                    objectList[Random.Range(0, objectList.Count - 1)].GetComponent<CharacterBase>().AddSpecial();
                }
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

    void HandleAI(CharacterBase AI)
    {
        while (AI.currentEnemy == null)
        {
            int selectPlayer = Random.Range(0, objectList.Count - 1);
            if (!objectList[selectPlayer].GetComponent<CharacterBase>().isDowned)
                AI.currentEnemy = objectList[selectPlayer].gameObject;
        }

        int decisionValue = Random.Range(1, 7);
        if (decisionValue != 6)
            AI.AIDecision(decisionValue);
        else
        {
            for (int x = 0; x < objectList.Count-1; x++)
                if (!objectList[x].GetComponent<CharacterBase>().isDowned)
                {
                    AI.currentEnemy = objectList[x].gameObject;
                    AI.AIDecision(4);
                }
            Debug.Log("AI - MultiAttack");
            AI.currentEnemy = null;
        }

        objectList[objectList.Count - 1].hasCompletedTurn = true;
    }

    void EndGame()
    {
        int downedPlayers = 0;
        for (int x = 0; x < objectList.Count-1; x++)
        {
            if (objectList[x].gameObject.GetComponent<CharacterBase>().isDowned)
            {
                downedPlayers++;
            }
        }

        if (objectList[objectList.Count - 1].GetComponent<CharacterBase>().isDowned == true)
        {
            // END GAME PLAYERS WIN
            gameOver = true;
            Debug.Log("PLAYERS WIN");
            victory = true;
            return;
        }
        else if (downedPlayers == objectList.Count - 1 && objectList[objectList.Count - 1].GetComponent<CharacterBase>().isDowned == false)
        {
            // END GAME PLAYERS LOSE
            Debug.Log("AI WINS");
            victory = false;
            gameOver = true;
            return;
        }
    }

    private void Start()
    {
        int startingBossHealth = 25;
        for (int x = 0; x < objectList.Count - 1; x++)
            startingBossHealth -= 5;

        objectList[objectList.Count - 1].GetComponent<CharacterBase>().health -= startingBossHealth;
    }
    ///
}
