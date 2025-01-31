# 🎮 Tower Defense AR

An immersive augmented reality tower defense game where players defend their territory by strategically placing towers in the real world environment. Built with Unity and AR Foundation, this game combines classic tower defense mechanics with cutting-edge AR technology.

## 🌟 Features

- **Augmented Reality Experience**
  - Seamless real-world environment integration
  - Dynamic tower placement in AR space
  - Interactive gameplay in physical surroundings
  - Responsive AR touch controls

- **Tower Defense Mechanics**
  - Strategic tower placement system
  - Multiple tower types with unique abilities
  - Tower upgrade paths
  - Resource management system

- **Enemy AI System**
  - Advanced pathfinding using DFS algorithm
  - Multiple enemy types with different behaviors
  - Wave-based progression
  - Dynamic difficulty scaling

- **Game Systems**
  - Score tracking and leaderboards
  - Level progression system
  - Resource economy
  - Wave management

## 🔧 Technical Specifications

### Requirements
- Unity 2022.3.15f1
- AR-capable mobile device
- AR Foundation package
- Compatible with ARKit (iOS) and ARCore (Android)

### Architecture Highlights
- Modular component system
- Optimized AR performance
- Efficient pathfinding implementation
- Robust game state management

## 🎯 Core Gameplay Mechanics

### Tower System
1. Place towers in AR space
2. Upgrade and enhance tower capabilities
3. Strategic positioning for maximum effectiveness
4. Resource management for tower deployment

### Enemy Waves
1. Progressive wave difficulty
2. Various enemy types and patterns
3. Strategic pathfinding challenges
4. Boss waves and special events

## 🚀 Development Setup

1. Clone the repository
```bash
git clone https://github.com/OmerKilicc/tower-defense-ar.git
cd tower-defense-ar
```

2. Open project in Unity 2022.3.15f1

3. Install required packages:
   - AR Foundation
   - AR Subsystems
   - XR Plugin Management

## 🎨 Project Structure

```
Assets/
├── _Scripts/
│   ├── AR/             # AR functionality
│   ├── Debug/          # Debugging tools
│   ├── Enemy/          # Enemy AI
│   ├── Events/         # Events system
│   ├── Grid/           # Grid management
│   ├── Managers/       # Game managers
│   ├── Player/         # Player controls
│   ├── Projectile/     # Projectiles
│   └── Tower/          # Tower behaviors
├── _Prefabs/
│   ├── ARXR/           # ARXR prefabs
│   ├── Enemy/          # Enemy prefabs
│   ├── Grid/           # Grid prefabs
│   ├── Managers/       # Managers prefabs
│   ├── Tower/          # Tower prefabs
│   └── UI/             # UI prefabs
├── ThirdParty/         # Third-party libraries
├── TextMesh Pro/       # TextMesh Pro assets
├── SOs/                # Scriptable Objects
├── SFX/                # Sound effects
├── Scenes/             # Game scenes
├── Plugins/            # Plugins
├── Meshes/             # 3D meshes
└── Materials/          # Materials
```

## ⚡ Key Components

### Game Manager
- Central game state control
- Wave management
- Score tracking
- Resource economy

### AR System
- Environment scanning
- Surface detection
- Tower placement validation
- Touch interaction handling

### Tower Manager
- Tower spawning
- Upgrade system
- Target acquisition
- Attack patterns

### Enemy System
- Spawn management
- Pathfinding
- Behavior patterns
- Difficulty scaling

## 📝 Development Notes

- Optimized for mobile AR performance
- Focus on user experience and intuitive controls
- Regular testing on both iOS and Android devices
- Emphasis on code maintainability and scalability

## 📜 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

---

Built with Unity 2022.3.15f1