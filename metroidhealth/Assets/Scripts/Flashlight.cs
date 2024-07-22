using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flashlight : MonoBehaviour
{

    [SerializeField] private UnityEngine.Rendering.Universal.Light2D _flashlight;

    float interval = 5f;
    float timer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //transform.LookAt(Input.mousePosition);
        Vector3 mousePos = Input.mousePosition;
		//mousePos.z = 0f;

		Vector3 objectPos = Camera.main.WorldToScreenPoint (transform.position);
		mousePos.x = mousePos.x - objectPos.x;
		mousePos.y = mousePos.y - objectPos.y;

		float angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg - 90;
		transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

        // Light flicker
        timer += Time.deltaTime;
        if (timer > interval)
        {
            _flashlight.enabled = !_flashlight.enabled;
            if (_flashlight.enabled) 
            {
                interval = Random.Range(5f, 15f);
            } else 
            {
                interval = Random.Range(0f, 0.2f);
            }
            
            timer = 0;
        }
    }
}
