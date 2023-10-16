using System;
using Enums;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    private const int MaxPlayers = 2;
    private const string Level = "Level";
    private ChessLevel _playerLevel = ChessLevel.Beginner;
    
    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    public void Connect()
    {
        if (PhotonNetwork.IsConnected)
        {
            Debug.LogError($"Connected to server. Looking for random room with level {_playerLevel}...");
            PhotonNetwork.JoinRandomRoom(new ExitGames.Client.Photon.Hashtable { { Level, _playerLevel } }, MaxPlayers);
        }
        else
            PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        Debug.LogError($"Connected to server. Looking for room with level {_playerLevel}...");
        PhotonNetwork.JoinRandomRoom();
    }
    
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.LogError($"Joining random room failed because of {message}. Creating new room with player level {_playerLevel}...");
        PhotonNetwork.CreateRoom(null, new RoomOptions
        {
            CustomRoomPropertiesForLobby = new [] { Level },
            MaxPlayers = MaxPlayers,
            CustomRoomProperties = new Hashtable{{Level, _playerLevel}}
        });
    }
    
    public override void OnJoinedRoom()
    {
        Debug.LogError($"Player {PhotonNetwork.LocalPlayer.ActorNumber} joined room {PhotonNetwork.CurrentRoom.Name} with level {(ChessLevel) PhotonNetwork.CurrentRoom.CustomProperties[Level]}");
    }
    
    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        Debug.LogError($"Player {newPlayer.ActorNumber} joined room {PhotonNetwork.CurrentRoom.Name}");
    }

    public void SetPlayerLevel(ChessLevel level)
    {
        _playerLevel = level;
        PhotonNetwork.LocalPlayer.SetCustomProperties(new Hashtable{{Level, _playerLevel}});
    }
}
