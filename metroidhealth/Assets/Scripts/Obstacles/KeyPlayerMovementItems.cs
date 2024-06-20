using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class KeyPlayerMovementItems : Item
{
    [SerializeField] [Range(1, 5)] private int ability;
    private PlayerMovement _playerMovement;

    private void Start()
    {
        _playerMovement = GameObject.FindWithTag("Player").GetComponent<PlayerMovement>();
    }

    protected override void Interact()
    {
        switch (ability)
        {
            case 1:
                _playerMovement.SetDoubleJump(true);
                break;
            case 2:
                _playerMovement.SetDash(true);
                break;
            case 3:
                _playerMovement.SetWallJump(true);
                break;
            case 4:
                _playerMovement.SetSuperDash(true);
                break;
            case 5:
                _playerMovement.SetDownDash(true);
                break;
        }
    }
}
