using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCDialogue : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private string[] text;
    [SerializeField] private float bufferSpeed;
    private int _dialogueIndex = -1;
    private int _dialogueTextIndex = 1;

    private bool _NPCInRange = false;
    private bool _spaceKeyPressed = false;
    private bool _bufferingText = false;
    private bool _canBufferNextLetter = true;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && _NPCInRange)
        {
            _spaceKeyPressed = true;
        }
    }

    void FixedUpdate()
    {
        if (_NPCInRange)
        {
            if (_spaceKeyPressed) 
            {

                if (_bufferingText) 
                {
                    // stop buffering, show the whole line
                    _bufferingText = false;
                    print(text[_dialogueIndex]);
                    _dialogueTextIndex = 1;
                    _canBufferNextLetter = true;
                }
                else
                {
                    // go to the next text line
                    _dialogueIndex += 1;
                    if (_dialogueIndex >= text.Length)
                    {
                        // cancel dialogue
                        _dialogueIndex = -1;
                    }
                    else
                    {
                        // start buffering current line
                        _bufferingText = true;
                    }
                    
                }

                _spaceKeyPressed = false;
            }
            else
            {
                if (_bufferingText && _canBufferNextLetter)
                {
                    StartCoroutine(BufferLine());
                }
            }
        }
        else
        {
            // player left NPC range
            _dialogueIndex = -1;
            _dialogueTextIndex = 1;
            _spaceKeyPressed = false;
            _bufferingText = false;
            _canBufferNextLetter = true;
        }
    }

    private IEnumerator BufferLine()
    {
        _canBufferNextLetter = false;
        print(text[_dialogueIndex].Substring(0, _dialogueTextIndex));
        _dialogueTextIndex += 1;
        if (_dialogueTextIndex > text[_dialogueIndex].Length)
        {
            // finished a line
            _dialogueTextIndex = 1;
            _bufferingText = false;
        }
        yield return new WaitForSeconds(bufferSpeed);
        _canBufferNextLetter = true;
        
    }
    private void OnTriggerEnter2D(Collider2D col) 
    {
        _NPCInRange = true;
    }

    private void OnTriggerExit2D(Collider2D other) 
    {
        _NPCInRange = false;
    }
}
