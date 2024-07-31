using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeBlink : MonoBehaviour
{
    [SerializeField] GameObject _eye1;
    [SerializeField] GameObject _eye2;

    float interval = 5f;
    float timer;
    bool blinked = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer > interval)
        {
            blinked = !blinked;
            if (blinked)
            {
                _eye1.transform.localScale = new Vector3(0.2f, 0.03f, 1.0f);
                _eye2.transform.localScale = new Vector3(0.2f, 0.03f, 1.0f);
                interval = Random.Range(0f, 0.5f);
            } else
            {
                _eye1.transform.localScale = new Vector3(0.2f, 0.11f, 1.0f);
                _eye2.transform.localScale = new Vector3(0.2f, 0.11f, 1.0f);
                interval = Random.Range(5f, 10f);
            }

            
            timer = 0;
        }
    }
}
