using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    [Header("Windows")]
    [SerializeField] private GameObject mainWindow;
    [SerializeField] private GameObject pausedWindow;
    [SerializeField] private GameObject pausedMainWindow;
    [SerializeField] private GameObject settingsWindow;
    [SerializeField] private GameObject victoryWindow;
    [SerializeField] private GameObject defeatWindow;
    [SerializeField] private GameObject exitWindow;
    [Header("Buttons")]
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button settingsBackButton;
    [Space]
    [SerializeField] private Button victoryRestartButton;
    [SerializeField] private Button victoryExitButton;
    [Space]
    [SerializeField] private Button defeatRestartButton;
    [SerializeField] private Button defeatExitButton;
    [Space]
    [SerializeField] private Button exitButton;
    [SerializeField] private Button exitConfirmButton;
    [SerializeField] private Button exitBackButton;
    [Header("Dialogues")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private TextMeshProUGUI speakerText;
    [SerializeField] private TextMeshProUGUI skipText;
    [Header("Notes")]
    [SerializeField] private GameObject noteWindow;
    [SerializeField] private TextMeshProUGUI noteText;
    [Header("Interactions")]
    [SerializeField] private TextMeshProUGUI interactText;

    public bool IsPaused { get; private set; } = false;

    private KeyCode currentDialogueSkipKey = KeyCode.None;
    private bool attemptedToSkipDialogue = false;
    private bool canSkipDialogue = false;

    private KeyCode currentDialogueFastForwardKey = KeyCode.None;
    private bool attemptedToFastForwardDialogue = false;
    private bool canFastForwardDialogue = false;

    private bool isNoteOpened = false;
    private bool attemptedToCloseNote = false;
    private bool canCloseNote = false;

    private List<Interactable> visibleInteractables = new List<Interactable>();

    private void Awake()
    {
        Time.timeScale = 1f;

        Cursor.lockState = CursorLockMode.Locked;

        mainWindow.SetActive(true);
        pausedWindow.SetActive(false);
        pausedMainWindow.SetActive(true);
        settingsWindow.SetActive(false);
        victoryWindow.SetActive(false);
        defeatWindow.SetActive(false);
        exitWindow.SetActive(false);

        dialoguePanel.SetActive(false);
        noteWindow.SetActive(false);
        interactText.gameObject.SetActive(false);

        settingsButton.onClick.AddListener(OnSettingsButtonClick);
        settingsBackButton.onClick.AddListener(OnSettingsBackButtonClick);
        victoryRestartButton.onClick.AddListener(OnVictoryRestartButtonClick);
        victoryExitButton.onClick.AddListener(OnVictoryExitButtonClick);
        defeatRestartButton.onClick.AddListener(OnDefeatRestartButtonClick);
        defeatExitButton.onClick.AddListener(OnDefeatExitButtonClick);
        exitButton.onClick.AddListener(OnExitButtonClick);
        exitConfirmButton.onClick.AddListener(OnExitConfirmButtonClick);
        exitBackButton.onClick.AddListener(OnExitBackButtonClick);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !isNoteOpened)
        {
            if (!IsPaused)
                Pause();
            else
                Resume();
        }

        if (Input.GetKeyDown(currentDialogueSkipKey) && canSkipDialogue)
            attemptedToSkipDialogue = true;

        if (Input.GetKeyDown(currentDialogueFastForwardKey) && canFastForwardDialogue)
            attemptedToFastForwardDialogue = true;

        if (Input.GetKeyDown(KeyCode.Escape) && canCloseNote)
            attemptedToCloseNote = true;

        interactText.gameObject.SetActive(visibleInteractables.Count > 0);

        if (Player.Instance.IsActionLocked && interactText.gameObject.activeSelf)
            interactText.gameObject.SetActive(false);
    }

    private void Pause()
    {
        IsPaused = true;

        Cursor.lockState = CursorLockMode.None;

        Time.timeScale = 0f;

        mainWindow.SetActive(false);
        pausedWindow.SetActive(true);
        pausedMainWindow.SetActive(true);
        settingsWindow.SetActive(false);
        exitWindow.SetActive(false);
    }

    private void Resume()
    {
        IsPaused = false;

        Cursor.lockState = CursorLockMode.Locked;

        Time.timeScale = 1f;

        mainWindow.SetActive(true);
        pausedWindow.SetActive(false);
    }

    private void OnSettingsButtonClick()
    {
        pausedMainWindow.SetActive(false);
        settingsWindow.SetActive(true);
        exitWindow.SetActive(false);
    }

    private void OnSettingsBackButtonClick()
    {
        pausedMainWindow.SetActive(true);
        settingsWindow.SetActive(false);
        exitWindow.SetActive(false);
    }

    private void OnVictoryRestartButtonClick()
    {
        SerializeManager.SetCheckpointEventDepth(0);
        SerializeManager.SetCheckpointPlayerPosition(Vector3.up);

        SceneManager.LoadScene(1);
    }

    private void OnVictoryExitButtonClick()
    {
        SerializeManager.SetCheckpointEventDepth(0);
        SerializeManager.SetCheckpointPlayerPosition(Vector3.up);

        SceneManager.LoadScene(0);
    }

    private void OnDefeatRestartButtonClick()
    {
        SceneManager.LoadScene(1);
    }

    private void OnDefeatExitButtonClick()
    {
        SceneManager.LoadScene(0);
    }

    private void OnExitButtonClick()
    {
        pausedMainWindow.SetActive(false);
        settingsWindow.SetActive(false);
        exitWindow.SetActive(true);
    }

    private void OnExitConfirmButtonClick()
    {
        Time.timeScale = 1f;

        Cursor.lockState = CursorLockMode.None;

        SceneManager.LoadScene(0);
    }

    private void OnExitBackButtonClick()
    {
        pausedMainWindow.SetActive(true);
        settingsWindow.SetActive(false);
        exitWindow.SetActive(false);
    }

    public void PlayDialogue(EventDialogue dialogue)
    {
        StartCoroutine(DialogueCoroutine(dialogue));
    }

    private IEnumerator DialogueCoroutine(EventDialogue dialogue)
    {
        dialogue.isPlayed = false;

        canFastForwardDialogue = false;
        canSkipDialogue = false;

        Player.Instance.IsActionLocked = true;

        dialoguePanel.SetActive(true);

        visibleInteractables.Clear();

        foreach (DialogueLine line in dialogue.lines)
        {
            currentDialogueFastForwardKey = line.fastForwardKey;
            currentDialogueSkipKey = line.skipKey;

            dialogueText.text = "";
            speakerText.text = line.speaker;

            char[] characters = line.text.ToCharArray();

            for (int i = 0; i < characters.Length; i++)
            {
                canFastForwardDialogue = true;

                skipText.text = "Skip:\n" + line.GetFastForwardKeyString();

                dialogueText.text += characters[i];

                yield return new WaitForSeconds(0.05f);

                // sound

                if (attemptedToFastForwardDialogue)
                {
                    dialogueText.text = line.text;

                    attemptedToFastForwardDialogue = false;

                    canFastForwardDialogue = false;

                    break;
                }
            }

            canFastForwardDialogue = false;
            canSkipDialogue = true;

            skipText.text = "Skip:\n" + line.GetSkipKeyString();

            while (!attemptedToSkipDialogue)
                yield return null;

            attemptedToSkipDialogue = false;

            canSkipDialogue = false;

            dialogueText.text = "";
            speakerText.text = "";
            skipText.text = "";
        }

        Player.Instance.IsActionLocked = false;

        dialoguePanel.SetActive(false);

        dialogue.isPlayed = true;
    }

    public void AddInteractable(Interactable interactable)
    {
        if(!visibleInteractables.Contains(interactable))
            visibleInteractables.Add(interactable);
    }

    public void RemoveInteractable(Interactable interactable)
    {
        if (visibleInteractables.Contains(interactable))
            visibleInteractables.Remove(interactable);
    }

    public void DisplayNote(string text)
    {
        StartCoroutine(DisplayNoteCoroutine(text));
    }

    private IEnumerator DisplayNoteCoroutine(string text)
    {
        isNoteOpened = true;
        attemptedToCloseNote = false;
        canCloseNote = true;

        noteWindow.SetActive(true);
        noteText.text = text;

        mainWindow.SetActive(false);

        Player.Instance.IsActionLocked = true;

        Time.timeScale = 0f;

        while (!attemptedToCloseNote)
            yield return new WaitForSecondsRealtime(0.01f);

        isNoteOpened = false;
        attemptedToCloseNote = false;
        canCloseNote = false;

        noteWindow.SetActive(false);
        noteText.text = "";

        mainWindow.SetActive(true);

        Player.Instance.IsActionLocked = false;

        Time.timeScale = 1f;
    }

    public void OnVictory()
    {
        Time.timeScale = 0f;

        Cursor.lockState = CursorLockMode.None;

        mainWindow.SetActive(false);
        pausedWindow.SetActive(false);
        pausedMainWindow.SetActive(false);
        settingsWindow.SetActive(false);
        victoryWindow.SetActive(true);
        defeatWindow.SetActive(false);
        exitWindow.SetActive(false);
    }

    public void OnDefeat()
    {
        Time.timeScale = 0f;

        Cursor.lockState = CursorLockMode.None;

        mainWindow.SetActive(false);
        pausedWindow.SetActive(false);
        pausedMainWindow.SetActive(false);
        settingsWindow.SetActive(false);
        victoryWindow.SetActive(false);
        defeatWindow.SetActive(true);
        exitWindow.SetActive(false);
    }
}