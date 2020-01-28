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
    public Button creditsButton;
    public Button quitButton;
    public Button backToMenuButton;
    public Text creditText;
    public bool debug = false;

    // Start()
    public void Start()
    {
        // Setting the event handlers
        startButton.onClick.AddListener(StartGame);
        creditsButton.onClick.AddListener(ShowCredits);
        quitButton.onClick.AddListener(QuitGame);
        backToMenuButton.onClick.AddListener(ExitCredits);
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

    /// <summary>
    /// ShowCredits() - Shows the credits of the game
    /// </summary>
    public void ShowCredits()
    {
        if (debug) Debug.Log("ShowCredits() called!");
        // Hiding the main menu options
        startButton.gameObject.SetActive(false);
        creditsButton.gameObject.SetActive(false);
        quitButton.gameObject.SetActive(false);

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
    /// ExitCredits() - Exits the credit (and shows the main menus)
    /// </summary>
    public void ExitCredits()
    {
        if (debug) Debug.Log("ExitCredits() called!");
        // Hiding the credits stuff
        backToMenuButton.gameObject.SetActive(false);
        creditText.gameObject.SetActive(false);

        // Showing the main menu items
        startButton.gameObject.SetActive(true);
        creditsButton.gameObject.SetActive(true);
        quitButton.gameObject.SetActive(true);
    }
}
