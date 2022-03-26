using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitButton : MonoBehaviour
{
    public void QuitToMainMenu() {
        SceneManager.LoadScene("main_menu", LoadSceneMode.Single);
    }
}
