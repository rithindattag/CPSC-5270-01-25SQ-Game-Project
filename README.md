# 🧱 Ashfall Descent

_A Reflex-Based Endless Runner Game in Godot 4.1_

---

## 🎮 Overview

**Ashfall Descent** is a 3D endless runner game built in **Godot Engine 4.1**, where players sprint through a volcanic terrain, dodging obstacles and collecting coins. The game combines fast-paced reflex mechanics with visually immersive shaders and modular code architecture using well-established **Software Design Patterns**.

---

## 🧩 Aesthetic Goals

- **Challenge**: Players must time their actions precisely (jump, slide, switch lanes) to survive increasing difficulty and score.
- **Sensation**: Engaging visual atmosphere driven by custom shaders (lava floor, glow coins, heat distortion), ambient music, and fast movement.

---

## ⚙️ Key Mechanics

- Lane-based movement using keyboard inputs
- Jumping and sliding to avoid different obstacle types
- Coin collection with animated and glowing coin pickups
- Score increases over time and with coin collection
- Speed gradually increases to intensify gameplay

---

## 🏗️ Software Design Patterns

### ✅ Creational Patterns:

- **Factory Pattern**: `StandardObstacleFactory.cs` dynamically generates obstacles from scenes.
- **Builder Pattern**: `ICoinFormationBuilder` allows structured creation of coin formations (`RowCoinBuilder`, `DiagonalCoinBuilder`, `SingleCoinBuilder`).
- **Abstract Factory (conceptual)**: Interchangeable formation strategies via a unified interface.
- **Singleton**: `LaneManager` manages per-lane speed logic.

### ✅ Structural Patterns:

- **Facade**: `Player.cs` simplifies complex transitions between states and animations.
- **Composite**: Uniform handling of obstacles/coins for spawning and cleanup via interfaces.
- **Module**: Clear file layout separating concerns (Managers, Builders, Commands, States).
- **Adapter-like**: Input abstraction via `PlayerInputFacade`.

### ✅ Behavioral Patterns:

- **Command Pattern**: `ICommand` encapsulates logic for jump, slide, and lane switching.
- **State Pattern**: `IPlayerState` interface manages `RunState`, `JumpState`, `IdleState`, and `SlideState`.
- **Strategy Pattern**: `SwitchLeftStrategy` and `SwitchRightStrategy` drive lane change logic.
- **Observer Pattern**: Godot signals handle `score_updated`, `start_game`, and `gameover` notifications.

---

## 🎨 Custom Shaders

All shaders are custom-written in `.gdshader` format using GLSL.

1. **Lava Floor Scroll Shader** (`GameManager.gdshader`)

   - Scrolls the floor texture vertically and applies horizontal edge fade using `smoothstep`.

2. **Heat Distortion Shader** (`HeatDistortion.gdshader`)

   - Simulates heat haze via vertical scrolling texture distortion.

3. **Coin Glow Shader** (`CoinGlow.gdshader`)
   - Emits a glowing golden effect to make coins visually distinct.

✅ Each shader is functional and used in gameplay.

---

## 🛠️ Godot Engine Features Used

- **Physics**: `CharacterBody3D`, collision detection, and gravity logic
- **AnimationPlayer**: Smooth transition between run, jump, and slide animations
- **Timers**: For spawning obstacles and score updates
- **Signals**: Decoupled communication between `Player`, `GameManager`, and `UIManager`
- **Audio**: Background music playback with volume control
- **PackedScene**: Dynamic instancing of obstacles and coins
- **Export Variables**: Easily configurable values for tuning

---

## 🚀 How to Run the Game

1. **Clone the repository**:
   ```bash
   git clone https://github.com/rithindattag/CPSC-5270-01-25SQ-Game-Project.git
   cd ashfall-descent
   git checkout final
   ```
