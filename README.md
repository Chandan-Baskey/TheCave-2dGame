<p align="center">
  <img src="https://github.com/Chandan-Baskey/TheCave-2dGame/blob/78952d69bc35617b33004612b6f429bacfd49ff9/Escape.jpg?raw=true" alt="Escape The Cave banner" width="100%">
</p>

<h1 align="center">Escape The Cave</h1>

<p align="center">
  <img src="https://img.shields.io/badge/Unity-2D-000000?logo=unity&logoColor=white">
  <img src="https://img.shields.io/badge/Language-C%23-239120?logo=csharp&logoColor=white">
  <img src="https://img.shields.io/badge/Genre-Platformer-blue">
  <img src="https://img.shields.io/badge/Physics-Rigidbody2D-orange">
  <img src="https://img.shields.io/badge/UI-TextMeshPro-purple">
  <img src="https://img.shields.io/badge/Status-In%20Development-yellow">
</p>

**Run. Jump. Climb. Shoot. Get out alive** 

**Escape The Cave** is a single-player survival action-adventure shooting game set deep underground. Navigate dangerous cave levels, blast patrolling enemies, dodge deadly hazards, collect coins, and fight your way to the exit — with only three lives standing between you and a full reset.

---

## 📸 Screenshots

<p align="center">
  <img src="https://github.com/Chandan-Baskey/TheCave-2dGame/blob/78952d69bc35617b33004612b6f429bacfd49ff9/Main%20%20View.png?raw=true" width="600">
  <br><em>Start View — climbable terrain and enemy encounters</em>
</p>

| Game Level | Game Level |
|---|---|
| <img src="https://github.com/Chandan-Baskey/TheCave-2dGame/blob/78952d69bc35617b33004612b6f429bacfd49ff9/Escape%20View2.jpg?raw=true" width="400"> | <img src="https://github.com/Chandan-Baskey/TheCave-2dGame/blob/78952d69bc35617b33004612b6f429bacfd49ff9/Escape%20view.jpg?raw=true" width="400"> |
---

## 🎮 Features

- **Full 2D platformer movement** — run, jump, and double-jump with `Rigidbody2D` physics
- **Vertical climbing system** — dedicated `Climb` layer with its own gravity override and looping climb audio
- **Ranged combat** — player fires directional bullets that destroy enemies on contact
- **Patrolling enemies** — enemies reverse direction automatically when they leave a trigger zone (edge/wall detection)
- **Coin collection & scoring** — coins add to score and update the HUD in real time via TextMeshPro
- **Lives & respawn system** — player has multiple lives; death reloads the current scene, and running out of lives restarts the level sequence
- **Persistent game state** — `GameSession` and `ScenePersist` survive scene loads using `DontDestroyOnLoad` and self-destruct when returning to the Main Menu
- **Level transitions** — trigger-based exit zones load the next scene in the build order after a short delay, wrapping back to Level 1 after the final level
- **Pause to Menu** — pressing `Escape` cleans up session state and returns to the Main Menu at any time
- **Oscillating platforms/hazards** — `Vector3.Lerp` + `PingPong` driven back-and-forth movement for moving level geometry
- **Audio feedback** — footsteps, jump, climb loop, coin pickup, and death clips

---

## 🏗️ Architecture Overview

```
┌─────────────────────────────────────────────────────────────────────┐
│                            MAIN MENU (Scene 0)                      │
│                               MainUI.cs                              │
│                    PLayGame() ──► LoadScene(1)                       │
└───────────────────────────────┬───────────────────────────────────┘
                                 │
                                 ▼
┌─────────────────────────────────────────────────────────────────────┐
│                        GAMEPLAY SCENES (1..N)                       │
│                                                                       │
│  ┌───────────────┐        ┌────────────────┐      ┌──────────────┐ │
│  │ PlayerMovement │──────► │     Bullet     │      │ EnemyMovement│ │
│  │  Run/Jump/     │  Fire  │  travels, kills│─────►│  patrol,     │ │
│  │  Climb/Die     │        │  "Enemy" tag   │ hits │  flip on exit│ │
│  └──────┬─────────┘        └────────────────┘      └──────────────┘ │
│         │ dies                                                       │
│         ▼                                                            │
│  ┌───────────────┐   AddToScore()   ┌───────────────┐                │
│  │  GameSession   │◄────────────────│  CoinPickup    │                │
│  │  lives/score/  │                 │  destroys self │                │
│  │  HUD text      │                 └───────────────┘                │
│  └──────┬─────────┘                                                  │
│         │ PlayerDeath()                                              │
│         │  ├─ lives > 1 → reload current scene                       │
│         │  └─ lives = 0 → ScenePersist.Reset() + LoadScene(1)        │
│         ▼                                                            │
│  ┌───────────────┐        ┌────────────────┐      ┌──────────────┐ │
│  │  ScenePersist  │        │    LvLExit     │      │ FloorMovement│ │
│  │  DontDestroy,  │        │  trigger→delay │      │ (Oscillator) │ │
│  │  cleans on     │        │  →LoadScene    │      │  moving      │ │
│  │  Main Menu load│        │  (next / wrap) │      │  platforms   │ │
│  └───────────────┘        └────────────────┘      └──────────────┘ │
│                                                                       │
│  ┌───────────────┐                                                   │
│  │  PauseToMenu   │  Esc ► destroy GameSession, reset ScenePersist,  │
│  │                │      LoadScene(0)                                 │
│  └───────────────┘                                                   │
└───────────────────────────────────────────────────────────────────────┘
```

---

## 📜 Script Breakdown

### `PlayerMovement.cs`
The core player controller. Handles horizontal movement, jumping, double-jumping, climbing, flipping, shooting, and death, all driven from a single `Update()` loop.

| Inspector Field | Type | Purpose |
|---|---|---|
| `moveSpeed` | `float` | Horizontal run speed |
| `jumpSpeed` | `float` | Initial jump velocity |
| `doublejumpSpeed` | `float` | Velocity applied on the air double-jump |
| `climbSpeed` | `float` | Vertical speed while climbing |
| `bullet` | `GameObject` | Bullet prefab spawned on attack |
| `gun` | `Transform` | Muzzle point bullets spawn from |
| `kick` | `Vector2` | Velocity applied to the body on death (default `(0, 10)`) |
| `groundChecker` | `Transform` | Origin of the ground `OverlapCircle` check |
| `groundLayer` | `LayerMask` | Layer(s) considered "ground" |
| `moveClip` / `jumpClip` / `dieClip` / `climbClip` | `AudioClip` | Footstep, jump, death, and climb loop sounds |

Key behavior:
```csharp
void Die()
{
    if (playerCollider.IsTouchingLayers(LayerMask.GetMask("Enemy", "Hazards")))
    {
        isAlive = false;
        playerAnim.SetTrigger("Dying");
        playerBody.velocity = kick;
        PlayPersistentClip(dieClip);
        FindAnyObjectByType<GameSession>().PlayerDeath();
    }
}
```
Death is detected purely via collider layer overlap (not a dedicated trigger callback), and the death clip is played through a temporary, `DontDestroyOnLoad` `GameObject` so it survives the scene reload that follows.

Climbing checks `IsTouchingLayers(LayerMask.GetMask("Climb"))` every frame, zeroes gravity, and drives vertical velocity from the `Vertical` input axis while looping a climb audio source.

### `GameSession.cs`
Tracks lives and score across the whole run, and drives the HUD (`TextMeshProUGUI` for lives/score). Uses the singleton pattern via `DontDestroyOnLoad` + a duplicate check in `Awake()`.

| Inspector Field | Type | Purpose |
|---|---|---|
| `playerLives` | `int` | Starting/current life count (default 3) |
| `score` | `int` | Current coin score |
| `livesText` | `TextMeshProUGUI` | HUD lives label |
| `scoreText` | `TextMeshProUGUI` | HUD score label |

```csharp
public void PlayerDeath()
{
    if (playerLives > 1) TakeLife();
    else RestSession();
}
```
`TakeLife()` decrements lives and reloads the *current* scene (`SceneManager.GetActiveScene().buildIndex`). `RestSession()` resets `ScenePersist` and hard-jumps to build index `1`, regardless of which level the player died on.

### `ScenePersist.cs`
A `DontDestroyOnLoad` marker object that self-destructs whenever the Main Menu (build index `0`) loads, and de-duplicates itself if more than one instance exists in a scene.

```csharp
public void ResetScenePersist()
{
    //FindFirstObjectByType<ScenePersist>().ResetScenePersist();
    //SceneManager.LoadScene(1);   // goes to LvL1, not MainMenu
    Destroy(gameObject);
}
```
Despite the name, `ResetScenePersist()` doesn't currently reset anything — it just destroys the object. The commented-out lines suggest an earlier version reloaded LvL1 directly from here.

### `CoinPickup.cs`
Simple one-shot pickup. On trigger with the `Player` tag, adds `coinPickCount` to the score, plays a pickup clip, and removes itself.

| Inspector Field | Type | Purpose |
|---|---|---|
| `coinPick` | `AudioClip` | Pickup sound effect |
| `coinPickCount` | `int` | Score value of this coin (default 1) |

### `Bullet.cs`
Fired by the player; travels in a fixed horizontal direction and destroys the first `Enemy`-tagged object (or any collider) it touches.

| Inspector Field | Type | Purpose |
|---|---|---|
| `bulletSpeed` | `float` | Horizontal travel speed |

```csharp
void Start()
{
    bullet = GetComponent<Rigidbody2D>();
    player = FindFirstObjectByType<PlayerMovement>();
    xSpeed = player.transform.localScale.x * bulletSpeed;
}
```
Direction is derived once, at spawn, from the player's `localScale.x` sign (which `PlayerMovement.Flip()` uses to represent facing direction).

### `EnemyMovement.cs`
Patrols horizontally at a constant speed and reverses direction whenever it exits a trigger collider (typically a ledge/wall sensor).

| Inspector Field | Type | Purpose |
|---|---|---|
| `moveSpeed` | `float` | Patrol speed (default 1) |

```csharp
private void OnTriggerExit2D(Collider2D collision)
{
    moveSpeed = -moveSpeed;
    Flip();
}
```
This flips on *any* trigger exit, not just a specific "edge detector" tag/layer — so any trigger collider leaving the enemy's collider (including the player, a bullet, or a coin) will reverse its patrol direction.

### `LvLExit.cs`
Trigger volume placed at the end of a level. On player contact, waits `loadDeleyTime` seconds, then advances to the next build index, wrapping back to build index `1` after the last scene.

| Inspector Field | Type | Purpose |
|---|---|---|
| `loadDeleyTime` | `float` | Delay before loading the next scene (default 2s) |

### `FloorMovement.cs`
Despite the filename, the class inside is named `Oscillator`. Moves the attached object back and forth between its start position and `startPosition + movementVector` using `Mathf.PingPong` and `Vector3.Lerp` — used for moving platforms or hazards.

| Inspector Field | Type | Purpose |
|---|---|---|
| `movementVector` | `Vector3` | Offset defining the far end of the oscillation path |
| `speed` | `float` | Oscillation speed |

### `MainUI.cs`
Drives the Main Menu buttons: Play (with a click-sound delay before loading the first level) and Quit.

| Inspector Field | Type | Purpose |
|---|---|---|
| `clickClip` | `AudioClip` | Button click sound |

### `PauseToMenu.cs`
Listens for `Escape` at any point during gameplay, tears down the current `GameSession` and `ScenePersist`, and returns to the Main Menu (build index `0`).

---

## 🎮 Controls

| Input | Action |
|---|---|
| `A` / `D` or Left/Right Arrow | Move left / right |
| `Space` | Jump (press again in mid-air for double jump) |
| `W` / `S` or Up/Down Arrow | Climb up / down (only while touching a `Climb` layer collider) |
| Left Mouse Button | Fire bullet |
| `Escape` | Return to Main Menu |

---

## 🛠️ Setup & Installation

1. **Clone the repository**
   ```bash
   git clone https://github.com/Chandan-Baskey/TheCave-2dGame.git
   ```
2. **Open in Unity Hub** — Unity `2022.3+` (LTS) recommended.
3. **Install dependencies** via Package Manager:
   - TextMeshPro (for HUD score/lives text)
4. **Configure required Tags:**
   - `Player`
   - `Enemy`
5. **Configure required Layers:**
   - `Ground` — used by `groundLayer` overlap check in `PlayerMovement`
   - `Climb` — used by `PlayerMovement.Climb()` via `LayerMask.GetMask("Climb")`
   - `Hazards` — used together with `Enemy` in `PlayerMovement.Die()`
6. **Build Settings → Scenes in Build:**
   - Index `0` — Main Menu
   - Index `1..N` — Cave levels, in play order (level wrap-around in `LvLExit` and `GameSession.RestSession()` assumes index `1` is the first playable level)
7. **Press Play** from the Main Menu scene.

---

## 📁 Project Structure

```
TheCave-2dGame/
├── Assets/
│   └── Scripts/
│       ├── PlayerMovement.cs     # Movement, jump, climb, attack, death
│       ├── Bullet.cs             # Player projectile
│       ├── EnemyMovement.cs      # Patrol AI
│       ├── CoinPickup.cs         # Collectible scoring
│       ├── GameSession.cs        # Lives, score, HUD, persistence
│       ├── ScenePersist.cs       # Cross-scene persistence marker
│       ├── LvLExit.cs            # Level-end trigger → next scene
│       ├── FloorMovement.cs      # Oscillating platform/hazard
│       ├── MainUI.cs             # Main menu Play/Quit
│       └── PauseToMenu.cs        # Escape-to-menu handler
├── Escape.jpg                    # Banner image
├── Main View.png                 # Main menu screenshot
├── Escape view.jpg                # Level screenshot
└── Escape View2.jpg               # Level screenshot
```

---

## 🐞 Known Issues

| Issue | Location | Detail |
|---|---|---|
| Redundant deactivation before destroy | `CoinPickup.cs` | `gameObject.SetActive(false)` immediately followed by `Destroy(gameObject)` — the `SetActive` call has no effect |
| Enemy flips on any trigger exit | `EnemyMovement.cs` | `OnTriggerExit2D` reverses direction for *any* exiting collider (player, bullet, coin, etc.), not just a dedicated edge/wall sensor |
| Class/filename mismatch | `FloorMovement.cs` | File is named `FloorMovement.cs` but declares `public class Oscillator` — works once attached, but won't show up under "Add Component → FloorMovement" |
| No null-check on GameSession lookup | `PlayerMovement.cs`, `PauseToMenu.cs` | `FindAnyObjectByType<GameSession>().PlayerDeath()` and similar calls will throw `NullReferenceException` if no `GameSession` exists in the scene |
| Hardcoded scene indices | `LvLExit.cs`, `GameSession.cs`, `PauseToMenu.cs` | Level wrap-around (`nextScene = 1`), session restart (`LoadScene(1)`), and menu return (`LoadScene(0)`) all assume a fixed build order rather than reading it from config |
| No attack cooldown | `PlayerMovement.cs` | `Attack()` instantiates a bullet on every `GetMouseButtonDown(0)` with no fire-rate limit; rapid clicking is unthrottled |
| Misleading method name | `ScenePersist.cs` | `ResetScenePersist()` only destroys the object; it doesn't reset any state, and leaves commented-out legacy code in place |
| Typos in public API | `MainUI.cs`, `LvLExit.cs`, `GameSession.cs` | `PLayGame()`, `loadDeleyTime`, and `RestSession()` (likely meant `ResetSession`) — functional but inconsistent naming |
| Dead/unused variable | `EnemyMovement.cs` | `isMoving` is computed every frame but never used (only referenced by commented-out audio code) |
| Layer name coupling by string | `PlayerMovement.cs` | Ground uses a serialized `LayerMask`, but climb and hazard/enemy layers are looked up via hardcoded `LayerMask.GetMask("Climb")` / `GetMask("Enemy", "Hazards")` strings — renaming those layers in the editor silently breaks gameplay |

---

## 🗺️ Roadmap

- [ ] Fix enemy patrol to only flip on a dedicated edge/wall sensor (tag or layer filter)
- [ ] Add fire-rate cooldown to player attack
- [ ] Replace hardcoded scene indices with a data-driven level sequence
- [ ] Add null-safety guards around `FindAnyObjectByType`/`FindFirstObjectByType` lookups
- [ ] Rename `FloorMovement.cs`'s `Oscillator` class (or the file) so they match
- [ ] Clean up commented-out legacy code (`ScenePersist`, `PlayerMovement.Jumpping()`, `EnemyMovement` audio)
- [ ] Add checkpoint system so death doesn't always reload the full scene
- [ ] Expand enemy variety (flying, ranged, ground-hugging types)
- [ ] Add a pause menu overlay (currently `Escape` exits straight to Main Menu)

---

<p align="center">Built with Unity · <a href="https://github.com/Chandan-Baskey">Chandan-Baskey</a></p>
