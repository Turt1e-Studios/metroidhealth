using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    // Parent class of any class that can be interacted with by pressing space
    
    private void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player") && Input.GetKey(KeyCode.Space))
        {
            Interact();
            Destroy(gameObject);
        }
    }

    protected virtual void Interact() {}
}
