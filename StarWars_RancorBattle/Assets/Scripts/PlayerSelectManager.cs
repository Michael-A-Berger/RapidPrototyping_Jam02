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
            if(!to)
            {
                throw new Exception("Waaaaaaah!");
            }

            //If it didn't even move...
            DropArea from = item.home;
            if (from == to)
            {
                Draggable.ResetPositions();
                return;
            }

            int fromInd = Array.IndexOf(dropAreas, from);
            from.Vacate();

            //Oh boy. Watch out.
            int howDo = (fromInd < 4 ? 1 : 0) + (toInd < 4 ? 2 : 0) + (to.con)



            item.Settle(to);
        }
    }
}
