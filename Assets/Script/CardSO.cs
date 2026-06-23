using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;
using Object = UnityEngine.Object;
using System;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Card", menuName = "Grass/Card", order = 1)]
public class CardSO : ScriptableObject {

    public int id;
    public string cardName;
    public Sprite image;
    public int maxCard;
    public Heat heat;
    public Cash cash;
    public Protection protection;
    public Nirvana nirvana;
    public Maledizioni maledizioni;
    public Effects effects;
    public bool inDeck = true;
    public bool onBoard = false;

    public static CardSO createInstance(int code, CardSO card) {
        CardSO result = ScriptableObject.CreateInstance<CardSO>();
        result.id = code;
        result.cardName = card.cardName;
        result.image = card.image;
        result.maxCard = card.maxCard;
        result.heat = card.heat;
        result.cash = card.cash;
        result.protection = card.protection;
        result.nirvana = card.nirvana;
        result.maledizioni = card.maledizioni;
        result.effects = card.effects;
        result.inDeck = card.inDeck;
        result.onBoard = card.onBoard;
        return result;
    }

    public enum Effects {
        Null,
        MARKET_OPEN,
        MARKET_CLOSE,
        SOLDI,
        HEAT_ON,
        HEAT_OFF,
        NIRVANA,
        MALEDIZIONI,
        BANKER,
        STEAL,
        PAY_FINE,
        PROTECTION
    }

    public enum Heat {
        Null,
        FELONY,
        DETAINED,
        SS,
        BUST

    }

    public enum Cash {
        Null,
        K5 = 5,
        K25 = 25,
        K50 = 50,
        K100 = 100
    }

    public enum Protection {
        Null,
        K25 = 25,
        K50 = 50
    }

    public enum Nirvana {
        Null,
        STONEHIGH,
        EUPHORIA
    }

    public enum Maledizioni {
        Null,
        SOLD_OUT,
        DOUBLECROSSED,
        UTTERLY_WIPED_OUT
    }

}