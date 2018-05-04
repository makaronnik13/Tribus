using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class PushablePolygon : MonoBehaviour
{
    public UnityEvent OnPush;

    public void OnMouseDown()
    {
        if (OnPush!=null)
        {
            OnPush.Invoke();
        }
    }
}
