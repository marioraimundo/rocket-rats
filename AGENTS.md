# Rocket Rats

Flappy Bird clone built with Unity 6000.0.2f1 (URP 2D).

## Project structure

- `Assets/` — all game scripts, prefabs, scenes, images, and settings live in the root (no custom `.asmdef`; everything compiles into `Assembly-CSharp`)
- `Packages/manifest.json` — package dependencies; notable: New Input System (`com.unity.inputsystem` 1.8.2), URP, Visual Scripting
- Single scene: `Assets/Scenes/SampleScene.unity`

## Scripts

| File | Purpose |
|---|---|
| `Assets/Bird.cs` | Player: Space=flap, Left/Right arrows for horizontal movement |
| `Assets/PipeMovScript.cs` | Pipe movement leftward + destroy at dead zone |
| `Assets/PipeSpawnerScript.cs` | Timed pipe spawning with random Y offset |
| `Assets/scoreTrigger.cs` | Score trigger (empty — logic not implemented) |
| `Assets/backgroundScroller.cs` | Parallax scrolling via material texture offset |

## Input

Uses the **New Input System**. Action map: `Assets/InputSystem_Actions.inputactions`.
Current input is polled via `Input.GetKey` in `Bird.Update()` (direct key reads, not action callbacks).

## Testing

No test assembly definitions or test files exist in `Assets/`. No automated test suite configured.

## Build & dev

- Open `Assets/Scenes/SampleScene.unity` in the editor to run/test
- `Rocket_Rats.sln` at repo root for IDE integration
- No custom build/format/lint/typecheck commands; all via Unity Editor

## MCP (Unity)

- **Unity MCP server** roda em `http://127.0.0.1:8080/mcp` (FastMCP, gerenciado pelo próprio Unity Editor via `Library/MCPForUnity/`)
- `opencode.json` já está configurado com `type: "http"` apontando para essa URL
- O servidor é iniciado automaticamente quando o Unity Editor abre o projeto; 30+ ferramentas expostas (animation, core, scripting_ext, ui, etc.)
