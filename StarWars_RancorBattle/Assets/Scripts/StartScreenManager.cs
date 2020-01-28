using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartScreenManager : MonoBehaviour
{
    // Attributes
    public Button startButton;
    public string nextScene = "";
    public Button controlsButton;
    public Button creditsButton;
    public Button quitButton;
    public Button backToMenuButton;
    public Text controlsText;
    public Text creditText;
    public bool debug = false;

    // Start()
    public void Start()
    {
        // Setting the event handlers
        startButton.onClick.AddListener(StartGame);
        controlsButton.onClick.AddListener(ShowControls);
        creditsButton.onClick.AddListener(ShowCredits);
        quitButton.onClick.AddListener(QuitGame);
        backToMenuButton.onClick.AddListener(ExitSubmenu);
    }

    /// <summary>
    /// SetMainMenuVisibility() - Shows or hides the main menu options (based on "enabled")
    /// </summary>
    /// <param name="enabled"></param>
    public void SetMainMenuVisibility(bool enabled)
    {
        startButton.gameObject.SetActive(enabled);
        controlsButton.gameObject.SetActive(enabled);
        creditsButton.gameObject.SetActive(enabled);
        quitButton.gameObject.SetActive(enabled);
    }

    /// <summary>
    /// StartGame() - Starts the game (by changing the scene)
    /// </summary>
    public void StartGame()
    {
        // IF the next scene string is undefined, make a fuss
        if (nextScene.Length < 1)
        {
            Debug.LogError("'nextScene' is not defined!");
        }
        // ELSE... (next scene string is defined)
        else
        {
            if (debug) Debug.Log("StartGame() called!");
            // Loading the next scene
            SceneManager.LoadSceneAsync(nextScene);
        }
    }

    public void ShowControls()
    {
        if (debug) Debug.Log("ShowControls() called!");
        SetMainMenuVisibility(false);

        // Showing the controls info
        backToMenuButton.gameObject.SetActive(true);
        controlsText.gameObject.SetActive(true);
    }

    /// <summary>
    /// ShowCredits() - Shows the credits of the game
    /// </summary>
    public void ShowCredits()
    {
        if (debug) Debug.Log("ShowCredits() called!");
        // Hiding the main menu options
        SetMainMenuVisibility(false);

        // Showing the credits info
        backToMenuButton.gameObject.SetActive(true);
        creditText.gameObject.SetActive(true);
    }

    /// <summary>
    /// QuitGame() - Quits the game (for real)
    /// </summary>
    public void QuitGame()
    {
        if (debug) Debug.Log("QuitGame() called!");
        Application.Quit();
    }

    /// <summary>
    /// ExitSubmenu() - Exits the submenu (and shows the main menus)
    /// </summary>
    public void ExitSubmenu()
    {
        if (debug) Debug.Log("ExitSubmenu() called!");
        // Hiding the credits stuff
        backToMenuButton.gameObject.SetActive(false);
        creditText.gameObject.SetActive(false);
        controlsText.gameObject.SetActive(false);

        // Showing the main menu items
        SetMainMenuVisibility(true);
    }
}
