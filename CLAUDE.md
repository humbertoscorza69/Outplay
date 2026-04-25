# CLAUDE.md — Project Outplay

This file is read by Claude Code on every session. It defines how code is written for this project. Keep edits tight and operational — design vision belongs in design docs, not here.

## Project Overview

Project Outplay is a top-down 2D magical PvP arena game built in Unity 6 with URP. 12–16 players enter a shrinking arena and fight with wands using skillshot projectiles, mana, cooldowns, and abilities, competing for top-3 placement. Combat is manual aim, skill-based — no auto-aim, no random crits. Long-term vision includes crypto-native paid queues, but Sprint 0–3 is gameplay first.

## Tech Stack

- **Engine:** Unity 6 with Universal Render Pipeline (URP), 2D mode
- **Language:** C# with the new Unity Input System (`com.unity.inputsystem` 1.19.0)
- **Networking:** FishNet (planned for Sprint 2 — not implemented yet)
- **Authority model:** Server-authoritative for all combat and economy logic
- **Target platforms:** WebGL first, then Windows, mobile later

## Design Pillars (these constrain every gameplay decision)

1. **Skill First** — outcomes depend on aim, movement, timing, positioning, resource management. No random crits, no auto-aim, no RNG-determined kills.
2. **Readability Over Complexity** — players must understand what hit them, what spell was cast, and why they died. Hitboxes match visuals.
3. **Competitive Fairness** — no client-authoritative damage, no hidden pay-to-win, no stat-check victories.
4. **Clutch & Rage Loop** — produce healthy frustration ("I missed the dodge"), never confused frustration ("I don't know what happened").
5. **Build Expression Without Pay-to-Win** — gear and unlocks open options, never grant raw power advantages that new players can't overcome.
6. **Gameplay First, Money Layer Later** — the game must be fun without any monetary stake before paid mode is implemented.

## Architecture Conventions

- **Server authority by design.** All combat logic (damage, hits, cooldowns, mana, kills) must be authoritative on the server once multiplayer lands. Write single-player code now so it ports cleanly: separate **decision logic** (what should happen) from **presentation logic** (visual/audio feedback) from the start.
- **ScriptableObjects for static data.** Wand definitions, spell definitions, rune data, gear stats — all SOs. Use plain C# classes or components for runtime state.
- **Avoid singletons** except for clear cross-cutting infrastructure (game manager, audio manager). Prefer dependency injection or direct references for gameplay systems.
- **Object pooling** for any frequently spawned/despawned object (projectiles, VFX, damage numbers). Never `Instantiate`/`Destroy` in combat code.
- **Events over polling.** Use C# events or UnityEvents for cross-system communication, not `Update()` polling for state changes.

## Folder Structure (under `Assets/_Project/`)

- `Scripts/` organized by domain: `Player/`, `Combat/`, `Enemies/`, `Core/`, `Input/`
- `Prefabs/` mirrors `Scripts/` organization
- `Scenes/` — gameplay scenes only, no test/sandbox scenes committed
- `Art/Sprites/`, `Art/VFX/`, `Audio/SFX/`, `Audio/Music/`, `Settings/`, `Input/`
- Always place new code under `_Project/`, never at the `Assets/` root.

## Code Style

- **Namespaces:** all code under `Outplay.[Domain]` (e.g., `Outplay.Combat`, `Outplay.Player`).
- **Casing:** PascalCase for classes, methods, properties. camelCase for fields and locals. Private serialized fields use `[SerializeField] private` with **no underscore prefix**.
- Prefer `var` for local variables when the type is obvious from the right-hand side.
- One `MonoBehaviour` per file; file name matches class name.
- XML doc comments on public APIs, especially anything called across systems.
- Keep `MonoBehaviour`s small — extract logic to plain C# classes when possible to keep code testable.

## Hard Do-Nots

- ❌ No `Input.GetKey` or other legacy input — new Input System only.
- ❌ No client-authoritative damage, healing, kills, or rewards.
- ❌ No random crits, dodge chance, or RNG-determined damage outcomes.
- ❌ No auto-aim, aim assist, or homing projectiles unless a specific ability calls for it (design exception, not a default).
- ❌ No `FindObjectOfType` or `GameObject.Find` in `Update` or hot paths. Cache references in `Awake`/`Start`.
- ❌ No `Instantiate`/`Destroy` for projectiles or VFX — use pooling.
- ❌ No "while we're here" feature additions. Stick to the prompt.
- ❌ Never assume crypto/payment integration — that's a separate, later layer. Don't add wallet, token, or payment code unless explicitly requested.

## Working Style

- When asked to do a task, **propose the approach before writing code** if it touches more than one file or system.
- When uncertain about a design decision (e.g., "should this be a ScriptableObject or a plain class?"), **ask before deciding**.
- **Stop and confirm before scope creep.** If a task reveals an issue outside its scope, surface it and ask — don't silently fix.
- Commit changes at logical checkpoints with clear messages following **Conventional Commits** style: `feat:`, `fix:`, `refactor:`, `chore:`, `docs:`, `test:`, etc.

## Open Questions (don't decide unilaterally)

- Final TTK target (currently 15–25s between equally skilled players).
- Whether ultimates use mana, charge, or both.
- Whether gear stats are normalized in paid queues.
- Final lobby size (target 12–16).

## Out of Scope for Sprint 0–1

- Multiplayer / networking
- Crypto, wallets, payments
- Matchmaking, accounts, persistence
- Battle pass, quests, cosmetics
- Procedural map generation
- Multiple wands or abilities (single wand, single basic attack first)
