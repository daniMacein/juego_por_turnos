using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class Dialogo : MonoBehaviour
{
    private bool isPlayerinRange;
    private bool didDialogueStart;
    private bool isLineFullyShown;
    private int lineIndex;
    private float typingTime = 0.05f;
    [SerializeField] private GameObject dialoguePanel; 
    [SerializeField] private TMP_Text dialogueText;
    [SerializeField, TextArea(4,6)] private string[] dialogueLines;
    private InputSystem_Actions inputActions;

    private void Awake()
    {
        inputActions = new InputSystem_Actions();
    }
    private void OnEnable()
    {
        inputActions.Enable();
        inputActions.Player.Interact.performed += OnInteract;
    }
    private void OnDisable()
    {
        inputActions.Player.Interact.performed -= OnInteract;
        inputActions.Disable();
    }
    // Update is called once per frame
    private void OnInteract(InputAction.CallbackContext context)
    {
        if (!context.performed) return; 

        if (!isPlayerinRange) return;

        if (!didDialogueStart)
        {
            StartDialogue();
        }
        else if (isLineFullyShown)
        {
            NextDialogueLine();
        }
        else
        {
            StopAllCoroutines();
            dialogueText.text = dialogueLines[lineIndex];
            isLineFullyShown = true;
        }
    }

    private void StartDialogue()
    {
        didDialogueStart = true;
        lineIndex = 0;
        dialoguePanel.SetActive(true);
        Time.timeScale = 0f; // Detiene el tiempo del juego para que el jugador no se mueva
        //mientras escuche el dialogo 
        StartCoroutine(ShowLine());
    }

    private void NextDialogueLine()
    {
        lineIndex++;
        if(lineIndex < dialogueLines.Length)
        {
            StartCoroutine(ShowLine());
        }
        else
        {
            dialoguePanel.SetActive(false);
            didDialogueStart = false;
            Time.timeScale = 1f; // Reanuda el tiempo del juego
        }
    }

    private IEnumerator ShowLine()
    {
        dialogueText.text = string.Empty;
        isLineFullyShown = false;
        foreach(char ch in dialogueLines[lineIndex])
        {
            dialogueText.text += ch;
            yield return new WaitForSecondsRealtime(typingTime); 
        }
        isLineFullyShown = true;
    }
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerinRange = true;
            Debug.Log("El jugador entró en el área del diálogo.");
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        
        if (collision.CompareTag("Player"))
        {
            isPlayerinRange = false;
            Debug.Log("El jugador salió del área del diálogo.");
        }
    }
}
