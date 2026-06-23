using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
public class LobbyManager : MonoBehaviour
{
    public Lobby joinedLobby = null;
    private float heartBeatTimer;
    private float lobbyUpdateTimer;
    public static LobbyManager instance;
    public InputField lobbyName;
    public Toggle isPrivate;
    public GameObject scrollViewContent;
    public GameObject lobbyTemplate;
    public Text lobbyNameText;
    public Text lobbyCodeText;
    public Dropdown numPlayerCreate;
    public InputField joinCode;
    [SerializeField] public Image privateButtonImage;
    [SerializeField] public Sprite privateSprite;
    [SerializeField] public Sprite publicSprite;
    public List<GameObject> players;
    public GameObject playerTemplate;
    private bool IsReady = false;
    public Image imageBtnRNR;
    public Button btnStart;
    [SerializeField] public Sprite readySprite;
    [SerializeField] public Sprite notReadySprite;

    private bool inGame = false;
    public string relayCode = null;    

    void Awake()
    {
        instance = this;
    }

    private void Update() {
        HandleLobbyPollForUpdates();
        if(joinedLobby != null && joinedLobby.HostId == AuthenticationService.Instance.PlayerId) {
            HandleLobbyHeartBeat();
        }
    }

    //Send Heartbeat to unity services
    private async void HandleLobbyHeartBeat() {
        if(joinedLobby != null) {
            heartBeatTimer -= Time.deltaTime;
            if(heartBeatTimer < 0f) {
                float heartBeatTimerMax = 15;
                heartBeatTimer = heartBeatTimerMax;

                await LobbyService.Instance.SendHeartbeatPingAsync(joinedLobby.Id);
            }
        }
    }

    //Update lobby
    private async void HandleLobbyPollForUpdates() {
        if(joinedLobby != null) {
            lobbyUpdateTimer -= Time.deltaTime;
            if(lobbyUpdateTimer < 0f) {
                float lobbyUpdateTimerMax = 1.5f;
                lobbyUpdateTimer = lobbyUpdateTimerMax;

                Lobby lobby = await LobbyService.Instance.GetLobbyAsync(joinedLobby.Id);
                joinedLobby = lobby;
                
                UpdatePlayerObject(joinedLobby.Players.Count);
            }
        }
    }

    //Create Lobby
    public async void createLobby()
    {
        CreateLobbyOptions options = new CreateLobbyOptions {
            IsPrivate = isPrivate.isOn,
            Player = GetPlayer()
        };
        Debug.Log("numPlayerCreate.captionText: " + numPlayerCreate.captionText);
        GameManager.numPlayers =  Int32.Parse(numPlayerCreate.captionText.text);
        Lobby lobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName.text, GameManager.numPlayers, options);
        joinedLobby = lobby;    

        MenuManager.Instance.preTransition("LOBBY");
    }

    //Return List of public lobby
    public async void queryLobby() 
    {
        foreach (Transform child in scrollViewContent.transform) {
                GameObject.Destroy(child.gameObject);
            }
        
        try
        {
            QueryLobbiesOptions options = new QueryLobbiesOptions();
            options.Count = 25;

            // Filter for open lobbies only
            options.Filters = new List<QueryFilter>()
            {
                new QueryFilter(
                    field: QueryFilter.FieldOptions.AvailableSlots,
                    op: QueryFilter.OpOptions.GT,
                    value: "0")
            };

            // Order by newest lobbies first
            options.Order = new List<QueryOrder>()
            {
                new QueryOrder(
                    asc: false,
                    field: QueryOrder.FieldOptions.Created)
            };

            QueryResponse lobbies = await Lobbies.Instance.QueryLobbiesAsync(options);

            foreach (Lobby lobby in lobbies.Results)
            {
                GameObject btn = (GameObject)Instantiate(lobbyTemplate);
                btn.GetComponentInChildren<Text>().text = lobby.Name;
                btn.GetComponentInChildren<Button>().GetComponentInChildren<Text>().text = lobby.Id;
                btn.transform.SetParent(scrollViewContent.transform, false);
            }

        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }

    //Refresh list of lobby
    public void refreshLobby()
    {
        queryLobby();
    }

    public void initializedLobby()
    {
        try 
        {
            lobbyNameText.text = "";
            lobbyCodeText.text = "Code: ";
            btnStart.transform.gameObject.SetActive(false);
            imageBtnRNR.sprite = readySprite;
            UpdatePlayerObject(joinedLobby.Players.Count);
            if(joinedLobby != null) {
                lobbyNameText.text = joinedLobby.Name;
                lobbyCodeText.text = lobbyCodeText.text + joinedLobby.LobbyCode;
            }
            
        
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }

    //Leave Lobby
    public async void leaveLobby()
    {
        if(joinedLobby.Players.Count >= 2 && joinedLobby.HostId == AuthenticationService.Instance.PlayerId) {
            await Lobbies.Instance.UpdateLobbyAsync(joinedLobby.Id, new UpdateLobbyOptions {
                HostId = joinedLobby.Players[1].Id
            });
        }
        await LobbyService.Instance.RemovePlayerAsync(joinedLobby.Id, AuthenticationService.Instance.PlayerId);
        joinedLobby = null;
        IsReady = false;
    }

    //Join with code to lobby
    public async void joinWithCode()
    {
        try
        {
            JoinLobbyByCodeOptions joinLobbyByCodeOptions = new JoinLobbyByCodeOptions {
                Player = GetPlayer()
            };

            Lobby result = await LobbyService.Instance.JoinLobbyByCodeAsync(joinCode.text, joinLobbyByCodeOptions);
            if (result != null)
            {
                joinedLobby = result;
                MenuManager.Instance.preTransition("LOBBY");    
            }
            
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }
    
    //Change GUI Private/Public
    public void UpdatePrivateButtonImageSprite()
    {
        if (isPrivate.isOn)
        {
            privateButtonImage.sprite = privateSprite;
        }
        else
        {
            privateButtonImage.sprite = publicSprite;
        }
    }

    //Create Dictonary to lobby for player
    public Player GetPlayer() {

        return new Player {
                Data = new Dictionary<String, PlayerDataObject> {
                    {"PlayerName", new PlayerDataObject(PlayerDataObject.VisibilityOptions.Member, AuthenticationService.Instance.PlayerName)},
                    { "Status", new PlayerDataObject(PlayerDataObject.VisibilityOptions.Member, "false")},
                    {"RelayCode", new PlayerDataObject(PlayerDataObject.VisibilityOptions.Member, "null")}
                }
        };
    }

    //Update GUI GameObject player in lobby
    private void UpdatePlayerObject(int numPlayer) {
        int i = 0;
        bool allReady = true;

        if(!inGame) {
            foreach (GameObject player in players) {
                if(player.transform.childCount > 0) {
                    GameObject.Destroy(player.transform.GetChild(0).gameObject);
                }
            }

            foreach (GameObject player in players) {
                GameObject btn = (GameObject)Instantiate(playerTemplate);
                btn.transform.SetParent(player.transform, false);
           
                if(i < numPlayer) {
                    if(joinedLobby.HostId == AuthenticationService.Instance.PlayerId) {
                        btnStart.transform.gameObject.SetActive(true);
                    }

                    if(String.Compare(joinedLobby.Players[i].Data["RelayCode"].Value, "null") != 0) {                        
                        if(joinedLobby.HostId != AuthenticationService.Instance.PlayerId) {
                            GameManager.instance.join(joinedLobby.Players[i].Data["RelayCode"].Value);
                            MenuManager.Instance.preTransition("GAMECLIENT");
                            inGame = true;
                        }
                    }
                
                    btn.transform.GetChild(0).gameObject.SetActive(false);
                    btn.transform.GetChild(1).gameObject.SetActive(true);
                    btn.transform.GetChild(1).GetChild(0).gameObject.GetComponentInChildren<Text>().text = joinedLobby.Players[i].Data["PlayerName"].Value.Split('#')[0];
                    if(!Convert.ToBoolean(joinedLobby.Players[i].Data["Status"].Value)) {
                        btn.transform.GetChild(1).GetChild(1).gameObject.GetComponentInChildren<Image>().sprite = notReadySprite;
                        allReady = false;
                    } else {
                        btn.transform.GetChild(1).GetChild(1).gameObject.GetComponentInChildren<Image>().sprite = readySprite;
                    } 
                }
                i++;
            }

            if(numPlayer == GameManager.numPlayers) {
                if(allReady) {
                    btnStart.interactable = true;
                } else {
                    btnStart.interactable = false;
                }
            } else {
                btnStart.interactable = false;
            }
        }
    }

    //Update Player Status True/False
    public void UpdatePlayerStatus() {
        if(IsReady) {
            LobbyService.Instance.UpdatePlayerAsync(joinedLobby.Id, AuthenticationService.Instance.PlayerId, new UpdatePlayerOptions {
                Data = new Dictionary<String, PlayerDataObject> {
                    {"Status", new PlayerDataObject(PlayerDataObject.VisibilityOptions.Member, "false")}
                }
            });
            imageBtnRNR.sprite = readySprite;
            IsReady = false;
        } else {
            LobbyService.Instance.UpdatePlayerAsync(joinedLobby.Id, AuthenticationService.Instance.PlayerId, new UpdatePlayerOptions {
                Data = new Dictionary<String, PlayerDataObject> {
                    {"Status", new PlayerDataObject(PlayerDataObject.VisibilityOptions.Member, "true")}
                }
            });
            imageBtnRNR.sprite = notReadySprite;
            IsReady = true;
        }
    }

    //Start Game
    public async void StartGame() {
        MenuManager.Instance.preTransition("GAME");
        relayCode = await GameManager.instance.startHost(); 
        inGame = true;
        getRelayCode();
    }

    //Set Relay code in lobby
    public void getRelayCode() {
        LobbyService.Instance.UpdatePlayerAsync(joinedLobby.Id, AuthenticationService.Instance.PlayerId, new UpdatePlayerOptions {
                Data = new Dictionary<String, PlayerDataObject> {
                    {"RelayCode", new PlayerDataObject(PlayerDataObject.VisibilityOptions.Member, relayCode)}
                }
            });
    }

    async void OnApplicationQuit()
    {
        if(joinedLobby != null) {
            if(joinedLobby.Players.Count >= 2) {
                await Lobbies.Instance.UpdateLobbyAsync(joinedLobby.Id, new UpdateLobbyOptions {
                    HostId = joinedLobby.Players[1].Id
                });
            }
            await LobbyService.Instance.RemovePlayerAsync(joinedLobby.Id, AuthenticationService.Instance.PlayerId);
            joinedLobby = null;
            IsReady = false;
        }
    }
    
}
