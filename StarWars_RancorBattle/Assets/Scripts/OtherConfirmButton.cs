using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OtherConfirmButton : MonoBehaviour
{
    public PlayerReorderManager selectManager;

    public void YellThatYoureDone()
    {
        selectManager.done = true;
    }
}
