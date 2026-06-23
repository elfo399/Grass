using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;
using Object = UnityEngine.Object;
using Unity.Services.Authentication;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking;
using Unity.Networking.Transport;
using Unity.Networking.Transport.Relay;

public class MenuManager : MonoBehaviour {
    
    public static MenuManager Instance;
    public GameObject  objBackground;
    public GameObject  options;
    public GameObject  start;
    public GameObject  info;
    public GameObject  rules;
    public GameObject  host;
    public GameObject  join;
    public GameObject  nickname;
    public GameObject lobby;
    public GameObject  game;
    public GameObject preGame;
    public GameObject endGame;
    public GameObject Tutorial;
    public GameObject selectedCardTutorial;
    public GameObject CardTutorial;
    public Sprite background;
    public Sprite preBackground;
    public Sprite gameBackground;
    [SerializeField] public VideoClip startClip;
    [SerializeField] public VideoClip preClip;
    [SerializeField] public VideoClip postClip;
    [SerializeField] public VideoClip completeClip;
    [SerializeField] public VideoPlayer videoPlayer;
    public static String WHERE = null;

    public Sprite marketOpenTutorialSprite;
    public Sprite marketCloseTutorialSprite;
    public Sprite peddleTutorialSprite;
    public Sprite protectionTutorialSprite;
    public Sprite maledizioniTutorialSprite;
    public Sprite nirvanaTutorialSprite;
    public Sprite stealTutorialSPrite;
    public Sprite payFineTutorailSprite;
    public Sprite HeatOnTutorialSprite;
    public Sprite HeatOffTutorialSprite;
    public Sprite bankerTutorialSprite;

    public Sprite marketOpenTutorialDescriptionSprite;
    public Sprite marketCloseTutorialDescriptionSprite;
    public Sprite peddleTutorialDescriptionSprite;
    public Sprite protectionTutorialDescriptionSprite;
    public Sprite maledizioniTutorialDescriptionSprite;
    public Sprite nirvanaTutorialDescriptionSprite;
    public Sprite stealTutorialDescriptionSPrite;
    public Sprite payFineTutorailDescriptionSprite;
    public Sprite HeatOnTutorialDescriptionSprite;
    public Sprite HeatOffTutorialDescriptionSprite;
    public Sprite bankerTutorialDescriptionSprite;

    private void Awake()
    {
        Instance = this;
        options.SetActive(false);
        start.SetActive(false);
        info.SetActive(false);
        rules.SetActive(false);
        join.SetActive(false);
        host.SetActive(false);
        nickname.SetActive(false);
        lobby.SetActive(false);
        preGame.SetActive(false);
        game.SetActive(false);
        endGame.SetActive(false);
        Tutorial.SetActive(false);
        selectedCardTutorial.SetActive(false);
        CardTutorial.SetActive(false);  
        objBackground.GetComponent<Image>().sprite = preBackground;
        videoPlayer.clip = preClip;
        videoPlayer.Play();
        videoPlayer.loopPointReached += startup;

        Application.targetFrameRate = 60;
        QualitySettings.vSyncCount = 0;
    }
    
    void startup(UnityEngine.Video.VideoPlayer vp)
    {
        objBackground.GetComponent<Image>().sprite = background;
        start.SetActive(true);
        videoPlayer.clip = postClip;    
        videoPlayer.loopPointReached -= startup;
        videoPlayer.Play();
    }

    public void preTransition(String where)
    {
        WHERE = where;
        videoPlayer.clip = preClip;
        videoPlayer.Play();
        videoPlayer.loopPointReached += EndReached;
        
    }

    public void postTransition()
    {
        videoPlayer.clip = postClip;
        videoPlayer.loopPointReached -= EndReached;
        videoPlayer.Play();
    }
    
    public void QuitGame()
    {
        videoPlayer.clip = preClip;
        videoPlayer.Play();
        videoPlayer.loopPointReached += QuitApp;
        
    }

    void QuitApp(UnityEngine.Video.VideoPlayer vp)
    {
        Application.Quit();
    }
    
    void EndReached(UnityEngine.Video.VideoPlayer vp)
    {
        switch(WHERE) {
            case "OPTIONS":
                activeOptions();
                break;
            case "START":
                activeStart();
                break;
            case "RULES":
                activeRules();
                break;
            case "HOST":
                activeHost();
                break;
            case "JOIN":
                activeJoin();
                break;
            case "LOBBY":
                activeLobby();
                break;
            case "GAME":
                activeGame();
                break;
            case "GAMECLIENT":
                activeGameClient();
                break;
            case "TUTORIAL":
                activeSelectedCardTutorial();
                break;

            case "PEDDLE":
                activePeddleTutorial();
                break;
            case "PROTECTION":
                activeProtectionTutorial();
                break;
            case "MARKETOPEN":
                activeMarketOpenTutorial();
                break;
            case "MARKETCLOSE":
                activeMarketCloseTutorial();
                break;
            case "HEATON":
                activeHeatOnTutorial();
                break;
            case "HEATOFF":
                activeHeatOffTutorial();
                break;
            case "PAYFINE":
                activePayFineTutorial();
                break;
            case "STEAL":
                activeStealTutorial();
                break;
            case "MALEDIZIONI":
                activeMaledizioniTutorial();
                break;
            case "NIRVANA":
                activeNirvanaTutorial();
                break;
            case "BANKER":
                activeBankerTutorial();
                break;
        }
        postTransition();
    }

    public void activeOptions()
    {
        options.SetActive(true);
        start.SetActive(false);
        info.SetActive(false);
        rules.SetActive(false);
        join.SetActive(false);
        host.SetActive(false);
        nickname.SetActive(false);
        lobby.SetActive(false);
        preGame.SetActive(false);
        game.SetActive(false);
        endGame.SetActive(false);
        Tutorial.SetActive(false);
        selectedCardTutorial.SetActive(false);
        CardTutorial.SetActive(false);  
    }
    
    public void activeStart()
    {
        options.SetActive(false);
        start.SetActive(true);
        info.SetActive(false);
        rules.SetActive(false);
        join.SetActive(false);
        host.SetActive(false);
        nickname.SetActive(false);
        lobby.SetActive(false);
        preGame.SetActive(false);
        game.SetActive(false);
        endGame.SetActive(false);
        Tutorial.SetActive(false);
        selectedCardTutorial.SetActive(false);
        CardTutorial.SetActive(false);  
        objBackground.GetComponent<Image>().sprite = background; 
    }
    
    public void activeInfo()
    {
        options.SetActive(false);
        start.SetActive(false);
        info.SetActive(true);
        rules.SetActive(false);
        join.SetActive(false);
        host.SetActive(false);
        nickname.SetActive(false);
        lobby.SetActive(false);
        preGame.SetActive(false);
        game.SetActive(false);
        endGame.SetActive(false);
        Tutorial.SetActive(false);
        selectedCardTutorial.SetActive(false);
        CardTutorial.SetActive(false);  
    }
    
    public void activeRules()
    {
        options.SetActive(false);
        start.SetActive(false);
        info.SetActive(false);
        rules.SetActive(true);
        join.SetActive(false);
        host.SetActive(false);
        nickname.SetActive(false);
        lobby.SetActive(false);
        preGame.SetActive(false);
        game.SetActive(false);
        endGame.SetActive(false);
        Tutorial.SetActive(false);
        selectedCardTutorial.SetActive(false);
        CardTutorial.SetActive(false);  
    }
    
    public void activeHost()
    {
        options.SetActive(false);
        start.SetActive(false);
        info.SetActive(false);
        rules.SetActive(false);
        join.SetActive(false);
        host.SetActive(true);
        lobby.SetActive(false);
        preGame.SetActive(false);
        game.SetActive(false);
        endGame.SetActive(false);
        Tutorial.SetActive(false);
        selectedCardTutorial.SetActive(false);
        CardTutorial.SetActive(false);  
        if (AuthenticationService.Instance.PlayerName == null)
        {
            nickname.SetActive(true);
        }
    }
    
    public void activeJoin()
    {
        LobbyManager.instance.queryLobby();
        options.SetActive(false);
        start.SetActive(false);
        info.SetActive(false);
        rules.SetActive(false);
        join.SetActive(true);
        host.SetActive(false);
        lobby.SetActive(false);
        preGame.SetActive(false);
        game.SetActive(false);
        endGame.SetActive(false);
        Tutorial.SetActive(false);
        selectedCardTutorial.SetActive(false);
        CardTutorial.SetActive(false);  
        if (AuthenticationService.Instance.PlayerName == null)
        {
            nickname.SetActive(true);
        }
    }
    
    public void activeLobby()
    {
        options.SetActive(false);
        start.SetActive(false);
        info.SetActive(false);
        rules.SetActive(false);
        join.SetActive(false);
        host.SetActive(false);
        lobby.SetActive(true);
        nickname.SetActive(false);
        preGame.SetActive(false);
        game.SetActive(false);
        endGame.SetActive(false);
        Tutorial.SetActive(false);
        selectedCardTutorial.SetActive(false);
        CardTutorial.SetActive(false);  
        LobbyManager.instance.initializedLobby();
    }

    public void activeGame() {
        options.SetActive(false);
        start.SetActive(false);
        info.SetActive(false);
        rules.SetActive(false);
        join.SetActive(false);
        host.SetActive(false);
        lobby.SetActive(false);
        nickname.SetActive(false);
        preGame.SetActive(true);
        game.SetActive(true);
        endGame.SetActive(false);
        Tutorial.SetActive(false);
        selectedCardTutorial.SetActive(false);
        CardTutorial.SetActive(false);  
        objBackground.GetComponent<Image>().sprite = gameBackground; 
         
    }

    public void activeGameClient() {
        options.SetActive(false);
        start.SetActive(false);
        info.SetActive(false);
        rules.SetActive(false);
        join.SetActive(false);
        host.SetActive(false);
        lobby.SetActive(false);
        nickname.SetActive(false);
        preGame.SetActive(true);
        game.SetActive(true);
        endGame.SetActive(false);
        Tutorial.SetActive(false);
        selectedCardTutorial.SetActive(false);
        CardTutorial.SetActive(false);  
        objBackground.GetComponent<Image>().sprite = gameBackground;         
    }

    public void activeSelectedCardTutorial() {
        options.SetActive(false);
        start.SetActive(false);
        info.SetActive(false);
        rules.SetActive(false);
        join.SetActive(false);
        host.SetActive(false);
        lobby.SetActive(false);
        nickname.SetActive(false);
        preGame.SetActive(false);
        game.SetActive(false);
        endGame.SetActive(false);
        Tutorial.SetActive(true);
        selectedCardTutorial.SetActive(true);
        CardTutorial.SetActive(false); 
    }

    public void activePeddleTutorial() {
        options.SetActive(false);
        start.SetActive(false);
        info.SetActive(false);
        rules.SetActive(false);
        join.SetActive(false);
        host.SetActive(false);
        lobby.SetActive(false);
        nickname.SetActive(false);
        preGame.SetActive(false);
        game.SetActive(false);
        endGame.SetActive(false);
        selectedCardTutorial.SetActive(false);
        CardTutorial.SetActive(true);   
        CardTutorial.transform.GetChild(1).GetChild(0).gameObject.GetComponentInChildren<Image>().sprite = peddleTutorialSprite;       
    }

    public void activeProtectionTutorial() {
        options.SetActive(false);
        start.SetActive(false);
        info.SetActive(false);
        rules.SetActive(false);
        join.SetActive(false);
        host.SetActive(false);
        lobby.SetActive(false);
        nickname.SetActive(false);
        preGame.SetActive(false);
        game.SetActive(false);
        endGame.SetActive(false);
        selectedCardTutorial.SetActive(false);
        CardTutorial.SetActive(true);       
        CardTutorial.transform.GetChild(1).GetChild(0).gameObject.GetComponentInChildren<Image>().sprite = protectionTutorialSprite;       
    }

    public void activeMarketOpenTutorial() {
        options.SetActive(false);
        start.SetActive(false);
        info.SetActive(false);
        rules.SetActive(false);
        join.SetActive(false);
        host.SetActive(false);
        lobby.SetActive(false);
        nickname.SetActive(false);
        preGame.SetActive(false);
        game.SetActive(false);
        endGame.SetActive(false);
        selectedCardTutorial.SetActive(false);
        CardTutorial.SetActive(true);       
        CardTutorial.transform.GetChild(1).GetChild(0).gameObject.GetComponentInChildren<Image>().sprite = marketOpenTutorialSprite;       
    }

    public void activeMarketCloseTutorial() {
        options.SetActive(false);
        start.SetActive(false);
        info.SetActive(false);
        rules.SetActive(false);
        join.SetActive(false);
        host.SetActive(false);
        lobby.SetActive(false);
        nickname.SetActive(false);
        preGame.SetActive(false);
        game.SetActive(false);
        endGame.SetActive(false);
        selectedCardTutorial.SetActive(false);
        CardTutorial.SetActive(true);       
        CardTutorial.transform.GetChild(1).GetChild(0).gameObject.GetComponentInChildren<Image>().sprite = marketCloseTutorialSprite;       
    }

    public void activeHeatOnTutorial() {
        options.SetActive(false);
        start.SetActive(false);
        info.SetActive(false);
        rules.SetActive(false);
        join.SetActive(false);
        host.SetActive(false);
        lobby.SetActive(false);
        nickname.SetActive(false);
        preGame.SetActive(false);
        game.SetActive(false);
        endGame.SetActive(false);
        selectedCardTutorial.SetActive(false);
        CardTutorial.SetActive(true);  
        CardTutorial.transform.GetChild(1).GetChild(0).gameObject.GetComponentInChildren<Image>().sprite = HeatOnTutorialSprite;            
    }

    public void activeHeatOffTutorial() {
        options.SetActive(false);
        start.SetActive(false);
        info.SetActive(false);
        rules.SetActive(false);
        join.SetActive(false);
        host.SetActive(false);
        lobby.SetActive(false);
        nickname.SetActive(false);
        preGame.SetActive(false);
        game.SetActive(false);
        endGame.SetActive(false);
        selectedCardTutorial.SetActive(false);
        CardTutorial.SetActive(true);      
        CardTutorial.transform.GetChild(1).GetChild(0).gameObject.GetComponentInChildren<Image>().sprite = HeatOffTutorialSprite;        
    }

    public void activePayFineTutorial() {
        options.SetActive(false);
        start.SetActive(false);
        info.SetActive(false);
        rules.SetActive(false);
        join.SetActive(false);
        host.SetActive(false);
        lobby.SetActive(false);
        nickname.SetActive(false);
        preGame.SetActive(false);
        game.SetActive(false);
        endGame.SetActive(false);
        selectedCardTutorial.SetActive(false);
        CardTutorial.SetActive(true);       
        CardTutorial.transform.GetChild(1).GetChild(0).gameObject.GetComponentInChildren<Image>().sprite = payFineTutorailSprite;       
    }

    public void activeStealTutorial() {
        options.SetActive(false);
        start.SetActive(false);
        info.SetActive(false);
        rules.SetActive(false);
        join.SetActive(false);
        host.SetActive(false);
        lobby.SetActive(false);
        nickname.SetActive(false);
        preGame.SetActive(false);
        game.SetActive(false);
        endGame.SetActive(false);
        selectedCardTutorial.SetActive(false);
        CardTutorial.SetActive(true);   
        CardTutorial.transform.GetChild(1).GetChild(0).gameObject.GetComponentInChildren<Image>().sprite = stealTutorialSPrite;           
    }

    public void activeMaledizioniTutorial() {
        options.SetActive(false);
        start.SetActive(false);
        info.SetActive(false);
        rules.SetActive(false);
        join.SetActive(false);
        host.SetActive(false);
        lobby.SetActive(false);
        nickname.SetActive(false);
        preGame.SetActive(false);
        game.SetActive(false);
        endGame.SetActive(false);
        selectedCardTutorial.SetActive(false);
        CardTutorial.SetActive(true); 
        CardTutorial.transform.GetChild(1).GetChild(0).gameObject.GetComponentInChildren<Image>().sprite = maledizioniTutorialSprite;             
    }

    public void activeNirvanaTutorial() {
        options.SetActive(false);
        start.SetActive(false);
        info.SetActive(false);
        rules.SetActive(false);
        join.SetActive(false);
        host.SetActive(false);
        lobby.SetActive(false);
        nickname.SetActive(false);
        preGame.SetActive(false);
        game.SetActive(false);
        endGame.SetActive(false);
        selectedCardTutorial.SetActive(false);
        CardTutorial.SetActive(true);    
        CardTutorial.transform.GetChild(1).GetChild(0).gameObject.GetComponentInChildren<Image>().sprite = nirvanaTutorialSprite;          
    }

    public void activeBankerTutorial() {
        options.SetActive(false);
        start.SetActive(false);
        info.SetActive(false);
        rules.SetActive(false);
        join.SetActive(false);
        host.SetActive(false);
        lobby.SetActive(false);
        nickname.SetActive(false);
        preGame.SetActive(false);
        game.SetActive(false);
        endGame.SetActive(false);
        selectedCardTutorial.SetActive(false);
        CardTutorial.SetActive(true); 
        CardTutorial.transform.GetChild(1).GetChild(0).gameObject.GetComponentInChildren<Image>().sprite = bankerTutorialSprite;             
    }

    public void afterSetNickname()
    {
        nickname.SetActive(false);
    }

    public void updateNickname()
    {
        nickname.SetActive(true);
    }

    public void openUrl(String _url)
    {
        Application.OpenURL(_url);
    }
}