using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBase : MonoBehaviour
{
    [SerializeField]
    public string charName;
    [SerializeField]
    public int health;
    [SerializeField]
    int defense;
    [SerializeField]
    int attack;
    [SerializeField]
    public int special;
    public bool CanSpecial { get { return special > 0; } }
    public bool isDowned;
    public CharacterBase currentEnemy;
    public CharacterBase currentTarget;

    //Co-opted from TurnProperty

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

    // Start is called before the first frame update
    void Awake()
    {
        switch(charName)
        {
            case "Mando":
                health = 3;
                defense = 4;
                attack = 2;
                special = 0;
                currentTarget = this;
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
                currentTarget = this;
                break;
            case "Ranc":
                health += 30;
                defense = 5;
                attack = 4;
                special = -1;
                currentTarget = this;
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

    public void GetDown()
    {
        isDowned = true;
    }

    public void Revive()
    {
        isDowned = false;
    }


    /// <summary>
    /// Overload that takes an offfset for the float text
    /// </summary>
    /// <param name="extraDamage"></param>
    /// <param name="displayOffset"></param>
    void Attacking(int extraDamage, Vector3 displayOffset)
    {
        int hitting = Random.Range(1, 7) + attack;
        Debug.Log("TO HIT: " + hitting);
        if (hitting > currentEnemy.GetComponent<CharacterBase>().defense)
        {
            currentEnemy.GetComponent<CharacterBase>().health -= attack + extraDamage;
            Debug.Log("HIT! - " + currentEnemy.GetComponent<CharacterBase>().charName + ": "
                + currentEnemy.GetComponent<CharacterBase>().health);

            FloatText.CreateFloatText("−" + (attack + extraDamage), new Color(100, 0, 0.1f),
                currentEnemy.transform.position + 2 * Vector3.up + displayOffset);
        }
        else
        {
            Debug.Log(
                "MISS! - " + currentEnemy.GetComponent<CharacterBase>().charName + ": " +
                currentEnemy.GetComponent<CharacterBase>().health);
            FloatText.CreateFloatText("Miss", new Color(0, 0.2f, 1),
                currentEnemy.transform.position + 2 * Vector3.up + displayOffset);
        }

        if (currentEnemy.GetComponent<CharacterBase>().health <= 0)
            currentEnemy.GetComponent<CharacterBase>().GetDown();
    }

    // handles attacking phase
    public void Attacking(int extraDamage)
    {
        int hitting = Random.Range(1, 7) + attack;
        Debug.Log("TO HIT: " + hitting);
        if (hitting > currentEnemy.GetComponent<CharacterBase>().defense)
        {
            currentEnemy.GetComponent<CharacterBase>().health -= attack + extraDamage;
            Debug.Log("HIT! - " + currentEnemy.GetComponent<CharacterBase>().charName + ": "
                + currentEnemy.GetComponent<CharacterBase>().health);

            FloatText.CreateFloatText("−" + (attack + extraDamage), new Color(100, 0, 0.1f),
                currentEnemy.transform.position + 2 * Vector3.up);
        }
        else
        {
            Debug.Log(
                "MISS! - " + currentEnemy.GetComponent<CharacterBase>().charName + ": " +
                currentEnemy.GetComponent<CharacterBase>().health);
            FloatText.CreateFloatText("Miss", new Color(0, 0.2f, 1),
                currentEnemy.transform.position + 2 * Vector3.up);
        }

        if (currentEnemy.GetComponent<CharacterBase>().health <= 0)
            currentEnemy.GetComponent<CharacterBase>().GetDown();
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
                currentTarget = hit.transform.gameObject.GetComponent<CharacterBase>();
            }
        }
    }

    public void UseSpecial()
    {
        // sets specials and determines which one is used
        if (special > 0)
        {
            switch (charName)
            {
                case "Mando":
                    FloatText.CreateFloatText("Empowered hit", Color.black, transform.position + 2 * Vector3.up);
                    Attacking(3);

                    special--;
                    break;
                case "Wook":
                    FloatText.CreateFloatText("Triple shot", Color.black, transform.position + 2 * Vector3.up);
                    for (int x = 0; x < 3; x++)
                        Attacking(0, Vector3.left * (x - 1));

                    special--;
                    break;
                case "Jed":
                    if (!currentTarget)
                    {
                        SelectTarget();
                    }
                    else
                    {
                        if (currentTarget.health <= 0)
                        {
                            currentTarget.GetComponent<CharacterBase>().Revive();
                            currentTarget.GetComponent<CharacterBase>().health = 2;
                            currentTarget = null;

                            special--;
                            CompleteTurn();
                        }
                        else
                        {
                            FloatText.CreateFloatText("He stilll live", new Color(1, 0, 0.8f) , currentTarget.transform.position + 2 * Vector3.up);
                            CompleteTurn();
                        }
                        currentTarget = null;
                    }
                    break;
                case "Bo":
                    if(!currentTarget)
                    {
                        SelectTarget();
                    }
                    else
                    {
                        if (currentTarget.health > 0)
                        {
                            FloatText.CreateFloatText("Heal!", Color.black, transform.position + 2 * Vector3.up);
                            currentTarget.GetComponent<CharacterBase>().health += 1;
                            FloatText.CreateFloatText("+1", Color.green, currentTarget.transform.position + 2 * Vector3.up);
                            Debug.Log("HEAL! - " + currentTarget.GetComponent<CharacterBase>().health);
                            CompleteTurn();

                            special--;
                        }
                        else
                        {
                            FloatText.CreateFloatText("He ded :(", Color.green, currentTarget.transform.position + 2 * Vector3.up);
                        }
                        currentTarget = null;
                    }
                    break;
            }

            // decrements special
        }
        else
        {
            throw new System.Exception("Check if your character can special before specialing. Pooor " + charName);
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
                Debug.Log("AI - Nothing");
                FloatText.CreateFloatText("Loafing...", Color.blue, transform.position + 2 * Vector3.up);
                currentEnemy = null;
                break;
            case 2:
            case 3:
            case 4:
                // Have player selected in the game loop through local player list
                Attacking(-3);
                Debug.Log("AI - Attacking");
                currentEnemy = null;
                break;
            case 5:
                health += 1;
                Debug.Log("AI - Healing - " + health);
                currentEnemy = null;
                FloatText.CreateFloatText("+1", Color.green, transform.position + 2 * Vector3.up);
                break;
        }
    }
}
