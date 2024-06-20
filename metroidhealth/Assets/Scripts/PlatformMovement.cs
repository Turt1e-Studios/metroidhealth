using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMovement : MonoBehaviour
{
    // Code for platforms to move from position to position
    
    [SerializeField] private Transform[] positions;
    [SerializeField] private float speed;

    private List<Vector2> _originalPositions;
    private Vector2 _velocity;
    private Vector2 _previous;
    private int _currentPos;
    
    // Start is called before the first frame update
    void Start()
    {
        _originalPositions = new List<Vector2>();
        foreach (Transform position in positions)
        {
            _originalPositions.Add(position.position);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if ((Vector2) transform.position == _originalPositions[_currentPos])
        {
            _currentPos = (_currentPos + 1) % positions.Length;
        }

        transform.position = Vector2.MoveTowards(transform.position, _originalPositions[_currentPos], speed * Time.deltaTime);
        //transform.position = Vector2.Lerp(transform.position, positions[_currentPos % positions.Length], speed * Time.deltaTime);
        
        _velocity = ((Vector2) (transform.position) - _previous) / Time.deltaTime;
        _previous = transform.position;
    }
    
    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            col.transform.SetParent(transform);
            // col.gameObject.transform.parent = gameObject.transform;
            //col.gameObject.GetComponent<PlayerMovement>().OverrideVelocity(true, _velocity);
        }
    }
    
    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.transform.SetParent(null);
            // other.gameObject.transform.parent = null;
            //other.gameObject.GetComponent<PlayerMovement>().OverrideVelocity(false, _velocity);
        }
    }
}
