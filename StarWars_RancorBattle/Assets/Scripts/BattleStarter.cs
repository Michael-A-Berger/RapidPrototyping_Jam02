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
    GameObject mandoPref, wookPref, jedPref, boPref, statPref;
    [SerializeField]
    TurnManager turnyBernie;
    readonly Vector3 separation = 2.5f * Vector3.right;
    Vector3 center = new Vector3(0, 0, -5);
    Vector3[] spawnLocations;
    public string playerPrefsKey = "";
    public bool debug = false;

    public static bool speed = false;

    // Properties
    private List<CharacterBase> playerObjects = new List<CharacterBase>();
    private CharacterBase boss;

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

            turnyBernie.objectList = new List<CharacterBase>();
            turnyBernie.spawnPositions = spawnLocations;

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
                if (current == null)
                    throw new Exception("Player prefab not found??? The character requested was \"" + players[num] + "\".");

                CharacterBase script = Instantiate(current, spawnLocations[num], Quaternion.identity).GetComponent<CharacterBase>();
                playerObjects.Add(script);
                Instantiate(statPref).GetComponent<StatDisplay>().SetCharacter(script);
                turnyBernie.objectList.Add(script);
            }
            GameObject badddie = GameObject.Find("Rancor");
            boss = badddie.GetComponent<CharacterBase>();
            Instantiate(statPref).GetComponent<StatDisplay>().SetCharacter(boss);
            foreach (CharacterBase player in playerObjects)
            {
                player.currentEnemy = boss;
            }

            turnyBernie.objectList.Add(boss);
            /*
            int startingBossHealth = 25;
            for (int x = 0; x < playerObjects.Count - 1; x++)
                startingBossHealth -= 5;

            badddie.GetComponent<CharacterBase>().health -= startingBossHealth;
            */
            boss.health = 15 + 5 * (playerObjects.Count);

            FloatText.CreateFloatText(
                "Battle start!",
                new Color(0x7F, 0xFF, 0x00),
                center + Vector3.up * 2 + Vector3.forward,
                8, 1.5f, 0.5f
            );
        }
    }



    // Update is called once per frame
    void Update()
    {

    }
}
