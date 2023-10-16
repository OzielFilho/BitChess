using System.Collections;
using System.Collections.Generic;
using Enums;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    [Header("Scene dependencies")] 
    [SerializeField] private NetworkManager networkManager;
    
    [SerializeField] private string nameScene;
    
    public void StartGame()
    {
        SceneManager.LoadScene(nameScene);
    }

    public void OnConnect()
    {
        networkManager.SetPlayerLevel(ChessLevel.Normal);
        networkManager.Connect();
    }
}
