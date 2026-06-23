using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Unity.Services.Authentication;
using UnityEngine.UI;
using Unity.Collections;

public class Drop : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler {

    
    public GameObject cashSpot;
    public GameObject marketSpot;
    public GameObject hassleSpot;
    public GameObject protection25kSpot;
    public GameObject protection50kSpot;
    public GameObject player_1;
    public GameObject player_2;
    public GameObject player_3;
    public GameObject player_4;

    public GameObject cash_5k;
    public GameObject cash_25k;
    public GameObject cash_50k;
    public GameObject cash_100k;

    public int waitSecond = 0;

    private bool runGame = true;
    private bool malFlow = false;
 
    public void OnPointerEnter(PointerEventData pointerEventData) {
        if(pointerEventData.pointerDrag == null) {
            return;
        }

        Drag drag = pointerEventData.pointerDrag.GetComponent<Drag>();
        if(drag != null) {
            drag.placeholderParent = this.transform;
        }
    }

    public void OnDrop(PointerEventData pointerEventData) {
        Debug.Log("Start drop");
        Drag drag = pointerEventData.pointerDrag.GetComponent<Drag>();
		if(drag != null) {
            Transform result = null;
            foreach(PlayerCustom player in GameManager.instance.players) { 
                if(player.myTurn && player.pickFromDeck) {
                    waitSecond = 1;
                    result = manageCard(this.transform, pointerEventData);
                    if (result != null) {
                    
                        if(result == hassleSpot.transform && hassleSpot.transform.childCount > 0) {
                            Destroy(hassleSpot.transform.GetChild(0).gameObject);
                        }

                        if(!malFlow) {
                            if(runGame){
                                AudioManager.Instance.PlayClipByName("Cardplaced");
                                pointerEventData.pointerDrag.GetComponent<ShowCard>().isSummoned = true;
                                GameManager.instance.EndTurn(waitSecond);
                                drag.parentToReturnTo = result;
                            }
                        } else {
                            malFlow = false;
                        }
                    }
                }
            }
		}
        Debug.Log("end drop");
    }

    public void OnPointerExit(PointerEventData pointerEventData) {
        if(pointerEventData.pointerDrag == null) {
            return;
        }

        Drag drag = pointerEventData.pointerDrag.GetComponent<Drag>();
        if(drag != null) {
            drag.placeholderParent = this.transform;
        }
    }

    public Transform manageCard(Transform destination, PointerEventData pointerEventData) {
        Transform result = null;
        switch(pointerEventData.pointerDrag.GetComponent<ShowCard>().cardData.effects) {
            case CardSO.Effects.MARKET_OPEN:
                result = manageMarketOpen(destination, pointerEventData);
                break;
            case CardSO.Effects.MARKET_CLOSE:
                result = managerMarketClose(destination, pointerEventData);
                break;
            case CardSO.Effects.SOLDI:
                result = manageCash(destination, pointerEventData);
                break;
            case CardSO.Effects.HEAT_ON:
                result = manageHeatOn(destination, pointerEventData);
                break;
            case CardSO.Effects.HEAT_OFF:
                result = manageHeatOff(destination, pointerEventData);
                break;
            case CardSO.Effects.NIRVANA:
                result = managerNirvana(destination, pointerEventData);
                break;
            case CardSO.Effects.MALEDIZIONI:
                result = ManageMaledizioni(destination, pointerEventData);
                break;
            case CardSO.Effects.BANKER:
                break;
            case CardSO.Effects.STEAL:
                result = manageSteal(destination, pointerEventData);
                break;
            case CardSO.Effects.PAY_FINE:
                result = managePlayFine(destination, pointerEventData);
                break;
            case CardSO.Effects.PROTECTION:
                result = managerProtection(destination, pointerEventData);
                break;
        }
        return result;
    } 

    public Transform manageSteal(Transform destination, PointerEventData pointerEventData) {
        Transform result = null;

        if(destination.transform == hassleSpot.transform) {
            result = hassleSpot.transform;
            foreach(PlayerCustom player in GameManager.instance.players) { 
                if(player.playerName == AuthenticationService.Instance.PlayerName) {
                    GameManager.instance.DropCardOnHassleServerRpc(player.playerName, pointerEventData.pointerDrag.GetComponent<ShowCard>().cardData.id);
                }
            }
            removeCard(pointerEventData);
        } 

        foreach(PlayerCustom player in GameManager.instance.players) { 
            if(player.playerName == AuthenticationService.Instance.PlayerName && player.market == PlayerCustom.Market.True && !player.heatOn) {
                if(destination.transform == player_1.transform  && player_1.transform.GetChild(0).GetChild(0).GetChild(2).GetComponentInChildren<Text>().text != AuthenticationService.Instance.PlayerName.Split('#')[0] && player_1.transform.GetChild(0).GetChild(0).GetChild(0).GetComponentInChildren<Image>().sprite == GameManager.instance.greenMarket) {
                    waitSecond = 7;
                    malFlow = true;
                    GameManager.instance.activePayfine(pointerEventData.pointerDrag.GetComponent<ShowCard>().cardData.id,  player_1.transform.GetChild(0).GetChild(0).GetChild(2).GetComponentInChildren<Text>().text, AuthenticationService.Instance.PlayerName);
                    removeCard(pointerEventData);
                    result = hassleSpot.transform;
                } else if(destination.transform == player_2.transform && player_2.transform.GetChild(0).GetChild(0).GetChild(2).GetComponentInChildren<Text>().text != AuthenticationService.Instance.PlayerName.Split('#')[0]  && player_2.transform.GetChild(0).GetChild(0).GetChild(0).GetComponentInChildren<Image>().sprite == GameManager.instance.greenMarket) {
                    waitSecond = 7;
                    malFlow = true;
                    GameManager.instance.activePayfine(pointerEventData.pointerDrag.GetComponent<ShowCard>().cardData.id,  player_2.transform.GetChild(0).GetChild(0).GetChild(2).GetComponentInChildren<Text>().text, AuthenticationService.Instance.PlayerName);
                    removeCard(pointerEventData);
                    result = hassleSpot.transform;
                } else if(destination.transform == player_3.transform &&player_3.transform.GetChild(0).GetChild(0).GetChild(2).GetComponentInChildren<Text>().text != AuthenticationService.Instance.PlayerName.Split('#')[0] && player_3.transform.GetChild(0).GetChild(0).GetChild(0).GetComponentInChildren<Image>().sprite == GameManager.instance.greenMarket) {
                    waitSecond = 7;
                    malFlow = true;
                    GameManager.instance.activePayfine(pointerEventData.pointerDrag.GetComponent<ShowCard>().cardData.id,  player_3.transform.GetChild(0).GetChild(0).GetChild(2).GetComponentInChildren<Text>().text, AuthenticationService.Instance.PlayerName);
                    removeCard(pointerEventData);
                    result = hassleSpot.transform;
                } else if(destination.transform == player_4.transform && player_4.transform.GetChild(0).GetChild(0).GetChild(2).GetComponentInChildren<Text>().text != AuthenticationService.Instance.PlayerName.Split('#')[0] && player_4.transform.GetChild(0).GetChild(0).GetChild(0).GetComponentInChildren<Image>().sprite == GameManager.instance.greenMarket) {
                    waitSecond = 7;
                    malFlow = true;
                    GameManager.instance.activePayfine(pointerEventData.pointerDrag.GetComponent<ShowCard>().cardData.id,  player_4.transform.GetChild(0).GetChild(0).GetChild(2).GetComponentInChildren<Text>().text, AuthenticationService.Instance.PlayerName);
                    removeCard(pointerEventData);
                    result = hassleSpot.transform;
                } 
            }
        }

        return result;
    }

    public Transform manageCash(Transform destination, PointerEventData pointerEventData) {
        Transform result = null;

        if(destination.transform == cashSpot.transform) {
            foreach(PlayerCustom player in GameManager.instance.players) { 
                if(player.playerName == AuthenticationService.Instance.PlayerName && player.market == PlayerCustom.Market.True && !player.heatOn) {
                    for(int i = 0; i < player.Hand.Count; i++) {
                        if(player.Hand[i].id == pointerEventData.pointerDrag.GetComponent<ShowCard>().cardData.id) {
                            switch(pointerEventData.pointerDrag.GetComponent<ShowCard>().cardData.cash) {
                                case CardSO.Cash.K5:
                                    player.marketPlace_5K.Add(player.Hand[i]);
                                    player.Hand.RemoveAt(i);
                                    result = cash_5k.transform;
                                    GameManager.instance.UpdatePlayerCashServerRpc(5000, player.playerName, pointerEventData.pointerDrag.GetComponent<ShowCard>().cardData.id);
                                    break;
                                case CardSO.Cash.K25:
                                    player.marketPlace_25K.Add(player.Hand[i]);
                                    player.Hand.RemoveAt(i);
                                    result = cash_25k.transform;
                                    GameManager.instance.UpdatePlayerCashServerRpc(25000, player.playerName, pointerEventData.pointerDrag.GetComponent<ShowCard>().cardData.id);
                                    break;
                                case CardSO.Cash.K50:
                                    player.marketPlace_50K.Add(player.Hand[i]);
                                    player.Hand.RemoveAt(i);
                                    result = cash_50k.transform;
                                    GameManager.instance.UpdatePlayerCashServerRpc(50000, player.playerName, pointerEventData.pointerDrag.GetComponent<ShowCard>().cardData.id);
                                    break;
                                case CardSO.Cash.K100:
                                    player.marketPlace_100k.Add(player.Hand[i]);
                                    player.Hand.RemoveAt(i);
                                    result = cash_100k.transform;
                                    GameManager.instance.UpdatePlayerCashServerRpc(100000, player.playerName, pointerEventData.pointerDrag.GetComponent<ShowCard>().cardData.id);
                                    break;
                            }
                        }
                    }
                }
            }
        } else if(destination.transform == hassleSpot.transform) {
            result = hassleSpot.transform;
            foreach(PlayerCustom player in GameManager.instance.players) { 
                if(player.playerName == AuthenticationService.Instance.PlayerName) {
                    GameManager.instance.DropCardOnHassleServerRpc(player.playerName, pointerEventData.pointerDrag.GetComponent<ShowCard>().cardData.id);
                }
            }
            removeCard(pointerEventData);
        }
        
        return result;
    }

    public Transform manageMarketOpen(Transform destination, PointerEventData pointerEventData) { 
        Transform result = null;
        if (destination.transform == marketSpot.transform) {
            foreach(PlayerCustom player in GameManager.instance.players) { 
                if(player.playerName == AuthenticationService.Instance.PlayerName && player.market == PlayerCustom.Market.Null) {
                    result = marketSpot.transform;
                    GameManager.instance.changeMarketTOGreen();
                    GameManager.instance.UpdatePlayerMarketOpenServerRpc(player.playerName, pointerEventData.pointerDrag.GetComponent<ShowCard>().cardData.id);
                }
            }
        } else if(destination.transform == hassleSpot.transform) {
            result = hassleSpot.transform;
            foreach(PlayerCustom player in GameManager.instance.players) { 
                if(player.playerName == AuthenticationService.Instance.PlayerName) {
                    GameManager.instance.DropCardOnHassleServerRpc(player.playerName, pointerEventData.pointerDrag.GetComponent<ShowCard>().cardData.id);
                }
            }
            removeCard(pointerEventData);
        }

        return result;
    }

    public Transform managerNirvana(Transform destination, PointerEventData pointerEventData) {
        Transform result = null;
        if(destination.transform == hassleSpot.transform) {
            foreach(PlayerCustom player in GameManager.instance.players) { 
                if(player.playerName == AuthenticationService.Instance.PlayerName) {
                    GameManager.instance.DropCardOnHassleServerRpc(player.playerName, pointerEventData.pointerDrag.GetComponent<ShowCard>().cardData.id);
                    result = hassleSpot.transform;
                }
            } 
            result = hassleSpot.transform;
        } else if(destination.transform == cashSpot.transform) {
            foreach(PlayerCustom player in GameManager.instance.players) {
                if(player.playerName == AuthenticationService.Instance.PlayerName && player.market != PlayerCustom.Market.Null) {
                    waitSecond = 7;
                    GameManager.instance.changeMarketTOGreen();
                    GameManager.instance.PlayNirvanaCardServerRpc(player.playerName, pointerEventData.pointerDrag.GetComponent<ShowCard>().cardData.id, pointerEventData.pointerDrag.GetComponent<ShowCard>().cardData.effects ,pointerEventData.pointerDrag.GetComponent<ShowCard>().cardData.nirvana);
                    result = hassleSpot.transform;
                }
            }
        }
        return result;
    }

    public Transform managerMarketClose(Transform destination, PointerEventData pointerEventData) {
        Transform result = null;
        if(destination.transform == hassleSpot.transform) {
            foreach(PlayerCustom player in GameManager.instance.players) {
                if(player.playerName == AuthenticationService.Instance.PlayerName) {
                    GameManager.instance.DropCardOnHassleServerRpc(player.playerName, pointerEventData.pointerDrag.GetComponent<ShowCard>().cardData.id);
                }
            }
            result = hassleSpot.transform;
        } else if(destination.transform == cashSpot.transform) {
            foreach(PlayerCustom player in GameManager.instance.players) { 
                if(player.playerName == AuthenticationService.Instance.PlayerName && player.market == PlayerCustom.Market.True && !player.heatOn) {
                    GameManager.instance.playMarketCloseServerRpc(player.playerName);
                    runGame = false;
                }
            }
        }

        return result;
    }

    public Transform ManageMaledizioni(Transform destination, PointerEventData pointerEventData) {
        Transform result = null;
        if(destination.transform == hassleSpot.transform) {
            foreach(PlayerCustom player in GameManager.instance.players) {
                if(player.playerName == AuthenticationService.Instance.PlayerName) {
                    GameManager.instance.DropMaledizioneServerRpc(player.playerName, pointerEventData.pointerDrag.GetComponent<ShowCard>().cardData.id, pointerEventData.pointerDrag.GetComponent<ShowCard>().cardData.maledizioni);
                    malFlow = true;
                }
            }
            result = hassleSpot.transform;
        }
        return result;
    }

    public Transform manageHeatOn(Transform destination, PointerEventData pointerEventData) {
        Transform result = null;

        if(destination.transform == player_1.transform  && player_1.transform.GetChild(0).GetChild(0).GetChild(2).GetComponentInChildren<Text>().text != AuthenticationService.Instance.PlayerName.Split('#')[0] && player_1.transform.GetChild(0).GetChild(0).GetChild(0).GetComponentInChildren<Image>().sprite == GameManager.instance.greenMarket) {
            waitSecond = 7;
            GameManager.instance.SendHeatOnServerRpc(pointerEventData.pointerDrag.GetComponent<ShowCard>().cardData.heat, pointerEventData.pointerDrag.GetComponent<ShowCard>().cardData.id, player_1.transform.GetChild(0).GetChild(0).GetChild(2).GetComponentInChildren<Text>().text, AuthenticationService.Instance.PlayerName);
            removeCard(pointerEventData);
            result = hassleSpot.transform;
        } else if(destination.transform == player_2.transform && player_2.transform.GetChild(0).GetChild(0).GetChild(2).GetComponentInChildren<Text>().text != AuthenticationService.Instance.PlayerName.Split('#')[0]  && player_2.transform.GetChild(0).GetChild(0).GetChild(0).GetComponentInChildren<Image>().sprite == GameManager.instance.greenMarket) {
            waitSecond = 7;
            GameManager.instance.SendHeatOnServerRpc(pointerEventData.pointerDrag.GetComponent<ShowCard>().cardData.heat, pointerEventData.pointerDrag.GetComponent<ShowCard>().cardData.id, player_2.transform.GetChild(0).GetChild(0).GetChild(2).GetComponentInChildren<Text>().text, AuthenticationService.Instance.PlayerName);
            removeCard(pointerEventData);
            result = hassleSpot.transform;
        } else if(destination.transform == player_3.transform &&player_3.transform.GetChild(0).GetChild(0).GetChild(2).GetComponentInChildren<Text>().text != AuthenticationService.Instance.PlayerName.Split('#')[0] && player_3.transform.GetChild(0).GetChild(0).GetChild(0).GetComponentInChildren<Image>().sprite == GameManager.instance.greenMarket) {
            waitSecond = 7;
            GameManager.instance.SendHeatOnServerRpc(pointerEventData.pointerDrag.GetComponent<ShowCard>().cardData.heat, pointerEventData.pointerDrag.GetComponent<ShowCard>().cardData.id, player_3.transform.GetChild(0).GetChild(0).GetChild(2).GetComponentInChildren<Text>().text, AuthenticationService.Instance.PlayerName);
            removeCard(pointerEventData);
            result = hassleSpot.transform;
        }  else if(destination.transform == player_4.transform && player_4.transform.GetChild(0).GetChild(0).GetChild(2).GetComponentInChildren<Text>().text != AuthenticationService.Instance.PlayerName.Split('#')[0] && player_4.transform.GetChild(0).GetChild(0).GetChild(0).GetComponentInChildren<Image>().sprite == GameManager.instance.greenMarket) {
            waitSecond = 7;
            GameManager.instance.SendHeatOnServerRpc(pointerEventData.pointerDrag.GetComponent<ShowCard>().cardData.heat, pointerEventData.pointerDrag.GetComponent<ShowCard>().cardData.id, player_4.transform.GetChild(0).GetChild(0).GetChild(2).GetComponentInChildren<Text>().text, AuthenticationService.Instance.PlayerName);
            removeCard(pointerEventData);
            result = hassleSpot.transform;
        } else if(destination.transform == hassleSpot.transform) {
            removeCard(pointerEventData);
            foreach(PlayerCustom player in GameManager.instance.players) {
                if(player.playerName == AuthenticationService.Instance.PlayerName) {
                    GameManager.instance.DropCardOnHassleServerRpc(player.playerName, pointerEventData.pointerDrag.GetComponent<ShowCard>().cardData.id);
                }
            }
            result = hassleSpot.transform;
        }

        return result;
    }

    public Transform manageHeatOff(Transform destination, PointerEventData pointerEventData) {
        Transform result = null;

        if(destination.transform == hassleSpot.transform) {
            foreach(PlayerCustom player in GameManager.instance.players) { 
                if(player.playerName == AuthenticationService.Instance.PlayerName) {
                    GameManager.instance.DropCardOnHassleServerRpc(player.playerName, pointerEventData.pointerDrag.GetComponent<ShowCard>().cardData.id);
                }
            }
            result = hassleSpot.transform;
        } else if(destination.transform == marketSpot.transform) {
            foreach(PlayerCustom player in GameManager.instance.players) { 
                if(player.playerName == AuthenticationService.Instance.PlayerName && player.heatOn) {
                    if(pointerEventData.pointerDrag.GetComponent<ShowCard>().cardData.heat.ToString() == player.type_HEatOn.ToString()) {
                        waitSecond = 7;
                        removeCard(pointerEventData);
                        GameManager.instance.SendHeatOffServerRpc(pointerEventData.pointerDrag.GetComponent<ShowCard>().cardData.heat, pointerEventData.pointerDrag.GetComponent<ShowCard>().cardData.id, player.playerName);
                        Destroy(marketSpot.transform.GetChild(1).gameObject);
                        GameManager.instance.changeMarketTOGreen();
                        result = hassleSpot.transform;
                    }
                }
            }
        }

        return result;
    }

    public Transform managerProtection(Transform destination, PointerEventData pointerEventData) {
        Transform result = null;
               
        if(destination.transform == cashSpot.transform) {
            switch(pointerEventData.pointerDrag.GetComponent<ShowCard>().cardData.protection) {
                case CardSO.Protection.K25:
                    result = manage25Protection(destination, pointerEventData);
                    break;
                case CardSO.Protection.K50:
                    result = manage50Protection(destination, pointerEventData);
                    break;
            }
        } else if(destination.transform == hassleSpot.transform) {
            foreach(PlayerCustom player in GameManager.instance.players) { 
                if(player.playerName == AuthenticationService.Instance.PlayerName) {
                    GameManager.instance.DropCardOnHassleServerRpc(player.playerName, pointerEventData.pointerDrag.GetComponent<ShowCard>().cardData.id);
                }
            }
            result = hassleSpot.transform;
        }
        
        return result;
    }

    public Transform manage25Protection(Transform destination, PointerEventData pointerEventData) {
        Transform result = null;

        foreach(PlayerCustom player in GameManager.instance.players) { 
            if(player.playerName == AuthenticationService.Instance.PlayerName) {
                if(player.protection_25K.Count < player.marketPlace_25K.Count) {
                    result = protection25kSpot.transform;
                    GameManager.instance.playProtection25ServerRpc(player.playerName, pointerEventData.pointerDrag.GetComponent<ShowCard>().cardData.id);

                    for(int i = 0; i < player.Hand.Count; i++) {
                        if(player.Hand[i].id == pointerEventData.pointerDrag.GetComponent<ShowCard>().cardData.id) {
                            player.protection_25K.Add(player.Hand[i]);
                            player.Hand.RemoveAt(i);
                        }
                    }
                }
            }
        }

        return result;
    }
    
    public Transform manage50Protection(Transform destination, PointerEventData pointerEventData) {
        Transform result = null;

        foreach(PlayerCustom player in GameManager.instance.players) {
            if(player.playerName == AuthenticationService.Instance.PlayerName) { 
                if(player.protection_50K.Count < player.marketPlace_50K.Count) {
                    result = protection50kSpot.transform;
                    GameManager.instance.playProtection50ServerRpc(player.playerName, pointerEventData.pointerDrag.GetComponent<ShowCard>().cardData.id);

                    for(int i = 0; i < player.Hand.Count; i++) {
                        if(player.Hand[i].id == pointerEventData.pointerDrag.GetComponent<ShowCard>().cardData.id) {
                            player.protection_50K.Add(player.Hand[i]);
                            player.Hand.RemoveAt(i);
                        }
                    }
                }
            }
        }

        return result;
    }

    public Transform managePlayFine(Transform destination, PointerEventData pointerEventData) {
        Transform result = null;

        if(destination.transform == marketSpot.transform) { 
            foreach(PlayerCustom player in GameManager.instance.players) { 
                if(player.playerName == AuthenticationService.Instance.PlayerName && player.heatOn) { 
                    
                    if(player.marketPlace_5K.Count > 0) {
                        waitSecond = 7;
                        result = hassleSpot.transform;
                        Destroy(marketSpot.transform.GetChild(2).gameObject);
                        removeCard(pointerEventData);
                        Destroy(cash_5k.transform.GetChild(cash_5k.transform.childCount - 1).gameObject);
                        player.cash = (Int32.Parse(player.cash) - 5000).ToString();
                        GameManager.instance.changeMarketTOGreen();
                        GameManager.instance.removeHeatOnServerRpc(pointerEventData.pointerDrag.GetComponent<ShowCard>().cardData.id, player.playerName, 5, pointerEventData.pointerDrag.GetComponent<ShowCard>().cardData.effects, pointerEventData.pointerDrag.GetComponent<ShowCard>().cardData.nirvana);
                    } else if(player.marketPlace_25K.Count > 0 && player.marketPlace_25K.Count > player.protection_25K.Count) {
                        waitSecond = 7;
                        result = hassleSpot.transform;
                        Destroy(marketSpot.transform.GetChild(2).gameObject);
                        removeCard(pointerEventData);
                        Destroy(cash_25k.transform.GetChild(cash_25k.transform.childCount - 1).gameObject);
                        player.cash = (Int32.Parse(player.cash) - 25000).ToString();
                        GameManager.instance.changeMarketTOGreen();
                        GameManager.instance.removeHeatOnServerRpc(pointerEventData.pointerDrag.GetComponent<ShowCard>().cardData.id, player.playerName, 25, pointerEventData.pointerDrag.GetComponent<ShowCard>().cardData.effects, pointerEventData.pointerDrag.GetComponent<ShowCard>().cardData.nirvana);
                    } else if(player.marketPlace_50K.Count > 0 && player.marketPlace_50K.Count > player.protection_50K.Count) {
                        waitSecond = 7;
                        result = hassleSpot.transform;
                        Destroy(marketSpot.transform.GetChild(2).gameObject);
                        removeCard(pointerEventData);
                        Destroy(cash_50k.transform.GetChild(cash_50k.transform.childCount - 1).gameObject);
                        player.cash = (Int32.Parse(player.cash) - 50000).ToString();
                        GameManager.instance.changeMarketTOGreen();
                        GameManager.instance.removeHeatOnServerRpc(pointerEventData.pointerDrag.GetComponent<ShowCard>().cardData.id, player.playerName, 50, pointerEventData.pointerDrag.GetComponent<ShowCard>().cardData.effects, pointerEventData.pointerDrag.GetComponent<ShowCard>().cardData.nirvana);
                    } else if(player.marketPlace_100k.Count > 0) {
                        waitSecond = 7;
                        result = hassleSpot.transform;
                        Destroy(marketSpot.transform.GetChild(2).gameObject);
                        removeCard(pointerEventData);
                        Destroy(cash_100k.transform.GetChild(cash_100k.transform.childCount - 1).gameObject);
                        player.cash = (Int32.Parse(player.cash) - 100000).ToString();
                        GameManager.instance.changeMarketTOGreen();
                        GameManager.instance.removeHeatOnServerRpc(pointerEventData.pointerDrag.GetComponent<ShowCard>().cardData.id, player.playerName, 100, pointerEventData.pointerDrag.GetComponent<ShowCard>().cardData.effects, pointerEventData.pointerDrag.GetComponent<ShowCard>().cardData.nirvana);
                    }
                }
            }
        }
        return result;
    }

    public void removeCard(PointerEventData pointerEventData) {
        foreach(PlayerCustom player in GameManager.instance.players) {
            if(player.playerName == AuthenticationService.Instance.PlayerName) {
                for(int i = 0; i < player.Hand.Count; i++) {
                    if(player.Hand[i].id == pointerEventData.pointerDrag.GetComponent<ShowCard>().cardData.id) {
                        player.Hand.RemoveAt(i);
                    }
                }
            }
        }
    }        
}

