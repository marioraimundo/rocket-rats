# Rocket Rats

Jetpack Rat clone built with Unity 6000.0.2f1 (URP 2D).

## Project structure

- `Assets/` — all game scripts, prefabs, scenes, and images (no custom `.asmdef`; everything compiles into `Assembly-CSharp`)
- `Assets/Editor/` — Editor scripts (menu items for one‑time scene setup)
- `Assets/Scenes/MainMenu.unity` — Main menu with Start/Options/Leaderboard/Credits panels
- `Assets/Scenes/StartGame.unity` — Gameplay scene
- `ProjectSettings/TagManager.asset` — custom tags: `Logic`, `Score`, `Spike`

## Scripts

| File | Purpose |
|---|---|
| `Assets/Rat.cs` | Player controller: Space=jetpack (hold to rise, drains stamina 20/s, speed 4), Left/Right/A/D arrows (consume stamina 5/s, speed 5), health (max 3, invulnerability 1s), stamina (max 100, consume 20/s jetpack + 5/s horizontal, recover 40/s immediately), head rotate -8° on jetpack (ear offset -15°, nose offset -10°), fire animation on jetpack OR horizontal movement, gun shoot on left click, auto‑finds hierarchy (`Body/null_head→Head/Head/null_Ear→Ear`, `null_nose→Nose`, `JetpackMiddle/JetpackLeft+Right/Fire`, `Gun`) |
| `Assets/HeartDisplay.cs` | Heart images UI; `Start()` auto‑finds `Hearts` container → `GetComponentsInChildren<Image>()` |
| `Assets/LogicManager.cs` | GameOver panel (PlayAgain→StartGame, MainMenu→MainMenu), shows cheese count, uses `Time.timeScale = 0` |
| `Assets/MainMenuManager.cs` | MainMenu scene logic: panel navigation (Main/Options/Leaderboard/Credits), StartGame→`SceneManager.LoadScene("StartGame")`, Quit |
| `Assets/PipeMovScript.cs` | Pipe movement leftward + destroy at dead zone |
| `Assets/PipeSpawnerScript.cs` | Timed pipe spawning; alternates between `pipe` and `pipeSpike` prefabs |
| `Assets/scoreTrigger.cs` | Empty (esvaziado) |
| `Assets/Cheese.cs` | Rotates cheese, detects collision with layer 3, increments `CheeseCounter` |
| `Assets/CheeseCounter.cs` | Cheese counter UI (Image + TMP_Text), auto‑found via `transform.Find("Cheese/CheeseIcon" + "/CheeseText")` |
| `Assets/backgroundScroller.cs` | Fixed‑speed dual‑copy infinite scroll (GameObject move) |

## Prefabs

| File | Notes |
|---|---|
| `Assets/Prefabs/Rat.prefab` | Rat character: Body→Head→Ear/Nose, Gun, JetpackMiddle→JetpackLeft+Right (+Fire children), ArmsBack, ArmsFront, Tail. References `HeadController`, `GunController`, `FireController` animators |
| `Assets/Pipe.prefab` | Normal obstacle (no tag) — physical collision only, no damage |
| `Assets/PipeSpike.prefab` | Spiked obstacle — `BottomPipe` has tag **"Spike"** → deals 1 damage |
| `Assets/GameUI.prefab` | Hearts + Stamina + Cheese counter (Image + TMP_Text) + CheeseCounter + HeartDisplay + GameOverPanel. Children: `Hearts` (Heart1-3), `Stamina` (StaminaText), `Cheese` (CheeseIcon, CheeseText) |
| `Assets/Background.prefab` | Full‑screen background with animated Duto; `SpriteRenderer` (background_clean) + `backgroundScroller`; child `Duto` with `SpriteRenderer` + `Animator` (DutoWater). Sorting: Background=-10, Duto=-4 |

## Animations

| File | Notes |
|---|---|
| `Assets/Animations/HeadBlink.anim` | 6 keyframes, 3s loop: blink frames at start + hold on `headBlink_spritesheet_0` for ~3s pause between cycles |
| `Assets/Animations/HeadController.controller` | Single-state: HeadBlink, looping |
| `Assets/Animations/GunIdle.anim` | 1 frame holding `weapon_spritesheet_0` |
| `Assets/Animations/GunShoot.anim` | 10 keyframes at 12fps (0→0.75s): frames _0→_8 then back to _0 for smooth transition |
| `Assets/Animations/GunController.controller` | Idle (GunIdle) → Shoot (GunShoot) trigger on `IsShooting` bool, returns to Idle |
| `Assets/Animations/FireThrust.anim` | 5-frame loop from `fire_spritesheet` at ~12fps |
| `Assets/Animations/FireController.controller` | Thrusting/Idle bool parameter |
| `Assets/DutoWater.anim` | 4-frame sprite animation at 8 fps, looping; sprites from `pipe_spritesheet.png` |
| `Assets/DutoWater.controller` | Animator controller for Duto (single state: DutoWater) |

## Images

| File | Notes |
|---|---|
| `Assets/images/background_clean.png` | 1920×1080 clean background (no dutos), SpriteMode Single, PPU=100 |
| `Assets/images/background.png` | 1920×1080 initial background |
| `Assets/images/heart.png` | Heart icon for UI |
| `Assets/images/queijo.png` | Cheese collectible sprite |
| `Assets/images/mainMenu_splashScreen.png` | Main menu background |
| `Assets/images/pipe_top.png` / `pipe_bot.png` | Pipe obstacle sprites |
| `Assets/images/pipe_spritesheet.png` | 4-frame spritesheet (200×447 each) for animated Duto |
| `Assets/images/pipeSpike_bot.png` | Spiked bottom pipe sprite |
| `Assets/images/character/body.png`, `head.png`, `ear.png`, `nose.png` | Rat character body parts |
| `Assets/images/character/arms.png`, `arm_back.png`, `tail.png` | Rat arms and tail |
| `Assets/images/character/Jetpack_Left.png`, `Jetpack_Right.png`, `Jetpack_middle.png` | Jetpack parts |
| `Assets/images/character/spritesheets/weapon_spritesheet.png` | 9 sprites (423×214 each), PPU=255, maxTex=4096, Center pivot |
| `Assets/images/character/spritesheets/headBlink_spritesheet.png` | 5 frames for blink animation |
| `Assets/images/character/spritesheets/fire_spritesheet.png` | 5 frames for jetpack fire animation |

## Character hierarchy (Rat.prefab)

```
Rat
└─ Body
   ├─ Head (or null_head → Head)
   │  ├─ Head (child sprite) — HeadBlink animator
   │  ├─ Ear (or null_Ear → Ear)
   │  └─ Nose (or null_nose → Nose)
   ├─ ArmsBack
   ├─ JetpackMiddle
   │  ├─ Jetpack_Left
   │  │  └─ Fire_Left — FireController
   │  └─ Jetpack_Right
   │     └─ Fire_Right — FireController
   ├─ ArmsFront
   ├─ Gun — GunController
   └─ Tail
```

The `null_*` fallback pattern supports both the prefab (`Head`) and StartGame scene (`null_head → Head`, `null_Ear → Ear`, `null_nose → Nose`) hierarchies via `??` in `Rat.Start()`.

## Input

Uses **New Input System** package (`Assets/InputSystem_Actions.inputactions`) but code polls via legacy `Input.GetKey` in `Rat.Update()`.

## Tags

- **Spike** — applied to PipeSpike's BottomPipe; Rat checks for this tag to deal damage

## MCP Server

- **Unity MCP** em `http://127.0.0.1:8080/mcp` (FastMCP, `Library/MCPForUnity/`)
- `opencode.json` configurado com `type: "http"`
- Session ID (`mcp-session-id` header) obtido no login e reutilizado

## MCP `execute_code` caveats

- Usa **CodeDom**, não referencia `Assembly-CSharp.dll`. Para acessar tipos do projeto:
  ```csharp
  System.Type.GetType("TypeName, Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null")
  ```
- UI `GameObject` precisa de `typeof(RectTransform)` no construtor:
  ```csharp
  new GameObject("name", typeof(RectTransform), typeof(Image))
  ```
  não use `AddComponent<RectTransform>()` (falha pois Transform já existe).
- Lambdas não funcionam; usar `Delegate.CreateDelegate` para eventos.
- Prefabs salvos com `PrefabUtility.SaveAsPrefabAssetAndConnect` podem perder remapeamento de referências a componentes filhos → usar `GetComponentsInChildren` no `Start()` em vez de confiar no array serializado.
- `AssetDatabase.DeleteAsset` é bloqueado pelo safety check do MCP.
- Para carregar sub‑sprites de uma spritesheet (SpriteMode Multiple), usar `LoadAllAssetRepresentationsAtPath` — `LoadAssetAtPath("...png[name]")` não funciona.
- Para ativar loop em animações Mecanim, usar `AnimationUtility.SetAnimationClipSettings(clip, settings)` com `settings.loopTime = true`. `clip.wrapMode` só funciona para animações Legacy.

## Editor utilities (`Assets/Editor/`)

| Script | Menu Item | Purpose |
|---|---|---|
| `RecreatePipePrefab.cs` | `Tools/Rocket Rats - Recreate Pipe Prefab` | Recria Pipe.prefab do zero (refs imagens legadas — pode precisar de updates) |
| `AssignPipeSpike.cs` | `Tools/Rocket Rats - Assign PipeSpike` | Atribui `pipeSpike` no PipeSpawnerScript da cena |
| `SetupHeartUI.cs` | `Tools/Rocket Rats - Setup Heart UI` | Cria Hearts + GameOverPanel no Canvas (obsoleto — usar GameUI.prefab) |

## Build & dev

- Open `Assets/Scenes/StartGame.unity` → Play
- `Rocket_Rats.sln` for IDE
- No automated tests; no lint/format/typecheck commands
