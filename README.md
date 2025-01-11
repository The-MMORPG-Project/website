<p align="center">
  <a href="" rel="noopener">
 <img width=600 height=225 src="https://i.imgur.com/FsnlF8g.png" alt="Project logo"></a>
</p>

<p align="center">The purpose of this project is to make multiplayer in Unity a piece of cake (mostly targeting dedicated MMORPG servers). The project consists of a launcher, a web server, a game server and a client. The idea is the user loads up the launcher, updates the client, launches the client, logs in to an account through the web server and then connects to the game server.</p>

## This project has been abandoned
- Project does not follow rules of thread safety
- 3rd party libs causing all sorts of issues
- Not a fan of Unity anymore

Please refer to [Redot Multiplayer Template Wiki](https://github.com/CSharpRedotTools/Template/wiki/Multiplayer). No pre-built scenarios have been setup for testing MMO environments. However the building blocks are there for you to easily send minimal data across the network almost effortlessly.

## How it Works
The launcher fetches the latest client releases from web server and then launches that release. The web server handles user authentication. The game server handles all game related networking events.

## Repositories
https://github.com/The-MMORPG-Project/Launcher

https://github.com/The-MMORPG-Project/Client

https://github.com/The-MMORPG-Project/Web-Server

https://github.com/The-MMORPG-Project/Game-Server
