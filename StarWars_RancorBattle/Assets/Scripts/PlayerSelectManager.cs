using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerSelectManager : MonoBehaviour
{
    [SerializeField]
    DropArea[] selectusAreas; //Drop areas for selected areas
    DropArea[] selendusAreas; //Drop areas for 
    // Start is called before the first frame update
    void Start()
    {
        //testArea = GameObject.Find("Panel").GetComponent<DropArea>();
    }

    public void CheckForDrop(Draggable item, PointerEventData eventData)
    {
        //if(PlayerSelecting)
        //{
        //    //Do player selecty things
        //}
        //else
        //{
        foreach(DropArea a in selectusAreas)
        {
            if (a.DropCheck(eventData.position))
            {
                item.transform.position = a.transform.position;
            }
        }
        //}
    }
}
