using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GrabObjects : MonoBehaviour
{
    // Allows player to grab and reposition objects
    
    [SerializeField] private Transform grabPoint;
    [SerializeField] private Transform rayPoint;
    [SerializeField] private float rayDistance;
    [SerializeField] private float throwForce;

    private GameObject _grabbedObject;
    private PlayerMovement _playerMovement;
    private int _layerIndex;
    private bool _spaceKeyPressed = false;
    private bool _JKeyPressed = false;

    // Start is called before the first frame update
    void Start()
    {
        _layerIndex = LayerMask.NameToLayer("Box");
        _playerMovement = GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _spaceKeyPressed = true;
        } else if (Input.GetKeyDown(KeyCode.J))
        {
            _JKeyPressed = true;
        }
    }
    void FixedUpdate()
    {
        Vector2 direction = _playerMovement.IsFacingRight() ? transform.right : transform.right * -1;
        RaycastHit2D hitInfo = Physics2D.Raycast(rayPoint.position, direction, rayDistance);
        //print(hitInfo.collider);
        //print(hitInfo.collider.gameObject.layer);
        if (hitInfo.collider != null && hitInfo.collider.gameObject.layer == _layerIndex)
        {
            if (_spaceKeyPressed) 
            {
                // grab object
                if (_grabbedObject == null)
                {
                    _grabbedObject = hitInfo.collider.gameObject;
                    _grabbedObject.GetComponent<Rigidbody2D>().isKinematic = true;
                    _grabbedObject.transform.position = grabPoint.position;
                    _grabbedObject.transform.SetParent(transform);
                }
                // release object
                else
                {
                    _grabbedObject.GetComponent<Rigidbody2D>().isKinematic = false;
                    _grabbedObject.transform.SetParent(null);
                    _grabbedObject = null;
                }
                _spaceKeyPressed = false;
            } else if (_JKeyPressed)
            {
                if (_grabbedObject != null)
                {
                    // throw with velocity
                    _grabbedObject.GetComponent<Rigidbody2D>().isKinematic = false;
                    _grabbedObject.transform.SetParent(null);
                    _grabbedObject.GetComponent<Rigidbody2D>().AddForce(direction * throwForce, ForceMode2D.Impulse);
                    _grabbedObject = null;
                     
                }
                
                _JKeyPressed = false;
            }

        }

        
        //Debug.DrawRay(rayPoint.position, direction * rayDistance);
    }
}
