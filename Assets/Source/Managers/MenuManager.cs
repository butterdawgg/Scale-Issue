using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class MenuManager : MonoBehaviour
{
    [Header("Windows")]
    [SerializeField] private GameObject mainWindow;
    [SerializeField] private GameObject settingsWindow;
    [SerializeField] private GameObject newGameWindow;
    [SerializeField] private GameObject exitWindow;
    [Header("Buttons")]
    [SerializeField] private Button newGameButton;
    [SerializeField] private Button newGameConfirmButton;
    [SerializeField] private Button newGameBackButton;
    [Space]
    [SerializeField] private Button continueButton;
    [Space]
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button settingsBackButton;
    [Space]
    [SerializeField] private Button exitButton;
    [SerializeField] private Button exitConfirmButton;
    [SerializeField] private Button exitBackButton;
    [Header("Texts")]
    [SerializeField] private TextMeshProUGUI mainMenuText;

    private void Awake()
    {
        mainWindow.SetActive(true);
        settingsWindow.SetActive(false);
        newGameWindow.SetActive(false);
        exitWindow.SetActive(false);

        newGameButton.onClick.AddListener(OnNewGameButtonClick);
        newGameConfirmButton.onClick.AddListener(OnNewGameConfirmButtonClick);
        newGameBackButton.onClick.AddListener(OnNewGameBackButtonClick);

        continueButton.onClick.AddListener(OnContinueButtonClick);

        settingsButton.onClick.AddListener(OnSettingsButtonClick);
        settingsBackButton.onClick.AddListener(OnSettingsBackButtonClick);

        exitButton.onClick.AddListener(OnExitButtonClick);
        exitConfirmButton.onClick.AddListener(OnExitConfirmButtonClick);
        exitBackButton.onClick.AddListener(OnExitBackButtonClick);
    }

    private void Update()
    {
        mainMenuText.rectTransform.localScale = Vector3.one + (new Vector3(Mathf.Sin(Time.time * 1.5f), Mathf.Sin(Time.time * 1.5f), Mathf.Sin(Time.time * 1.5f)) * 0.05f);

        if (SerializeManager.GetCheckpointEventDepth() == 0)
        {
            continueButton.GetComponent<MenuButton>().IsSelectable = false;
            continueButton.GetComponent<Button>().enabled = false;
        }
        else
        {
            continueButton.GetComponent<MenuButton>().IsSelectable = true;
            continueButton.GetComponent<Button>().enabled = true;
        }
    }

    private void OnNewGameButtonClick()
    {
        mainWindow.SetActive(false);
        settingsWindow.SetActive(false);
        newGameWindow.SetActive(true);
        exitWindow.SetActive(false);
    }

    private void OnNewGameConfirmButtonClick()
    {
        SerializeManager.SetCheckpointEventDepth(0);
        SerializeManager.SetCheckpointPlayerPosition(Vector3.up);

        for (int i = 0; i < 100; i++)
        {
            SerializeManager.SetEnemyDefeatedStatus(i, false);
        }

        SceneManager.LoadScene(1);
    }

    private void OnNewGameBackButtonClick()
    {
        mainWindow.SetActive(true);
        settingsWindow.SetActive(false);
        newGameWindow.SetActive(false);
        exitWindow.SetActive(false);
    }

    private void OnContinueButtonClick()
    {
        SceneManager.LoadScene(1);
    }

    private void OnSettingsButtonClick()
    {
        mainWindow.SetActive(false);
        settingsWindow.SetActive(true);
    }

    private void OnSettingsBackButtonClick()
    {
        mainWindow.SetActive(true);
        settingsWindow.SetActive(false);
    }

    private void OnExitButtonClick()
    {
        mainWindow.SetActive(false);
        settingsWindow.SetActive(false);
        newGameWindow.SetActive(false);
        exitWindow.SetActive(true);
    }

    private void OnExitConfirmButtonClick()
    {
        Application.Quit();
    }

    private void OnExitBackButtonClick()
    {
        mainWindow.SetActive(true);
        settingsWindow.SetActive(false);
        newGameWindow.SetActive(false);
        exitWindow.SetActive(false);
    }
}
