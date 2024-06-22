using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    // A plate that activates something else when touching an object
    
    [SerializeField] private GameObject target;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Object"))
        {
            target.GetComponent<Activatable>().Activate();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Object"))
        {
            target.GetComponent<Activatable>().Deactivate();
        }
    }
}
