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
}
