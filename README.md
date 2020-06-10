<p align="center">
  <a href="" rel="noopener">
 <img width=600 height=225 src="https://i.imgur.com/FsnlF8g.png" alt="Project logo"></a>
</p>

<div align="center">

  [![Discord][discord]][discord-url]

</div>

<p align="center">The purpose of this project is to make multiplayer in Unity a piece of cake, specifically for MMORPGs. The project consists of a launcher, a web server, a game server and a client. The idea is the user loads up the launcher, updates the client, launches the client, logs in to an account through the web server and then connects to the game server.</p>

## Features
### Launcher

1. Fetches latest client release / release notes and launches the client

### Client

1. UI Flow to Demonstrate Communication with Web Server / ENet Server
2. RPG Demo demonstrating the flow of RPG elements syrnonized over the network (planned / not here yet)

### Web Server

1. User Authentication
2. MySQL Database to Store User Accounts, Account Information and Purchases (account info / purchases not started on yet)
3. Website

### Game Server

1. Custom Console Buffer
    - Simultaneous Input Output Functionality
    - Scrolling Behavior
    - Command History
2. Fast Serialization / Deserialization with BinaryFormatter and BinaryReader
3. Reliable UDP Networking with ENet

## Contributing

The project is no where near complete, it receives frequent updates, is on-going and all contributions are welcomed.

If you would like to contribute, please join [The MMORPG Project](https://discord.gg/W4Nk9gt) Discord and talk to valk.

<!-- BADGES -->
[discord]: https://img.shields.io/discord/717790645900673084.svg
<!-- Discord Link -->
[discord-url]: https://discord.gg/W4Nk9gt
