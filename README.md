# 🗺️ Tilemaps2dGame

![Unity](https://img.shields.io/badge/Unity-2022.3%2B-black?logo=unity&logoColor=white)
![C#](https://img.shields.io/badge/C%23-239120?logo=c-sharp&logoColor=white)
![Cinemachine](https://img.shields.io/badge/Cinemachine-State%20Driven%20Camera-blue?logo=unity)
![Platform](https://img.shields.io/badge/Platform-PC-lightgrey)
![Genre](https://img.shields.io/badge/Genre-2D%20Platformer-orange)
![License](https://img.shields.io/badge/License-MIT-green)

A tile-based 2D platformer built in Unity, featuring run-and-jump movement, double jumping, vertical climbing, ranged combat, multi-level progression with wrap-around looping, a persistent life/score HUD across scene loads, and **Cinemachine State Driven Cameras** that swap virtual cameras based on player state (ground, climb, combat).

---

## 🎮 Game View

![Game View](https://github.com/Chandan-Baskey/Tilemaps2dGame/blob/3f3b0446c196eff6e71b34bf560e285a931d7d70/Game%20View.jpg?raw=true)

## 🗺️ Levels

| Level Select | Level 3 | Level 4 |
|---|---|---|
| ![Levels](https://github.com/Chandan-Baskey/Tilemaps2dGame/blob/3f3b0446c196eff6e71b34bf560e285a931d7d70/Game%20LvLs.jpg?raw=true) | ![Level 3](https://github.com/Chandan-Baskey/Tilemaps2dGame/blob/3f3b0446c196eff6e71b34bf560e285a931d7d70/Game%20LvL3.jpg?raw=true) | ![Level 4](https://github.com/Chandan-Baskey/Tilemaps2dGame/blob/3f3b0446c196eff6e71b34bf560e285a931d7d70/Game%20LvL4.jpg?raw=true) |

---

## 📖 Overview

The player navigates a series of tile-based levels, dealing with patrolling enemies and environmental hazards. Core verbs are **run, jump, double-jump, climb, and shoot**. Coins add to a persistent score, contact with an `Enemy`/`Hazards` layer costs a life, and reaching the level's exit trigger advances to the next scene after a short delay — looping back to the first playable level once the last one is cleared.

A `GameSession` singleton tracks lives and score across scene loads via `DontDestroyOnLoad`, paired with a `ScenePersist` object that survives transitions until explicitly reset on full game-over. The camera system uses **Cinemachine's State Driven Camera** to automatically blend between virtual cameras tied to player animation states — so the framing tightens during platforming, widens during climbs, and can punch in during combat, without any manual camera-switching code.

---

## ✨ Features

- **Hold-to-move platforming** — horizontal input drives `Rigidbody2D` velocity directly for responsive, analog-friendly movement
- **Jump + double jump** — single jump while grounded, one extra mid-air jump that resets only after landing
- **Wall/ladder climbing** — entering a `Climb`-tagged layer zeroes gravity and remaps vertical input to climb speed
- **Ranged combat** — left-click fires a directional bullet from a gun anchor; bullets inherit the player's facing direction
- **Sprite flipping** — facing direction driven by velocity sign via `localScale.x`, no separate flip flag needed
- **Enemy patrol AI** — enemies walk in a straight line and reverse direction when they exit a boundary trigger
- **Coin economy** — collectible coins add to score exactly once per coin (guarded by a `wasCollected` flag)
- **Lives & score HUD** — `TextMeshPro` UI driven by a cross-scene `GameSession` singleton
- **Level looping** — exiting the final level wraps back around to level 1 instead of crashing past `sceneCountInBuildSettings`
- **Cinemachine State Driven Camera** — virtual cameras automatically blend based on the player's Animator state (idle/run, climb, etc.), giving cinematic framing without manual scripting
- **Death & respawn flow** — losing a life reloads the current scene; running out of lives fully resets the session and returns to the main menu

---

## 🏗️ Architecture

```
┌──────────────────────────────────────────────────────────────────┐
│                          MAIN MENU SCENE                          │
│                            MainUI.cs                              │
│                    [Play] ──────────────► loads Scene 1           │
└──────────────────────────────────────────────────────────────────┘
                                  │
                                  ▼
┌──────────────────────────────────────────────────────────────────┐
│                         GAMEPLAY SCENES                           │
│                                                                     │
│   ┌─────────────────┐        ┌──────────────────────────────┐    │
│   │  PlayerMovement   │◄──────│   Cinemachine State Driven    │    │
│   │  - Move/Jump/Climb│        │   Camera (Animator-bound)     │    │
│   │  - Attack/Die     │───────►│   blends VCams per state      │    │
│   └────────┬─────────┘        └──────────────────────────────┘    │
│            │ instantiates                                          │
│            ▼                                                       │
│   ┌─────────────────┐        ┌──────────────────┐                 │
│   │     Bullet        │───X──►│  EnemyMovement     │                │
│   │  (kills Enemy tag)│        │  (patrol + flip)   │                │
│   └─────────────────┘        └──────────────────┘                 │
│                                                                     │
│   ┌─────────────────┐        ┌──────────────────┐                 │
│   │   CoinPickup      │───────►│   GameSession      │ (singleton)  │
│   │  (OnTrigger once) │        │  - score / lives   │  DontDestroy │
│   └─────────────────┘        │  - HUD text        │              │
│                                └─────────┬─────────┘                │
│                                          │ PlayerDeath()            │
│                                          ▼                          │
│                              ┌──────────────────┐                  │
│                              │  Reload scene OR   │                  │
│                              │  Reset & → Menu    │                  │
│                              └──────────────────┘                  │
│                                                                     │
│   ┌─────────────────┐        ┌──────────────────┐                 │
│   │     LvLExit       │───────►│  ScenePersist       │ (singleton)  │
│   │  (delay → load    │        │  survives loads,    │  DontDestroy │
│   │   next/loop scene)│        │  reset on game-over │              │
│   └─────────────────┘        └──────────────────┘                 │
└──────────────────────────────────────────────────────────────────┘
```

**Design notes:**
- `GameSession` and `ScenePersist` both use the *"count existing instances, destroy the duplicate"* singleton pattern rather than a static-reference singleton — simple and effective for scenes that may already contain a leftover instance after a reload.
- The world itself doesn't move in this project (unlike an endless-runner layout) — the player moves through static tilemap levels, and the camera does the work of framing via Cinemachine.

---

## 🎥 Cinemachine & State Driven Camera

This project uses **Cinemachine's State Driven Camera** to handle all camera framing, instead of hand-written `Camera.main` follow logic.

### How it's wired up

1. A **State Driven Camera** component sits at the top level and watches the player's `Animator`.
2. Each Animator state (e.g. `isRunning`, `isClimbing`, `Dying`) is mapped to a **child Virtual Camera** in the State Driven Camera's state map.
3. When `PlayerMovement` calls `playerAnim.SetBool("isClimbing", true)` or triggers `"Dying"`, Cinemachine detects the Animator state change on the next frame and **automatically blends** to the virtual camera assigned to that state — no extra script required on the camera side.
4. Each child Virtual Camera can have its own:
   - **Lens / FOV** (e.g. wider during climbs to show more vertical space)
   - **Framing Transposer offset** (e.g. tighter on the player during combat)
   - **Blend style and duration** (set on the State Driven Camera's "Default Blend" or per-state custom blends)

### Why this pattern fits the project

| Approach | Trade-off |
|---|---|
| Manual `Camera.main.transform` scripting | Tight coupling, needs a new `if` branch per state, hard to art-direct |
| **Cinemachine State Driven Camera** ✅ | Camera behavior lives entirely in the Inspector, driven by Animator states that already exist for animation — zero extra gameplay code, designer-friendly |

Because `PlayerMovement.cs` already drives `Animator.SetBool`/`SetTrigger` calls for `isRunning`, `isClimbing`, and `Dying`, the State Driven Camera piggybacks on state the project needed anyway — animation and camera framing stay perfectly in sync by construction.

### Suggested state → camera map

| Animator State | Virtual Camera Behavior |
|---|---|
| `isRunning` (idle/run) | Standard follow, neutral FOV |
| `isClimbing` | Slightly wider FOV, vertical dead-zone loosened to track climbing |
| `Dying` | Locked/static camera or slow push-in for impact |

---

## 🧩 Script Breakdown

### `PlayerMovement.cs`

The core controller. Runs all per-frame player logic out of a single `Update()` dispatcher.

| Method | Responsibility |
|---|---|
| `Movement()` | Reads `Input.GetAxis("Horizontal")`, sets `Rigidbody2D.velocity.x` directly (vertical velocity preserved) |
| `Jump()` | Ground jump on `Space`; allows exactly one double jump, re-armed only while grounded |
| `Flip()` | Flips sprite via `transform.localScale = (sign(velocity.x), 1)` — no separate `facingRight` bool needed |
| `Climb()` | While touching the `Climb` layer, zeroes gravity and maps vertical input to `climbSpeed`; restores gravity scale on exit |
| `Attack()` | Instantiates `bullet` at the `gun` anchor on left-click |
| `Die()` | On touching `Enemy`/`Hazards` layers: disables further input, triggers death animation, applies a launch-back `kick` velocity, and calls `GameSession.PlayerDeath()` |

```csharp
void Jump()
{
    if(Input.GetKeyDown(KeyCode.Space))
    {
        if(isGrounded) 
        {
            playerBody.velocity = new Vector2(playerBody.velocity.x, jumpSpeed);
            isGrounded = false;
        }
        else if(doublejump)
        {
            playerBody.velocity = new Vector2(playerBody.velocity.x, doublejumpSpeed);
            doublejump = false;
        }
    }
    if(isGrounded) doublejump = true;
}
```
The double jump flag is re-armed every frame the player is grounded, so a player can only ever cash in one extra jump per airborne phase — clean and predictable.

### `EnemyMovement.cs`

Minimal patrol AI: walks at a constant `moveSpeed`, and reverses both velocity and facing the moment it exits a trigger collider (typically a patrol-boundary zone placed at each end of its walk path).

```csharp
private void OnTriggerExit2D(Collider2D collision)
{
    moveSpeed = -moveSpeed;
    Flip();
}
```

### `Bullet.cs`

Reads the player's current facing (`localScale.x`) once in `Start()` to lock in a travel direction, then moves at constant velocity every frame. Destroys the target if tagged `Enemy`, and destroys itself on any trigger or physical collision — including walls.

### `CoinPickup.cs`

Trigger-based collectible. A `wasCollected` flag prevents a double-trigger (e.g. multiple overlapping frames before `Destroy()` actually removes the object) from awarding score twice.

### `LvLExit.cs`

```csharp
IEnumerator LoadNextLvL()
{
    yield return new WaitForSecondsRealtime(loadDeleyTime);
    int currntScene = SceneManager.GetActiveScene().buildIndex;
    int nextScene = currntScene + 1;

    if(nextScene == SceneManager.sceneCountInBuildSettings)
        nextScene = 1; // loop back to first gameplay scene

    FindFirstObjectByType<ScenePersist>().ResetScenePersist();
    SceneManager.LoadScene(nextScene);
}
```
A short coroutine delay gives an exit animation/SFX time to play before the load actually happens. The wrap-around check means the level sequence loops indefinitely rather than throwing an out-of-range scene index once the player clears the last level.

### `GameSession.cs`

Cross-scene singleton owning lives, score, and the HUD text. `PlayerDeath()` branches: if lives remain, `TakeLife()` decrements and reloads the *current* scene (checkpoint-less restart of the level); if it was the last life, `RestSession()` tears down persistence and returns to the menu scene.

### `ScenePersist.cs`

A lightweight `DontDestroyOnLoad` marker object. Uses the same "count instances, destroy extras" pattern as `GameSession` so re-entering a scene that already has one doesn't spawn duplicates. `ResetScenePersist()` is called explicitly on full game-over so the persistent object doesn't outlive the play session.

### `MainUI.cs`

Two-button main menu: `PLayGame()` loads scene index 1, `Quit()` calls `Application.Quit()`.

---

## 🎮 Controls

| Input | Action |
|---|---|
| `A` / `D` or Arrow Keys | Move left / right |
| `Space` | Jump (press again mid-air for double jump) |
| `W` / `S` or Up / Down | Climb up / down (while on a `Climb` surface) |
| `Left Mouse Button` | Shoot |

---

## 🛠️ Setup & Installation

1. **Clone the repository**
   ```bash
   git clone https://github.com/Chandan-Baskey/Tilemaps2dGame.git
   ```
2. **Open in Unity Hub** — Unity **2022.3 LTS** or newer recommended.
3. **Install required packages** via Package Manager:
   - `Cinemachine` (for the State Driven Camera)
   - `TextMeshPro` (HUD text — import TMP Essentials if prompted)
4. **Scene setup** — ensure all gameplay scenes plus the main menu are added to **File → Build Settings**, in the correct order (menu = index 0, first level = index 1, and so on), since `LvLExit` relies on build index math to progress and loop levels.
5. **Layers** — confirm `Ground`, `Climb`, `Enemy`, and `Hazards` layers exist and are assigned correctly on level geometry; `PlayerMovement` and `EnemyMovement` both depend on `LayerMask` checks against these names.
6. Press **Play** from the main menu scene.

---

## 📁 Project Structure

```
Tilemaps2dGame/
├── Assets/
│   ├── Scripts/
│   │   ├── PlayerMovement.cs
│   │   ├── EnemyMovement.cs
│   │   ├── Bullet.cs
│   │   ├── CoinPickup.cs
│   │   ├── LvLExit.cs
│   │   ├── GameSession.cs
│   │   ├── ScenePersist.cs
│   │   └── MainUI.cs
│   ├── Scenes/
│   ├── Tilemaps/
│   ├── Animations/
│   ├── Cinemachine/        # Virtual Cameras + State Driven Camera asset
│   └── Sprites/
├── Game View.jpg
├── Game LvLs.jpg
├── Game LvL3.jpg
└── Game LvL4.jpg
```

---

## 🐞 Known Issues

| Issue | Where | Detail |
|---|---|---|
| Ground check runs after death | `PlayerMovement.Update()` | `Physics2D.OverlapCircle` for `isGrounded` executes every frame *before* the `isAlive` early-return, so it keeps running uselessly after death |
| Dead/commented code left in source | `PlayerMovement.cs` | An unused `Jumpping()` method and a commented-out `Flip()` implementation remain in the file; safe to delete |
| Patrol flip depends on trigger geometry only | `EnemyMovement.cs` | Direction reverses purely on `OnTriggerExit2D` — if an enemy is ever spawned outside its boundary trigger or the trigger is misaligned with the platform edge, it can walk straight off a ledge |
| Bullet locks direction once | `Bullet.cs` | `xSpeed` is computed only in `Start()` from the player's facing at the *moment of spawn* — correct for a snapshot-fire weapon, but means rotating/flipping the player after firing has no effect on bullets already in flight (expected, just worth knowing if extending to homing/curving shots) |
| No checkpoint mid-level | `GameSession.TakeLife()` | Losing a life reloads the *entire current scene* rather than resuming from a checkpoint, so longer levels restart fully on death |

---

## 🚧 Roadmap / Future Improvements

- [ ] Mid-level checkpoints instead of full scene reload on death
- [ ] Clean up dead/commented code in `PlayerMovement.cs`
- [ ] Gate the ground-check physics call behind `isAlive` to avoid unnecessary post-death overlap checks
- [ ] Expand the Cinemachine state map with a dedicated combat/attack virtual camera
- [ ] Pooling for bullets and coins instead of `Instantiate`/`Destroy` per use
- [ ] Persistent high-score tracking across sessions (currently resets with `ScenePersist`)
- [ ] Mobile/touch input support alongside keyboard + mouse

---

## 👤 Credits

**Developer:** [Chandan-Baskey](https://github.com/Chandan-Baskey)
Built in Unity with Cinemachine, TextMeshPro, and the built-in 2D physics stack.

---

## 📄 License

This project is open source. Add a `LICENSE` file (MIT recommended) if you intend others to reuse the code.
