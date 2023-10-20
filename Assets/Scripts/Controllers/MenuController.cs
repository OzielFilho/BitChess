using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    [SerializeField] private string nameScene;
    [SerializeField] private GameObject options;
    [SerializeField] private GameObject menu;
    private void StartGame()
    {
        SceneManager.LoadScene(nameScene);
    }

    public void Select(int index)
    {
        PlayerPrefs.SetInt("SelectedPrefabBoard", index);
        PlayerPrefs.Save();
        StartGame();
    }
    public void QuitGame()
    {
        Debug.Log("Quit game ");
        Application.Quit();
    }
    public void OpenMenu()
    {
        menu.SetActive(false);
        options.SetActive(true);
    }
    public void QuitMenu()
    {
        Debug.Log("Quit menu ");
        menu.SetActive(true);
        options.SetActive(false);
    }
}
