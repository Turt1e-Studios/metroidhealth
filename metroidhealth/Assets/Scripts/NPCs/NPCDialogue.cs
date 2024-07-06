using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NPCDialogue : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private string[] text;
    [SerializeField] private float bufferSpeed;
    [SerializeField] private GameObject visualCue;
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI dialogueText;
    
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
        if (Input.GetKeyDown(KeyCode.Space) && _NPCInRange) // input handling
        {
            _spaceKeyPressed = true;
        }
    }

    void FixedUpdate()
    {
        if (_NPCInRange)
        {
            visualCue.SetActive(true);
            if (_spaceKeyPressed) 
            {
                dialoguePanel.SetActive(true);

                if (_bufferingText) 
                {
                    // space key pressed when buffering -> stop buffering, show the whole line
                    _bufferingText = false;
                    dialogueText.text = text[_dialogueIndex];
                    _dialogueTextIndex = 1;
                    _canBufferNextLetter = true;
                }
                else
                {
                    // go to the next text line
                    _dialogueIndex += 1;
                    if (_dialogueIndex >= text.Length)
                    {
                        // cancel dialogue (done with all text lines)
                        _dialogueIndex = -1;
                        dialoguePanel.SetActive(false);
                        dialogueText.text = "";
                        visualCue.GetComponent<SpriteRenderer>().color = new Color(255,255,255,255);
                    }
                    else
                    {
                        // start buffering line
                        _bufferingText = true;
                    }
                    
                }

                _spaceKeyPressed = false;
            }

            if (_bufferingText && _canBufferNextLetter)
            {
                StartCoroutine(BufferLine());
            }
            
        }
        
        else
        {
            // player left NPC range
            visualCue.SetActive(false);
        }
    }

    private IEnumerator BufferLine()
    {
        _canBufferNextLetter = false;
        dialogueText.text = text[_dialogueIndex].Substring(0, _dialogueTextIndex);
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
