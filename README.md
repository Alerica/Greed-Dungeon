## Developer & Contributions
**Alerica** (Game Designer & Artist) <br>
**adxze** (Game Programmer & Artist) <br>
**Albert** (Game programmer & Artist)

## About
Greed Dungeon is a strategic deck building roguelike where players battle through increasingly difficult stages using a deck of powerful cards. Manage your energy, build devastating combos, and survive against waves of enemies with unique status effects. Choose between retreating with your rewards or pushing deeper into the dungeon for greater treasures. Every decision matters as you balance risk and reward in this challenging turn-based adventure.

<br>

## Key Features

**Strategic Card Combat**: Deploy cards from your hand by dragging them to the battlefield. Each card costs energy and unleashes powerful effects, from direct damage to debilitating status effects like Burn, Stun, Frost, and Bleed.

**Progressive Difficulty**: Face increasingly powerful enemies as you advance through stages. Every 5 stages features a boss battle with enhanced health and abilities. Decide when to retreat and claim your rewards or push forward for greater glory.

**Risk Management**: Players have the option to continue or go back and claim the reward, if the player dies they will not get anything.

<table>
<tr>
<td align="center" width="50%">

<img width="100%" alt="gif1" src="https://github.com/adxze/adxze/blob/main/GDpic/Screenshot%202025-10-03%20at%2015.39.56.png">


</td>
<td align="center" width="50%">

<img width="100%" alt="gif1" src="https://github.com/adxze/adxze/blob/main/GDpic/Screenshot%202025-10-03%20at%2015.40.16.png">

</td>
</tr>
</table>

## Scene Flow
```mermaid
flowchart LR
  mm[Main Menu]
  battle[Battle]
  choice[Retreat or Continue]
  reward[Rewards Screen]
  defeat[Game Over]

  mm -- "Start Battle" --> battle
  battle -- "Stage Complete" --> choice
  choice -- "Continue" --> battle
  choice -- "Retreat" --> reward
  battle -- "Player Dies" --> defeat
  defeat -- "Restart" --> mm
  reward -- "Return" --> mm
```

## Layer / Module Design
```mermaid
---
config:
  theme: neutral
  look: neo
---
graph TD
    subgraph "Core Battle System"
        BM[BattleManager]
        BS[Battle State Machine]
        UI[Battle UI Manager]
    end
    
    subgraph "Card System"
        Card[Card Component]
        CardVisual[Card Visual]
        CardData[Card Data SO]
        CardHolder[Horizontal Card Holder]
        CardEffect[Card Effects]
    end
    
    subgraph "Deck Management"
        Deck[Deck Queue]
        Draw[Draw System]
        Shuffle[Shuffle Logic]
    end
    
    subgraph "Combat Entities"
        Player[Player]
        Enemy[Enemy]
        EnemySpawner[Enemy Spawner]
        StatusEffect[Status Effects]
    end
    
    subgraph "Dice System"
        DiceManager[Dice Manager 2D UI]
        Dice2D[Dice 2D UI]
    end
    
    subgraph "Audio System"
        AudioMgr[Audio Manager]
        SFX[SFX Library]
        Music[Music Library]
    end
    
    subgraph "Visual Effects"
        VFX[VFX Spawners]
        Lightning[Lightning Spawner]
        Generic[Generic Spawner]
        FloatingText[Floating Text]
    end
    
    BM --> BS
    BM --> Deck
    BM --> UI
    BM --> EnemySpawner
    
    Card --> CardVisual
    Card --> CardData
    CardHolder --> Card
    
    CardVisual --> CardEffect
    CardEffect --> Enemy
    CardEffect --> Player
    
    Enemy --> StatusEffect
    Player --> StatusEffect
    
    BM --> Player
    BM --> Enemy
    
    CardVisual --> VFX
    VFX --> Lightning
    VFX --> Generic
    
    DiceManager --> Dice2D
    
    UI --> FloatingText
    
    BM --> AudioMgr
    AudioMgr --> SFX
    AudioMgr --> Music
    
    classDef coreStyle fill:#e1f5fe,stroke:#01579b,stroke-width:2px
    classDef cardStyle fill:#e8f5e8,stroke:#1b5e20,stroke-width:2px
    classDef combatStyle fill:#ffebee,stroke:#b71c1c,stroke-width:2px
    classDef systemStyle fill:#fff3e0,stroke:#e65100,stroke-width:2px
    classDef audioStyle fill:#f3e5f5,stroke:#4a148c,stroke-width:2px
    
    class BM,BS,UI coreStyle
    class Card,CardVisual,CardData,CardHolder,CardEffect cardStyle
    class Player,Enemy,EnemySpawner,StatusEffect combatStyle
    class Deck,Draw,Shuffle,DiceManager,Dice2D,VFX,Lightning,Generic,FloatingText systemStyle
    class AudioMgr,SFX,Music audioStyle
```

## Modules and Features

The strategic card-based roguelike gameplay with turn-based combat, deck management, status effects, and progressive difficulty is powered by a comprehensive Unity C# scripting system.

| 📂 Name | 🎬 Scene | 📋 Responsibility |
|---------|----------|-------------------|
| **Battle System** | **Battle** | - Control overall battle state machine (NotStarted, PlayerTurn, EnemyTurn)<br/>- Manage turn flow and stage progression<br/>- Handle energy system (gain 3 per turn, max increases per stage)<br/>- Queue and shuffle deck with draw mechanics<br/>- Calculate rewards and difficulty scaling (1.04x health per stage)<br/>- Trigger retreat/continue decisions every 2+ stages |
| **Card System** | **Battle** | - Handle 3D card visuals with drag-and-drop interactions<br/>- Animate cards with curve positioning, tilt, and scale effects<br/>- Process card usage when dragged to top half of screen<br/>- Display card data (name, cost, description, artwork)<br/>- Apply card effects to random enemies<br/>- Manage hand layout with swap animations<br/>- Draw cards from deck and destroy after use |
| **Combat Entities** | **Battle** | - Track player/enemy health, attack, and defense<br/>- Process damage with defense reduction formula: `damage × (100/(100+defense))`<br/>- Apply and manage status effects (Burn, Stun, Frost, Bleed, etc.)<br/>- Execute turn-based effect processing<br/>- Spawn enemies with scaling health (boss every 5 stages)<br/>- Handle death and victory conditions |
| **Dice System** | **Battle** | - Manage collection of 2D dice with dynamic add/remove<br/>- Animate dice rolling with spin sprites and jump arcs<br/>- Roll all dice in wave pattern with delays<br/>- Calculate and report total results via callbacks<br/>- Play dice roll sound effects<br/>- Support auto-destroy after rolling |
| **Audio System** | **Persistent** | - Manage SFX and music libraries with volume control<br/>- Pool audio sources for efficient playback<br/>- Crossfade between music tracks<br/>- Support pitch randomization for variety<br/>- Persist across scene loads with DontDestroyOnLoad<br/>- Handle WebGL audio unlock |
| **Visual Effects** | **Battle** | - Display turn banners (Player Turn, Enemy Turn, Battle Start)<br/>- Show status effect notifications with colors<br/>- Spawn floating damage/heal numbers<br/>- Animate enemy attack jumps<br/>- Cast card VFX (Lightning, Generic effects)<br/>- Animate background transitions on stage completion |
| **UI Management** | **Battle** | - Update player health, energy, and stats display<br/>- Show current turn and stage numbers<br/>- Display enemy count and deck size<br/>- Spawn floating text for damage/heal/energy changes<br/>- Control battle UI panel visibility<br/>- Manage choice panels (retreat/continue) |

<br>

## Game Flow Chart
```mermaid
---
config:
  theme: redux
  look: neo
---
flowchart TD
  start([Battle Start])
  start --> dealCards[Deal Initial Hand]
  
  dealCards --> playerTurn[Player Turn Start]
  playerTurn --> drawCards[Draw Cards]
  drawCards --> addEnergy[Add Energy]
  
  addEnergy --> playerAction{Player Action}
  
  playerAction -->|Drag Card Up| checkEnergy{Enough Energy?}
  playerAction -->|End Turn Button| endTurn[End Player Turn]
  
  checkEnergy -->|Yes| castCard[Cast Card]
  checkEnergy -->|No| cancelCard[Cancel Card]
  
  castCard --> applyEffects[Apply Card Effects]
  applyEffects --> removeEnergy[Remove Energy Cost]
  removeEnergy --> destroyCard[Destroy Card]
  destroyCard --> checkEnemies{All Enemies Dead?}
  
  checkEnemies -->|Yes| stageComplete[Stage Complete]
  checkEnemies -->|No| playerAction
  
  cancelCard --> playerAction
  
  endTurn --> enemyTurn[Enemy Turn Start]
  
  enemyTurn --> processStatus[Process Status Effects]
  processStatus --> checkStun{Enemy Stunned?}
  
  checkStun -->|Yes| skipAttack[Skip Attack]
  checkStun -->|No| enemyAttack[Enemy Attacks]
  
  enemyAttack --> damagePlayer[Damage Player]
  skipAttack --> nextEnemy{More Enemies?}
  damagePlayer --> nextEnemy
  
  nextEnemy -->|Yes| processStatus
  nextEnemy -->|No| checkPlayerDead{Player Dead?}
  
  checkPlayerDead -->|Yes| gameOver[Game Over]
  checkPlayerDead -->|No| playerTurn
  
  stageComplete --> increaseStage[Increase Stage]
  increaseStage --> checkStage{Stage >= 2?}
  
  checkStage -->|Yes| showChoice[Show Retreat/Continue]
  checkStage -->|No| nextStage[Spawn Next Enemies]
  
  showChoice --> choice{Player Choice}
  
  choice -->|Continue| increaseMaxEnergy[Increase Max Energy]
  choice -->|Retreat| calculateReward[Calculate Rewards]
  
  increaseMaxEnergy --> checkBoss{Stage % 5 == 0?}
  
  checkBoss -->|Yes| spawnBoss[Spawn Boss]
  checkBoss -->|No| spawnEnemies[Spawn Normal Enemies]
  
  spawnBoss --> scaleHealth[Scale Boss Health]
  spawnEnemies --> scaleHealth
  
  scaleHealth --> nextStage
  nextStage --> playerTurn
  
  calculateReward --> showRewards[Display Rewards Screen]
  showRewards --> reset[Reset Game]
  reset --> mainMenu[Return to Main Menu]
  
  gameOver --> calculateReward
```

<br>

## Event Signal Diagram
```mermaid
classDiagram
    %% --- Battle Management ---
    class BattleManager {
        +OnBattleStart()
        +OnStageComplete(stage: int)
        +OnPlayerTurnStart()
        +OnEnemyTurnStart()
        +OnBattleEnd()
    }

    class BattleUIManager {
        +OnHealthUpdate(health: int)
        +OnEnergyUpdate(energy: int)
        +OnStageUpdate(stage: int)
        +OnTurnUpdate(turn: int)
    }

    %% --- Card System ---
    class Card {
        +PointerEnterEvent
        +PointerExitEvent
        +PointerDownEvent
        +PointerUpEvent
        +BeginDragEvent
        +EndDragEvent
        +SelectEvent
    }

    class CardVisual {
        +OnCardUsed(cardData: CardData)
        +OnEffectApplied(effect: CardEffect)
    }

    class HorizontalCardHolder {
        +OnCardDrawn(card: Card)
        +OnCardDestroyed(card: Card)
        +OnHandUpdated()
    }

    %% --- Dice System ---
    class DiceManager2DUI {
        +OnTotalRolled(total: int)
        +OnDiceAdded(count: int)
        +OnDiceCleared()
    }

    class Dice2DUI {
        +OnRolled(value: int)
        +OnRolledUnityEvent(value: int)
    }

    %% --- Combat Entities ---
    class Player {
        +OnHealthChanged(current: float)
        +OnDamageTaken(amount: float)
        +OnHealed(amount: float)
        +OnDeath()
    }

    class Enemy {
        +OnDamageTaken(amount: float, source: string)
        +OnStatusApplied(effect: StatusEffect)
        +OnTurnEffectsProcessed()
        +OnDeath()
    }

    class EnemySpawner {
        +OnEnemySpawned(enemy: GameObject)
    }

    %% --- Audio System ---
    class AudioManager {
        +OnSFXPlayed(key: string)
        +OnMusicChanged(key: string)
        +OnVolumeChanged(type: string, value: float)
    }

    %% --- Relations ---
    BattleManager --> BattleUIManager : updates
    BattleManager --> Player : manages
    BattleManager --> Enemy : manages
    BattleManager --> EnemySpawner : controls
    BattleManager --> HorizontalCardHolder : controls
    
    Card --> CardVisual : creates
    CardVisual --> Enemy : damages
    CardVisual --> Player : heals
    
    HorizontalCardHolder --> Card : manages
    
    DiceManager2DUI --> Dice2DUI : controls
    
    Enemy --> BattleManager : notifies death
    Player --> BattleManager : notifies death
    
    CardVisual --> AudioManager : requests sounds
    BattleManager --> AudioManager : requests music
```

<br>

## Controls
- **Left Click & Drag** - Drag card from hand
- **Drag to Top Half** - Cast card (if enough energy)
- **Hover Card** - Show enlarged preview
- **Hold Hover 2s** - Display card description tooltip
- **Right Click** - Deselect all cards
- **End Turn Button** - End player turn manually



## Progression System
- Stages increase in difficulty
- Enemy health scales: `baseHealth × 1.04^(stage-1)`
- Every 5 stages: Boss battle
- Every 2 stages: Choice to retreat or continue
- Retreating awards points based on:
  - Base points: 10
  - Enemy defeats: `(80 + stage×3) × enemiesDefeated`
  - Boss defeats: `1000 × bossesDefeated`
  - Stage bonus: `basePoints + stage×5×stage`
