using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBase : MonoBehaviour
{
    [SerializeField]
    string name;
    [SerializeField]
    public int health;
    [SerializeField]
    int defense;
    [SerializeField]
    int attack;
    [SerializeField]
    int special;
    public bool isDowned;
    public GameObject currentEnemy;
    public GameObject currentTarget;
    

    // Start is called before the first frame update
    void Awake()
    {
        switch(name)
        {
            case "Mando":
                health = 3;
                defense = 4;
                attack = 2;
                special = 0;
                currentTarget = gameObject;
                break;
            case "Bo":
                health = 4;
                defense = 2;
                attack = 1;
                special = 0;
                break;
            case "Jed":
                health = 2;
                defense = 5;
                attack = 2;
                special = 0;
                break;
            case "Wook":
                health = 5;
                defense = 2;
                attack = 2;
                special = 0;
                currentTarget = gameObject;
                break;
            case "Ranc":
                health = 20;
                defense = 5;
                attack = 3;
                special = -1;
                currentTarget = gameObject;
                break;
        }
    }

    // For debugging and checking status purposes
    public int Defense
    {
        get
        {
            return defense;
        }
    }

    public void IsDowned()
    {
        isDowned = !isDowned;
    }

    // handles attacking phase
    public void Attacking(int extraDamage)
    {
        if (Random.Range(1, 7) + attack < currentEnemy.GetComponent<CharacterBase>().defense)
        { 
            currentEnemy.GetComponent<CharacterBase>().health -= attack + extraDamage; 
            Debug.Log("HIT! - " + currentEnemy.GetComponent<CharacterBase>().health); 

        }
        else 
            Debug.Log("MISS! - " + currentEnemy.GetComponent<CharacterBase>().health);
    }

    // adds new special
    public void AddSpecial()
    {
        special++;
    }

    public void SelectTarget()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("clicking...");
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100))
            {
                currentTarget = hit.transform.gameObject;
            }
        }
    }

    public void UseSpecial()
    {
        // sets specials and determines which one is used
        if (special > 0)
        {
            switch (name)
            {
                case "Mando":
                    Attacking(3);
                    break;
                case "Wook":
                    for (int x = 0; x < 3; x++)
                        Attacking(0);
                    break;
                case "Jed":
                    currentTarget.GetComponent<CharacterBase>().isDowned = false;
                    currentTarget.GetComponent<CharacterBase>().health = 2;
                    currentTarget = null;
                    break;
                case "Bo":
                    currentTarget.GetComponent<CharacterBase>().health += 1;
                    Debug.Log("HEAL! - " + currentTarget.GetComponent<CharacterBase>().health);
                    currentTarget = null;
                    break;
            }

            // decrements special
            special--;
        }
    }

    public void AIDecision(int decision)
    {
        if (isDowned == true)
            return;

        switch (decision)
        {
            case 1:
                // nothing
                break;
            case 2:
            case 3:
            case 4:
                // Have player selected in the game loop through local player list
                Attacking(0);
                break;
            case 5:
                health += 1;
                break;
            case 6:
                // put this in a for loop for the main game loop
                Attacking(-2);
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
