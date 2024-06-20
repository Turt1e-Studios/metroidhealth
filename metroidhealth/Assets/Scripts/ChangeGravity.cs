using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeGravity : MonoBehaviour
{
    // This script inverses the player's gravity when they touch this object
    
    [SerializeField] private float gravityMultiplier;

    private Collider2D _collider2D;
    private Collider2D _playerCollider2D;
    private PlayerMovement _playerMovement;

    private bool _hasBeenTouched;
    
    // Start is called before the first frame update
    void Start()
    {
        _collider2D = GetComponent<Collider2D>();
        _playerCollider2D = GameObject.FindWithTag("Player").GetComponent<Collider2D>();
        _playerMovement = GameObject.FindWithTag("Player").GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_collider2D.IsTouching(_playerCollider2D))
        {
            print("is touching player");
            _playerMovement.SetNegativeGravity();
            _hasBeenTouched = true;
        }
        else if (_hasBeenTouched)
        {
            print("reseting gravity");
            _playerMovement.MultiplyGravity(-1f);
            _hasBeenTouched = false;
        }
    }

    // private void OnTriggerEnter2D(Collider2D col)
    // {
    //     if (col.CompareTag("Player"))
    //     {
    //         col.GetComponent<PlayerMovement>().MultiplyGravity(gravityMultiplier);
    //     }
    // }
    //
    // private void OnTriggerExit2D(Collider2D col)
    // {
    //     if (col.CompareTag("Player"))
    //     {
    //         col.GetComponent<PlayerMovement>().RevertGravity();
    //     }
    // }
}
