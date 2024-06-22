using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorMovement : Activatable
{
    // Movement of the door
    
    [SerializeField] private Transform originalTransform;
    [SerializeField] private Transform newTransform;
    [SerializeField] private float speed;

    private Vector2 _originalPosition;
    private Vector2 _newPosition;
    private Vector2 _currentTarget;
    private bool _moving;
    private bool _queued;

    private void Start()
    {
        _originalPosition = originalTransform.position;
        _newPosition = newTransform.position;
    }

    private void Update()
    {
        if ((Vector2) transform.position == _currentTarget)
        {
            if (_queued)
            {
                SwapTargets();
            }
            else
            {
                _moving = false;
            }
            _queued = false;
        }
        else if (_moving)
        {
            transform.position = Vector2.MoveTowards(transform.position, _currentTarget, speed * Time.deltaTime);
        }
    }

    public override void Activate()
    {
        if (_moving)
        {
            _queued = true;
            return;
        }
        
        _currentTarget = _newPosition;
        _moving = true;
    }

    public override void Deactivate()
    {
        if (_moving)
        {
            _queued = true;
            return;
        }

        _currentTarget = _originalPosition;
        _moving = true;
    }

    private void SwapTargets()
    {
        _currentTarget = (_currentTarget == _originalPosition) ? _newPosition : _originalPosition;
    }
}
