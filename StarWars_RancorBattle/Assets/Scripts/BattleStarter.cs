using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleStarter : MonoBehaviour
{
    // Attributes
    [SerializeField]
    List<string> players;
    [SerializeField]
    GameObject mandoPref, wookPref, jedPref, boPref;
    readonly Vector3 separation = 2.5f * Vector3.right;
    Vector3 center = new Vector3(0, 0, -5);
    Vector3[] spawnLocations;
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

            Vector3 start = center - separation * (players.Count - 1) / 2;

            spawnLocations = new Vector3[players.Count];

            for (int i = 0; i < players.Count; i++)
            {
                spawnLocations[i] = start + i * separation;
            }


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

        FloatText.CreateFloatText("Battle start!", new Color(0x7F, 0xFF, 0x00), center + Vector3.up * 1 + Vector3.forward, 8, 1.5f, 0.5f);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
