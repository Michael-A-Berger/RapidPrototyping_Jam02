using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Draggable : MonoBehaviour, IDragHandler, IDropHandler
{
    PlayerSelectManager daMan;
    Vector2 lastPostition;

    public string Value { get; private set; }
    public void SetPosition(Vector2 newP)
    {
        lastPostition = transform.position = newP;
    }

    public void ResetPosition()
    {
        transform.position = lastPostition;
    }

    // Start is called before the first frame update
    void Start()
    {
        daMan = GameObject.FindGameObjectWithTag("Manager").GetComponent<PlayerSelectManager>();
        lastPostition = transform.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
    }

    public void OnDrop(PointerEventData eventData)
    {
        daMan.CheckForDrop(this, eventData);
    }
}
