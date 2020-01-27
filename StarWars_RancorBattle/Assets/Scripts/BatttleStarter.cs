using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatttleStarter : MonoBehaviour
{
    List<string> players;
    [SerializeField]
    PlayerSelectManager teamSelector;

    [SerializeField]
    GameObject mandoPref, wookPref, jedPref, boPref;
    // Start is called before the first frame update
    void Start()
    {
        players = teamSelector.players;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
