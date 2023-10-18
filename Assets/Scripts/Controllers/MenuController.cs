using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    [SerializeField] private string nameScene;
    public void StartGame()
    {
        SceneManager.LoadScene(nameScene);
    }
    public void QuitGame()
    {
        Debug.Log("Quit game ");
        Application.Quit();
    }
}
