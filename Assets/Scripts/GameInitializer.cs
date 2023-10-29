using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class GameInitializer : MonoBehaviour
{
    [SerializeField] private StateMachineController stateMachineController;
    [SerializeField] private Board board;
    [SerializeField] private NetworkManager networkManager;
    [SerializeField] private Transform boardAnchor;
    
    public void CreateBoard()
    {
        if (!networkManager.IsRoomFull())
        {
            PhotonNetwork.Instantiate(board.name, boardAnchor.position, boardAnchor.rotation);
        }
    }

    public void InitializeController()
    {
        board = FindObjectOfType<Board>();
        var controller = Instantiate(stateMachineController);
        controller.SetDependencies(board, networkManager);
        board.SetDependencies(stateMachineController);
    }
}
