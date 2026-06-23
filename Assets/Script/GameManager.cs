using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using Unity.Services.Vivox;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking;
using Unity.Networking.Transport;
using Unity.Networking.Transport.Relay;
using VivoxUnity;
using UnityEngine;
using UnityEngine.UI;
using Unity.Collections;
using JetBrains.Annotations;
using System.ComponentModel;
using UnityEditor;
using System.Linq;

public class GameManager : NetworkBehaviour {

    public static GameManager instance;

    public static int numPlayers = 4;

    public NetworkList<int> remoteDeck;
    public NetworkList<FixedString64Bytes> remotePlayerName;
    public NetworkList<FixedString64Bytes> remoteCash;
    public NetworkList<FixedString64Bytes> remoteMarket;
    public NetworkList<int> remoteNumPlayer;
    public NetworkList<int> remoteHandP1;
    public NetworkList<int> remoteHandP2;
    public NetworkList<int> remoteHandP3;
    public NetworkList<int> remoteHandP4;
    public NetworkList<int> remoteMarketP1;
    public NetworkList<int> remoteMarketP2;
    public NetworkList<int> remoteMarketP3;
    public NetworkList<int> remoteMarketP4;
    public NetworkList<int> remoteHassle;
    public NetworkList<bool> remoteHeatOn;
    public NetworkList<FixedString64Bytes> remoteTypeHeatOn;
    public NetworkList<int> remoteMarketPlace5kP1;
    public NetworkList<int> remoteMarketPlace5kP2;
    public NetworkList<int> remoteMarketPlace5kP3;
    public NetworkList<int> remoteMarketPlace5kP4;
    public NetworkList<int> remoteMarketPlace25kP1;
    public NetworkList<int> remoteMarketPlace25kP2;
    public NetworkList<int> remoteMarketPlace25kP3;
    public NetworkList<int> remoteMarketPlace25kP4;
    public NetworkList<int> remoteMarketPlace50kP1;
    public NetworkList<int> remoteMarketPlace50kP2;
    public NetworkList<int> remoteMarketPlace50kP3;
    public NetworkList<int> remoteMarketPlace50kP4;
    public NetworkList<int> remoteMarketPlace100kP1;
    public NetworkList<int> remoteMarketPlace100kP2;
    public NetworkList<int> remoteMarketPlace100kP3;
    public NetworkList<int> remoteMarketPlace100kP4;
    public NetworkList<int> remoteProtection25kP1;
    public NetworkList<int> remoteProtection25kP2;
    public NetworkList<int> remoteProtection25kP3;
    public NetworkList<int> remoteProtection25kP4;
    public NetworkList<int> remoteProtection50kP1;
    public NetworkList<int> remoteProtection50kP2;
    public NetworkList<int> remoteProtection50kP3;
    public NetworkList<int> remoteProtection50kP4;
    public NetworkList<bool> remoteMyTurn;
    public NetworkList<bool> remotePickFromDeck;
    public NetworkList<int> remoteSkipTurn;


    //Realy var
    private Guid hostAllocationId;
    private string allocationRegion;
    public string joinCode;
    private Guid playerAllocationId;
    
    //PreGame banner
    public int countdownTime;
    public Text countdown;
    public Text lobbyName;

    //Card
    public List<CardSO> cardTypes;

    public List<CardSO> deck;
    public List<CardSO> allCard;
    public List<CardSO> marketSpace;
    public List<CardSO> hassle;

    public List<PlayerCustom> players;

    public GameObject Hand;
    public int order = 1;

    public GameObject cardTemplate;
    public GameObject deckSpot;

    public GameObject turnBanner;
    public int sleepTurnBannerTime;

    public GameObject playerTemplate;

    public GameObject player1Spot;
    public GameObject player2Spot;
    public GameObject player3Spot;
    public GameObject player4Spot;
    public GameObject marketPlace_5kSpot;
    public GameObject marketPlace_25kSpot;
    public GameObject marketPlace_50kSpot;
    public GameObject marketPlace_100kSpot;
    public GameObject protection_25kSpot;
    public GameObject protection_50kSpot;
    public GameObject marketSpot;
    public GameObject heatSpot;
    public GameObject binPrefab;
    public Sprite greenMarket;
    public Sprite YellowMarket;
    public Sprite RedMarket;
    public Sprite heatOnSSSprite;
    public Sprite heatOnBustSprite;
    public Sprite heatOnDetainedSprite;
    public Sprite heatOnFelonySprite;
    public Sprite InnocentSprite;
    public Sprite stonehighSprite;
    public Sprite euphoriaSprite;
    public Sprite payFineSprite;
    public Sprite stealSprite;
    public GameObject bin;
    public GameObject visitPlayer;

    public GameObject endGameNick1;
    public GameObject endGameNick2;
    public GameObject endGameNick3;
    public GameObject endGameNick4;

    public GameObject endGameProfittoProtetto1;
    public GameObject endGameProfittoProtetto2;
    public GameObject endGameProfittoProtetto3;
    public GameObject endGameProfittoProtetto4;

    public GameObject endGameProfittoRischio1;
    public GameObject endGameProfittoRischio2;
    public GameObject endGameProfittoRischio3;
    public GameObject endGameProfittoRischio4;

    public GameObject endGameBanker1;
    public GameObject endGameBanker2;
    public GameObject endGameBanker3;
    public GameObject endGameBanker4;

    public GameObject endGameMulte1;
    public GameObject endGameMulte2;
    public GameObject endGameMulte3;
    public GameObject endGameMulte4;

    public GameObject endGameCartaAlta1;
    public GameObject endGameCartaAlta2;
    public GameObject endGameCartaAlta3;
    public GameObject endGameCartaAlta4;

    public GameObject endGameNetto1;
    public GameObject endGameNetto2;
    public GameObject endGameNetto3;
    public GameObject endGameNetto4;

    public GameObject endGameBonus1;
    public GameObject endGameBonus2;
    public GameObject endGameBonus3;
    public GameObject endGameBonus4;

    public GameObject endGameTotale1;
    public GameObject endGameTotale2;
    public GameObject endGameTotale3;
    public GameObject endGameTotale4;
    public GameObject EndGame;
    private int count = 0;

    public GameObject endGameMedal1;
    public GameObject endGameMedal2;
    public GameObject endGameMedal3;
    public GameObject endGameMedal4;

    public Sprite medalGold;
    public Sprite medalSilver;
    public Sprite medalBronze;

    public GameObject maledizioniBanner;
    public GameObject cardsMaledizioniSlot;
    public GameObject malWaitText;
    public GameObject pickCardBanner;
    public GameObject backStealCard;
    public GameObject textMaledizioniAndSteal;

    public int malCount;
    private bool endDeck = false;
    
    // Start is called before the first frame update
    async void Awake()
    {
        instance = this;
        try
        {
            await UnityServices.InitializeAsync();
            PlayerManager.instance.Initialized();
            NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnectedCallback;

            deck = new List<CardSO>();

            marketSpace = new List<CardSO>();
            hassle = new List<CardSO>();

            remoteDeck = new NetworkList<int>();

            remotePlayerName = new NetworkList<FixedString64Bytes>();
            remoteCash = new NetworkList<FixedString64Bytes>();
            remoteMarket = new NetworkList<FixedString64Bytes>();
            remoteNumPlayer = new NetworkList<int>();

            remoteHandP1 = new NetworkList<int>();
            remoteHandP2 = new NetworkList<int>();
            remoteHandP3 = new NetworkList<int>();
            remoteHandP4 = new NetworkList<int>();

            remoteMarketP1 = new NetworkList<int>();
            remoteMarketP2 = new NetworkList<int>(); 
            remoteMarketP3 = new NetworkList<int>();
            remoteMarketP4 = new NetworkList<int>();

            remoteHassle = new NetworkList<int>();

            remoteHeatOn = new NetworkList<bool>();
            remoteTypeHeatOn = new NetworkList<FixedString64Bytes>();

            remoteMarketPlace5kP1 = new NetworkList<int>();
            remoteMarketPlace5kP2 = new NetworkList<int>();
            remoteMarketPlace5kP3 = new NetworkList<int>();
            remoteMarketPlace5kP4 = new NetworkList<int>();

            remoteMarketPlace25kP1 = new NetworkList<int>();
            remoteMarketPlace25kP2 = new NetworkList<int>();
            remoteMarketPlace25kP3 = new NetworkList<int>();
            remoteMarketPlace25kP4 = new NetworkList<int>();

            remoteMarketPlace50kP1 = new NetworkList<int>();
            remoteMarketPlace50kP2 = new NetworkList<int>();
            remoteMarketPlace50kP3 = new NetworkList<int>();
            remoteMarketPlace50kP4 = new NetworkList<int>();

            remoteMarketPlace100kP1 = new NetworkList<int>();
            remoteMarketPlace100kP2 = new NetworkList<int>();
            remoteMarketPlace100kP3 = new NetworkList<int>();
            remoteMarketPlace100kP4 = new NetworkList<int>();

            remoteProtection25kP1 = new NetworkList<int>();
            remoteProtection25kP2 = new NetworkList<int>();
            remoteProtection25kP3 = new NetworkList<int>();
            remoteProtection25kP4 = new NetworkList<int>();

            remoteProtection50kP1 = new NetworkList<int>();
            remoteProtection50kP2 = new NetworkList<int>();
            remoteProtection50kP3 = new NetworkList<int>();
            remoteProtection50kP4 = new NetworkList<int>();

            remoteMyTurn = new NetworkList<bool>();
            remotePickFromDeck = new NetworkList<bool>();
            remoteSkipTurn = new NetworkList<int>();

            remoteDeck.OnListChanged += remoteDeckOnOnListChanged;

            remotePlayerName.OnListChanged += remotePlayerNameOnOnListChanged;
            remoteCash.OnListChanged += remoteCashOnOnListChanged;
            remoteNumPlayer.OnListChanged += remoteNumPlayerOnOnListChanged;
            remoteMarket.OnListChanged += remoteMarketOnOnListChanged;

            remoteHandP1.OnListChanged += remoteHandP1OnOnListChanged;
            remoteHandP2.OnListChanged += remoteHandP2OnOnListChanged;
            remoteHandP3.OnListChanged += remoteHandP3OnOnListChanged;
            remoteHandP4.OnListChanged += remoteHandP4OnOnListChanged;

            remoteMarketP1.OnListChanged += remoteMarketP1OnListChanged;
            remoteMarketP2.OnListChanged += remoteMarketP2OnListChanged;
            remoteMarketP3.OnListChanged += remoteMarketP3OnListChanged;
            remoteMarketP4.OnListChanged += remoteMarketP4OnListChanged;

            remoteHassle.OnListChanged += remoteHassleOnListChanged;

            remoteHeatOn.OnListChanged += remoteHeatOnOnOnListChanged;
            remoteTypeHeatOn.OnListChanged += remoteTypeHeatOnOnOnListChanged;

            remoteMarketPlace5kP1.OnListChanged += remoteMarketPlace5kP1OnOnListChanged;
            remoteMarketPlace5kP2.OnListChanged += remoteMarketPlace5kP2OnOnListChanged;
            remoteMarketPlace5kP3.OnListChanged += remoteMarketPlace5kP3OnOnListChanged;
            remoteMarketPlace5kP4.OnListChanged += remoteMarketPlace5kP4OnOnListChanged;

            remoteMarketPlace25kP1.OnListChanged += remoteMarketPlace25kP1OnOnListChanged;
            remoteMarketPlace25kP2.OnListChanged += remoteMarketPlace25kP2OnOnListChanged;
            remoteMarketPlace25kP3.OnListChanged += remoteMarketPlace25kP3OnOnListChanged;
            remoteMarketPlace25kP4.OnListChanged += remoteMarketPlace25kP4OnOnListChanged;

            remoteMarketPlace50kP1.OnListChanged += remoteMarketPlace50kP1OnOnListChanged;
            remoteMarketPlace50kP2.OnListChanged += remoteMarketPlace50kP2OnOnListChanged;
            remoteMarketPlace50kP3.OnListChanged += remoteMarketPlace50kP3OnOnListChanged;
            remoteMarketPlace50kP4.OnListChanged += remoteMarketPlace50kP4OnOnListChanged;

            remoteMarketPlace100kP1.OnListChanged += remoteMarketPlace100kP1OnOnListChanged;
            remoteMarketPlace100kP2.OnListChanged += remoteMarketPlace100kP2OnOnListChanged;
            remoteMarketPlace100kP3.OnListChanged += remoteMarketPlace100kP3OnOnListChanged;
            remoteMarketPlace100kP4.OnListChanged += remoteMarketPlace100kP4OnOnListChanged;

            remoteProtection25kP1.OnListChanged += remoteProtection25kP1OnOnListChanged;
            remoteProtection25kP2.OnListChanged += remoteProtection25kP2OnOnListChanged;
            remoteProtection25kP3.OnListChanged += remoteProtection25kP3OnOnListChanged;
            remoteProtection25kP4.OnListChanged += remoteProtection25kP4OnOnListChanged;

            remoteProtection50kP1.OnListChanged += remoteProtection50kP1OnOnListChanged;
            remoteProtection50kP2.OnListChanged += remoteProtection50kP2OnOnListChanged;
            remoteProtection50kP3.OnListChanged += remoteProtection50kP3OnOnListChanged;
            remoteProtection50kP4.OnListChanged += remoteProtection50kP4OnOnListChanged;
            
            remoteMyTurn.OnListChanged += remoteMyTurnOnOnListChanged;
            remotePickFromDeck.OnListChanged += remotePickFromDeckOnOnListChanged;
            remoteSkipTurn.OnListChanged += remoteSkipTurnOnOnListChanged;

            //VivoxService.Instance.Initialize();
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
    }

    public void remoteDeckOnOnListChanged(NetworkListEvent<int> changeevent) {
        Debug.Log("Start deck change event");
        List<CardSO> deckApp = new List<CardSO>();

        foreach(int cardId in remoteDeck) {
            deckApp.Add(allCard[cardId]);
        }

        deck = deckApp;

        Debug.Log("end deck change event");
        
    }

    

    public void remotePlayerNameOnOnListChanged(NetworkListEvent<FixedString64Bytes> changeevent) {
        Debug.Log("end player change event");
        if(remotePlayerName.Count == 0) {
            return;
        }
        
        int i = 0;
        foreach(FixedString64Bytes playerName in remotePlayerName) {
            players[i].playerName = playerName.ToString();
            i++;
        }

        Debug.Log("Player Name");
        foreach(PlayerCustom player in players) {
            Debug.Log("["+player.playerName+"]");
        }
        Debug.Log("end player change event");
    }

    public void remoteCashOnOnListChanged(NetworkListEvent<FixedString64Bytes> changeevent) {
        Debug.Log("start cash change event");
        if(remoteCash.Count == 0) {
            return;
        }

        int i = 0;
        foreach(FixedString64Bytes cash in remoteCash) {
            players[i].cash = cash.ToString();
            i++;
        }

        Debug.Log("Player Cash");
        foreach(PlayerCustom player in players) {
            Debug.Log("["+player.cash+"]");
        }
        Debug.Log("end cash change event");
    }

    public void remoteNumPlayerOnOnListChanged(NetworkListEvent<int> changeevent) {
        Debug.Log("start NumPlayer change event");
        if(remoteNumPlayer.Count == 0) {
            return;
        }

        int i = 0;
        foreach(int numPlayer in remoteNumPlayer) {
            players[i].numPlayer = numPlayer;
            i++;
        }

        Debug.Log("Player Num player");
        foreach(PlayerCustom player in players) {
            Debug.Log("["+player.numPlayer+"]");
        }
        Debug.Log("end NumPlayer change event");
    }

    public void remoteMarketOnOnListChanged(NetworkListEvent<FixedString64Bytes> changeevent) {
        Debug.Log("start Market change event");
        if(remoteMarket.Count == 0) {
            return;
        }

        int i = 0;
        foreach(FixedString64Bytes market in remoteMarket) {
            players[i].market = StringtoMarketEnum(market.ToString());
            i++;
        }

        Debug.Log("Player market");
        foreach(PlayerCustom player in players) {
            Debug.Log("["+player.market+"]");
        }
        Debug.Log("end Market change event");
    }

    public void remoteHandP1OnOnListChanged(NetworkListEvent<int> changeevent) {
        Debug.Log("start Hand P1 change event");
        if(remoteHandP1.Count == 0) {
            return;
        }

        List<CardSO> HandApp = new List<CardSO>();

        foreach(int cardId in remoteHandP1) {
            HandApp.Add(allCard[cardId]);
        }

        players[0].Hand = HandApp;

        Debug.Log("Hand P1");
        foreach(CardSO card in players[0].Hand) {
            Debug.Log("["+card.id+"]");
        }
        Debug.Log("end Hand P1 change event");
    }

    public void remoteHandP2OnOnListChanged(NetworkListEvent<int> changeevent) {
        Debug.Log("start Hand P2 change event");
        if(remoteHandP2.Count == 0) {
            return;
        }

        List<CardSO> HandApp = new List<CardSO>();

        foreach(int cardId in remoteHandP2) {
            HandApp.Add(allCard[cardId]);
        }

        players[1].Hand = HandApp;
        
        Debug.Log("Hand P2");
        foreach(CardSO card in players[1].Hand) {
            Debug.Log("["+card.id+"]");
        }
        Debug.Log("end Hand P2 change event");
    }

    public void remoteHandP3OnOnListChanged(NetworkListEvent<int> changeevent) {
        Debug.Log("start Hand P3 change event");
        if(remoteHandP3.Count == 0) {
            return;
        }

        List<CardSO> HandApp = new List<CardSO>();

        foreach(int cardId in remoteHandP3) {
            HandApp.Add(allCard[cardId]);
        }

        players[2].Hand = HandApp;

        Debug.Log("Hand P3");
        foreach(CardSO card in players[2].Hand) {
            Debug.Log("["+card.id+"]");
        }
        Debug.Log("end Hand P3 change event");
    }

    public void remoteHandP4OnOnListChanged(NetworkListEvent<int> changeevent) {
        Debug.Log("start Hand P4 change event");
        if(remoteHandP4.Count == 0) {
            return;
        }

        List<CardSO> HandApp = new List<CardSO>();

        foreach(int cardId in remoteHandP4) {
            HandApp.Add(allCard[cardId]);
        }

        players[3].Hand = HandApp;

        Debug.Log("Hand P4");
        foreach(CardSO card in players[3].Hand) {
            Debug.Log("["+card.id+"]");
        }
        Debug.Log("end Hand P4 change event");
    }

    public void remoteMarketP1OnListChanged(NetworkListEvent<int> changeevent) {
        Debug.Log("start Market P1 change event");
        if(remoteMarketP1.Count == 0) {
            return;
        }

        List<CardSO> marketSpaceApp = new List<CardSO>();

        foreach(int cardId in remoteMarketP1) {
            marketSpaceApp.Add(allCard[cardId]);
        }

        players[0].marketSpace = marketSpaceApp;

        Debug.Log("MarektSpace P1");
        foreach(CardSO card in players[0].marketSpace) {
            Debug.Log("["+card.id+"]");
        }
        Debug.Log("start Market P1 change event");
    }

    public void remoteMarketP2OnListChanged(NetworkListEvent<int> changeevent) {
        Debug.Log("start Market P2 change event");
        if(remoteMarketP2.Count == 0) {
            return;
        }

        List<CardSO> marketSpaceApp = new List<CardSO>();

        foreach(int cardId in remoteMarketP2) {
            marketSpaceApp.Add(allCard[cardId]);
        }

        players[1].marketSpace = marketSpaceApp;

        Debug.Log("MarektSpace P2");
        foreach(CardSO card in players[1].marketSpace) {
            Debug.Log("["+card.id+"]");
        }
        Debug.Log("end Market P2 change event");
    }

    public void remoteMarketP3OnListChanged(NetworkListEvent<int> changeevent) {
        Debug.Log("start Market P3 change event");
        if(remoteMarketP3.Count == 0) {
            return;
        }

        List<CardSO> marketSpaceApp = new List<CardSO>();

        foreach(int cardId in remoteMarketP3) {
            marketSpaceApp.Add(allCard[cardId]);
        }

        players[2].marketSpace = marketSpaceApp;

        Debug.Log("MarektSpace P3");
        foreach(CardSO card in players[2].marketSpace) {
            Debug.Log("["+card.id+"]");
        }
        Debug.Log("end Market P3 change event");
    }

    public void remoteMarketP4OnListChanged(NetworkListEvent<int> changeevent) {
        Debug.Log("start Market P4 change event");
        if(remoteMarketP4.Count == 0) {
            return;
        }

        List<CardSO> marketSpaceApp = new List<CardSO>();

        foreach(int cardId in remoteMarketP4) {
            marketSpaceApp.Add(allCard[cardId]);
        }
        
        players[3].marketSpace = marketSpaceApp;

        Debug.Log("MarektSpace P4");
        foreach(CardSO card in players[3].marketSpace) {
            Debug.Log("["+card.id+"]");
        }
        Debug.Log("end Market P4 change event");
    }

    public void remoteHassleOnListChanged(NetworkListEvent<int> changeevent) {
        Debug.Log("start Hassle change event");
        if(remoteHassle.Count == 0) {
            return;
        }

        List<CardSO> hassleApp = new List<CardSO>();

        foreach(int cardId in remoteHassle) {
            hassleApp.Add(allCard[cardId]);
        }

        hassle = hassleApp;

        Debug.Log("Hassle");
        foreach(CardSO card in hassle) {
            Debug.Log("["+card.id+"]");
        }
        Debug.Log("end Hassle change event");
    }

    public void remoteHeatOnOnOnListChanged(NetworkListEvent<bool> changeevent) {
        Debug.Log("start Heat change event");
        if(remoteHeatOn.Count == 0) {
            return;
        }

        int i = 0;
        foreach(bool HeatOn in remoteHeatOn) {
            players[i].heatOn = HeatOn;
            i++;
        }

        Debug.Log("Player heat on");
        foreach(PlayerCustom player in players) {
            Debug.Log("["+player.heatOn+"]");
        }
        Debug.Log("end Heat change event");
    }

    public void remoteTypeHeatOnOnOnListChanged(NetworkListEvent<FixedString64Bytes> changeevent) {
        Debug.Log("start TypeHeatOn change event");
        if(remoteTypeHeatOn.Count == 0) {
            return;
        }

        int i = 0;
        foreach(FixedString64Bytes TypeHeatOn in remoteTypeHeatOn) {
            players[i].type_HEatOn = StringtoHeatOnEnum(TypeHeatOn.ToString());
            i++;
        }

        Debug.Log("Player type heat on");
        foreach(PlayerCustom player in players) {
            Debug.Log("["+player.type_HEatOn+"]");
        }
        Debug.Log("end TypeHeatOn change event");
    }

    public void remoteMarketPlace5kP1OnOnListChanged(NetworkListEvent<int> changeevent) {
        Debug.Log("start MarketPlace5K P1 change event");
        if(remoteMarketPlace5kP1.Count == 0) {
            return;
        }

        List<CardSO> MarketPlace5kApp = new List<CardSO>();

        foreach(int cardId in remoteMarketPlace5kP1) {
            MarketPlace5kApp.Add(allCard[cardId]);
        }

        players[0].marketPlace_5K = MarketPlace5kApp;

        Debug.Log("5K P1");
        foreach(CardSO card in players[0].marketPlace_5K) {
            Debug.Log("["+card.id+"]");
        }
        Debug.Log("end MarketPlace5K P1 change event");
    }

    public void remoteMarketPlace5kP2OnOnListChanged(NetworkListEvent<int> changeevent) {
        Debug.Log("start MarketPlace5K P2 change event");
        if(remoteMarketPlace5kP2.Count == 0) {
            return;
        }

        List<CardSO> MarketPlace5kApp = new List<CardSO>();

        foreach(int cardId in remoteMarketPlace5kP2) {
            MarketPlace5kApp.Add(allCard[cardId]);
        }

        players[1].marketPlace_5K = MarketPlace5kApp;

        Debug.Log("5K P2");
        foreach(CardSO card in players[1].marketPlace_5K) {
            Debug.Log("["+card.id+"]");
        }
        Debug.Log("end MarketPlace5K P2 change event");
    }

    public void remoteMarketPlace5kP3OnOnListChanged(NetworkListEvent<int> changeevent) {
        Debug.Log("start MarketPlace5K P3 change event");
        if(remoteMarketPlace5kP3.Count == 0) {
            return;
        }

        List<CardSO> MarketPlace5kApp = new List<CardSO>();

        foreach(int cardId in remoteMarketPlace5kP3) {
            MarketPlace5kApp.Add(allCard[cardId]);
        }

        players[2].marketPlace_5K = MarketPlace5kApp;

        Debug.Log("5K P3");
        foreach(CardSO card in players[2].marketPlace_5K) {
            Debug.Log("["+card.id+"]");
        }
        Debug.Log("end MarketPlace5K P3 change event");
    }

    public void remoteMarketPlace5kP4OnOnListChanged(NetworkListEvent<int> changeevent) {
        Debug.Log("start MarketPlace5K P4 change event");
        if(remoteMarketPlace5kP4.Count == 0) {
            return;
        }


        List<CardSO> MarketPlace5kApp = new List<CardSO>();

        foreach(int cardId in remoteMarketPlace5kP4) {
            MarketPlace5kApp.Add(allCard[cardId]);
        }

        players[3].marketPlace_5K = MarketPlace5kApp;

        Debug.Log("5K P4");
        foreach(CardSO card in players[3].marketPlace_5K) {
            Debug.Log("["+card.id+"]");
        }
        Debug.Log("end MarketPlace5K P4 change event");
    }

    public void remoteMarketPlace25kP1OnOnListChanged(NetworkListEvent<int> changeevent) {
        Debug.Log("start MarketPlace25K P1 change event");
        if(remoteMarketPlace25kP1.Count == 0) {
            return;
        }

        List<CardSO> MarketPlace25kApp = new List<CardSO>();

        foreach(int cardId in remoteMarketPlace25kP1) {
            MarketPlace25kApp.Add(allCard[cardId]);
        }

        players[0].marketPlace_25K = MarketPlace25kApp;

        Debug.Log("25K P1");
        foreach(CardSO card in players[0].marketPlace_25K) {
            Debug.Log("["+card.id+"]");
        }
        Debug.Log("end MarketPlace25K P1 change event");
    }

    public void remoteMarketPlace25kP2OnOnListChanged(NetworkListEvent<int> changeevent) {
        Debug.Log("start MarketPlace25K P2 change event");
        if(remoteMarketPlace25kP2.Count == 0) {
            return;
        }

        List<CardSO> MarketPlace25kApp = new List<CardSO>();

        foreach(int cardId in remoteMarketPlace25kP2) {
            MarketPlace25kApp.Add(allCard[cardId]);
        }

        players[1].marketPlace_25K = MarketPlace25kApp;

        Debug.Log("25K P2");
        foreach(CardSO card in players[1].marketPlace_25K) {
            Debug.Log("["+card.id+"]");
        }
        Debug.Log("end MarketPlace25K P2 change event");
    }

    public void remoteMarketPlace25kP3OnOnListChanged(NetworkListEvent<int> changeevent) {
        Debug.Log("start MarketPlace25K P3 change event");
        if(remoteMarketPlace25kP3.Count == 0) {
            return;
        }

        List<CardSO> MarketPlace25kApp = new List<CardSO>();

        foreach(int cardId in remoteMarketPlace25kP3) {
            MarketPlace25kApp.Add(allCard[cardId]);
        }

        players[2].marketPlace_25K = MarketPlace25kApp;

        Debug.Log("25K P3");
        foreach(CardSO card in players[2].marketPlace_25K) {
            Debug.Log("["+card.id+"]");
        }
        Debug.Log("end MarketPlace25K P3 change event");
    }

    public void remoteMarketPlace25kP4OnOnListChanged(NetworkListEvent<int> changeevent) {
        Debug.Log("start MarketPlace25K P4 change event");
        if(remoteMarketPlace25kP4.Count == 0) {
            return;
        }

        List<CardSO> MarketPlace25kApp = new List<CardSO>();

        foreach(int cardId in remoteMarketPlace25kP4) {
            MarketPlace25kApp.Add(allCard[cardId]);
        }

        players[3].marketPlace_25K = MarketPlace25kApp;

        Debug.Log("25K P4");
        foreach(CardSO card in players[3].marketPlace_25K) {
            Debug.Log("["+card.id+"]");
        }
        Debug.Log("end MarketPlace25K P4 change event");
    }

    public void remoteMarketPlace50kP1OnOnListChanged(NetworkListEvent<int> changeevent) {
        Debug.Log("start MarketPlace50K P1 change event");
        if(remoteMarketPlace50kP1.Count == 0) {
            return;
        }

        List<CardSO> MarketPlace50kApp = new List<CardSO>();

        foreach(int cardId in remoteMarketPlace50kP1) {
            MarketPlace50kApp.Add(allCard[cardId]);
        }

        players[0].marketPlace_50K = MarketPlace50kApp;

        Debug.Log("50K P1");
        foreach(CardSO card in players[0].marketPlace_50K) {
            Debug.Log("["+card.id+"]");
        }
        Debug.Log("end MarketPlace50K P1 change event");
    }

    public void remoteMarketPlace50kP2OnOnListChanged(NetworkListEvent<int> changeevent) {
        Debug.Log("start MarketPlace50K P2 change event");
        if(remoteMarketPlace50kP2.Count == 0) {
            return;
        }

        List<CardSO> MarketPlace50kApp = new List<CardSO>();

        foreach(int cardId in remoteMarketPlace50kP2) {
            MarketPlace50kApp.Add(allCard[cardId]);
        }

        players[1].marketPlace_50K = MarketPlace50kApp;

        Debug.Log("50K P2");
        foreach(CardSO card in players[1].marketPlace_50K) {
            Debug.Log("["+card.id+"]");
        }
        Debug.Log("end MarketPlace50K P2 change event");
    }

    public void remoteMarketPlace50kP3OnOnListChanged(NetworkListEvent<int> changeevent) {
        Debug.Log("start MarketPlace50K P3 change event");
        if(remoteMarketPlace50kP3.Count == 0) {
            return;
        }

        List<CardSO> MarketPlace50kApp = new List<CardSO>();

        foreach(int cardId in remoteMarketPlace50kP3) {
            MarketPlace50kApp.Add(allCard[cardId]);
        }

        players[2].marketPlace_50K = MarketPlace50kApp;

        Debug.Log("50K P3");
        foreach(CardSO card in players[2].marketPlace_50K) {
            Debug.Log("["+card.id+"]");
        }
        Debug.Log("end MarketPlace50K P3 change event");
    }

    public void remoteMarketPlace50kP4OnOnListChanged(NetworkListEvent<int> changeevent) {
        Debug.Log("start MarketPlace50K P4 change event");
        if(remoteMarketPlace50kP4.Count == 0) {
            return;
        }

        List<CardSO> MarketPlace50kApp = new List<CardSO>();

        foreach(int cardId in remoteMarketPlace50kP4) {
            MarketPlace50kApp.Add(allCard[cardId]);
        }

        players[3].marketPlace_50K = MarketPlace50kApp;

        Debug.Log("50K P4");
        foreach(CardSO card in players[3].marketPlace_50K) {
            Debug.Log("["+card.id+"]");
        }
        Debug.Log("end MarketPlace50K P4 change event");
    }

    public void remoteMarketPlace100kP1OnOnListChanged(NetworkListEvent<int> changeevent) {
        Debug.Log("start MarketPlace100K P1 change event");
        if(remoteMarketPlace100kP1.Count == 0) {
            return;
        }

        List<CardSO> MarketPlace100kApp = new List<CardSO>();

        foreach(int cardId in remoteMarketPlace100kP1) {
            MarketPlace100kApp.Add(allCard[cardId]);
        }

        players[0].marketPlace_100k = MarketPlace100kApp;

        Debug.Log("100K P1");
        foreach(CardSO card in players[0].marketPlace_100k) {
            Debug.Log("["+card.id+"]");
        }
        Debug.Log("end MarketPlace100K P1 change event");
    }

    public void remoteMarketPlace100kP2OnOnListChanged(NetworkListEvent<int> changeevent) {
        Debug.Log("start MarketPlace100K P2 change event");
        if(remoteMarketPlace100kP2.Count == 0) {
            return;
        }
        List<CardSO> MarketPlace100kApp = new List<CardSO>();

        foreach(int cardId in remoteMarketPlace100kP2) {
            MarketPlace100kApp.Add(allCard[cardId]);
        }

        players[1].marketPlace_100k = MarketPlace100kApp;

        Debug.Log("100K P2");
        foreach(CardSO card in players[1].marketPlace_100k) {
            Debug.Log("["+card.id+"]");
        }
        Debug.Log("end MarketPlace100K P2 change event");
    }

    public void remoteMarketPlace100kP3OnOnListChanged(NetworkListEvent<int> changeevent) {
        Debug.Log("start MarketPlace100K P3 change event");
        if(remoteMarketPlace100kP3.Count == 0) {
            return;
        }

        List<CardSO> MarketPlace100kApp = new List<CardSO>();

        foreach(int cardId in remoteMarketPlace100kP3) {
            MarketPlace100kApp.Add(allCard[cardId]);
        }

        players[2].marketPlace_100k = MarketPlace100kApp;

        Debug.Log("100K P3");
        foreach(CardSO card in players[2].marketPlace_100k) {
            Debug.Log("["+card.id+"]");
        }
        Debug.Log("end MarketPlace100K P3 change event");
    }

    public void remoteMarketPlace100kP4OnOnListChanged(NetworkListEvent<int> changeevent) {
        Debug.Log("start MarketPlace100K P4 change event");
        if(remoteMarketPlace100kP4.Count == 0) {
            return;
        }

        List<CardSO> MarketPlace100kApp = new List<CardSO>();

        foreach(int cardId in remoteMarketPlace100kP4) {
            MarketPlace100kApp.Add(allCard[cardId]);
        }

        players[3].marketPlace_100k = MarketPlace100kApp;

        Debug.Log("100K P4");
        foreach(CardSO card in players[3].marketPlace_100k) {
            Debug.Log("["+card.id+"]");
        }
        Debug.Log("end MarketPlace100K P4 change event");
    }

    public void remoteProtection25kP1OnOnListChanged(NetworkListEvent<int> changeevent) {
        Debug.Log("start Protection25k P1 change event");
        if(remoteProtection25kP1.Count == 0) {
            return;
        }

        List<CardSO> Protection25kApp = new List<CardSO>();

        foreach(int cardId in remoteProtection25kP1) {
            Protection25kApp.Add(allCard[cardId]);
        }

        players[0].protection_25K = Protection25kApp;

        Debug.Log("25K Protection P1");
        foreach(CardSO card in players[0].protection_25K) {
            Debug.Log("["+card.id+"]");
        }
        Debug.Log("end Protection25k P1 change event");
    }

    public void remoteProtection25kP2OnOnListChanged(NetworkListEvent<int> changeevent) {
        Debug.Log("start Protection25k P2 change event");
        if(remoteProtection25kP2.Count == 0) {
            return;
        }

        List<CardSO> Protection25kApp = new List<CardSO>();

        foreach(int cardId in remoteProtection25kP2) {
            Protection25kApp.Add(allCard[cardId]);
        }

        players[1].protection_25K = Protection25kApp;

        Debug.Log("25K Protection P2");
        foreach(CardSO card in players[1].protection_25K) {
            Debug.Log("["+card.id+"]");
        }
        Debug.Log("end Protection25k P2 change event");
    }

    public void remoteProtection25kP3OnOnListChanged(NetworkListEvent<int> changeevent) {
        Debug.Log("start Protection25k P3 change event");
        if(remoteProtection25kP3.Count == 0) {
            return;
        }

        List<CardSO> Protection25kApp = new List<CardSO>();

        foreach(int cardId in remoteProtection25kP3) {
            Protection25kApp.Add(allCard[cardId]);
        }

        players[2].protection_25K = Protection25kApp;

        Debug.Log("25K Protection P3");
        foreach(CardSO card in players[2].protection_25K) {
            Debug.Log("["+card.id+"]");
        }
        Debug.Log("end Protection25k P3 change event");
    }

    public void remoteProtection25kP4OnOnListChanged(NetworkListEvent<int> changeevent) {
        Debug.Log("start Protection25k P4 change event");
        if(remoteProtection25kP4.Count == 0) {
            return;
        }

        List<CardSO> Protection25kApp = new List<CardSO>();

        foreach(int cardId in remoteProtection25kP4) {
            Protection25kApp.Add(allCard[cardId]);
        }

        players[3].protection_25K = Protection25kApp;

        Debug.Log("25K Protection P4");
        foreach(CardSO card in players[3].protection_25K) {
            Debug.Log("["+card.id+"]");
        }
        Debug.Log("end Protection25k P4 change event");
    }

    public void remoteProtection50kP1OnOnListChanged(NetworkListEvent<int> changeevent) {
        Debug.Log("start Protection50k P1 change event");
        if(remoteProtection50kP1.Count == 0) {
            return;
        }

        List<CardSO> Protection50kApp = new List<CardSO>();

        foreach(int cardId in remoteProtection50kP1) {
            Protection50kApp.Add(allCard[cardId]);
        }

        players[0].protection_50K = Protection50kApp;

        Debug.Log("50K Protection P1");
        foreach(CardSO card in players[0].protection_50K) {
            Debug.Log("["+card.id+"]");
        }
        Debug.Log("end Protection50k P1 change event");
    }

    public void remoteProtection50kP2OnOnListChanged(NetworkListEvent<int> changeevent) {
        Debug.Log("start Protection50k P2 change event");
        if(remoteProtection50kP2.Count == 0) {
            return;
        }

        List<CardSO> Protection50kApp = new List<CardSO>();

        foreach(int cardId in remoteProtection50kP2) {
            Protection50kApp.Add(allCard[cardId]);
        }

        players[1].protection_50K = Protection50kApp;

        Debug.Log("50K Protection P2");
        foreach(CardSO card in players[1].protection_50K) {
            Debug.Log("["+card.id+"]");
        }
        Debug.Log("end Protection50k P2 change event");
    }

    public void remoteProtection50kP3OnOnListChanged(NetworkListEvent<int> changeevent) {
        Debug.Log("start Protection50k P3 change event");
        if(remoteProtection50kP3.Count == 0) {
            return;
        }

        List<CardSO> Protection50kApp = new List<CardSO>();

        foreach(int cardId in remoteProtection50kP3) {
            Protection50kApp.Add(allCard[cardId]);
        }

        players[2].protection_50K = Protection50kApp;

        Debug.Log("50K Protection P3");
        foreach(CardSO card in players[2].protection_50K) {
            Debug.Log("["+card.id+"]");
        }
        Debug.Log("end Protection50k P3 change event");
    }


    public void remoteProtection50kP4OnOnListChanged(NetworkListEvent<int> changeevent) {
        Debug.Log("start Protection50k P4 change event");
        if(remoteProtection50kP4.Count == 0) {
            return;
        }

        List<CardSO> Protection50kApp = new List<CardSO>();

        foreach(int cardId in remoteProtection50kP4) {
            Protection50kApp.Add(allCard[cardId]);
        }

        players[3].protection_50K = Protection50kApp;

        Debug.Log("50K Protection P4");
        foreach(CardSO card in players[3].protection_50K) {
            Debug.Log("["+card.id+"]");
        }
        Debug.Log("end Protection50k P4 change event");
    }

    public void remoteMyTurnOnOnListChanged(NetworkListEvent<bool> changeevent) {
        Debug.Log("start MyTurn change event");
        if(remoteMyTurn.Count == 0) {
            return;
        }

        int i = 0;
        foreach(bool myTurn in remoteMyTurn) {
            players[i].myTurn = myTurn;
            i++;
        }

        Debug.Log("Player my turn");
        foreach(PlayerCustom player in players) {
            Debug.Log("["+player.playerName+"]");
            Debug.Log("["+player.myTurn+"]");
        }
        Debug.Log("end MyTurn change event");
    }

    public void remotePickFromDeckOnOnListChanged(NetworkListEvent<bool> changeevent) {
        Debug.Log("start PickFromDeck change event");
        if(remotePickFromDeck.Count == 0) {
            return;
        }

        int i = 0;
        foreach(bool pickFromDeck in remotePickFromDeck) {
            players[i].pickFromDeck = pickFromDeck;
            i++;
        }

        Debug.Log("Player pick from deck");
        foreach(PlayerCustom player in players) {
            Debug.Log("["+player.pickFromDeck+"]");
        }
        Debug.Log("end PickFromDeck change event");
    }

    public void remoteSkipTurnOnOnListChanged(NetworkListEvent<int> changeevent) {
        Debug.Log("start SkipTurn change event");
        if(remoteSkipTurn.Count == 0) {
            return;
        }

        int i = 0;
        foreach(int skipTurn in remoteSkipTurn) {
            players[i].skipTurn = skipTurn;
            i++;
        }

        Debug.Log("Player skip turn");
        foreach(PlayerCustom player in players) {
            Debug.Log("["+player.skipTurn+"]");
        }
        Debug.Log("end SkipTurn change event");
    }

    //Start Relay Host
    public async Task<string> startHost() {
        Debug.Log("Start Host Pre Game");
        Allocation allocation = await RelayService.Instance.CreateAllocationAsync(4);
        hostAllocationId = allocation.AllocationId;
        allocationRegion = allocation.Region;
        joinCode = await RelayService.Instance.GetJoinCodeAsync(hostAllocationId);

        RelayServerData relayServerData = new RelayServerData(allocation, "dtls");
        NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);

        NetworkManager.Singleton.StartHost();

        Debug.Log("hostAllocationId: " + hostAllocationId);
        Debug.Log("allocationRegion: " + allocationRegion);
        Debug.Log("joinCode: " + joinCode);

        players = new List<PlayerCustom>();

        players.Add(new PlayerCustom(order, true));

        order++;

        Debug.Log("end Host Pre Game");

        return joinCode;
    }

    //Start Join Relay 
    public async void join(string code) {
        Debug.Log("Start Join Realy");
        var joinAllocation = await RelayService.Instance.JoinAllocationAsync(code);
        playerAllocationId = joinAllocation.AllocationId;

        RelayServerData relayServerData = new RelayServerData(joinAllocation, "dtls");
        NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);

        NetworkManager.Singleton.StartClient();

        Debug.Log("playerAllocationId: " + playerAllocationId);
        Debug.Log("end Join Realy");
    }

    private void OnClientConnectedCallback(ulong clientId) {
        Debug.Log("Start new client event");
        if(NetworkManager.Singleton.IsHost) {
            count++;
            Debug.Log("NetworkManager.Singleton.ConnectedClientsList: " + NetworkManager.Singleton.ConnectedClientsList.Count);
            if(count == numPlayers) {
                getNickNameClientRpc(numPlayers);
            }
        }
        Debug.Log("end new client event");
    }

    [ServerRpc(RequireOwnership = false)]
    public void AddNewPlayerServerRpc(String playerName) {
        Debug.Log("insert new player");
        players.Add(new PlayerCustom(order, playerName));
        order++;
        if(players.Count == numPlayers) {
            PreGameClientRpc();
        }
    }

    [ClientRpc]
    public void getNickNameClientRpc(int num) {
        Debug.Log("Request nickname from clients");
        if(!NetworkManager.Singleton.IsHost) {
            Debug.Log("clients send own nickname");
            numPlayers = num;
            players = new List<PlayerCustom>();
            for(int i = 0; i < numPlayers; i++) {
                players.Add(new PlayerCustom(0, null));
            }
            AddNewPlayerServerRpc(AuthenticationService.Instance.PlayerName);
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void  updateRemoteDateServerRpc() {
            Debug.Log("Update remote Deck start");

            for(int j = 0; j < numPlayers; j++) {
                if(j == 0) {
                    remoteHandP1.Clear();
                    remoteMarketP1.Clear();
                    remoteMarketPlace5kP1.Clear();
                    remoteMarketPlace25kP1.Clear();
                    remoteMarketPlace50kP1.Clear();
                    remoteMarketPlace100kP1.Clear();
                    remoteProtection25kP1.Clear();
                    remoteProtection50kP1.Clear();
                } else if (j == 1) {
                    remoteHandP2.Clear();
                    remoteMarketP2.Clear(); 
                    remoteMarketPlace5kP2.Clear();
                    remoteMarketPlace25kP2.Clear();
                    remoteMarketPlace50kP2.Clear();
                    remoteMarketPlace100kP2.Clear();
                    remoteProtection25kP2.Clear();
                    remoteProtection50kP2.Clear();
                } else if (j == 2) {
                    remoteHandP3.Clear();
                    remoteMarketP3.Clear();
                    remoteMarketPlace5kP3.Clear();
                    remoteMarketPlace25kP3.Clear();
                    remoteMarketPlace50kP3.Clear();
                    remoteMarketPlace100kP3.Clear();
                    remoteProtection25kP3.Clear();
                    remoteProtection50kP3.Clear();
                } else if (j == 3) {
                    remoteHandP4.Clear();
                    remoteMarketP4.Clear();
                    remoteMarketPlace5kP4.Clear();
                    remoteMarketPlace25kP4.Clear();
                    remoteMarketPlace50kP4.Clear();
                    remoteMarketPlace100kP4.Clear();
                    remoteProtection25kP4.Clear();
                    remoteProtection50kP4.Clear();
                }
            }

            remotePlayerName.Clear();
            remoteCash.Clear();
            remoteMarket.Clear();
            remoteNumPlayer.Clear();
            remoteHeatOn.Clear();
            remoteTypeHeatOn.Clear();
            remoteMyTurn.Clear();
            remotePickFromDeck.Clear();
            remoteSkipTurn.Clear();

        int i = 0;
        foreach(PlayerCustom app in players) {
            if(i == 0) {
                remotePlayerName.Add(app.playerName);
                remoteCash.Add(app.cash);
                remoteMarket.Add(app.market.ToString());
                remoteNumPlayer.Add(app.numPlayer);

                foreach(CardSO card in app.Hand) {
                    remoteHandP1.Add(card.id);
                }

                foreach(CardSO card in app.marketSpace) {
                    remoteMarketP1.Add(card.id);
                }
                
                remoteHeatOn.Add(app.heatOn);
                remoteTypeHeatOn.Add(app.type_HEatOn.ToString());

                foreach(CardSO card in app.marketPlace_5K) {
                    remoteMarketPlace5kP1.Add(card.id);
                }
                
                if(app.marketPlace_5K.Count == 0) {
                    clear5kClientRpc(0);
                }

                foreach(CardSO card in app.marketPlace_25K) {
                    remoteMarketPlace25kP1.Add(card.id);
                }

                if(app.marketPlace_25K.Count == 0) {
                    clear25kClientRpc(0);
                }

                foreach(CardSO card in app.marketPlace_50K) {
                    remoteMarketPlace50kP1.Add(card.id);
                }

                if(app.marketPlace_50K.Count == 0) {
                    clear50kClientRpc(0);
                }

                foreach(CardSO card in app.marketPlace_100k) {
                    remoteMarketPlace100kP1.Add(card.id);
                }

                if(app.marketPlace_100k.Count == 0) {
                    clear100kClientRpc(0);
                }

                foreach(CardSO card in app.protection_25K) {
                    remoteProtection25kP1.Add(card.id);
                }

                if(app.protection_25K.Count == 0) {
                    clearProtection25kClientRpc(0);
                }

                foreach(CardSO card in app.protection_50K) {
                    remoteProtection50kP1.Add(card.id);
                }

                if(app.protection_50K.Count == 0) {
                    clearProtection50kClientRpc(0);
                }

                remoteMyTurn.Add(app.myTurn);
                remotePickFromDeck.Add(app.pickFromDeck);
                remoteSkipTurn.Add(app.skipTurn);
            } else if (i == 1) {
                remotePlayerName.Add(app.playerName);
                remoteCash.Add(app.cash);
                remoteMarket.Add(app.market.ToString());
                remoteNumPlayer.Add(app.numPlayer);

                foreach(CardSO card in app.Hand) {
                    remoteHandP2.Add(card.id);
                }

                foreach(CardSO card in app.marketSpace) {
                    remoteMarketP2.Add(card.id);
                }
                
                remoteHeatOn.Add(app.heatOn);
                remoteTypeHeatOn.Add(app.type_HEatOn.ToString());

                foreach(CardSO card in app.marketPlace_5K) {
                    remoteMarketPlace5kP2.Add(card.id);
                }

                foreach(CardSO card in app.marketPlace_25K) {
                    remoteMarketPlace25kP2.Add(card.id);
                }
                foreach(CardSO card in app.marketPlace_50K) {
                    remoteMarketPlace50kP2.Add(card.id);
                }
                foreach(CardSO card in app.marketPlace_100k) {
                    remoteMarketPlace100kP2.Add(card.id);
                }
                foreach(CardSO card in app.protection_25K) {
                    remoteProtection25kP2.Add(card.id);
                }
                foreach(CardSO card in app.protection_50K) {
                    remoteProtection50kP2.Add(card.id);
                }

                remoteMyTurn.Add(app.myTurn);
                remotePickFromDeck.Add(app.pickFromDeck);
                remoteSkipTurn.Add(app.skipTurn);

                if(app.marketPlace_5K.Count == 0) {
                    clear5kClientRpc(1);
                }

                if(app.marketPlace_25K.Count == 0) {
                    clear25kClientRpc(1);
                }

                if(app.marketPlace_50K.Count == 0) {
                    clear50kClientRpc(1);
                }

                if(app.marketPlace_100k.Count == 0) {
                    clear100kClientRpc(1);
                }

                if(app.protection_25K.Count == 0) {
                    clearProtection25kClientRpc(1);
                }
				
				if(app.protection_50K.Count == 0) {
                    clearProtection50kClientRpc(1);
                }

            } else if(i == 2) {
                remotePlayerName.Add(app.playerName);
                remoteCash.Add(app.cash);
                remoteMarket.Add(app.market.ToString());
                remoteNumPlayer.Add(app.numPlayer);

                foreach(CardSO card in app.Hand) {
                    remoteHandP3.Add(card.id);
                }

                foreach(CardSO card in app.marketSpace) {
                    remoteMarketP3.Add(card.id);
                }
                
                remoteHeatOn.Add(app.heatOn);
                remoteTypeHeatOn.Add(app.type_HEatOn.ToString());

                foreach(CardSO card in app.marketPlace_5K) {
                    remoteMarketPlace5kP3.Add(card.id);
                }

                foreach(CardSO card in app.marketPlace_25K) {
                    remoteMarketPlace25kP3.Add(card.id);
                }
                foreach(CardSO card in app.marketPlace_50K) {
                    remoteMarketPlace50kP3.Add(card.id);
                }
                foreach(CardSO card in app.marketPlace_100k) {
                    remoteMarketPlace100kP3.Add(card.id);
                }
                foreach(CardSO card in app.protection_25K) {
                    remoteProtection25kP3.Add(card.id);
                }
                foreach(CardSO card in app.protection_50K) {
                    remoteProtection50kP3.Add(card.id);
                }

                remoteMyTurn.Add(app.myTurn);
                remotePickFromDeck.Add(app.pickFromDeck);
                remoteSkipTurn.Add(app.skipTurn);

                if(app.marketPlace_5K.Count == 0) {
                    clear5kClientRpc(2);
                }

                if(app.marketPlace_25K.Count == 0) {
                    clear25kClientRpc(2);
                }

                if(app.marketPlace_50K.Count == 0) {
                    clear50kClientRpc(2);
                }

                if(app.marketPlace_100k.Count == 0) {
                    clear100kClientRpc(2);
                }

                if(app.protection_25K.Count == 0) {
                    clearProtection25kClientRpc(2);
                }
				
				if(app.protection_50K.Count == 0) {
                    clearProtection50kClientRpc(2);
                }
            } else if (i == 3) {
                remotePlayerName.Add(app.playerName);
                remoteCash.Add(app.cash);
                remoteMarket.Add(app.market.ToString());
                remoteNumPlayer.Add(app.numPlayer);

                foreach(CardSO card in app.Hand) {
                    remoteHandP4.Add(card.id);
                }

                foreach(CardSO card in app.marketSpace) {
                    remoteMarketP4.Add(card.id);
                }
                
                remoteHeatOn.Add(app.heatOn);
                remoteTypeHeatOn.Add(app.type_HEatOn.ToString());

                foreach(CardSO card in app.marketPlace_5K) {
                    remoteMarketPlace5kP4.Add(card.id);
                }

                foreach(CardSO card in app.marketPlace_25K) {
                    remoteMarketPlace25kP4.Add(card.id);
                }
                foreach(CardSO card in app.marketPlace_50K) {
                    remoteMarketPlace50kP4.Add(card.id);
                }
                foreach(CardSO card in app.marketPlace_100k) {
                    remoteMarketPlace100kP4.Add(card.id);
                }
                foreach(CardSO card in app.protection_25K) {
                    remoteProtection25kP4.Add(card.id);
                }
                foreach(CardSO card in app.protection_50K) {
                    remoteProtection50kP4.Add(card.id);
                }

                remoteMyTurn.Add(app.myTurn);
                remotePickFromDeck.Add(app.pickFromDeck);
                remoteSkipTurn.Add(app.skipTurn);

                if(app.marketPlace_5K.Count == 0) {
                    clear5kClientRpc(3);
                }

                if(app.marketPlace_25K.Count == 0) {
                    clear25kClientRpc(3);
                }

                if(app.marketPlace_50K.Count == 0) {
                    clear50kClientRpc(3);
                }

                if(app.marketPlace_100k.Count == 0) {
                    clear100kClientRpc(3);
                }

                if(app.protection_25K.Count == 0) {
                    clearProtection25kClientRpc(3);
                }
				
				if(app.protection_50K.Count == 0) {
                    clearProtection50kClientRpc(3);
                }
            }

            i++;
        }
        Debug.Log("Update remote Deck end");
    }

    

    [ClientRpc]
    public void PreGameClientRpc() {
        lobbyName.text = LobbyManager.instance.joinedLobby.Name;  
        buildSyncDeck();
        if(NetworkManager.Singleton.IsHost) {
            deck.Shuffle();
            createPlayerHand();
            updateRemoteDateServerRpc();
            SaveCardIdOnNetwork();
            finishPreGameServerRpc();
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void finishPreGameServerRpc() {
        StartGameClientRpc();
    }
    
    [ServerRpc(RequireOwnership = false)]
    public void StartTurnServerRpc() {
        Debug.Log("start turn server rpc start");
        StartTurnClientRpc();
        Debug.Log("start turn server rpc end");
    }

    [ClientRpc]
    public void StartTurnClientRpc() {
        Debug.Log("New Turn start");
        showTurnGui();
        showPlayer();
        Debug.Log("New Turn end");
    }

    

    [ClientRpc]
    public void StartGameClientRpc() {
        StartCoroutine(CountdownToStart());
        Debug.Log("Start all game");
    }
    
    //Manager Game
    private void managerGame() {
        Debug.Log("Start Manager Game");

        turnBanner.SetActive(true);

        showTurnGui();
        
        showPlayer();
    }

    public void showTurnGui() {
        Debug.Log("show turn gui start");
        turnBanner.GetComponentInChildren<Text>().text = "";
        foreach(PlayerCustom player in players) {
            if(player.myTurn) {
                Debug.Log("AuthenticationService.Instance.PlayerName: " + AuthenticationService.Instance.PlayerName);
                Debug.Log("player.playerName: " + player.playerName);
                if(player.playerName == AuthenticationService.Instance.PlayerName) { 
                    pickCardBanner.SetActive(true);
                    turnBanner.GetComponentInChildren<Text>().text = "It's your turn";
                } else {
                    turnBanner.GetComponentInChildren<Text>().text = player.playerName.Split('#')[0] + "'s turn";
                }     
                turnBanner.SetActive(true);
                StartCoroutine(SleepTurnBanner());    
            }
        }
        Debug.Log("show turn gui start");
    }

    public void showHeatON(CardSO.Heat heatOn, String playerName) {
            heatSpot.GetComponentInChildren<Text>().text = playerName;
            switch(heatOn) {
                case CardSO.Heat.BUST:
                    heatSpot.GetComponent<Image>().sprite = heatOnBustSprite;
                    heatSpot.GetComponent<Image>().SetNativeSize();
                    heatSpot.SetActive(true);
                    StartCoroutine(startClip("Heaton_bust"));  
                    break;
                case CardSO.Heat.FELONY:
                    heatSpot.GetComponent<Image>().sprite = heatOnFelonySprite;
                    heatSpot.GetComponent<Image>().SetNativeSize();
                    heatSpot.SetActive(true);
                    StartCoroutine(startClip("Heaton_felony"));  
                    break;
                case CardSO.Heat.SS:
                    heatSpot.GetComponent<Image>().sprite = heatOnSSSprite;
                    heatSpot.GetComponent<Image>().SetNativeSize();
                    heatSpot.SetActive(true);
                    StartCoroutine(startClip("Heaton_search"));  
                    break;
                case CardSO.Heat.DETAINED:
                    heatSpot.GetComponent<Image>().sprite = heatOnDetainedSprite;
                    heatSpot.GetComponent<Image>().SetNativeSize();
                    heatSpot.SetActive(true);
                    StartCoroutine(startClip("Heaton_detained"));  
                    break;
            }          
    }

    public void showHeatOff(CardSO.Heat heatOff) {
        foreach(PlayerCustom player in players) {
            if(player.myTurn) {
                heatSpot.GetComponentInChildren<Text>().text = player.playerName.Split('#')[0];
            }
        }
        
        switch(heatOff) {
            case CardSO.Heat.BUST:
                heatSpot.GetComponent<Image>().sprite = InnocentSprite;
                heatSpot.GetComponent<Image>().SetNativeSize();
                heatSpot.SetActive(true);
                StartCoroutine(startClip("Heatoff_bust"));  
                break;
            case CardSO.Heat.FELONY:
                heatSpot.GetComponent<Image>().sprite = InnocentSprite;
                heatSpot.GetComponent<Image>().SetNativeSize();
                heatSpot.SetActive(true);
                StartCoroutine(startClip("Heatoff_felony"));  
                break;
            case CardSO.Heat.SS:
                heatSpot.GetComponent<Image>().sprite = InnocentSprite;
                heatSpot.GetComponent<Image>().SetNativeSize();
                heatSpot.SetActive(true);
                StartCoroutine(startClip("Heatoff_search"));  
                break;
            case CardSO.Heat.DETAINED:
                heatSpot.GetComponent<Image>().sprite = InnocentSprite;
                heatSpot.GetComponent<Image>().SetNativeSize();
                heatSpot.SetActive(true);
                StartCoroutine(startClip("Heatoff_detained"));  
                break;
        }          
        
    }

    public void showUtility(CardSO.Effects Utility, String playerName, CardSO.Nirvana nirvanaType) {
        heatSpot.GetComponentInChildren<Text>().text = "";
        foreach(PlayerCustom player in players) {
            switch(Utility) {
                case CardSO.Effects.NIRVANA:
                    if(nirvanaType == CardSO.Nirvana.STONEHIGH) {
                        heatSpot.GetComponent<Image>().sprite = stonehighSprite;
                    } else if (nirvanaType == CardSO.Nirvana.EUPHORIA) {
                        heatSpot.GetComponent<Image>().sprite = euphoriaSprite;
                    }
                    heatSpot.GetComponent<Image>().SetNativeSize();
                    heatSpot.SetActive(true);
                    StartCoroutine(startClip("nirvana"));  
                    break;
                case CardSO.Effects.STEAL:
                    heatSpot.GetComponent<Image>().sprite = stealSprite;
                    heatSpot.GetComponent<Image>().SetNativeSize();
                    heatSpot.SetActive(true);
                    StartCoroutine(startClip("steal"));  
                    break;
                case CardSO.Effects.PAY_FINE:
                    heatSpot.GetComponent<Image>().sprite = payFineSprite;
                    heatSpot.GetComponent<Image>().SetNativeSize();
                    heatSpot.SetActive(true);
                    StartCoroutine(startClip("steal"));  
                    break;
            }          
        }
    }

    IEnumerator startClip(String _clipName){
        Sound soundToPlay = Array.Find(AudioManager.Instance.sounds, dummySound => dummySound.clipName == _clipName);
        if(soundToPlay != null) {
            soundToPlay.source.Play();
            yield return new WaitWhile (()=> soundToPlay.source.isPlaying);

            heatSpot.SetActive(false);
        }
    }


    //Manager Countdown
    void countdownUI(string value) {
        countdown.text = value;
    }

    IEnumerator CountdownToStart() {
        
        yield return new WaitForSeconds(1f);
        
        while(countdownTime > 0) {
            countdownUI(countdownTime.ToString());

            yield return new WaitForSeconds(1f);

            countdownTime--;
        }

        countdownUI("Start!");

        yield return new WaitForSeconds(1f);

        MenuManager.Instance.preGame.SetActive(false);

        managerGame();
    }

    IEnumerator SleepTurnBanner() {

        sleepTurnBannerTime = 2;
        
        while(sleepTurnBannerTime > 0) {

            yield return new WaitForSeconds(1f);

            sleepTurnBannerTime--;
        }

        yield return new WaitForSeconds(1f);

        turnBanner.SetActive(false);

    }

    IEnumerator SleepPassTurn(int waitSecond) {

        sleepTurnBannerTime = waitSecond;
        
        while(sleepTurnBannerTime > 0) {

            yield return new WaitForSeconds(0.5f);

            sleepTurnBannerTime--;
        }

        yield return new WaitForSeconds(0.5f);

        StartTurnServerRpc();
    }

    //Manager Deck
    public void buildSyncDeck() {
        deck = new List<CardSO>();
        int j = 0;
        foreach(CardSO card in cardTypes) {
            for(int i = 0; i < card.maxCard; i++) {
                CardSO app = CardSO.createInstance(j, card);
                j++;
                deck.Add(app);
                allCard.Add(app);               
            }
        }
    }

    public void SaveCardIdOnNetwork() { 
        foreach(CardSO card in deck) {
            remoteDeck.Add(card.id);
        }
    }

    public void createPlayerHand() {

    foreach(PlayerCustom player in players) {
        Debug.Log(player.playerName);
        int numIter = 0;

        while(numIter < 6) {   

            deck[0].inDeck = false;
            player.Hand.Add(deck[0]);
            deck.RemoveAt(0);

            numIter++;
        }
    }
   }

   public void showPlayer() {
    Debug.Log("show player start");
     if(player1Spot.transform.childCount > 0) {
        GameObject.Destroy(player1Spot.transform.GetChild(0).gameObject);
    }
    if(player2Spot.transform.childCount > 0) {
        GameObject.Destroy(player2Spot.transform.GetChild(0).gameObject);
    }
    if(player3Spot.transform.childCount > 0) {
        GameObject.Destroy(player3Spot.transform.GetChild(0).gameObject);
    }
    if(player4Spot.transform.childCount > 0) {
        GameObject.Destroy(player4Spot.transform.GetChild(0).gameObject);
    }
    int i = 0;
    foreach(PlayerCustom player in players) { 
        if(i == 0) {
            Debug.Log("playerName: " + player.playerName);
            GameObject play = (GameObject)Instantiate(playerTemplate);

            if(player.myTurn) {
                play.transform.GetChild(0).GetChild(2).GetComponentInChildren<Text>().color = Color.yellow;
            }

            play.transform.GetChild(0).GetChild(2).GetComponentInChildren<Text>().text = player.playerName.Split('#')[0];
            play.transform.GetChild(0).GetChild(1).GetComponentInChildren<Text>().text = player.cash +  "$";
            if(player.market == PlayerCustom.Market.True) {
                play.transform.GetChild(0).GetChild(0).GetComponentInChildren<Image>().sprite = greenMarket;
            } else if (player.market == PlayerCustom.Market.False) {
                play.transform.GetChild(0).GetChild(0).GetComponentInChildren<Image>().sprite = RedMarket;
            } else {
                play.transform.GetChild(0).GetChild(0).GetComponentInChildren<Image>().sprite = YellowMarket;
            }

            if(player.myTurn) {
                play.transform.GetChild(0).GetChild(2).GetComponentInChildren<Text>().color = Color.yellow;
            }
            
            play.transform.SetParent(player1Spot.transform, false);
            
        } else if (i == 1) {
            Debug.Log("playerName: " + player.playerName);
            GameObject play = (GameObject)Instantiate(playerTemplate);

            if(player.myTurn) {
                play.transform.GetChild(0).GetChild(2).GetComponentInChildren<Text>().color = Color.yellow;
            }

            play.transform.GetChild(0).GetChild(2).GetComponentInChildren<Text>().text = player.playerName.Split('#')[0];
            play.transform.GetChild(0).GetChild(1).GetComponentInChildren<Text>().text = player.cash +  "$";
            if(player.market == PlayerCustom.Market.True) {
                play.transform.GetChild(0).GetChild(0).GetComponentInChildren<Image>().sprite = greenMarket;
            } else if (player.market == PlayerCustom.Market.False) {
                play.transform.GetChild(0).GetChild(0).GetComponentInChildren<Image>().sprite = RedMarket;
            } else {
                play.transform.GetChild(0).GetChild(0).GetComponentInChildren<Image>().sprite = YellowMarket;
            }
            play.transform.SetParent(player2Spot.transform, false);
        } else if(i == 2) {
            Debug.Log("playerName: " + player.playerName);
            GameObject play = (GameObject)Instantiate(playerTemplate);

            if(player.myTurn) {
                play.transform.GetChild(0).GetChild(2).GetComponentInChildren<Text>().color = Color.yellow;
            }

            play.transform.GetChild(0).GetChild(2).GetComponentInChildren<Text>().text = player.playerName.Split('#')[0];
            play.transform.GetChild(0).GetChild(1).GetComponentInChildren<Text>().text = player.cash +  "$";
            if(player.market == PlayerCustom.Market.True) {
                play.transform.GetChild(0).GetChild(0).GetComponentInChildren<Image>().sprite = greenMarket;
            } else if (player.market == PlayerCustom.Market.False) {
                play.transform.GetChild(0).GetChild(0).GetComponentInChildren<Image>().sprite = RedMarket;
            } else {
                play.transform.GetChild(0).GetChild(0).GetComponentInChildren<Image>().sprite = YellowMarket;
            }
            play.transform.SetParent(player3Spot.transform, false);
        } else if (i == 3) {
            Debug.Log("playerName: " + player.playerName);
            GameObject play = (GameObject)Instantiate(playerTemplate);

            if(player.myTurn) {
                play.transform.GetChild(0).GetChild(2).GetComponentInChildren<Text>().color = Color.yellow;
            }

            play.transform.GetChild(0).GetChild(2).GetComponentInChildren<Text>().text = player.playerName.Split('#')[0];
            play.transform.GetChild(0).GetChild(1).GetComponentInChildren<Text>().text = player.cash +  "$";
            if(player.market == PlayerCustom.Market.True) {
                play.transform.GetChild(0).GetChild(0).GetComponentInChildren<Image>().sprite = greenMarket;
            } else if (player.market == PlayerCustom.Market.False) {
                play.transform.GetChild(0).GetChild(0).GetComponentInChildren<Image>().sprite = RedMarket;
            } else {
                play.transform.GetChild(0).GetChild(0).GetComponentInChildren<Image>().sprite = YellowMarket;
            }
            play.transform.SetParent(player4Spot.transform, false);
        }

        i++;
    }
        showHand();
        Debug.Log("show player end");
   }

   IEnumerator SleepMoveCardTOBin() {
        for (int i = 0; i < Hand.transform.childCount; i++) {
            if(!Hand.transform.GetChild(i).gameObject.activeSelf) {
                Hand.transform.GetChild(i).gameObject.transform.SetParent(bin.transform);
                i--;
                yield return new WaitForSeconds(0.5f);
            }    
        }

        for (int i = 0; i < marketPlace_5kSpot.transform.childCount; i++) {
            if(!marketPlace_5kSpot.transform.GetChild(i).gameObject.activeSelf) {
                marketPlace_5kSpot.transform.GetChild(i).gameObject.transform.SetParent(bin.transform);
                i--;
                yield return new WaitForSeconds(0.5f);
            }   
        }

        for (int i = 0; i < marketPlace_25kSpot.transform.childCount; i++) {
            if(!marketPlace_25kSpot.transform.GetChild(i).gameObject.activeSelf) {
                marketPlace_25kSpot.transform.GetChild(i).gameObject.transform.SetParent(bin.transform);
                i--;
                yield return new WaitForSeconds(0.5f);
            }
        }

        for (int i = 0; i < marketPlace_50kSpot.transform.childCount; i++) {
            if(!marketPlace_50kSpot.transform.GetChild(i).gameObject.activeSelf) {
                marketPlace_50kSpot.transform.GetChild(i).gameObject.transform.SetParent(bin.transform);
                i--;
                yield return new WaitForSeconds(0.5f);
            }
        }

        for (int i = 0; i < marketPlace_100kSpot.transform.childCount; i++) {
            if(!marketPlace_100kSpot.transform.GetChild(i).gameObject.activeSelf) {
                marketPlace_100kSpot.transform.GetChild(i).gameObject.transform.SetParent(bin.transform);
                i--;
                yield return new WaitForSeconds(0.5f);
            }
        }

        for (int i = 0; i < protection_25kSpot.transform.childCount; i++) {
            if(!protection_25kSpot.transform.GetChild(i).gameObject.activeSelf) {
                protection_25kSpot.transform.GetChild(i).gameObject.transform.SetParent(bin.transform);
                i--;
                yield return new WaitForSeconds(0.5f);
            }
        }

        for (int i = 0; i < protection_50kSpot.transform.childCount; i++) {
            if(!protection_50kSpot.transform.GetChild(i).gameObject.activeSelf) {
                protection_50kSpot.transform.GetChild(i).gameObject.transform.SetParent(bin.transform);
                i--;
                yield return new WaitForSeconds(0.5f);
            }
        }

        for (int i = 0; i < marketSpot.transform.childCount; i++) {
            if(!marketSpot.transform.GetChild(i).gameObject.activeSelf) {
                marketSpot.transform.GetChild(i).gameObject.transform.SetParent(bin.transform);
                i--;
                yield return new WaitForSeconds(0.5f);
            }
        }

        for (int i = 0; i < cardsMaledizioniSlot.transform.childCount; i++) {
            if(!cardsMaledizioniSlot.transform.GetChild(i).gameObject.activeSelf) {
                cardsMaledizioniSlot.transform.GetChild(i).gameObject.transform.SetParent(bin.transform);
                i--;
                yield return new WaitForSeconds(0.5f);
            }
        }
        yield return new WaitForSeconds(0.5f);
        GameObject.Destroy(bin);
    }

    //print frist hand in start game
   private void showHand() {

    bin = (GameObject)Instantiate(binPrefab);
    bin.transform.SetParent(this.transform);
    bin.SetActive(false);

    Debug.Log("show hand start");

    Debug.Log("Hand Destroy");
    for (int i = 0; i < Hand.transform.childCount; i++) {
        Hand.transform.GetChild(i).gameObject.SetActive(false);
    }

    Debug.Log("marketPlace_5kSpot Destroy");
    for (int i = 0; i < marketPlace_5kSpot.transform.childCount; i++) {
        marketPlace_5kSpot.transform.GetChild(i).gameObject.SetActive(false);
    }

    Debug.Log("marketPlace_25kSpot Destroy");
    for (int i = 0; i < marketPlace_25kSpot.transform.childCount; i++) {
        marketPlace_25kSpot.transform.GetChild(i).gameObject.SetActive(false);
    }

    Debug.Log("marketPlace_50kSpot Destroy");
    for (int i = 0; i < marketPlace_50kSpot.transform.childCount; i++) {
        marketPlace_50kSpot.transform.GetChild(i).gameObject.SetActive(false);
    }

    Debug.Log("marketPlace_100kSpot Destroy");
    for (int i = 0; i < marketPlace_100kSpot.transform.childCount; i++) {
        marketPlace_100kSpot.transform.GetChild(i).gameObject.SetActive(false);
    }

    Debug.Log("protection_25kSpot Destroy");
    for (int i = 0; i < protection_25kSpot.transform.childCount; i++) {
        protection_25kSpot.transform.GetChild(i).gameObject.SetActive(false);
    }

    Debug.Log("protection_50kSpot Destroy");
    for (int i = 0; i < protection_50kSpot.transform.childCount; i++) {
        protection_50kSpot.transform.GetChild(i).gameObject.SetActive(false);
    }

    Debug.Log("marketSpot Destroy");
    for (int i = 0; i < cardsMaledizioniSlot.transform.childCount; i++) {
        cardsMaledizioniSlot.transform.GetChild(i).gameObject.SetActive(false);
    }

    Debug.Log("maledizione and steal Destroy");
    for (int i = 0; i < marketSpot.transform.childCount; i++) {
        marketSpot.transform.GetChild(i).gameObject.SetActive(false);
    }

    foreach(PlayerCustom player in players) {
        if(player.playerName == AuthenticationService.Instance.PlayerName) {
            foreach(CardSO cardSO in player.Hand) {
                Debug.Log("Hand: " + cardSO.cardName);
                GameObject card = (GameObject)Instantiate(cardTemplate);
                card.GetComponent<ShowCard>().cardData = cardSO;
                card.transform.SetParent(Hand.transform, false);
            }

            foreach(CardSO cardSO in player.marketPlace_5K) {
                Debug.Log("marketPlace_5K: " + cardSO.cardName);
                GameObject card = (GameObject)Instantiate(cardTemplate);
                card.GetComponent<ShowCard>().cardData = cardSO;
                card.GetComponent<ShowCard>().isSummoned = true;
                card.transform.SetParent(marketPlace_5kSpot.transform, false);
            }

            foreach(CardSO cardSO in player.marketPlace_25K) {
                Debug.Log("marketPlace_25K: " + cardSO.cardName);
                GameObject card = (GameObject)Instantiate(cardTemplate);
                card.GetComponent<ShowCard>().cardData = cardSO;
                card.GetComponent<ShowCard>().isSummoned = true;
                card.transform.SetParent(marketPlace_25kSpot.transform, false);
            }

            foreach(CardSO cardSO in player.marketPlace_50K) {
                Debug.Log("marketPlace_50K: " + cardSO.cardName);
                GameObject card = (GameObject)Instantiate(cardTemplate);
                card.GetComponent<ShowCard>().cardData = cardSO;
                card.GetComponent<ShowCard>().isSummoned = true;
                card.transform.SetParent(marketPlace_50kSpot.transform, false);
            }

            foreach(CardSO cardSO in player.marketPlace_100k) {
                Debug.Log("marketPlace_100k: " + cardSO.cardName);
                GameObject card = (GameObject)Instantiate(cardTemplate);
                card.GetComponent<ShowCard>().cardData = cardSO;
                card.GetComponent<ShowCard>().isSummoned = true;
                card.transform.SetParent(marketPlace_100kSpot.transform, false);
            }

            foreach(CardSO cardSO in player.protection_25K) {
                Debug.Log("protection_25K: " + cardSO.cardName);
                GameObject card = (GameObject)Instantiate(cardTemplate);
                card.GetComponent<ShowCard>().cardData = cardSO;
                card.GetComponent<ShowCard>().isSummoned = true;
                card.transform.SetParent(protection_25kSpot.transform, false);
            }

            foreach(CardSO cardSO in player.protection_50K) {
                Debug.Log("protection_50K: " + cardSO.cardName);
                GameObject card = (GameObject)Instantiate(cardTemplate);
                card.GetComponent<ShowCard>().cardData = cardSO;
                card.GetComponent<ShowCard>().isSummoned = true;
                card.transform.SetParent(protection_50kSpot.transform, false);
            }

            foreach(CardSO cardSO in player.marketSpace) {
                Debug.Log("marketSpace: " + cardSO.cardName);
                GameObject card = (GameObject)Instantiate(cardTemplate);
                card.GetComponent<ShowCard>().cardData = cardSO;
                card.GetComponent<ShowCard>().isSummoned = true;
                card.transform.SetParent(marketSpot.transform, false);
            }
        }

        StartCoroutine(SleepMoveCardTOBin());
    }   
    Debug.Log("show hand end");
   }

    //Get card of top of deck
   public void getCard() {
    foreach(PlayerCustom player in players) {
        if(player.myTurn && !player.pickFromDeck) {
            if(player.playerName == AuthenticationService.Instance.PlayerName) {
                pickCardBanner.SetActive(false);
                deck[0].inDeck = false;
                GameObject card = (GameObject)Instantiate(cardTemplate);
                card.GetComponent<ShowCard>().cardData = deck[0];
                card.transform.SetParent(Hand.transform, false);
                if(!NetworkManager.Singleton.IsHost) {
                    player.Hand.Add(deck[0]);
                }
                addCardHandToDeckServerRpc(deck[0].id, player.playerName);
                deck.RemoveAt(0);
                player.pickFromDeck = true;
                if(deck.Count == 0) {
                    Destroy(deckSpot);
                    endDeck = true;
                }
            }
        }
    }
   }

    [ServerRpc(RequireOwnership = false)]
    public void addCardHandToDeckServerRpc(int cardId, FixedString64Bytes playerName) {
        for (int i = 0; i < numPlayers; i++) {
            if(players[i].playerName == playerName.ToString()) {
                players[i].Hand.Add(allCard[cardId]);
            }
        }
    }

    [ServerRpc(RequireOwnership = false)]
   public void removeFirstDeckCardServerRpc() {
        Debug.Log("remove first deck card Start");
        remoteDeck.RemoveAt(0);
        Debug.Log("remove first deck card Start");
   }



   public void EndTurn(int waitSecond) {
    Debug.Log("End turn Start");
    if(endDeck) {
        playMarketCloseServerRpc("");
    }
    removeFirstDeckCardServerRpc();
    ChangeTurnServerRpc();
    updateRemoteDateServerRpc();
    StartCoroutine(SleepPassTurn(waitSecond));
    Debug.Log("End turn end");
   }


    public void changeMarketTOGreen() {
        foreach(PlayerCustom player in players) {
            if(player.playerName == AuthenticationService.Instance.PlayerName) {
                if(player.playerName.Split('#')[0] == player1Spot.transform.GetChild(0).GetChild(0).GetChild(2).GetComponentInChildren<Text>().text) {
                    player1Spot.transform.GetChild(0).GetChild(0).GetChild(0).GetComponentInChildren<Image>().sprite = greenMarket;
                } else if(player.playerName.Split('#')[0] == player2Spot.transform.GetChild(0).GetChild(0).GetChild(2).GetComponentInChildren<Text>().text) {
                    player2Spot.transform.GetChild(0).GetChild(0).GetChild(0).GetComponentInChildren<Image>().sprite = greenMarket;
                } else if(player.playerName.Split('#')[0] == player3Spot.transform.GetChild(0).GetChild(0).GetChild(2).GetComponentInChildren<Text>().text) {
                    player3Spot.transform.GetChild(0).GetChild(0).GetChild(0).GetComponentInChildren<Image>().sprite = greenMarket;
                } else if(player.playerName.Split('#')[0] == player4Spot.transform.GetChild(0).GetChild(0).GetChild(2).GetComponentInChildren<Text>().text) {
                    player4Spot.transform.GetChild(0).GetChild(0).GetChild(0).GetComponentInChildren<Image>().sprite = greenMarket;
                }
            }  
        }
    }

    public void changeMarketTORed() {
        foreach(PlayerCustom player in players) {
            if(player.playerName == AuthenticationService.Instance.PlayerName) {
                if(player.playerName.Split('#')[0] == player1Spot.transform.GetChild(0).GetChild(0).GetChild(2).GetComponentInChildren<Text>().text) {
                    player1Spot.transform.GetChild(0).GetChild(0).GetChild(0).GetComponentInChildren<Image>().sprite = RedMarket;
                } else if(player.playerName.Split('#')[0] == player2Spot.transform.GetChild(0).GetChild(0).GetChild(2).GetComponentInChildren<Text>().text) {
                    player2Spot.transform.GetChild(0).GetChild(0).GetChild(0).GetComponentInChildren<Image>().sprite = RedMarket;
                } else if(player.playerName.Split('#')[0] == player3Spot.transform.GetChild(0).GetChild(0).GetChild(2).GetComponentInChildren<Text>().text) {
                    player3Spot.transform.GetChild(0).GetChild(0).GetChild(0).GetComponentInChildren<Image>().sprite = RedMarket;
                } else if(player.playerName.Split('#')[0] == player4Spot.transform.GetChild(0).GetChild(0).GetChild(2).GetComponentInChildren<Text>().text) {
                    player4Spot.transform.GetChild(0).GetChild(0).GetChild(0).GetComponentInChildren<Image>().sprite = RedMarket;
                }
            }
        }
    }

    public void changeMarketTOYellow() {
        foreach(PlayerCustom player in players) {
            if(player.playerName == AuthenticationService.Instance.PlayerName) {
                if(player.playerName.Split('#')[0] == player1Spot.transform.GetChild(0).GetChild(0).GetChild(2).GetComponentInChildren<Text>().text) {
                    player1Spot.transform.GetChild(0).GetChild(0).GetChild(0).GetComponentInChildren<Image>().sprite = YellowMarket;
                } else if(player.playerName.Split('#')[0] == player2Spot.transform.GetChild(0).GetChild(0).GetChild(2).GetComponentInChildren<Text>().text) {
                    player2Spot.transform.GetChild(0).GetChild(0).GetChild(0).GetComponentInChildren<Image>().sprite = YellowMarket;
                } else if(player.playerName.Split('#')[0] == player3Spot.transform.GetChild(0).GetChild(0).GetChild(2).GetComponentInChildren<Text>().text) {
                    player3Spot.transform.GetChild(0).GetChild(0).GetChild(0).GetComponentInChildren<Image>().sprite = YellowMarket;
                } else if(player.playerName.Split('#')[0] == player4Spot.transform.GetChild(0).GetChild(0).GetChild(2).GetComponentInChildren<Text>().text) {
                    player4Spot.transform.GetChild(0).GetChild(0).GetChild(0).GetComponentInChildren<Image>().sprite = YellowMarket;
                }
            } 
        }
    }

    public void updateCashBanner(int value) {
        foreach(PlayerCustom player in players) {
            if(AuthenticationService.Instance.PlayerName.Split('#')[0] == player1Spot.transform.GetChild(0).GetChild(0).GetChild(2).GetComponentInChildren<Text>().text) {
                int app = value + Int32.Parse(player1Spot.transform.GetChild(0).GetChild(0).GetChild(1).GetComponentInChildren<Text>().text.Split('$')[0]);
                player1Spot.transform.GetChild(0).GetChild(0).GetChild(1).GetComponentInChildren<Text>().text = String.Concat(app.ToString(), "$");
            }
        }
    }

    public PlayerCustom.Market StringtoMarketEnum(string input) {
        PlayerCustom.Market result = PlayerCustom.Market.Null;
        if(PlayerCustom.Market.Null.ToString() == input) {
            result = PlayerCustom.Market.Null;
        } else if (PlayerCustom.Market.True.ToString() == input) {
            result = PlayerCustom.Market.True;
        } else if (PlayerCustom.Market.False.ToString() == input) {
            result = PlayerCustom.Market.False;
        }
        return result;
    }

    public PlayerCustom.Heat StringtoHeatOnEnum(string input) {
        PlayerCustom.Heat result = PlayerCustom.Heat.Null;

        if (PlayerCustom.Heat.FELONY.ToString() == input) {
            result = PlayerCustom.Heat.FELONY;
        } else if(PlayerCustom.Heat.DETAINED.ToString() == input) {
            result = PlayerCustom.Heat.DETAINED;
        } else if (PlayerCustom.Heat.SS.ToString() == input) {
            result = PlayerCustom.Heat.SS;
        } else if (PlayerCustom.Heat.BUST.ToString() == input) {
            result = PlayerCustom.Heat.BUST;
        }

        return result;
    }

    [ServerRpc(RequireOwnership = false)]
    public void DropMaledizioneServerRpc(FixedString64Bytes playerName, int idCard, CardSO.Maledizioni maledizioneType) {
        for(int i = 0; i < numPlayers; i++) {
            if(players[i].playerName == playerName.ToString()) {
                switch(maledizioneType) {
                    case CardSO.Maledizioni.SOLD_OUT:
                        players[i].skipTurn = -2;
                        if(players[i].marketPlace_5K.Count > 0) {
                            players[i].cash = (Int32.Parse(players[i].cash) - 5000).ToString();
                            players[i].marketPlace_5K.RemoveAt(0);
                        } else if(players[i].marketPlace_25K.Count > 0 && players[i].marketPlace_25K.Count > players[i].protection_25K.Count) {
                            players[i].cash = (Int32.Parse(players[i].cash) - 25000).ToString();
                            players[i].marketPlace_25K.RemoveAt(0);
                        } else if(players[i].marketPlace_50K.Count > 0 && players[i].marketPlace_50K.Count > players[i].protection_50K.Count) {
                            players[i].cash = (Int32.Parse(players[i].cash) - 50000).ToString();
                            players[i].marketPlace_50K.RemoveAt(0);
                        } else if(players[i].marketPlace_100k.Count > 0) {
                            players[i].cash = (Int32.Parse(players[i].cash) - 100000).ToString();
                            players[i].marketPlace_100k.RemoveAt(0);
                        }
                        DropCardOnHassleServerRpc(players[i].playerName, idCard);
                        switchCardMalClientRpc(idCard);
                        break;
                    case CardSO.Maledizioni.DOUBLECROSSED:
                        players[i].skipTurn = -3;
                        if(players[i].marketPlace_100k.Count > 0) {
                            players[i].cash = (Int32.Parse(players[i].cash) - 100000).ToString();
                            players[i].marketPlace_100k.RemoveAt(0);
                        }  else if(players[i].marketPlace_50K.Count > 0 && players[i].marketPlace_50K.Count > players[i].protection_50K.Count) {
                            players[i].cash = (Int32.Parse(players[i].cash) - 50000).ToString();
                            players[i].marketPlace_50K.RemoveAt(0);
                        } else if(players[i].marketPlace_25K.Count > 0 && players[i].marketPlace_25K.Count > players[i].protection_25K.Count) {
                            players[i].cash = (Int32.Parse(players[i].cash) - 25000).ToString();
                            players[i].marketPlace_25K.RemoveAt(0);
                        } else if(players[i].marketPlace_5K.Count > 0) {
                            players[i].cash = (Int32.Parse(players[i].cash) - 5000).ToString();
                            players[i].marketPlace_5K.RemoveAt(0);
                        }
                        DropCardOnHassleServerRpc(players[i].playerName, idCard);
                        switchCardMalClientRpc(idCard);
                        break;
                    case CardSO.Maledizioni.UTTERLY_WIPED_OUT:
                        players[i].skipTurn = -3;
                        players[i].market = PlayerCustom.Market.Null;
                        foreach(CardSO card in players[i].marketPlace_5K) {
                            players[i].marketPlace_5K.RemoveAt(players[i].marketPlace_5K.Count - 1);
                        }
                        foreach(CardSO card in players[i].marketPlace_25K) {
                            players[i].marketPlace_25K.RemoveAt(players[i].marketPlace_25K.Count - 1);
                        }
                        foreach(CardSO card in players[i].marketPlace_50K) {
                            players[i].marketPlace_50K.RemoveAt(players[i].marketPlace_50K.Count - 1);
                        }
                        foreach(CardSO card in players[i].marketPlace_100k) {
                            players[i].marketPlace_100k.RemoveAt(players[i].marketPlace_100k.Count - 1);
                        }
                        foreach(CardSO card in players[i].protection_25K) {
                            players[i].protection_25K.RemoveAt(players[i].protection_25K.Count - 1);
                        }
                        foreach(CardSO card in players[i].protection_50K) {
                            players[i].protection_50K.RemoveAt(players[i].protection_50K.Count - 1);
                        }
                        foreach(CardSO card in players[i].marketSpace) {
                            players[i].marketSpace.RemoveAt(players[i].marketSpace.Count - 1);
                        }
                        players[i].cash = "0";
                        DropCardOnHassleServerRpc(players[i].playerName, idCard);
                        switchCardMalClientRpc(idCard);
                        
                        break;
                }
            }
        }
    }

    public void malWaitEnemy() {
        malWaitText.SetActive(true);
        for (int i = 0; i < cardsMaledizioniSlot.transform.childCount; i++) {
            cardsMaledizioniSlot.transform.GetChild(i).gameObject.GetComponent<Button>().interactable = false;
        }
    }

    [ClientRpc]
    public void switchCardMalClientRpc(int idCard) {
        maledizioniBanner.SetActive(true);
        textMaledizioniAndSteal.GetComponentInChildren<Text>().text = "Select card to pass";
        for(int i = 0; i < numPlayers; i++) {
            if(players[i].playerName == AuthenticationService.Instance.PlayerName) {
                foreach(CardSO cardSO in players[i].Hand) {
                    if(idCard != cardSO.id) {
                        GameObject card = (GameObject)Instantiate(cardTemplate);
                        card.GetComponent<ShowCard>().cardData = cardSO;
                        card.GetComponent<ShowCard>().maledizione = true;
                        card.transform.SetParent(cardsMaledizioniSlot.transform, false);
                    }  
                }
            }
        }
    }

     [ServerRpc(RequireOwnership = false)]
     public void maledizionePassCardServerRpc(int idCard,  FixedString64Bytes playerName) {

        int destination = 0;

        for(int i = 0; i < numPlayers; i++) {
            if(players[i].playerName.ToString() == playerName.ToString()) {
                destination = players[i].numPlayer;

                if(destination == numPlayers) {
                    destination = 0;
                }

                for(int j = 0; j < players[i].Hand.Count; j++) {
                    if(players[i].Hand[j].id == idCard) {
                        players[i].Hand.RemoveAt(j);
                    }
                }

                players[destination].Hand.Add(allCard[idCard]);
                malCount++;
            }
        }

        if(malCount == numPlayers) {
            malCount = 0;
            resumeGameFromMalClientRpc(playerName.ToString().Split('#')[0], 0);
        }
    }

    [ClientRpc]
     public void resumeGameFromMalClientRpc(FixedString64Bytes playerName, int timer) {
        maledizioniBanner.SetActive(false);
        malWaitText.SetActive(false);
        backStealCard.SetActive(false);
        if(playerName.ToString() == AuthenticationService.Instance.PlayerName.Split('#')[0]) {
            EndTurn(timer);
        }
     }

    [ServerRpc(RequireOwnership = false)]
    public void ChangeTurnServerRpc() {
        Debug.Log("Pass Turn Start");

        bool flag = true;

        do {

            for(int i = 0; i < numPlayers; i++) {
                if(players[i].myTurn) {
                    players[i].myTurn = false;
                    players[i].pickFromDeck = false;

                    if(players[i].skipTurn == 1) {
                        players[i].skipTurn = 0;
                        players[i].myTurn = true;
                        break;
                    } else if (players[i].skipTurn == 0) {
                        if(players[i].numPlayer == numPlayers) {
                            players[0].myTurn = true;
                            break;
                        } else {
                            players[players[i].numPlayer].myTurn = true;
                            break;
                        }
                    } else if (players[i].skipTurn < 0) {
                        if(players[i].numPlayer == numPlayers) {
                            players[i].skipTurn++;
                            players[0].myTurn = true;
                            break;
                        } else {
                            players[i].skipTurn++; 
                            players[players[i].numPlayer].myTurn = true;
                            break;
                        }
                    }
                }
            }

            foreach(PlayerCustom player in players) {
                if(player.myTurn && player.skipTurn == 0) {
                    flag = false;
                }
            }

        } while(flag);


        Debug.Log("Pass Turn End");
    }

    [ServerRpc(RequireOwnership = false)]
    public void UpdatePlayerCashServerRpc(int cash, FixedString64Bytes playerName, int idCard) {
        for(int j = 0; j < numPlayers; j++) {
            if(players[j].playerName == playerName.ToString()) {
                players[j].cash = (Int32.Parse(players[j].cash) + cash).ToString();
                for(int i = 0; i < players[j].Hand.Count; i++) {
                    if(players[j].Hand[i].id == idCard) {
                        if(cash == 5000) {
                            players[j].marketPlace_5K.Add(players[j].Hand[i]);
                        } else if(cash == 25000) {
                            players[j].marketPlace_25K.Add(players[j].Hand[i]);
                        } else if(cash == 50000) {
                            players[j].marketPlace_50K.Add(players[j].Hand[i]);
                        } else if(cash == 100000) {
                            players[j].marketPlace_100k.Add(players[j].Hand[i]);
                        }
                        players[j].Hand.RemoveAt(i);
                    }
                }
            }
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void UpdatePlayerMarketOpenServerRpc(FixedString64Bytes playerName, int idCard) {
        for(int j = 0; j < numPlayers; j++) {
            if(players[j].playerName == playerName.ToString()) {
                players[j].market = PlayerCustom.Market.True;
                for(int i = 0; i < players[j].Hand.Count; i++) {
                    if(players[j].Hand[i].id == idCard) {
                        players[j].marketSpace.Add(players[j].Hand[i]);
                        players[j].Hand.RemoveAt(i);
                    }
                }
            }
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void DropCardOnHassleServerRpc(FixedString64Bytes playerName, int idCard) {
        for(int j = 0; j < numPlayers; j++) {
            if(players[j].playerName == playerName.ToString()) {
                for(int i = 0; i < players[j].Hand.Count; i++) {
                    if(players[j].Hand[i].id == idCard) {
                        players[j].Hand.RemoveAt(i);
                    }
                }
            }
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void SendHeatOnServerRpc(CardSO.Heat type, int idCard, FixedString64Bytes playerNameEnemy, FixedString64Bytes playerNameMy) {
        for(int i = 0; i < numPlayers; i++) {
            if(players[i].playerName.Split('#')[0] == playerNameEnemy.ToString()) {
                players[i].market = PlayerCustom.Market.False;
                players[i].marketSpace.Add(allCard[idCard]);
                players[i].heatOn = true;
                players[i].type_HEatOn = switchEnumCardToPlayer(type);
            }
        }
        activeSoundEffectHeatOnClientRpc(type, playerNameEnemy);
    }

    [ClientRpc]
    public void activeSoundEffectHeatOnClientRpc(CardSO.Heat type, FixedString64Bytes playerName) {
        showHeatON(type, playerName.ToString());
    }

    [ClientRpc]
    public void activeSoundEffectHeatOffClientRpc(CardSO.Heat type) {
        showHeatOff(type);
    }

    [ClientRpc]
    public void activeSoundEffectUtilityClientRpc(CardSO.Effects utility, FixedString64Bytes playerName, CardSO.Nirvana nirvanaType) {
        showUtility(utility, playerName.ToString(), nirvanaType);
    }

    [ServerRpc(RequireOwnership = false)]
    public void SendHeatOffServerRpc(CardSO.Heat type, int idCard, FixedString64Bytes playerName) {
        for(int i = 0; i < numPlayers; i++) {
            if(players[i].playerName == playerName.ToString()) {
                players[i].market = PlayerCustom.Market.True;
                players[i].marketSpace.RemoveAt(1);
                players[i].heatOn = false;
                DropCardOnHassleServerRpc(playerName, idCard);
                activeSoundEffectHeatOffClientRpc(type);
            }
        }
    }

    public PlayerCustom.Heat switchEnumCardToPlayer(CardSO.Heat type) {
        Debug.Log("INPUT: " + type);
        PlayerCustom.Heat result = PlayerCustom.Heat.Null;
        String typeString = type.ToString();
        Debug.Log("type_string: " + typeString);
        Debug.Log("PlayerCustom.Heat.FELONY.ToString() " + PlayerCustom.Heat.FELONY.ToString());
        if(typeString == PlayerCustom.Heat.FELONY.ToString()) {
            result = PlayerCustom.Heat.FELONY;
        } else if (typeString == PlayerCustom.Heat.DETAINED.ToString()) {
            result = PlayerCustom.Heat.DETAINED;
        } else if (typeString == PlayerCustom.Heat.SS.ToString()) {
            result = PlayerCustom.Heat.SS;
        } else if (typeString == PlayerCustom.Heat.BUST.ToString()) {
            result = PlayerCustom.Heat.BUST;
        }
        Debug.Log("RESULT: " + result);
        return result;
    }

    public CardSO.Heat switchEnumPlayerToCard(PlayerCustom.Heat type) {
        CardSO.Heat result = CardSO.Heat.Null;
        String typeString = type.ToString();
        if(typeString == CardSO.Heat.FELONY.ToString()) {
            result = CardSO.Heat.FELONY;
        } else if (typeString == CardSO.Heat.DETAINED.ToString()) {
            result = CardSO.Heat.DETAINED;
        } else if (typeString == CardSO.Heat.SS.ToString()) {
            result = CardSO.Heat.SS;
        } else if (typeString == CardSO.Heat.BUST.ToString()) {
            result = CardSO.Heat.BUST;
        }
        return result;
    }

    [ServerRpc(RequireOwnership = false)]
    public void removeHeatOnServerRpc(int idCard, FixedString64Bytes playerName, int valore, CardSO.Effects effects, CardSO.Nirvana nirvanaType) {
        for(int i = 0; i < numPlayers; i++) {
            if(players[i].playerName == playerName.ToString()) {
                players[i].market = PlayerCustom.Market.True;
                players[i].marketSpace.RemoveAt(1);
                players[i].heatOn = false;
                switch(valore) {
                    case 5:
                        players[i].marketPlace_5K.RemoveAt(0);
                        players[i].cash = (Int32.Parse(players[i].cash) - 5000).ToString();
                        break;
                    case 25:
                        players[i].marketPlace_25K.RemoveAt(0);
                        players[i].cash = (Int32.Parse(players[i].cash) - 25000).ToString();
                        break;
                    case 50:
                        players[i].marketPlace_50K.RemoveAt(0);
                        players[i].cash = (Int32.Parse(players[i].cash) - 50000).ToString();
                        break;
                    case 100:
                        players[i].marketPlace_100k.RemoveAt(0);
                        players[i].cash = (Int32.Parse(players[i].cash) - 100000).ToString();
                        break;
                }
                DropCardOnHassleServerRpc(playerName, idCard);
                activeSoundEffectUtilityClientRpc(effects, playerName, nirvanaType);
            }
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void PlayNirvanaCardServerRpc(FixedString64Bytes playerName, int idCard, CardSO.Effects nirvana, CardSO.Nirvana nirvanaType) {
        for(int i = 0; i < numPlayers; i++) {
            if(players[i].playerName == playerName.ToString()) {
                if(players[i].market == PlayerCustom.Market.False) {
                    players[i].market = PlayerCustom.Market.True;
                    players[i].marketSpace.RemoveAt(1);
                    players[i].heatOn = false;
                }

                if(nirvanaType.ToString() == CardSO.Nirvana.STONEHIGH.ToString()) {
                    for(int j = 0; j < numPlayers; j++) {
                        if(players[j].playerName != players[i].playerName) {
                            if(players[j].marketPlace_5K.Count > 0) {
                                players[i].marketPlace_5K.Add(players[j].marketPlace_5K[0]);
                                players[j].marketPlace_5K.RemoveAt(0);

                                players[i].cash = (Int32.Parse(players[i].cash) + 5000).ToString();
                                players[j].cash = (Int32.Parse(players[j].cash) - 5000).ToString();

                            } else if(players[j].marketPlace_25K.Count > 0 && players[j].marketPlace_25K.Count > players[j].protection_25K.Count) {
                                players[i].marketPlace_25K.Add(players[j].marketPlace_25K[0]);
                                players[j].marketPlace_25K.RemoveAt(0);

                                players[i].cash = (Int32.Parse(players[i].cash) + 25000).ToString();
                                players[j].cash = (Int32.Parse(players[j].cash) - 25000).ToString();

                            } else if(players[j].marketPlace_50K.Count > 0 && players[j].marketPlace_50K.Count > players[j].protection_50K.Count) {
                                players[i].marketPlace_50K.Add(players[j].marketPlace_50K[0]);
                                players[j].marketPlace_50K.RemoveAt(0);

                                players[i].cash = (Int32.Parse(players[i].cash) + 50000).ToString();
                                players[j].cash = (Int32.Parse(players[j].cash) - 50000).ToString();

                            } else if(players[j].marketPlace_100k.Count > 0) {
                                players[i].marketPlace_100k.Add(players[j].marketPlace_100k[0]);
                                players[j].marketPlace_100k.RemoveAt(0);

                                players[i].cash = (Int32.Parse(players[i].cash) + 100000).ToString();
                                players[j].cash = (Int32.Parse(players[j].cash) - 100000).ToString();

                            }
                        }
                    }
                } else {
                    for(int j = 0; j < numPlayers; j++) {
                        if(players[j].playerName != players[i].playerName) {
                            if(players[j].marketPlace_100k.Count > 0) {
                                players[i].marketPlace_100k.Add(players[j].marketPlace_100k[0]);
                                players[j].marketPlace_100k.RemoveAt(0);

                                players[i].cash = (Int32.Parse(players[i].cash) + 100000).ToString();
                                players[j].cash = (Int32.Parse(players[j].cash) - 100000).ToString();

                            }  else if(players[j].marketPlace_50K.Count > 0 && players[j].marketPlace_50K.Count > players[j].protection_50K.Count) {
                                players[i].marketPlace_50K.Add(players[j].marketPlace_50K[0]);
                                players[j].marketPlace_50K.RemoveAt(0);
    
                                players[i].cash = (Int32.Parse(players[i].cash) + 50000).ToString();
                                players[j].cash = (Int32.Parse(players[j].cash) - 50000).ToString();
    
                            } else if(players[j].marketPlace_25K.Count > 0 && players[j].marketPlace_25K.Count > players[j].protection_25K.Count) {
                                players[i].marketPlace_25K.Add(players[j].marketPlace_25K[0]);
                                players[j].marketPlace_25K.RemoveAt(0);

                                players[i].cash = (Int32.Parse(players[i].cash) + 25000).ToString();
                                players[j].cash = (Int32.Parse(players[j].cash) - 25000).ToString();

                            } else if(players[j].marketPlace_5K.Count > 0) {
                                players[i].marketPlace_5K.Add(players[j].marketPlace_5K[0]);
                                players[j].marketPlace_5K.RemoveAt(0);

                                players[i].cash = (Int32.Parse(players[i].cash) + 5000).ToString();
                                players[j].cash = (Int32.Parse(players[j].cash) - 5000).ToString();

                            }
                        }
                    }
                }

                
                players[i].skipTurn = 1;
            }
        }
        DropCardOnHassleServerRpc(playerName, idCard);
        activeSoundEffectUtilityClientRpc(nirvana, playerName, nirvanaType);
    }

    public void visitPlayerClick(String playerName) {
        visitPlayer.SetActive(true);
        visitPlayer.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = playerName;
        loadPlayer(playerName);
    }

    public void loadPlayer(String playerName) {
        Debug.Log("Hand Destroy");
        for (int i = 0; i < Hand.transform.childCount; i++) {
            Hand.transform.GetChild(i).gameObject.SetActive(false);
        }

        Debug.Log("marketPlace_5kSpot Destroy");
        for (int i = 0; i < marketPlace_5kSpot.transform.childCount; i++) {
            marketPlace_5kSpot.transform.GetChild(i).gameObject.SetActive(false);
        }

        Debug.Log("marketPlace_25kSpot Destroy");
        for (int i = 0; i < marketPlace_25kSpot.transform.childCount; i++) {
            marketPlace_25kSpot.transform.GetChild(i).gameObject.SetActive(false);
        }

        Debug.Log("marketPlace_50kSpot Destroy");
        for (int i = 0; i < marketPlace_50kSpot.transform.childCount; i++) {
            marketPlace_50kSpot.transform.GetChild(i).gameObject.SetActive(false);
        }

        Debug.Log("marketPlace_100kSpot Destroy");
        for (int i = 0; i < marketPlace_100kSpot.transform.childCount; i++) {
            marketPlace_100kSpot.transform.GetChild(i).gameObject.SetActive(false);
        }

        Debug.Log("protection_25kSpot Destroy");
        for (int i = 0; i < protection_25kSpot.transform.childCount; i++) {
            protection_25kSpot.transform.GetChild(i).gameObject.SetActive(false);
        }

        Debug.Log("protection_50kSpot Destroy");
        for (int i = 0; i < protection_50kSpot.transform.childCount; i++) {
            protection_50kSpot.transform.GetChild(i).gameObject.SetActive(false);
        }

        Debug.Log("marketSpot Destroy");
        for (int i = 0; i < marketSpot.transform.childCount; i++) {
            marketSpot.transform.GetChild(i).gameObject.SetActive(false);
        }

        foreach(PlayerCustom player in players) {
            if(player.playerName.Split('#')[0] == playerName) {

                foreach(CardSO cardSO in player.marketPlace_5K) {
                    Debug.Log("marketPlace_5K: " + cardSO.cardName);
                    GameObject card = (GameObject)Instantiate(cardTemplate);
                    card.GetComponent<ShowCard>().cardData = cardSO;
                    card.GetComponent<ShowCard>().isSummoned = true;
                    card.transform.SetParent(marketPlace_5kSpot.transform, false);
                }

                foreach(CardSO cardSO in player.marketPlace_25K) {
                    Debug.Log("marketPlace_25K: " + cardSO.cardName);
                    GameObject card = (GameObject)Instantiate(cardTemplate);
                    card.GetComponent<ShowCard>().cardData = cardSO;
                    card.GetComponent<ShowCard>().isSummoned = true;
                    card.transform.SetParent(marketPlace_25kSpot.transform, false);
                }

                foreach(CardSO cardSO in player.marketPlace_50K) {
                    Debug.Log("marketPlace_50K: " + cardSO.cardName);
                    GameObject card = (GameObject)Instantiate(cardTemplate);
                    card.GetComponent<ShowCard>().cardData = cardSO;
                    card.GetComponent<ShowCard>().isSummoned = true;
                    card.transform.SetParent(marketPlace_50kSpot.transform, false);
                }

                foreach(CardSO cardSO in player.marketPlace_100k) {
                    Debug.Log("marketPlace_100k: " + cardSO.cardName);
                    GameObject card = (GameObject)Instantiate(cardTemplate);
                    card.GetComponent<ShowCard>().cardData = cardSO;
                    card.GetComponent<ShowCard>().isSummoned = true;
                    card.transform.SetParent(marketPlace_100kSpot.transform, false);
                }

                foreach(CardSO cardSO in player.protection_25K) {
                    Debug.Log("protection_25K: " + cardSO.cardName);
                    GameObject card = (GameObject)Instantiate(cardTemplate);
                    card.GetComponent<ShowCard>().cardData = cardSO;
                    card.GetComponent<ShowCard>().isSummoned = true;
                    card.transform.SetParent(protection_25kSpot.transform, false);
                }

                foreach(CardSO cardSO in player.protection_50K) {
                    Debug.Log("protection_50K: " + cardSO.cardName);
                    GameObject card = (GameObject)Instantiate(cardTemplate);
                    card.GetComponent<ShowCard>().cardData = cardSO;
                    card.GetComponent<ShowCard>().isSummoned = true;
                    card.transform.SetParent(protection_50kSpot.transform, false);
                }

                foreach(CardSO cardSO in player.marketSpace) {
                    Debug.Log("marketSpace: " + cardSO.cardName);
                    GameObject card = (GameObject)Instantiate(cardTemplate);
                    card.GetComponent<ShowCard>().cardData = cardSO;
                    card.GetComponent<ShowCard>().isSummoned = true;
                    card.transform.SetParent(marketSpot.transform, false);
                }
            }
        }
    }

    public void returnI() {
        visitPlayer.SetActive(false);
        showHand();
    }

    [ServerRpc(RequireOwnership = false)]
    public void playProtection25ServerRpc(FixedString64Bytes playerName, int idCard) {
        for(int i = 0; i < numPlayers; i++) {
            if(players[i].playerName == playerName.ToString()) {
                players[i].protection_25K.Add(allCard[idCard]);
                DropCardOnHassleServerRpc(playerName, idCard);
            }
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void playMarketCloseServerRpc(FixedString64Bytes playerName) {
        EndGameClientRpc(playerName);
    }

    [ServerRpc(RequireOwnership = false)]
    public void playProtection50ServerRpc(FixedString64Bytes playerName, int idCard) {
        for(int i = 0; i < numPlayers; i++) {
            if(players[i].playerName == playerName.ToString()) {
                players[i].protection_50K.Add(allCard[idCard]);
                DropCardOnHassleServerRpc(playerName, idCard);
            }
        }
    }


    [ClientRpc]
    public void EndGameClientRpc(FixedString64Bytes playerName) {
        EndGame.SetActive(true);

        bool banker = false;
        String bankerName = "";
        int bankerValue = 1;
        int maledizioniValue = 0;
        int highestCard = 0;

        List<Classifica> giocatori = new List<Classifica>();

        for(int i = 0; i < numPlayers; i++) {
            
            if(i == 0) {
                endGameNick1.GetComponent<Text>().text = players[i].playerName.Split('#')[0];

                endGameProfittoProtetto1.GetComponent<Text>().text = ((players[i].protection_25K.Count * 25000) + (players[i].protection_50K.Count * 50000)).ToString() + "$";
                endGameProfittoRischio1.GetComponent<Text>().text = (Int32.Parse(players[i].cash) - (players[i].protection_25K.Count * 25000) + (players[i].protection_50K.Count * 50000)).ToString()+ "$";
        
                foreach(CardSO item in players[i].Hand) {
                    if(item.effects == CardSO.Effects.MALEDIZIONI) {
                        switch(item.maledizioni) {
                            case CardSO.Maledizioni.SOLD_OUT:
                            maledizioniValue = maledizioniValue + 25000;
                                break;
                            case CardSO.Maledizioni.DOUBLECROSSED:
                                maledizioniValue = maledizioniValue + 50000;
                                break;
                            case CardSO.Maledizioni.UTTERLY_WIPED_OUT:
                                maledizioniValue = maledizioniValue + 100000;
                                break;
                        }
                    } else if(item.effects == CardSO.Effects.BANKER) {
                        bankerName = players[i].playerName;
                        banker = true;
                    } else if (item.effects == CardSO.Effects.SOLDI) {
                        switch(item.cash) {
                            case CardSO.Cash.K5:
                                if(highestCard < 5000) {
                                    highestCard = 5000;
                                }
                                break;
                            case CardSO.Cash.K25:
                                if(highestCard < 25000) {
                                    highestCard = 25000;
                                }
                                break;
                            case CardSO.Cash.K50:
                                if(highestCard < 50000) {
                                    highestCard = 50000;
                                }
                                break;
                            case CardSO.Cash.K100:
                                if(highestCard < 100000) {
                                    highestCard = 1000000;
                                }
                                break;    
                        }
                    }
                }

                endGameMulte1.GetComponent<Text>().color = Color.red;
                endGameMulte1.GetComponent<Text>().text = "-" + maledizioniValue + "$";

                endGameCartaAlta1.GetComponent<Text>().color = Color.red;
                endGameCartaAlta1.GetComponent<Text>().text = "-" + highestCard + "$";

                if(banker) {
                    if(players[i].playerName != bankerName) {
                        if(Int32.Parse(endGameProfittoRischio1.GetComponent<Text>().text.Replace("$", "")) >= 5000) {
                            bankerValue++;
                        }
                    }
                }

                if(players[i].playerName == playerName.ToString()) {
                    endGameBonus1.GetComponent<Text>().text = "+" + 25000 + "$";
                } else {
                    endGameBonus1.GetComponent<Text>().text = "0$";
                }

                maledizioniValue = 0;
                highestCard = 0;
            } else if(i == 1) {
                endGameNick2.GetComponent<Text>().text = players[i].playerName.Split('#')[0];

                endGameProfittoProtetto2.GetComponent<Text>().text = ((players[i].protection_25K.Count * 25000) + (players[i].protection_50K.Count * 50000)).ToString() + "$";
                endGameProfittoRischio2.GetComponent<Text>().text = (Int32.Parse(players[i].cash) - (players[i].protection_25K.Count * 25000) + (players[i].protection_50K.Count * 50000)).ToString()+ "$";
                
                foreach(CardSO item in players[i].Hand) {
                    if(item.effects == CardSO.Effects.MALEDIZIONI) {
                        switch(item.maledizioni) {
                            case CardSO.Maledizioni.SOLD_OUT:
                            maledizioniValue = maledizioniValue + 25000;
                                break;
                            case CardSO.Maledizioni.DOUBLECROSSED:
                                maledizioniValue = maledizioniValue + 50000;
                                break;
                            case CardSO.Maledizioni.UTTERLY_WIPED_OUT:
                                maledizioniValue = maledizioniValue + 100000;
                                break;
                        }
                    } else if(item.effects == CardSO.Effects.BANKER) {
                        bankerName = players[i].playerName;
                        banker = true;
                    } else if (item.effects == CardSO.Effects.SOLDI) {
                        switch(item.cash) {
                            case CardSO.Cash.K5:
                                if(highestCard < 5000) {
                                    highestCard = 5000;
                                }
                                break;
                            case CardSO.Cash.K25:
                                if(highestCard < 25000) {
                                    highestCard = 25000;
                                }
                                break;
                            case CardSO.Cash.K50:
                                if(highestCard < 50000) {
                                    highestCard = 50000;
                                }
                                break;
                            case CardSO.Cash.K100:
                                if(highestCard < 100000) {
                                    highestCard = 1000000;
                                }
                                break;    
                        }
                    }
                }

                endGameMulte2.GetComponent<Text>().color = Color.red;
                endGameMulte2.GetComponent<Text>().text = "-" + maledizioniValue + "$";

                endGameCartaAlta2.GetComponent<Text>().color = Color.red;
                endGameCartaAlta2.GetComponent<Text>().text = "-" + highestCard + "$";

                if(banker) {
                    if(players[i].playerName != bankerName) {
                        if(Int32.Parse(endGameProfittoRischio2.GetComponent<Text>().text.Replace("$", "")) >= 5000) {
                            bankerValue++;
                        }
                    }
                }

                if(players[i].playerName == playerName.ToString()) {
                    endGameBonus2.GetComponent<Text>().text = "+" + 25000 + "$";
                } else {
                    endGameBonus2.GetComponent<Text>().text = "0$";
                }

                maledizioniValue = 0;
                highestCard = 0;
            } else if(i == 2) {
                endGameNick3.GetComponent<Text>().text = players[i].playerName.Split('#')[0];

                endGameProfittoProtetto3.GetComponent<Text>().text = ((players[i].protection_25K.Count * 25000) + (players[i].protection_50K.Count * 50000)).ToString() + "$";
                endGameProfittoRischio3.GetComponent<Text>().text = (Int32.Parse(players[i].cash) - (players[i].protection_25K.Count * 25000) + (players[i].protection_50K.Count * 50000)).ToString()+ "$";
                

                foreach(CardSO item in players[i].Hand) {
                    if(item.effects == CardSO.Effects.MALEDIZIONI) {
                        switch(item.maledizioni) {
                            case CardSO.Maledizioni.SOLD_OUT:
                            maledizioniValue = maledizioniValue + 25000;
                                break;
                            case CardSO.Maledizioni.DOUBLECROSSED:
                                maledizioniValue = maledizioniValue + 50000;
                                break;
                            case CardSO.Maledizioni.UTTERLY_WIPED_OUT:
                                maledizioniValue = maledizioniValue + 100000;
                                break;
                        }
                    } else if(item.effects == CardSO.Effects.BANKER) {
                        bankerName = players[i].playerName;
                        banker = true;
                    } else if (item.effects == CardSO.Effects.SOLDI) {
                        switch(item.cash) {
                            case CardSO.Cash.K5:
                                if(highestCard < 5000) {
                                    highestCard = 5000;
                                }
                                break;
                            case CardSO.Cash.K25:
                                if(highestCard < 25000) {
                                    highestCard = 25000;
                                }
                                break;
                            case CardSO.Cash.K50:
                                if(highestCard < 50000) {
                                    highestCard = 50000;
                                }
                                break;
                            case CardSO.Cash.K100:
                                if(highestCard < 100000) {
                                    highestCard = 1000000;
                                }
                                break;    
                        }
                    }
                }

                endGameMulte3.GetComponent<Text>().color = Color.red;
                endGameMulte3.GetComponent<Text>().text = "-" + maledizioniValue + "$";

                endGameCartaAlta3.GetComponent<Text>().color = Color.red;
                endGameCartaAlta3.GetComponent<Text>().text = "-" + highestCard + "$";

                if(banker) {
                    if(players[i].playerName != bankerName) {
                        if(Int32.Parse(endGameProfittoRischio3.GetComponent<Text>().text.Replace("$", "")) >= 5000) {
                            bankerValue++;
                        }
                    }
                }

                if(players[i].playerName == playerName.ToString()) {
                    endGameBonus3.GetComponent<Text>().text = "+" + 25000 + "$";
                } else {
                    endGameBonus3.GetComponent<Text>().text = "0$";
                }

                maledizioniValue = 0;
                highestCard = 0;
            } else if(i == 3) {
                endGameNick4.GetComponent<Text>().text = players[i].playerName.Split('#')[0];

                endGameProfittoProtetto4.GetComponent<Text>().text = ((players[i].protection_25K.Count * 25000) + (players[i].protection_50K.Count * 50000)).ToString() + "$";
                endGameProfittoRischio4.GetComponent<Text>().text = (Int32.Parse(players[i].cash) - (players[i].protection_25K.Count * 25000) + (players[i].protection_50K.Count * 50000)).ToString() + "$";
                
                foreach(CardSO item in players[i].Hand) {
                    if(item.effects == CardSO.Effects.MALEDIZIONI) {
                        switch(item.maledizioni) {
                            case CardSO.Maledizioni.SOLD_OUT:
                            maledizioniValue = maledizioniValue + 25000;
                                break;
                            case CardSO.Maledizioni.DOUBLECROSSED:
                                maledizioniValue = maledizioniValue + 50000;
                                break;
                            case CardSO.Maledizioni.UTTERLY_WIPED_OUT:
                                maledizioniValue = maledizioniValue + 100000;
                                break;
                        }
                    } else if(item.effects == CardSO.Effects.BANKER) {
                        bankerName = players[i].playerName;
                        banker = true;
                    } else if (item.effects == CardSO.Effects.SOLDI) {
                        switch(item.cash) {
                            case CardSO.Cash.K5:
                                if(highestCard < 5000) {
                                    highestCard = 5000;
                                }
                                break;
                            case CardSO.Cash.K25:
                                if(highestCard < 25000) {
                                    highestCard = 25000;
                                }
                                break;
                            case CardSO.Cash.K50:
                                if(highestCard < 50000) {
                                    highestCard = 50000;
                                }
                                break;
                            case CardSO.Cash.K100:
                                if(highestCard < 100000) {
                                    highestCard = 1000000;
                                }
                                break;    
                        }
                    }
                }

                endGameMulte4.GetComponent<Text>().color = Color.red;
                endGameMulte4.GetComponent<Text>().text = "-" + maledizioniValue + "$";

                endGameCartaAlta4.GetComponent<Text>().color = Color.red;
                endGameCartaAlta4.GetComponent<Text>().text = "-" + highestCard + "$";

                if(banker) {
                    if(players[i].playerName != bankerName) {
                        if(Int32.Parse(endGameProfittoRischio4.GetComponent<Text>().text.Replace("$", "")) >= 5000) {
                            bankerValue++;
                        }
                    }
                }

                if(players[i].playerName == playerName.ToString()) {
                    endGameBonus4.GetComponent<Text>().text = "+" + 25000 + "$";
                } else {
                    endGameBonus4.GetComponent<Text>().text = "0$";
                }

                maledizioniValue = 0;
                highestCard = 0;
            }
            
            
        }

        for(int i = 0; i < numPlayers; i++) {
            if(i == 0) {
                if(banker) {
                    if(players[i].playerName == bankerName) {
                        endGameBanker1.GetComponent<Text>().text = "+" + bankerValue * 5000 + "$";
                    } else {
                        if(Int32.Parse(endGameProfittoRischio1.GetComponent<Text>().text) >= 5000) {
                            endGameBanker1.GetComponent<Text>().color = Color.red;
                            endGameBanker1.GetComponent<Text>().text = "-5000$";
                        } else {
                            endGameBanker1.GetComponent<Text>().text = "0$";
                        }
                    }
                } else {
                    endGameBanker1.GetComponent<Text>().text = "0$";
                }

                endGameNetto1.GetComponent<Text>().text = (Int32.Parse(endGameProfittoProtetto1.GetComponent<Text>().text.Replace("$", "")) + Int32.Parse(endGameProfittoRischio1.GetComponent<Text>().text.Replace("$", "")) + Int32.Parse(endGameBanker1.GetComponent<Text>().text.Replace("$", "")) + Int32.Parse(endGameMulte1.GetComponent<Text>().text.Replace("$", "")) + Int32.Parse(endGameCartaAlta1.GetComponent<Text>().text.Replace("$", ""))).ToString()  + "$";
                endGameTotale1.GetComponent<Text>().text = (Int32.Parse(endGameNetto1.GetComponent<Text>().text.Replace("$", "")) + Int32.Parse(endGameBonus1.GetComponent<Text>().text.Replace("$", ""))).ToString() + "$";
                giocatori.Add(new Classifica(1, Int32.Parse(endGameTotale1.GetComponent<Text>().text.Replace("$", ""))));
            } else if (i == 1) {
                if(banker) {
                    if(players[i].playerName == bankerName) {
                        endGameBanker2.GetComponent<Text>().text = "+" + bankerValue * 5000 + "$";
                    } else {
                        if(Int32.Parse(endGameProfittoRischio2.GetComponent<Text>().text) >= 5000) {
                            endGameBanker2.GetComponent<Text>().color = Color.red;
                            endGameBanker2.GetComponent<Text>().text = "-5000$";
                        } else {
                            endGameBanker2.GetComponent<Text>().text = "0$";
                        }
                    }
                } else {
                    endGameBanker2.GetComponent<Text>().text = "0$";
                }

                endGameNetto2.GetComponent<Text>().text = (Int32.Parse(endGameProfittoProtetto2.GetComponent<Text>().text.Replace("$", "")) + Int32.Parse(endGameProfittoRischio2.GetComponent<Text>().text.Replace("$", "")) + Int32.Parse(endGameBanker2.GetComponent<Text>().text.Replace("$", "")) + Int32.Parse(endGameMulte2.GetComponent<Text>().text.Replace("$", "")) + Int32.Parse(endGameCartaAlta2.GetComponent<Text>().text.Replace("$", ""))).ToString() + "$";
                endGameTotale2.GetComponent<Text>().text = (Int32.Parse(endGameNetto2.GetComponent<Text>().text.Replace("$", "")) + Int32.Parse(endGameBonus2.GetComponent<Text>().text.Replace("$", ""))).ToString() + "$";
                giocatori.Add(new Classifica(2, Int32.Parse(endGameTotale2.GetComponent<Text>().text.Replace("$", ""))));
            } else if (i == 2) {
                if(banker) {
                    if(players[i].playerName == bankerName) {
                        endGameBanker3.GetComponent<Text>().text = "+" + bankerValue * 5000 + "$";
                    } else {
                        if(Int32.Parse(endGameProfittoRischio3.GetComponent<Text>().text) >= 5000) {
                            endGameBanker3.GetComponent<Text>().color = Color.red;
                            endGameBanker3.GetComponent<Text>().text = "-5000$";
                        } else {
                            endGameBanker3.GetComponent<Text>().text = "0$";
                        }
                    }
                } else {
                    endGameBanker3.GetComponent<Text>().text = "0$";
                }

                endGameNetto3.GetComponent<Text>().text = (Int32.Parse(endGameProfittoProtetto3.GetComponent<Text>().text.Replace("$", "")) + Int32.Parse(endGameProfittoRischio3.GetComponent<Text>().text.Replace("$", "")) + Int32.Parse(endGameBanker3.GetComponent<Text>().text.Replace("$", "")) + Int32.Parse(endGameMulte3.GetComponent<Text>().text.Replace("$", "")) + Int32.Parse(endGameCartaAlta3.GetComponent<Text>().text.Replace("$", ""))).ToString() + "$";
                endGameTotale3.GetComponent<Text>().text = (Int32.Parse(endGameNetto3.GetComponent<Text>().text.Replace("$", "")) + Int32.Parse(endGameBonus3.GetComponent<Text>().text.Replace("$", ""))).ToString() + "$";
                giocatori.Add(new Classifica(3, Int32.Parse(endGameTotale3.GetComponent<Text>().text.Replace("$", ""))));
            } else if (i == 3) {
                if(banker) {
                    if(players[i].playerName == bankerName) {
                        endGameBanker4.GetComponent<Text>().text = "+" + bankerValue * 5000 + "$";
                    } else {
                        if(Int32.Parse(endGameProfittoRischio4.GetComponent<Text>().text) >= 5000) {
                            endGameBanker4.GetComponent<Text>().color = Color.red;
                            endGameBanker4.GetComponent<Text>().text = "-5000$";
                        } else {
                            endGameBanker4.GetComponent<Text>().text = "0$";
                        }
                    }
                } else {
                    endGameBanker4.GetComponent<Text>().text = "0$";
                }

                endGameNetto4.GetComponent<Text>().text = (Int32.Parse(endGameProfittoProtetto4.GetComponent<Text>().text.Replace("$", "")) + Int32.Parse(endGameProfittoRischio4.GetComponent<Text>().text.Replace("$", "")) + Int32.Parse(endGameBanker4.GetComponent<Text>().text.Replace("$", "")) + Int32.Parse(endGameMulte4.GetComponent<Text>().text.Replace("$", "")) + Int32.Parse(endGameCartaAlta4.GetComponent<Text>().text.Replace("$", ""))).ToString() + "$";
                endGameTotale4.GetComponent<Text>().text = (Int32.Parse(endGameNetto4.GetComponent<Text>().text.Replace("$", "")) + Int32.Parse(endGameBonus4.GetComponent<Text>().text.Replace("$", ""))).ToString() + "$";
                giocatori.Add(new Classifica(4, Int32.Parse(endGameTotale4.GetComponent<Text>().text.Replace("$", ""))));
            }
        }

        giocatori = giocatori.OrderByDescending(g => g.Punteggio).ToList();

        Sprite nowSPrite = medalGold;

        int posizione = 1;
        foreach (var giocatore in giocatori)
        {

            if(posizione == 1) {
                nowSPrite = medalGold;
            } else if(posizione == 2) {
                nowSPrite = medalSilver;
            } else if(posizione == 3) {
                nowSPrite = medalBronze;
            } else if(posizione == 4) {
                break;
            }

            if(giocatore.ID == 1) {
                endGameMedal1.SetActive(true);
                endGameMedal1.GetComponent<Image>().sprite = nowSPrite;
                posizione++;
            } else if(giocatore.ID == 2) {
                endGameMedal2.SetActive(true);
                endGameMedal2.GetComponent<Image>().sprite = nowSPrite;
                posizione++;
            } else if(giocatore.ID == 3) {
                endGameMedal3.SetActive(true);
                endGameMedal3.GetComponent<Image>().sprite = nowSPrite;
                posizione++;
            } else if(giocatore.ID == 4) {
                endGameMedal4.SetActive(true);
                endGameMedal4.GetComponent<Image>().sprite = nowSPrite;
                posizione++;
            }
        }

    }

    public void activePayfine(int idCard, FixedString64Bytes playerEnemy, FixedString64Bytes playerPlay) {
        maledizioniBanner.SetActive(true);
        backStealCard.SetActive(true);
        textMaledizioniAndSteal.GetComponentInChildren<Text>().text = "Select card to steal";
        int numToShow = 0;
        for(int i = 0; i < numPlayers; i++) {
            if(players[i].playerName.Split('#')[0] == playerEnemy) {
                foreach(CardSO cardSO in players[i].marketPlace_5K) {
                    GameObject card = (GameObject)Instantiate(cardTemplate);
                    card.GetComponent<ShowCard>().cardData = cardSO;
                    card.GetComponent<ShowCard>().steal = true;
                    card.GetComponent<ShowCard>().playerOwner = playerEnemy.ToString();
                    card.GetComponent<ShowCard>().playCard = idCard;
                    card.transform.SetParent(cardsMaledizioniSlot.transform, false);
                }
                foreach(CardSO cardSO in players[i].marketPlace_25K) {
                    numToShow =players[i].marketPlace_25K.Count - players[i].protection_25K.Count;
                    if(numToShow > 0) {
                        GameObject card = (GameObject)Instantiate(cardTemplate);
                        card.GetComponent<ShowCard>().cardData = cardSO;
                        card.GetComponent<ShowCard>().steal = true;
                        card.GetComponent<ShowCard>().playerOwner = playerEnemy.ToString();
                        card.GetComponent<ShowCard>().playCard = idCard;
                        card.transform.SetParent(cardsMaledizioniSlot.transform, false);
                        if(cardsMaledizioniSlot.transform.childCount == numToShow) {
                            break;
                        }
                    }
                }
                foreach(CardSO cardSO in players[i].marketPlace_50K) {
                    numToShow = players[i].marketPlace_50K.Count - players[i].protection_50K.Count;
                    if(numToShow > 0) {
                        GameObject card = (GameObject)Instantiate(cardTemplate);
                        card.GetComponent<ShowCard>().cardData = cardSO;
                        card.GetComponent<ShowCard>().steal = true;
                        card.GetComponent<ShowCard>().playerOwner = playerEnemy.ToString();
                        card.GetComponent<ShowCard>().playCard = idCard;
                        card.transform.SetParent(cardsMaledizioniSlot.transform, false);
                        if(cardsMaledizioniSlot.transform.childCount == numToShow) {
                            break;
                        }
                    }
                }
                foreach(CardSO cardSO in players[i].marketPlace_100k) {
                    GameObject card = (GameObject)Instantiate(cardTemplate);
                    card.GetComponent<ShowCard>().cardData = cardSO;
                    card.GetComponent<ShowCard>().steal = true;
                    card.GetComponent<ShowCard>().playerOwner = playerEnemy.ToString();
                    card.GetComponent<ShowCard>().playCard = idCard;
                    card.transform.SetParent(cardsMaledizioniSlot.transform, false);
                }
            }
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void stealPassCardServerRpc(int idCard, FixedString64Bytes playerName, int playCard) {
        for(int i = 0; i < numPlayers; i++) { 
            if(players[i].myTurn) {
                DropCardOnHassleServerRpc(players[i].playerName, playCard);
                switch(allCard[idCard].cash) {
                    case CardSO.Cash.K5:
                        players[i].marketPlace_5K.Add(allCard[idCard]);
                        players[i].cash = (Int32.Parse(players[i].cash) + 5000).ToString();
                        break;
                    case CardSO.Cash.K25:
                        players[i].marketPlace_25K.Add(allCard[idCard]);
                        players[i].cash = (Int32.Parse(players[i].cash) + 25000).ToString();
                        break;
                    case CardSO.Cash.K50:
                        players[i].marketPlace_50K.Add(allCard[idCard]);
                        players[i].cash = (Int32.Parse(players[i].cash) + 50000).ToString();
                        break;
                    case CardSO.Cash.K100:
                        players[i].marketPlace_100k.Add(allCard[idCard]);
                        players[i].cash = (Int32.Parse(players[i].cash) + 100000).ToString();
                        break;
                }
            }
            if(players[i].playerName.Split('#')[0] == playerName.ToString()) {
                switch(allCard[idCard].cash) {
                    case CardSO.Cash.K5:
                        players[i].marketPlace_5K.RemoveAt(0);
                        players[i].cash = (Int32.Parse(players[i].cash) - 5000).ToString();
                        break;
                    case CardSO.Cash.K25:
                        players[i].marketPlace_25K.RemoveAt(0);
                        players[i].cash = (Int32.Parse(players[i].cash) - 25000).ToString();
                        break;
                    case CardSO.Cash.K50:
                        players[i].marketPlace_50K.RemoveAt(0);
                        players[i].cash = (Int32.Parse(players[i].cash) - 50000).ToString();
                        break;
                    case CardSO.Cash.K100:
                        players[i].marketPlace_100k.RemoveAt(0);
                        players[i].cash = (Int32.Parse(players[i].cash) - 100000).ToString();
                        break;
                }
            }
        }
        activeSoundEffectUtilityClientRpc(CardSO.Effects.STEAL, playerName, CardSO.Nirvana.Null);
        resumeGameFromMalClientRpc(playerName, 7);
    }

    public void annullaSteal() {
        maledizioniBanner.SetActive(false);
        backStealCard.SetActive(false);
        Debug.Log("maledizione and steal Destroy");
        for (int i = 0; i < cardsMaledizioniSlot.transform.childCount; i++) {
            cardsMaledizioniSlot.transform.GetChild(i).gameObject.SetActive(false);
        }
        StartCoroutine(SleepMoveCardTOBin());
    }

    [ClientRpc]
    public void clear5kClientRpc(int playerId) {
        if(!NetworkManager.Singleton.IsHost) {
            players[playerId].marketPlace_5K = new List<CardSO>();
        }
    }

    [ClientRpc]
    public void clear25kClientRpc(int playerId) {
        if(!NetworkManager.Singleton.IsHost) {
            players[playerId].marketPlace_25K = new List<CardSO>();
        }
    }

    [ClientRpc]
    public void clear50kClientRpc(int playerId) {
        if(!NetworkManager.Singleton.IsHost) {
            players[playerId].marketPlace_50K = new List<CardSO>();
        }
    }

    [ClientRpc]
    public void clear100kClientRpc(int playerId) {
        if(!NetworkManager.Singleton.IsHost) {
            players[playerId].marketPlace_100k = new List<CardSO>();
        }
    }

    [ClientRpc]
    public void clearProtection25kClientRpc(int playerId) {
        if(!NetworkManager.Singleton.IsHost) {
            players[playerId].protection_25K = new List<CardSO>();
        }
    }

    [ClientRpc]
    public void clearProtection50kClientRpc(int playerId) {
        if(!NetworkManager.Singleton.IsHost) {
            players[playerId].protection_50K = new List<CardSO>();
        }
    }
}   


static class MyExtensions
  {
    public static void Shuffle<T>(this IList<T> list)
    {
      int n = list.Count;
      while (n > 1)
      {
        n--;
        int k = ThreadSafeRandom.ThisThreadsRandom.Next(n + 1);
        T value = list[k];
        list[k] = list[n];
        list[n] = value;
      }
    }
  }

  public static class ThreadSafeRandom
  {
      [ThreadStatic] private static System.Random Local;

      public static System.Random ThisThreadsRandom
      {
          get { return Local ?? (Local = new System.Random(unchecked(Environment.TickCount * 31 + Thread.CurrentThread.ManagedThreadId))); }
      }
  }

  public class Classifica
{
    public int ID { get; set; }
    public int Punteggio { get; set; }
    
    public Classifica(int id, int punteggio)
    {
        ID = id;
        Punteggio = punteggio;
    }
}
