using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class PlayerSelectManager : MonoBehaviour
{
    [SerializeField]
    DropArea[] dropAreas;
    //Drop areas for selected areas, not-yet-selected areas

    [SerializeField]
    List<string> players = new List<string>();

    // Start is called before the first frame update
    void Start()
    {

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

                //Oh boy. Watch out.
                int howDo = (to.holding ? 4 : 0) + (toInd >= 4 ? 2 : 0) + (fromInd >= 4 ? 1 : 0);
                //msb: destination fillled?
                //2nd: destination is unselected?
                //lsb: source is unselected?

                switch (howDo)
                {
                    case 0: //from selectus to empty selectus
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

                        break;

                    case 1: //from seligendus to empty selectus
                        to = dropAreas[players.Count];
                        item.Settle(to);
                        players.Add(item.Value);
                        break;

                    case 2: //from selectus to empty seligendus
                        players.Remove(item.Value);

                        for (int i = fromInd; i < players.Count; i++)
                        {
                            DropArea a = dropAreas[i], b = dropAreas[i + 1];
                            b.holding.Settle(a);
                            b.Vacate();
                        }

                        item.Settle(to);
                        break;

                    case 3: //from seligendus to empty seligendus
                        item.Settle(to);
                        break;

                    //Let's dance

                    case 4: //from selectus to occcupied selectus
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
                        break;

                    case 5: //from seligendus to occupied selectus
                        players.Insert(toInd, item.Value);
                        for (int i = players.Count - 1; i > toInd; i--)
                        {
                            //print("Moving " + i + " to " + (i + 1));
                            DropArea a = dropAreas[i - 1], b = dropAreas[i];
                            a.holding.Settle(b);
                            a.Vacate();
                        }
                        item.Settle(to);
                        break;

                    case 6: //from selectus to occcupied seligendus
                        players.Remove(item.Value);

                        for (int i = fromInd; i < players.Count; i++)
                        {
                            DropArea a = dropAreas[i], b = dropAreas[i + 1];
                            b.holding.Settle(a);
                            b.Vacate();
                        }

                        for (int i = 4; true; i++) //I live on the edge
                        {
                            if (!dropAreas[i].holding)
                            {
                                item.Settle(dropAreas[i]);
                                break;
                            }
                        }
                        break;

                    case 7: //from seligendus to occupied seligendus
                        for (int i = 4; true; i++) //Enan Danger Munzar
                        {
                            if (!dropAreas[i].holding)
                            {
                                item.Settle(dropAreas[i]);
                                break;
                            }
                        }
                        break;
                }

            }

        }
    }
}
