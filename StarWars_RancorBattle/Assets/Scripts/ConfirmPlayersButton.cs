using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ConfirmPlayersButton : MonoBehaviour
{
    // Attributes
    public string nextSceneName = "";
    public PlayerSelectManager selectManager;
    public string playerPrefsKey = "";
    public bool debug = false;

    /// <summary>
    /// SaveSelectionToPreferences() - Saves the currently-selected player
    /// draggable object order to the player preferences so that they can
    /// be used in the next scene.
    /// </summary>
    public void SaveSelectionToPreferences()
    {
        // IF there is no selection made in the player select manager, log an error
        if (selectManager.players.Count < 1)
        {
            if (debug) Debug.LogError("Player selection not made!");
        }
        // ELSE... (selection has been made)
        else
        {
            // Saving the selection order to a string
            string toSave = "";
            for (int num = 0; num < selectManager.players.Count; num++)
            {
                toSave += selectManager.players[num];
                if (num < selectManager.players.Count - 1)
                {
                    toSave += ",";
                }
            }

            // IF the player preferences key name is not defined, squack at the dev
            if (playerPrefsKey.Length < 1)
            {
                if (debug) Debug.LogError("'playerPrefsKey' is undefined!");
            }
            // ELSE... (the player preferences key name is defined)
            else
            {
                // Saving the selection order string to the player preferences
                PlayerPrefs.SetString(playerPrefsKey, toSave);
            }
        }
    }

    /// <summary>
    /// ChangeScene() - Loads the scene defined by the 'nextSceneName' string.
    /// </summary>
    public void ChangeScene()
    {
        // IF the next scene name is not defined, yell at the developer
        if (nextSceneName.Length < 1)
        {
            Debug.LogError("'nextSceneName' is undefined!");
        }
        //ELSE... (the next scene name is defined)
        else
        {
            // Loading the next scene
            SceneManager.LoadSceneAsync(nextSceneName);
        }
    }

    /// <summary>
    /// SaveAndChange() - Saves the player order and changes the scene.
    /// </summary>
    public void SaveAndChange()
    {
        SaveSelectionToPreferences();
        ChangeScene();
    }
}
