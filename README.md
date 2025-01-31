# Tower Defense AR

## Description
A brief overview of the Tower Defense AR game.

## Installation
1. Clone the repository:
   ```bash
   git clone https://github.com/OmerKilicc/tower-defense-ar.git
   cd tower-defense-ar
   ```
2. Install dependencies:
   ```
   Unity Version: 2024.3.15f1
   ```

## Architecture Overview

### Project Structure Overview

1. **Assets**: Contains all the game assets, including textures, models, audio files, and other resources used in the game.
2. **Packages**: Includes references to Unity packages that the project depends on, providing additional functionality.
3. **ProjectSettings**: Holds various configuration files for the Unity project, such as graphics and player settings.

### Key Components

- **Game Engine**: Built using Unity, supporting 2D and 3D game development, including AR capabilities.
- **AR Support**: Utilizes Unity's AR Foundation for building AR experiences across different platforms.
- **Game Logic**: Contains scripts defining game mechanics, such as enemy behavior, player controls, and game state management.

### General Architecture Overview

1. **Game Manager**:
   - Central script that manages the overall game state, including starting and ending the game, managing levels, and tracking player scores.

2. **Tower Class**:
   - Represents the towers that players can place. This class typically includes properties such as damage, range, and firing rate, as well as methods for targeting enemies and firing projectiles.

3. **Enemy Class**:
   - Represents the enemies that will move along a path toward the player's base. This class usually includes properties like health, speed, and behavior patterns (e.g., different types of enemies with unique abilities).

4. **Pathfinding**:
   - Logic to determine the path that enemies will take to reach the player's base. This implementation uses Depth First Search (DFS) for pathfinding.

5. **Projectile Class**:
   - Represents the projectiles fired by towers. This class typically includes properties for speed, damage, and methods for detecting collisions with enemies.

6. **UI Manager**:
   - Manages the user interface elements, such as displaying the player's score, health, and available towers. It handles user input for placing towers and upgrading them.

7. **Level Manager**:
   - Responsible for loading and managing different levels, including spawning enemies and determining win/loss conditions.

8. **Audio Manager**:
   - Manages sound effects and background music throughout the game, enhancing the player's experience.

### Data Flow
- **User Input**: Captured through UI elements to place towers and interact with the game.
- **Game State Updates**: The Game Manager updates the game state based on player actions and enemy movements.
- **Rendering**: Unity handles rendering the game scene, including towers, enemies, projectiles, and UI elements.

### Summary

The Tower Defense AR project is structured to facilitate the development of an augmented reality game using Unity. It includes essential directories for assets, project settings, and version control, while leveraging Unity's powerful features for AR and game development.

## License
This project is licensed under the MIT License.
