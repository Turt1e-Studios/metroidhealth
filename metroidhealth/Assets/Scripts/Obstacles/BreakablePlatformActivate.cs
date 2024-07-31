using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakablePlatformActivate : MonoBehaviour
{

    private bool isBroken = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    void OnCollisionEnter2D(Collision2D collision)
    {
        if (isBroken) return;

        PlayerMovement playerMovement = collision.gameObject.GetComponent<PlayerMovement>();
        if (playerMovement != null && playerMovement.IsAnyDash())
        {
            isBroken = true;
            Destroy(gameObject);
        }
    }
}
