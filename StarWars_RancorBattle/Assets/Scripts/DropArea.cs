using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropArea : MonoBehaviour
{
    Draggable contains;

    Rect area;
    private void Start()
    {
        area = (transform as RectTransform).rect;
        area.x += transform.position.x;
        area.y += transform.position.y;
    }

    public bool DropCheck(Vector2 position)
    {
        return area.Contains(position);
    }

    public void Attach(Draggable item)
    {

    }
}
