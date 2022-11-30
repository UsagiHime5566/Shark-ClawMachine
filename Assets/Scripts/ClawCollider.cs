using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClawCollider : MonoBehaviour
{
    public Action<GameObject> OnHitDoll;
    private void OnTriggerEnter2D(Collider2D other) {
        Debug.Log("enter~" + other.name);

        OnHitDoll?.Invoke(other.gameObject);
    }
}
