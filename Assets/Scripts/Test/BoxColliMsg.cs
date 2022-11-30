using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxColliMsg : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D other) {
        Debug.Log("Enter~");
    }

    private void OnTriggerEnter2D(Collider2D other) {
        Debug.Log("Trigger~");
    }
}
