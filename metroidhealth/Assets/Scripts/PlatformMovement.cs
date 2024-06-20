using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMovement : MonoBehaviour
{
    // Code for platforms to move from position to position
    
    [SerializeField] private Transform[] positions;
    [SerializeField] private float speed;

    private List<Vector2> _originalPositions;
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
    }
}
