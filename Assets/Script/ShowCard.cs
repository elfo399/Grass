using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;
using Object = UnityEngine.Object;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Services.Authentication;

public class ShowCard : MonoBehaviour
{
    public CardSO cardData;
    public GameObject UIimage;
    public bool isSummoned = false;

    public bool maledizione = false;
    public bool steal = false;
    public String playerOwner = "";
    public int playCard = 0;

    // Start is called before the first frame update
    void Start()
    {
        UIimage.GetComponent<Image>().sprite = cardData.image;
    }

    // Update is called once per frame
    void Update()
    {   
        if(isSummoned) {
            this.GetComponent<Drag>().enabled = false;
        }
    }

    public void clickCard() {
        if(maledizione) {
            Debug.Log("Send card");
            GameManager.instance.malWaitEnemy();
            GameManager.instance.maledizionePassCardServerRpc(this.cardData.id, AuthenticationService.Instance.PlayerName);
        } else if(steal) {
            Debug.Log("Send card");
            Debug.Log("playerOwner: " + this.playerOwner);
            GameManager.instance.stealPassCardServerRpc(this.cardData.id, this.playerOwner, this.playCard);
        }
    }
}
