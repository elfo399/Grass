<div align="center">
  <picture>
    <source media="(prefers-color-scheme: dark)" srcset="Assets/resourcies/Loghi/White.png">
    <img src="Assets/resourcies/Loghi/Black.png" alt="Logo di Grass" width="520">
  </picture>

  <h1>Grass: The Card Game</h1>

  <p><em>Apri il mercato, proteggi i profitti e manda in fumo i piani degli avversari.</em></p>

  <p>
    <img src="https://img.shields.io/badge/Unity-2021.3.21f1-222222?logo=unity&logoColor=white" alt="Unity 2021.3.21f1">
    <img src="https://img.shields.io/badge/C%23-.NET-512BD4?logo=csharp&logoColor=white" alt="C#">
    <img src="https://img.shields.io/badge/Multiplayer-2--4%20giocatori-16A34A" alt="Multiplayer da 2 a 4 giocatori">
    <img src="https://img.shields.io/badge/Mazzo-104%20carte-EAB308" alt="Mazzo da 104 carte">
  </p>
</div>

---

**Grass** è un adattamento digitale multiplayer del card game omonimo. Da 2 a 4 giocatori competono in una partita a turni per costruire il mercato più redditizio, proteggere i guadagni e ostacolare gli avversari con controlli, multe, furti e carte speciali.

> **Contenuto 18+** — Il gioco usa in chiave satirica riferimenti allo spaccio e al consumo di cannabis.

## Indice

- [Il gioco in breve](#il-gioco-in-breve)
- [Come si gioca](#come-si-gioca)
- [Carte](#carte)
- [Controlli](#controlli)
- [Avvio del progetto](#avvio-del-progetto)
- [Testare il multiplayer](#testare-il-multiplayer)
- [Tecnologie e architettura](#tecnologie-e-architettura)
- [Struttura del repository](#struttura-del-repository)
- [Documentazione](#documentazione)
- [Stato del progetto](#stato-del-progetto)

## Il gioco in breve

| | |
|---|---|
| **Giocatori** | Da 2 a 4, online |
| **Modalità** | Competitiva, a turni |
| **Mazzo** | 104 carte |
| **Mano iniziale** | 6 carte per giocatore |
| **Obiettivo** | Terminare la partita con il patrimonio più alto |
| **Fine partita** | Carta `Market Close` oppure esaurimento del mazzo |

Ogni turno inizia pescando una carta. Il giocatore sceglie poi come sviluppare il proprio mercato o come rallentare quello degli altri, trascinando una carta dalla mano verso lo spazio valido sul tavolo.

## Come si gioca

1. **Entra nella partita** — scegli un nickname, crea una lobby pubblica o privata oppure unisciti tramite elenco o codice.
2. **Preparati** — quando tutti i giocatori sono presenti e hanno confermato lo stato *Ready*, l'host può avviare la partita.
3. **Pesca** — all'inizio del tuo turno seleziona il mazzo per aggiungere una carta alla mano.
4. **Gioca una carta** — apri il mercato, accumula carte `Peddle`, proteggi i profitti oppure usa una carta d'attacco o utilità.
5. **Chiudi e calcola** — `Market Close` conclude la partita; profitti, protezioni, penalità e bonus determinano la classifica finale.

## Carte

| Categoria | Funzione |
|---|---|
| **Market Open / Close** | Apre il mercato personale o termina la partita. |
| **Peddle** | Genera profitti da **$5.000**, **$25.000**, **$50.000** o **$100.000**. |
| **Protected** | Mette al sicuro profitti fino a $25.000 o $50.000. |
| **Heat On** | Blocca temporaneamente il mercato di un avversario. |
| **Heat Off / Pay Fine** | Rimuove un blocco specifico o consente di pagare per liberarsene. |
| **Stonehigh / Euphoria** | Elimina un `Heat On` e attiva effetti favorevoli sui profitti. |
| **Sold Out / Doublecrossed / Utterly Wiped Out** | Impone scarti, turni saltati e possibili penalità a fine partita. |
| **Steal Your Neighbor's Pot** | Ruba un profitto non protetto da un mercato avversario valido. |
| **The Banker** | Assegna un bonus economico al possessore durante il conteggio finale. |

Il regolamento completo è consultabile anche direttamente dal menu del gioco.

## Controlli

Il progetto usa un'interfaccia completamente controllabile con il mouse.

| Azione | Comando |
|---|---|
| Navigare nei menu | **Clic sinistro** |
| Pescare | **Clic** sul mazzo durante il proprio turno |
| Giocare una carta | **Trascina e rilascia** dalla mano allo spazio di destinazione |
| Selezionare carte durante furti o maledizioni | **Clic** sulla carta richiesta |
| Ispezionare un giocatore | **Clic** sul relativo riquadro |

## Avvio del progetto

### Requisiti

- [Unity Hub](https://unity.com/download)
- **Unity Editor 2021.3.21f1 LTS**
- Connessione a Internet per Authentication, Lobby e Relay
- Un progetto Unity Gaming Services configurato, se non si usa il collegamento UGS già salvato nel repository

### Esecuzione nell'Editor

1. Apri Unity Hub e seleziona **Add project from disk**.
2. Indica la cartella principale del repository, quella che contiene `Assets`, `Packages` e `ProjectSettings`.
3. Apri il progetto con Unity **2021.3.21f1** e attendi il ripristino dei pacchetti.
4. Apri `Assets/Scenes/Game.unity`.
5. Premi **Play**.

La scena `Game` è già registrata nelle Build Settings ed è l'unica scena necessaria: menu, lobby, tutorial e partita sono pannelli gestiti nello stesso flusso UI.

### Configurazione Unity Gaming Services

Il multiplayer richiede questi servizi:

- **Authentication** con accesso anonimo;
- **Lobby** per ricerca, creazione, codici e stato *Ready*;
- **Relay** per collegare host e client senza esporre direttamente la rete locale.

Se il progetto viene associato a un nuovo ambiente UGS, abilita i tre servizi dal Unity Dashboard e verifica il collegamento in **Project Settings → Services**.

## Testare il multiplayer

Per una prova locale servono almeno due istanze dell'applicazione:

1. crea una build standalone da **File → Build Settings**;
2. avvia una sessione nell'Editor e una dalla build, oppure usa due build separate;
3. nella prima istanza crea la lobby, scegli il numero di giocatori e impostati su *Ready*;
4. nelle altre istanze cerca la lobby pubblica o inserisci il codice mostrato dall'host;
5. quando tutti sono pronti, l'host avvia la partita.

## Tecnologie e architettura

| Componente | Ruolo nel progetto |
|---|---|
| **Unity 2021 LTS + C#** | Runtime, scena, UI, audio e video. |
| **Netcode for GameObjects 1.2.0** | RPC e sincronizzazione dello stato condiviso. |
| **Unity Transport 1.3.4** | Trasporto di rete usato da Netcode. |
| **UGS Authentication 2.6.1** | Identità anonima e nickname del giocatore. |
| **UGS Lobby 1.0.3** | Lobby pubbliche/private, codici, heartbeat e stato dei partecipanti. |
| **UGS Relay 1.0.5** | Connessione tra host e client. |
| **uGUI + TextMesh Pro** | Menu, tavolo di gioco, testi e controlli. |
| **Paroxe PDF Renderer** | Visualizzazione del regolamento PDF dentro l'applicazione. |

### Responsabilità degli script principali

| Script | Responsabilità |
|---|---|
| `GameManager.cs` | Flusso della partita, turni, mazzo, RPC, stato sincronizzato e classifica finale. |
| `LobbyManager.cs` | Creazione e ricerca lobby, codici, heartbeat, giocatori pronti e avvio host. |
| `PlayerManager.cs` | Inizializzazione UGS, autenticazione anonima e nickname. |
| `MenuManager.cs` | Navigazione tra menu, tutorial, lobby e tavolo tramite transizioni video. |
| `CardSO.cs` | Modello dati delle carte realizzato con `ScriptableObject`. |
| `Drag.cs` / `Drop.cs` | Interazione drag-and-drop e applicazione delle regole per ogni tipo di carta. |
| `StructTO.cs` / `playerStructTO.cs` | Serializzazione dello stato di gioco per la rete. |
| `AudioManager.cs` | Musica, effetti, mixer, volumi e mute persistente. |

Lo stato condiviso è mantenuto in `NetworkList` e aggiornato attraverso `ServerRpc` e `ClientRpc`. L'host costruisce e mescola il mazzo, distribuisce sei carte a ciascun partecipante e propaga ai client le variazioni di mano, mercato, protezioni, blocchi e turni.

## Struttura del repository

```text
Grass/
├── Assets/
│   ├── Card/                 # Immagini e ScriptableObject delle 104 carte
│   ├── Scenes/Game.unity     # Scena principale
│   ├── Script/               # Gameplay, UI, audio e multiplayer
│   ├── resourcies/           # Tutorial, loghi, font, video e regolamento
│   ├── Paroxe/PDFRenderer/   # Plugin per la lettura del PDF
│   └── TextMesh Pro/         # Risorse testuali
├── Doc/                      # Game Design Document
├── Packages/                 # Manifest e lock dei pacchetti Unity
└── ProjectSettings/          # Configurazione del progetto Unity
```

## Documentazione

- [Regolamento completo in italiano](<Assets/resourcies/PDF/Grass_Rules_2022_IT-Italian-Italiano_A4_organized (1).pdf>)
- [Game Design Document](<Doc/GDD - Game Design Document - template.docx>)
- [Sito del gioco](https://www.grasscardgame.com/)

## Stato del progetto

Il repository contiene il ciclo principale di una partita singola: autenticazione, lobby, Relay, distribuzione delle carte, gestione dei turni, effetti, sincronizzazione e classifica finale.

Elementi indicati come sviluppi successivi nella documentazione o nel codice:

- accordi commerciali e scambi forzati;
- modalità a più round fino al raggiungimento di $250.000;
- inizializzazione della chat vocale Vivox, presente tra le dipendenze ma attualmente disabilitata nel codice;
- test automatici e pipeline di build/distribuzione.

## Licenza e attribuzioni

Il repository non contiene al momento un file `LICENSE`. Prima di riutilizzare o distribuire codice, carte, font, audio, video o altri asset, verificane titolarità e condizioni d'uso. Il GDD attribuisce le illustrazioni originali delle carte a **Jeff London (1979)**.

---

<div align="center">
  <sub>Progetto Unity configurato come <strong>GRASS</strong> · PinkoFlamingo</sub>
</div>
