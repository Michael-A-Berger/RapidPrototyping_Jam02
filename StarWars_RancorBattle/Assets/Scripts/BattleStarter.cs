using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleStarter : MonoBehaviour
{
    // Attributes
    public List<string> players;
    [SerializeField]
    PlayerSelectManager teamSelector;
    [SerializeField]
    GameObject mandoPref, wookPref, jedPref, boPref;
    public Vector3[] spawnLocations;
    public string playerPrefsKey = "";
    public bool debug = false;

    // Properties
    private List<GameObject> playerObjects = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        // players = teamSelector.players;

        // IF the player prefs key name is not set, squack at the dev
        if (playerPrefsKey.Length < 1)
        {
            if (debug) Debug.LogError("'playerPrefsKey' is undefined!");
        }
        // ELSE... (the player preferences key name is set)
        else
        {
            // Getting the list of players to spawn
            string toSpawn = PlayerPrefs.GetString(playerPrefsKey);
            players = new List<string>(toSpawn.Split(','));

            // Spawning the players
            for (int num = 0; num < players.Count; num++)
            {
                // Defining the current prefab
                GameObject current = null;

                // SWITCH for each player type
                switch (players[num])
                {
                    case "Mando":
                        current = mandoPref;
                        break;
                    case "Wook":
                        current = wookPref;
                        break;
                    case "Jed":
                        current = jedPref;
                        break;
                    case "Bo":
                        current = boPref;
                        break;
                    default:
                        if (debug) Debug.LogError("Prefab type of, '" + players[num] + "' is not defined!");
                        break;
                }

                // Spawning the current game object in the right spawn location
                if (current != null)
                {
                    playerObjects.Add(Instantiate(current, spawnLocations[num], Quaternion.identity));
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
