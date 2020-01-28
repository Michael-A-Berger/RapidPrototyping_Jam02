using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;

public class PlayerReorderManager : MonoBehaviour
{
    public List<string> players;
    public bool done = false;
    [SerializeField]
    GameObject mandObj, wookObj, jedObj, boObj;
    Draggable mandoSel, wookSel, jedSel, boSel;
    [SerializeField]
    DropArea[] dropAreas;
    // Start is called before the first frame update
    private void Awake()
    {
        mandoSel    = mandObj.GetComponent<Draggable>();
        wookSel     = wookObj.GetComponent<Draggable>();
        jedSel      =  jedObj.GetComponent<Draggable>();
        boSel       =   boObj.GetComponent<Draggable>();
    }
    public void Init(List<CharacterBase> old)
    {
        players = new List<string>();

        mandObj.SetActive(false);
        wookObj.SetActive(false);
        jedObj.SetActive(false);
        boObj.SetActive(false);


        for (int i = 0; i < old.Count - 1; i++)
        {
            players.Add(old[i].charName);
            switch (players[i])
            {
                case "Mando":
                    mandObj.SetActive(true);
                    mandoSel.Settle(dropAreas[i]);
                    break;
                case "Wook":
                    wookObj.SetActive(true);
                    wookSel.Settle(dropAreas[i]);
                    break;
                case "Jed":
                    jedObj.SetActive(true);
                    print(jedSel);
                    jedSel.Settle(dropAreas[i]);
                    break;
                case "Bo":
                    boObj.SetActive(true);
                    boSel.Settle(dropAreas[i]);
                    break;
            }
        }
    }
    public void CheckForDrop(PointerEventData eventData)
    {
        Draggable item = null;
        int toInd = -1;
        DropArea to = null;
        for (int i = 0; i < dropAreas.Length; i++)
        {
            to = dropAreas[i];
            if (to.DropCheck(eventData.position))
            {
                item = Draggable.FindFloating();
                toInd = i;
                break;
            }
        }

        if (!item) Draggable.ResetPositions(); //No one was dropped into a valid DropArea
        else
        {
            if (!to)
            {
                throw new Exception("Waaaaaaah!");
            }

            //If it didn't even move...
            DropArea from = item.home;
            if (!from)
            {
                //Came from nowhere? (Initialisation)
                item.Settle(to);
            }
            else
            {

                if (from == to)
                {
                    Draggable.ResetPositions();
                    return;
                }

                int fromInd = Array.IndexOf(dropAreas, from);
                from.Vacate();

                //This is a pleasant fiction.
                bool empty = !to.holding;

                if (empty)
                {   //case 0:
                    //from selectus to empty selectus
                    players.Remove(item.Value);

                    //print("From " + fromInd + ", and there are " + players.Count);

                    for (int i = fromInd; i < players.Count; i++)
                    {
                        DropArea a = dropAreas[i], b = dropAreas[i + 1];
                        b.holding.Settle(a);
                        b.Vacate();
                    }


                    to = dropAreas[players.Count];
                    item.Settle(to);
                    players.Add(item.Value);

                }
                else
                {   //case 4:
                    //from selectus to occcupied selectus
                    players.Remove(item.Value); players.Insert(toInd, item.Value);
                    if (toInd < fromInd)
                    {
                        for (int i = fromInd - 1; i > toInd - 1; i--)
                        {
                            //print("Moving " + i + " to " + (i + 1));
                            DropArea a = dropAreas[i], b = dropAreas[i + 1];
                            a.holding.Settle(b);
                            a.Vacate();
                        }
                    }
                    else
                    {
                        for (int i = fromInd; i < toInd; i++)
                        {
                            DropArea a = dropAreas[i], b = dropAreas[i + 1];

                            b.holding.Settle(a);
                            b.Vacate();
                        }
                    }
                    item.Settle(to);
                }

            }

        }
    }
}
