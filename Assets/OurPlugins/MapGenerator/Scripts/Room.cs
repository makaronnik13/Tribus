using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tribus
{	
public class Room : MonoBehaviour {

    public Vector2 position;
    public bool visited = false;

    private SpriteRenderer _renderer;
    private SpriteRenderer spriteRenderer
    {
        get
        {
            if (!_renderer)
            {
                _renderer = GetComponentsInChildren<SpriteRenderer>()[0];
            }
            return _renderer;
        }
    }

    private SpriteRenderer _emptySpaceRenderer;
    private SpriteRenderer emptySpaceSpriteRenderer
    {
        get
        {
            if (!_emptySpaceRenderer)
            {
                _emptySpaceRenderer = GetComponentsInChildren<SpriteRenderer>()[1];
            }
            return _emptySpaceRenderer;
        }
    }

    public enum RoomState
    {
        Near,
        Hidden,
        Far
    }

    private RoomState _currentRoomState;
    public RoomState CurrentRoomState
    {
        get
        {
            return _currentRoomState;
        }
        set
        {
            _currentRoomState = value;
            if (value == RoomState.Hidden)
            {
                spriteRenderer.enabled = false;
            }
            else
            {
                spriteRenderer.enabled = true;
                emptySpaceSpriteRenderer.enabled = true;
                if (value == RoomState.Near)
                {
                    spriteRenderer.color = Color.white;
                }
                else
                {
                    spriteRenderer.color = Color.gray;
                }
            }
        }
    }

	public void GoTo()
    {
        if (CurrentRoomState != RoomState.Near)
        {
            return;
        }
        GetComponentInParent<RoomMap>().TryToGoTo(this);
    }


    public void Init(Vector2 v)
    {
        position = v;
        CurrentRoomState = RoomState.Hidden;
    }

    public void ComeToRoom()
    {
        FindObjectOfType<RoomActivator>().ActivateEncouter(visited);
        visited = true;
    }
}
}
