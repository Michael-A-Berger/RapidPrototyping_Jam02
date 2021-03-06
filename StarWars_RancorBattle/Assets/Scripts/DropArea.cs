﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropArea : MonoBehaviour
{
    public Draggable holding { get; private set; }

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

    public void Accept(Draggable item)
    {
        holding = item;
    }

    public void Vacate()
    {
        holding = null;
    }
}
