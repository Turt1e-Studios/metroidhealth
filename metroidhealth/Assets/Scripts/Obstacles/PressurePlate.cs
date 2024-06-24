using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    // A plate that activates something else when touching an object
    
    [SerializeField] private GameObject target;

    private int _contactCount = 0;
    private void OnTriggerEnter2D(Collider2D col)
    {
        if ((col.gameObject.CompareTag("Box") || col.gameObject.CompareTag("Player")) && OnBottom(col))
        {
            _contactCount += 1;
            target.GetComponent<Activatable>().Activate();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if ((other.gameObject.CompareTag("Box") || other.gameObject.CompareTag("Player")) && OnBottom(other))
        {
            _contactCount -= 1;
            if (_contactCount <= 0) 
            {
                target.GetComponent<Activatable>().Deactivate();
            }
            
        }
    }

    private bool OnBottom(Collider2D topCollider)
    {
        BoxCollider2D bottomCollider = GetComponent<BoxCollider2D>();
        
        float topObject = topCollider.bounds.min.y;
        float bottomObject = bottomCollider.bounds.max.y;
        return topObject >= bottomObject - 0.1f;
    }
}
