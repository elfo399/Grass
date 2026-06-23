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

public class visitManager : MonoBehaviour
{
    public Text playerName;

    public void goTOVisit() {
        if(playerName.text != AuthenticationService.Instance.PlayerName.Split('#')[0]) {
            GameManager.instance.visitPlayerClick(playerName.text);
        } else {
            GameManager.instance.returnI();
        }
    }
}
