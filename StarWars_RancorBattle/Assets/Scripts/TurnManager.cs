using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TurnManager : MonoBehaviour
{
    // Attributes
    public GameObject reorderMenu;
    public List<CharacterBase> objectList;
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
    private PlayerReorderManager reorderer;

    [SerializeField]
    private bool paused = false;
        

    // StartRound()
    public void StartRound()
    {
        FloatText.CreateFloatText("Round start", Color.black, Vector3.zero);
        print("StartRound");
        if (objectList.Count < 1)
        {
            Debug.LogError("'objectList' in TurnManager is not defined!");
        }
        else
        {
            if (!gameOver)
            {
                LookFrom(new Vector3(0, 14, -5), new Vector3(0, 0, -4));
                paused = true;
                StartCoroutine("Reorder");
            }
            else
            {
                SceneManager.LoadScene("Player Select");
                Debug.Log("Game has ended");
            }
        }
    }

    IEnumerator Reorder()
    {
        print("Reordering?");
        reorderMenu.SetActive(true);
        reorderer.Init(objectList);
        reorderer.done = false;
        Dictionary<string, CharacterBase> fromNames = new Dictionary<string, CharacterBase>();
        foreach(CharacterBase c in objectList)
        {
            if(c.name != "Ranc") fromNames.Add(c.charName, c);
        }

        while(!reorderer.done)
        {
            yield return null;
        }

        reorderMenu.SetActive(false);
        attemptingRound = true;

        Reposition(fromNames, reorderer.players);


        currentIndex = 0;
        objectList[0].isCurrentTurn = true;
        MoveCamera();
        if (debug)
        {
            Debug.Log("Start of Round!");
            Debug_CurrentTurn();
        }
        paused = false;
    }

    public Vector3[] spawnPositions;
    void Reposition(Dictionary<string, CharacterBase> ids, List<string> order)
    {
        for (int i = 0; i < order.Count; i++)
        {
            string p = order[i];
            ids[p].transform.position = spawnPositions[i];
            objectList[i] = ids[p];
        }
    }

    // MoveCamera()
    public void MoveCamera()
    {
        // IF the camera script exists...
        if (cameraScript != null)
        {
            // Telling the camera to move to the new position + rotation
            CharacterBase current = objectList[currentIndex];
            Transform currentT = current.gameObject.transform;
            Vector3 newLoc = currentT.position + currentT.rotation * relativeCameraPos;
            Vector3 newRot = currentT.rotation * relativeCameraRot;
            cameraScript.targetPos = newLoc;
            cameraScript.targetDir.SetLookRotation(newRot);
        }
        else
        {
            throw new System.Exception("Turn manager neeeds acccesss to a camera script. Where'd you put it you nitwit???");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameOver)
        {
            if (paused) return;
            CheckForEndGame();

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
                            objectList[currentIndex].CompleteTurn();
                        else
                        {
                            HandlePlayer();
                            if (paused) return;
                        }
                    }
                    else
                    {
                        HandleAI(objectList[objectList.Count - 1].GetComponent<CharacterBase>());
                        Debug.Log("AI TURN");
                        CharacterBase recipient = objectList[Random.Range(0, objectList.Count - 1)];
                        recipient.AddSpecial();
                        FloatText.CreateFloatText("Specialable", new Color(1, 1, 0), recipient.transform.position + 2 * Vector3.up);
                    }
                    ///

                    // IF the current turn object has completed its action...
                    if (objectList[currentIndex].hasCompletedTurn)
                    {
                        // Telling the current object that it's turn is over
                        objectList[currentIndex].isCurrentTurn = false;
                        currentIndex++;
                        special = false;

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
                    foreach (CharacterBase prop in objectList)
                    {
                        prop.hasCompletedTurn = false;
                        prop.isCurrentTurn = false;
                    }
                    if (debug) Debug.Log("End of Round!");

                    StartRound();
                }
            }
        }
    }

    void Look(Vector3 where)
    {
        Vector3 cameraDir = where - cameraScript.transform.position;
        cameraScript.targetDir = Quaternion.LookRotation(cameraDir);
    }

    void LookFrom(Vector3 source, Vector3 target)
    {
        Vector3 cameraDir = target - source;
        cameraScript.targetDir = Quaternion.LookRotation(cameraDir);
        cameraScript.targetPos = source;
    }

    IEnumerator Stop(float howLong)
    {
        float timer = 1;
        while (timer >= 0)
        {
            timer -= Time.deltaTime;
            yield return null;
        }
        paused = false;
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
                        objectList[keyCode - 49].CompleteTurn();
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
        if (special)
        {
            objectList[currentIndex].UseSpecial();
            return;
        }
        /// <summary>
        /// Attacking Functionality
        /// Test Code for checking if the Attacking(int) function works for players.
        /// </summary>
        if (Input.GetKeyUp(KeyCode.A))
        {
            // Grabs the player script from the current objects turn and 
            objectList[currentIndex].Attacking(0);

            // Completes the players turn once Attacking(int) has finished
            objectList[currentIndex].CompleteTurn();

            if (!BattleStarter.speed)
            {
                Look(objectList[currentIndex].currentEnemy.transform.position);
                StartCoroutine("Stop", 1);

                paused = true;
            }
        }
        // Activates boolean to begin special functionality for players
        else if (Input.GetKeyDown(KeyCode.S))
        {
            if (objectList[currentIndex].CanSpecial)
            {
                special = true;
                Debug.Log("SPECIAL STARTED...");

                if (!BattleStarter.speed)
                {
                    FloatText.CreateFloatText("Specialing...", Color.black,
                        objectList[currentIndex].transform.position + 2 * Vector3.up);
                    LookFrom(new Vector3(0, 14, -5), new Vector3(0, 0, -4));
                    StartCoroutine("Stop", 0.1f);
                    paused = true;
                }
            }
            else
            {
                FloatText.CreateFloatText("Can't special", Color.black,
                    objectList[currentIndex].transform.position + 2*Vector3.up);
            }
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
                objectList[currentIndex].CompleteTurn();
            }
        }
    }

    void HandleAI(CharacterBase AI)
    {
        while (AI.currentEnemy == null)
        {
            int selectPlayer = Random.Range(0, objectList.Count - 1);
            if (!objectList[selectPlayer].GetComponent<CharacterBase>().isDowned)
                AI.currentEnemy = objectList[selectPlayer];
        }

        int decisionValue = Random.Range(1, 7);
        if (decisionValue != 6)
            AI.AIDecision(decisionValue);
        else
        {
            for (int x = 0; x < objectList.Count-1; x++)
                if (!objectList[x].GetComponent<CharacterBase>().isDowned)
                {
                    AI.currentEnemy = objectList[x];
                    AI.AIDecision(4);
                }
            Debug.Log("AI - MultiAttack");
            AI.currentEnemy = null;
        }

        objectList[objectList.Count - 1].CompleteTurn();

        StartCoroutine("Stop", 1);
        paused = true;
    }

    /// <summary>
    /// Actuallly, CHECK for end-of-game. Sets the values of victory and gameOver acccordingly (I asssume)
    /// </summary>
    void CheckForEndGame()
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

    private void Awake()
    {
        reorderer = reorderMenu.GetComponent<PlayerReorderManager>();
        reorderMenu.SetActive(false);
    }

    private void Start()
    {
        StartRound();
    }
    ///
}
