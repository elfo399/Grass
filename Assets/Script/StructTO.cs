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

public struct StructTO : INetworkSerializable, System.IEquatable<StructTO> {
    
    public int deckLength;
    public int[] deck;
    public int playersLength;
    public playerStructTO[] players;

    public StructTO(List<PlayerCustom> localPlayers, List<CardSO> localDeck) {

        deck = new int[localDeck.Count];
        int i = 0;
        foreach(CardSO card in localDeck) {
            deck[i] = card.id;
            i++;
        }

        deckLength = localDeck.Count;

        players = new playerStructTO[localPlayers.Count];
        i = 0;
        foreach(PlayerCustom player in localPlayers) {
            players[i] = new playerStructTO(player.playerName, player.cash, player.market.ToString(), player.numPlayer, player.Hand, player.heatOn, player.type_HEatOn.ToString(), player.marketPlace_5K, player.marketPlace_25K, player.marketPlace_50K, player.marketPlace_100k, player.protection_25K, player.protection_50K, player.myTurn, player.pickFromDeck, player.skipTurn);
            i++;
        }

        playersLength = localPlayers.Count;
    }


    // INetworkSerializable
    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {

        if(deck == null || players == null) {
            return;
        }
        // Serializza la lunghezza dell'array 'deck'
        serializer.SerializeValue(ref deckLength);

        // Serializza ciascun elemento nell'array 'deck'
        for (int i = 0; i < deckLength; i++)
        {
            serializer.SerializeValue(ref deck[i]);
        }

        // Serializza la lunghezza dell'array 'players'
        serializer.SerializeValue(ref playersLength);

        // Serializza o deserializza ciascuna istanza di 'playerStructTO' nell'array 'players'
        for (int i = 0; i < playersLength; i++)
        {
            players[i].NetworkSerialize(serializer);
        }
    }
    // ~INetworkSerializable

    public bool Equals(StructTO other) {

        if(other.deck == null && deck == null) {
            if(other.players == null && players == null) {
                return true;
            } else {
                if(other.players != null && players != null) {
                } else {
                    return false;
                }
            }
        } else {
            if(other.deck != null && deck != null) {
            } else {
                return false;
            }
        }
        
        if(other.deck.Length == deck.Length) {
            for (int i = 0; i < deck.Length; i++) {
                if(other.deck[i] != deck[i]) {
                    return false;
                }
            }
        } else {
            return false;
        }

        if(other.players.Length == players.Length) {
            for (int i = 0; i < players.Length; i++) {
                if(!players[i].Equals(other.players[i])) {
                    return false;
                }
            }
        } else {
            return false;
        }

        return true;
    }
}