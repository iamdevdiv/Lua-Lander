# Lua Lander

A 2D physics-based lunar lander game made in Unity 6 by following [Code Monkey's Lua Lander course](https://unitycodemonkey.teachable.com/p/lua-lander). I built this project to learn the fundamentals of 2D game development, Unity physics, and clean C# project architecture.

## Gameplay

The objective is to guide the lander to the landing pads across multiple levels while conserving fuel and managing momentum in low gravity.

- **Thrust and Momentum**: Firing thrusters burns fuel and pushes the ship in the direction it faces. Because there is minimal drag, the ship preserves speed and requires counter-steering to slow down.
- **Landing Constraints**: Landing successfully requires touching down upright and keeping impact speed below the maximum landing velocity. Landing too fast or at a steep angle destroys the ship.
- **Cargo and Hazards**: Levels include fuel refills to pick up, cargo crates to hook up and transport with rope physics, and automated turrets that fire at the lander.

## What I Learned and Implemented

- **Rigidbody2D Physics**: Applying forces and torque to handle spaceship momentum, gravity scale adjustments, and velocity dampening.
- **Landing Vector Math**: Using vector dot products to compare the lander's upward angle with world up, ensuring touchdowns only count as successful when the craft is upright.
- **C# Event Architecture**: Using standard C# `System.Action` and `EventHandler` events on the lander to trigger audio cues and particle effects without tightly coupling systems together.
- **Singletons for Core Systems**: Using singletons on persistent managers (`GameManager`, `SoundManager`, `MusicManager`) to handle global game states, audio volume, and scene reloading.
- **Data Configuration with ScriptableObjects**: Using ScriptableObjects to store cargo definitions and level pickup metadata.
- **Unity Input System**: Setting up input actions across keyboard, gamepads, and screen touch areas for mobile play.

## Controls

### Desktop
- **Main Thruster**: `W` or `Up Arrow`
- **Rotate Left**: `A` or `Left Arrow`
- **Rotate Right**: `D` or `Right Arrow`
- **Pause Menu**: `Escape`

### Mobile (Touchscreen)
- **Flight Controls**: Drag the on-screen joystick up for main thrust, or drag left and right to rotate.
- **Pause Menu**: Android back button

## Downloads

Pre-built binaries can be downloaded from the Releases tab:
- **Windows**: Download `LuaLander-Windows.zip`, extract the files, and run `LuaLander.exe`.
- **Android**: Download and install `LuaLander.apk`. Because this APK is signed with a default development key and not distributed through the Google Play Store, Android or Google Play Protect will flag it as an unknown app. Select "More details" and then "Install anyway" to complete the installation.

## Running from Source

1. Clone the repository:
   ```bash
   git clone https://github.com/iamdevdiv/LuaLander.git
   ```
2. Open the project folder in Unity Hub using Unity version `6000.0.55f1` or later.
3. Open `Assets/Scenes/MainMenuScene.unity` and click Play.

## Credits

Game design, artwork, audio assets, and project walkthrough created by [Code Monkey](https://unitycodemonkey.com/).
