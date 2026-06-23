using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Unity.Services.Authentication;

public class PlayerCustom : MonoBehaviour
{

    public string playerName;
    public string cash;
    public Market market = Market.Null;
    public int numPlayer;
    public List<CardSO> Hand = new List<CardSO>();
    public bool heatOn = false;
    public Heat type_HEatOn = Heat.Null;
    public List<CardSO> marketPlace_5K = new List<CardSO>();
    public List<CardSO> marketPlace_25K = new List<CardSO>();
    public List<CardSO> marketPlace_50K = new List<CardSO>();
    public List<CardSO> marketPlace_100k = new List<CardSO>();
    public List<CardSO> protection_25K = new List<CardSO>();
    public List<CardSO> protection_50K = new List<CardSO>();
    public List<CardSO> marketSpace = new List<CardSO>();
    public bool myTurn = false;
    public bool pickFromDeck = false;
    public int skipTurn = 0;

    public enum Market {
        Null,
        True,
        False
    }

    public enum Heat {
        Null,
        FELONY,
        DETAINED,
        SS,
        BUST

    }

    public PlayerCustom(int num) {
        this.playerName = AuthenticationService.Instance.PlayerName;
        this.cash = "0";
        this.market = Market.Null;
        this.numPlayer = num;
        this.Hand = new List<CardSO>();
    }

    public PlayerCustom(int num, string playerName) {
        this.playerName = playerName;
        this.cash = "0";
        this.market = Market.Null;
        this.numPlayer = num;
        this.Hand = new List<CardSO>();
    }

    public PlayerCustom(int num, bool turn) {
        this.playerName = AuthenticationService.Instance.PlayerName;
        this.cash = "0";
        this.market = Market.Null;
        this.numPlayer = num;
        this.Hand = new List<CardSO>();
        this.myTurn = true;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
