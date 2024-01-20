# Mojang.Api.Skins.Demo.Godot
## Overview
This repository showcases a demonstration of the [**'Mojang.Api.Skins'**](https://github.com/CwistSilver/Mojang.Api.Skins) in a Godot-based application. It enables users to enter a Minecraft player's name and then displays the current skin and cape of that player on a 3D Minecraft character model. The demo provides interactive features such as toggling individual 3D parts of the character and accessories, as well as displaying the cape as a 3D model, which can be toggled or shown as an Elytra. Additionally, it displays metadata in the upper left corner of the screen.

## Features
- **Player Skin and Cape Display**: Enter a player's name to fetch and display their current Minecraft skin and cape.
- **3D Model Interaction**: Interact with the 3D Minecraft character model by toggling different body parts and accessories.
- **Cape Customization**: Toggle between displaying the cape as a standard cape or as an Elytra.
- **Metadata Display**: View metadata about the player's skin and cape in real-time.
- **Demonstration of 'Mojang.Api.Skins'**: Utilizes the functionalities of the **'Mojang.Api.Skins'** library in a practical application.

## Getting Started
To get started with this demo, follow these steps:

1. **Clone the Repository**
```shell
git clone https://github.com/CwistSilver/Mojang.Api.Skins.Demo.Godot.git
```
2. **Open the Project in Godot**\
Launch Godot and import the project.
3. **Run the Demo**\
Execute the project within Godot to interact with the demo.

## Troubleshooting
### Issue with Loading Scene in Godot
**Problem Description:**\
When cloning the project and opening it in Godot, you might encounter an issue where the scene minecraft_classic_player-test.tscn does not load correctly. This can lead to the program not functioning as intended.

**Solution:**\
To resolve this issue, follow these steps:

1. **Navigate to the '3D_Models Folder'**: In the cloned project directory, locate the 3D_Models folder.
2. **Open the Scene Manually**: Find the file named **'minecraft_classic_player-test.tscn'** and double-click to open it manually in Godot.
3. **Close the Scene**: After the scene opens, close it without making any changes.

After performing these steps, the issue should be resolved, and the program will function as expected.
