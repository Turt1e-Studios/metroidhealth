using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxMovement : MonoBehaviour
{
    private Collision2D _parentMovingPlatform; // scuffed solution
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("MovingPlatform") && OnTop(col))
        {
            transform.SetParent(col.transform);
            _parentMovingPlatform = col;
        }
        else if (col.gameObject.CompareTag("Box") && OnTop(col))
        {
            _parentMovingPlatform = col.gameObject.GetComponent<BoxMovement>().getParentMovingPlatform();
            if (_parentMovingPlatform != null)
            {
                transform.SetParent(_parentMovingPlatform.transform);
            }
            
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("MovingPlatform"))
        {
            _parentMovingPlatform = null;
        }
        
    }

    private bool OnTop(Collision2D collision)
    {
        BoxCollider2D boxCollider = GetComponent<BoxCollider2D>();
        
        float boxBottom = boxCollider.bounds.min.y;
        float collisionTop = collision.collider.bounds.max.y;
        return boxBottom >= collisionTop - 0.1f;
    }

    public Collision2D getParentMovingPlatform()
    {
        return _parentMovingPlatform;
    }
}
