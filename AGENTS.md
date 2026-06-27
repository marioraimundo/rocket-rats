# Rocket Rats

Flappy Bird clone built with Unity 6000.0.2f1 (URP 2D).

## Project structure

- `Assets/` — all game scripts, prefabs, scenes, and images (no custom `.asmdef`; everything compiles into `Assembly-CSharp`)
- `Assets/Editor/` — Editor scripts (menu items for one‑time scene setup)
- Single scene: `Assets/Scenes/SampleScene.unity`
- `ProjectSettings/TagManager.asset` — custom tags: `Logic`, `Score`, `Spike`

## Scripts

| File | Purpose |
|---|---|
| `Assets/Bird.cs` | Player: Space=flap, Left/Right arrows, **health system** (maxHealth 3), invulnerability 1s, `OnCollisionEnter2D` → only calls `TakeDamage()` for tag `"Spike"` |
| `Assets/HeartDisplay.cs` | 3 heart Images UI; `Start()` auto‑finds children via `GetComponentsInChildren<Image>()` |
| `Assets/LogicManager.cs` | Score + GameOver panel + `RestartGame()` (recarrega cena) |
| `Assets/PipeMovScript.cs` | Pipe movement leftward + destroy at dead zone |
| `Assets/PipeSpawnerScript.cs` | Timed pipe spawning; **alternates** between `pipe` and `pipeSpike` prefabs |
| `Assets/scoreTrigger.cs` | Score trigger (empty — logic not implemented) |
| `Assets/backgroundScroller.cs` | Fixed‑speed dual‑copy infinite scroll (GameObject move) |

## Prefabs

| File | Notes |
|---|---|
| `Assets/Pipe.prefab` | Normal obstacle (no tag) — physical collision only, no damage |
| `Assets/PipeSpike.prefab` | Spiked obstacle — `BottomPipe` has tag **"Spike"** → deals 1 damage |
| `Assets/HeartUI.prefab` | 3 hearts UI (child of Canvas) |
| `Assets/Background.prefab` | Full‑screen background with animated Duto; `SpriteRenderer` (background_clean) + `backgroundScroller`; child `Duto` with `SpriteRenderer` + `Animator` (DutoWater). Sorting: Background=-10, Duto=-4 |

## Animations

| File | Notes |
|---|---|
| `Assets/DutoWater.anim` | 4-frame sprite animation at 8 fps, looping; sprites from `pipe_spritesheet.png` |
| `Assets/DutoWater.controller` | Animator controller for Duto (single state: DutoWater) |

## Images

| File | Notes |
|---|---|
| `Assets/images/background_clean.png` | 1920×1080 clean background (no dutos), SpriteMode Single, PPU=100 |
| `Assets/images/pipe_spritesheet.png` | 4-frame spritesheet (200×447 each) for animated duto, SpriteMode Multiple |

## Input

Uses **New Input System** (`Assets/InputSystem_Actions.inputactions`). Polled via `Input.GetKey` in `Bird.Update()`.

## Tags

- **Spike** — applied to PipeSpike's BottomPipe; Bird checks for this tag to deal damage

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
| `RecreatePipePrefab.cs` | `Tools/Rocket Rats - Recreate Pipe Prefab` | Recria Pipe.prefab do zero (usado quando o .prefab corrompeu) |
| `AssignPipeSpike.cs` | `Tools/Rocket Rats - Assign PipeSpike` | Atribui `pipeSpike` no PipeSpawnerScript da cena |
| `SetupHeartUI.cs` | `Tools/Rocket Rats - Setup Heart UI` | Cria Hearts + GameOverPanel no Canvas (não usar — prefere `execute_code`) |

## Build & dev

- Open `Assets/Scenes/SampleScene.unity` → Play
- `Rocket_Rats.sln` for IDE
- No automated tests; no lint/format/typecheck commands
