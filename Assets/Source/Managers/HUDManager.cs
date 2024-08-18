using System.Collections;
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
    [SerializeField] private GameObject exitWindow;
    [Header("Buttons")]
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button settingsBackButton;
    [SerializeField] private Button exitButton;
    [SerializeField] private Button exitConfirmButton;
    [SerializeField] private Button exitBackButton;
    [Header("Dialogues")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private TextMeshProUGUI speakerText;
    [SerializeField] private TextMeshProUGUI skipText;

    public bool IsPaused { get; private set; }


    private KeyCode currentDialogueSkipKey = KeyCode.None;
    private bool attemptedToSkipDialogue = false;
    private bool canSkipDialogue = false;

    private KeyCode currentDialogueFastForwardKey = KeyCode.None;
    private bool attemptedToFastForwardDialogue = false;
    private bool canFastForwardDialogue = false;

    private void Awake()
    {
        Time.timeScale = 1f;

        mainWindow.SetActive(true);
        pausedWindow.SetActive(false);
        pausedMainWindow.SetActive(true);
        settingsWindow.SetActive(false);
        exitWindow.SetActive(false);

        settingsButton.onClick.AddListener(OnSettingsButtonClick);
        settingsBackButton.onClick.AddListener(OnSettingsBackButtonClick);
        exitButton.onClick.AddListener(OnExitButtonClick);
        exitConfirmButton.onClick.AddListener(OnExitConfirmButtonClick);
        exitBackButton.onClick.AddListener(OnExitBackButtonClick);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
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
    }

    private void Pause()
    {
        IsPaused = true;

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

    private void OnExitButtonClick()
    {
        pausedMainWindow.SetActive(false);
        settingsWindow.SetActive(false);
        exitWindow.SetActive(true);
    }

    private void OnExitConfirmButtonClick()
    {
        Time.timeScale = 1f;

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

        yield return new WaitForSeconds(dialogue.initialDelay);

        Player.Instance.IsActionLocked = true;

        dialoguePanel.SetActive(true);

        canFastForwardDialogue = false;
        canSkipDialogue = false;

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

                yield return new WaitForSeconds(0.1f);

                // sound

                if (attemptedToFastForwardDialogue)
                {
                    dialogueText.text = line.text;

                    attemptedToFastForwardDialogue = false;

                    canFastForwardDialogue = false;

                    break;
                }
            }

            canSkipDialogue = true;

            skipText.text = "Skip:\n" + line.GetSkipKeyString();

            while (!attemptedToSkipDialogue)
                yield return new WaitForSeconds(0.1f);

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
}