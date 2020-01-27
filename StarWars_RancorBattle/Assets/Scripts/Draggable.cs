using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Draggable : MonoBehaviour, IDragHandler, IDropHandler
{
    PlayerSelectManager daMan;
    Vector2 lastPostition;
    public DropArea home { get; private set; }
    [SerializeField]
    string starting;

    bool beingDragged = true;

    static List<Draggable> instances = new List<Draggable>();
    /// <summary>
    /// Tellls every instance to set its position, if it is being draggged
    /// </summary>
    public static Draggable FindFloating()
    {
        foreach (Draggable d in instances)
        {
            if (d.beingDragged)
            {
                return d;
            }
        }
        return null;
    }

    public static void ResetPositions()
    {
        foreach (Draggable d in instances)
        {
            if(d.beingDragged) d.ResetPosition();
        }
    }


    public string Value;


    public void Settle(DropArea house)
    {
        beingDragged = false;
        house.Accept(this);
        home = house;
        lastPostition = house.transform.position;
        ResetPosition();
    }

    void ResetPosition()
    {
        transform.position = lastPostition;
    }


    // Start is called before the first frame update
    void Start()
    {
        daMan = GameObject.FindGameObjectWithTag("Manager").GetComponent<PlayerSelectManager>();
        Settle(GameObject.Find(starting).GetComponent<DropArea>());
        instances.Add(this);
    }

    public void OnDrag(PointerEventData eventData)
    {
        beingDragged = true;
        transform.position = eventData.position;
    }

    public void OnDrop(PointerEventData eventData)
    {
        daMan.CheckForDrop(eventData);
    }
}
