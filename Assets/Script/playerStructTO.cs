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

[Serializable]
public struct playerStructTO : INetworkSerializable, System.IEquatable<playerStructTO> {

    public string playerName;
    public string cash;
    public string market;
    public int numPlayer;
    public int HandLength;
    public int[] Hand;
    public bool heatOn;
    public string type_HEatOn;
    public int marketPlace_5KLength;
    public int[] marketPlace_5K;
    public int marketPlace_25KLength;
    public int[] marketPlace_25K;
    public int marketPlace_50KLength;
    public int[] marketPlace_50K;
    public int marketPlace_100KLength;
    public int[] marketPlace_100k;
    public int protection_25KLength;
    public int[] protection_25K;
    public int protection_50KLength;
    public int[] protection_50K;
    public bool myTurn;
    public bool pickFromDeck;
    public int skipTurn;

    public playerStructTO(string localPlayerName, string localCash, string localMarket, int localnumPlayer, List<CardSO> localHand, bool localHeatOn, string localTypeHeatOn, List<CardSO> localMarketPlace_5K, List<CardSO> localMarketPlace_25K,List<CardSO> localMarketPlace_50K, List<CardSO> localMarketPlace_100K, List<CardSO> localProtection_25K, List<CardSO> localProtection_50K, bool localMyturn, bool localPickFromDeck, int localSkipTurn) {

        Hand = new int[localHand.Count];
        int i = 0;
        foreach(CardSO card in localHand) {
            Hand[i] = card.id;
            i++;
        }

        HandLength = localHand.Count;

        marketPlace_5K = new int[localMarketPlace_5K.Count];
        i = 0;
        foreach(CardSO card in localMarketPlace_5K) {
            marketPlace_5K[i] = card.id;
            i++;
        }

        marketPlace_5KLength = localMarketPlace_5K.Count;

        marketPlace_25K = new int[localMarketPlace_25K.Count];
        i = 0;
        foreach(CardSO card in localMarketPlace_25K) {
            marketPlace_25K[i] = card.id;
            i++;
        }

        marketPlace_25KLength = localMarketPlace_25K.Count;

        marketPlace_50K = new int[localMarketPlace_50K.Count];
        i = 0;
        foreach(CardSO card in localMarketPlace_50K) {
            marketPlace_50K[i] = card.id;
            i++;
        }

        marketPlace_50KLength = localMarketPlace_50K.Count;

        marketPlace_100k = new int[localMarketPlace_100K.Count];
        i = 0;
        foreach(CardSO card in localMarketPlace_100K) {
            marketPlace_100k[i] = card.id;
            i++;
        }

        marketPlace_100KLength = localMarketPlace_100K.Count;

        protection_25K = new int[localProtection_25K.Count];
        i = 0;
        foreach(CardSO card in localProtection_25K) {
            protection_25K[i] = card.id;
            i++;
        }

        protection_25KLength = localProtection_25K.Count;

        protection_50K = new int[localProtection_50K.Count];
        i = 0;
        foreach(CardSO card in localProtection_50K) {
            protection_50K[i] = card.id;
            i++;
        }

        protection_50KLength = localProtection_50K.Count;


        playerName = localPlayerName;
        cash = localCash;
        market = localMarket;
        numPlayer = localnumPlayer;            
        heatOn = localHeatOn;
        type_HEatOn = localTypeHeatOn;
        myTurn = localMyturn;
        pickFromDeck = localPickFromDeck;
        skipTurn = localSkipTurn;
    }

    // INetworkSerializable
    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref playerName);
        serializer.SerializeValue(ref cash);
        serializer.SerializeValue(ref market);
        serializer.SerializeValue(ref numPlayer);
        serializer.SerializeValue(ref HandLength);

        for (int i = 0; i < HandLength; i++)
        {
            serializer.SerializeValue(ref Hand[i]);
        }

        serializer.SerializeValue(ref heatOn);
        serializer.SerializeValue(ref type_HEatOn);
        serializer.SerializeValue(ref marketPlace_5KLength);

        for (int i = 0; i < marketPlace_5KLength; i++)
        {
            serializer.SerializeValue(ref marketPlace_5K[i]);
        }

        serializer.SerializeValue(ref marketPlace_25KLength);

        for (int i = 0; i < marketPlace_25KLength; i++)
        {
            serializer.SerializeValue(ref marketPlace_25K[i]);
        }


        serializer.SerializeValue(ref marketPlace_50KLength);

        for (int i = 0; i < marketPlace_50KLength; i++)
        {
            serializer.SerializeValue(ref marketPlace_50K[i]);
        }


        serializer.SerializeValue(ref marketPlace_100KLength);

        for (int i = 0; i < marketPlace_100KLength; i++)
        {
            serializer.SerializeValue(ref marketPlace_100k[i]);
        }


        serializer.SerializeValue(ref protection_25KLength);

        for (int i = 0; i < protection_25KLength; i++)
        {
            serializer.SerializeValue(ref protection_25K[i]);
        }


        serializer.SerializeValue(ref protection_50KLength);

        for (int i = 0; i < protection_50KLength; i++)
        {
            serializer.SerializeValue(ref protection_50K[i]);
        }


        serializer.SerializeValue(ref myTurn);
        serializer.SerializeValue(ref pickFromDeck);
        serializer.SerializeValue(ref skipTurn);
        
    }
    // ~INetworkSerializable

    public bool Equals(playerStructTO other) {

        if (string.Equals(other.playerName, playerName, StringComparison.CurrentCultureIgnoreCase)) {
            if (string.Equals(other.cash, cash, StringComparison.CurrentCultureIgnoreCase)) {
                if (string.Equals(other.market, market, StringComparison.CurrentCultureIgnoreCase)) {
                    if (other.numPlayer == numPlayer) {
                        if (other.heatOn == heatOn) {
                            if (string.Equals(other.type_HEatOn, type_HEatOn, StringComparison.CurrentCultureIgnoreCase)) {
                                if (other.myTurn == myTurn) {
                                    if (other.pickFromDeck == pickFromDeck) {
                                        if (other.skipTurn == skipTurn) {

                                            if(other.Hand == null && Hand == null) {
                                                if(other.marketPlace_5K == null && marketPlace_5K == null) {
                                                    if(other.marketPlace_25K == null && marketPlace_25K == null) {
                                                        if(other.marketPlace_50K == null && marketPlace_50K == null) {
                                                            if(other.marketPlace_100k == null && marketPlace_100k == null) {
                                                                if(other.protection_25K == null && protection_25K == null) {
                                                                    if(other.protection_50K == null && protection_50K == null) {
                                                                        
                                                                    } else {
                                                                        return false;
                                                                    }
                                                                } else {
                                                                    return false;
                                                                }
                                                            } else {
                                                                return false;
                                                            }
                                                        } else {
                                                            return false;
                                                        }
                                                    } else {
                                                        return false;
                                                    }
                                                } else {
                                                    return false;
                                                }
                                            } else {
                                                return false;
                                            }

                                            if(other.Hand.Length == Hand.Length) {
                                                for (int i = 0; i < Hand.Length; i++) {
                                                    if(other.Hand[i] != Hand[i]) {
                                                        return false;
                                                    }
                                                }
                                            } else {
                                                return false;
                                            }

                                            if(other.marketPlace_5K.Length == marketPlace_5K.Length) {
                                                for (int i = 0; i < marketPlace_5K.Length; i++) {
                                                    if(other.marketPlace_5K[i] != marketPlace_5K[i]) {
                                                        return false;
                                                    }
                                                }
                                            }  else {
                                                return false;
                                            }

                                            if(other.marketPlace_25K.Length == marketPlace_25K.Length) {
                                                for (int i = 0; i < marketPlace_25K.Length; i++) {
                                                    if(other.marketPlace_25K[i] != marketPlace_25K[i]) {
                                                        return false;
                                                    }
                                                }
                                            }  else {
                                                return false;
                                            }

                                            if(other.marketPlace_50K.Length == marketPlace_50K.Length) {
                                                for (int i = 0; i < marketPlace_50K.Length; i++) {
                                                    if(other.marketPlace_50K[i] != marketPlace_50K[i]) {
                                                        return false;
                                                    }
                                                }
                                            }  else {
                                                return false;
                                            }

                                            if(other.marketPlace_100k.Length == marketPlace_100k.Length) {
                                                for (int i = 0; i < marketPlace_100k.Length; i++) {
                                                    if(other.marketPlace_100k[i] != marketPlace_100k[i]) {
                                                        return false;
                                                    }
                                                }
                                            }  else {
                                                return false;
                                            }

                                            if(other.protection_25K.Length == protection_25K.Length) {
                                                for (int i = 0; i < protection_25K.Length; i++) {
                                                    if(other.protection_25K[i] != protection_25K[i]) {
                                                        return false;
                                                    }
                                                }
                                            }  else {
                                                return false;
                                            }

                                            if(other.protection_50K.Length == protection_50K.Length) {
                                                for (int i = 0; i < protection_50K.Length; i++) {
                                                    if(other.protection_50K[i] != protection_50K[i]) {
                                                        return false;
                                                    }
                                                }
                                            }  else {
                                                return false;
                                            }
                                            
                                            return true;
                                        }
                                    }
                                }
                            }   
                        }
                    }
                }
            }   
        }

        return false;
    }
}