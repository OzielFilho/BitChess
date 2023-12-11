using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    [SerializeField] private string nameScene;
    [SerializeField] private GameObject options;
    [SerializeField] private GameObject menu;
    [SerializeField] private GameObject loseGame;
    [SerializeField] private GameObject winGame;
    private void StartGame()
    {
        menu.SetActive(true);
        options.SetActive(false);
        SceneManager.LoadScene(nameScene);
    }
    public void BackToMenu()
    {
        SceneManager.LoadScene(nameScene);
    }

    public void Select(int index)
    {
        PlayerPrefs.SetInt("SelectedPrefabBoard", index);
        PlayerPrefs.Save();
        StartGame();
    }

    public void WinGame()
    {
        winGame.SetActive(true);
    }
    public void LoseGame()
    {
        loseGame.SetActive(true);
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

    private string pdfUrl = "https://drive.google.com/file/d/1MqvGAmyEahuCJMRzNRCUIDJ7eBhR_Ppy/view?usp=sharing";

    public void AbrirURL()
    {
        Application.OpenURL(pdfUrl);
    }
}
