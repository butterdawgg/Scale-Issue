using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
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

    private bool isPaused = false;

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
            if (!isPaused)
                Pause();
            else
                Resume();
        }
    }

    private void Pause()
    {
        isPaused = true;

        Time.timeScale = 0f;

        mainWindow.SetActive(false);
        pausedWindow.SetActive(true);
        pausedMainWindow.SetActive(true);
        settingsWindow.SetActive(false);
        exitWindow.SetActive(false);
    }

    private void Resume()
    {
        isPaused = false;

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
}