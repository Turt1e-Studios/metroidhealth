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

    private GameObject _grabbedObject;
    private PlayerMovement _playerMovement;
    private int _layerIndex;
    
    // Start is called before the first frame update
    void Start()
    {
        _layerIndex = LayerMask.NameToLayer("Objects");
        _playerMovement = GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 direction = _playerMovement.IsFacingRight() ? transform.right : transform.right * -1;
        RaycastHit2D hitInfo = Physics2D.Raycast(rayPoint.position, direction, rayDistance);

        if (hitInfo.collider != null && hitInfo.collider.gameObject.layer == _layerIndex)
        {
            print("next to it");
            if (Input.GetKeyDown(KeyCode.Space))
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
            }
        }
        
        Debug.DrawRay(rayPoint.position, transform.right * rayDistance);
    }
}
