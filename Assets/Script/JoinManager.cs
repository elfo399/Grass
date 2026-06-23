using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.UI;

public class JoinManager : MonoBehaviour {
    public Text text;
    public async void joinLobby()
    {
        try
        {
            Debug.Log($"enterID: {text.text}");
            JoinLobbyByIdOptions joinLobbyByIdOptions = new JoinLobbyByIdOptions {
                Player = LobbyManager.instance.GetPlayer()
            };
            Lobby result = await LobbyService.Instance.JoinLobbyByIdAsync(text.text, joinLobbyByIdOptions);
            LobbyManager.instance.joinedLobby = result;
            MenuManager.Instance.preTransition("LOBBY");
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }
}
