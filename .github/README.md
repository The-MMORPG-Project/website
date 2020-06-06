<!-- Super secret comment, if you see this you're an amazing person! -->
<p align="center">
  <a href="" rel="noopener">
 <img width=600 height=225 src="https://i.imgur.com/FsnlF8g.png" alt="Project logo"></a>
</p>

<div align="center">
  
  [![Discord][discord]][discord-url]
  [![Maintenance][maintenance]][discord-url]
  [![Ask Me Anything!][ask-me-anything]][discord-url]
  [![Issues][issues]][issues-url]
  [![License][license]][license-url]
  
</div>

---

<p align="center">The purpose of this project is to make multiplayer in Unity a piece of cake, specifically for MMORPGs. The project consists of a launcher, a web server, a game server and a client. The idea is the user loads up the launcher, updates the client, launches the client, logs in to an account through the web server and then connects to the game server.
    <br>
</p>

## Table of Contents

1. [Features](#features)
2. [Contributing](#contributing)

## Features

**Launcher**

1. Fetches latest client release / release notes and launches the client

**Client**

1. UI Flow to Demonstrate Communication with Web Server / ENet Server
2. RPG Demo demonstrating the flow of RPG elements syrnonized over the network (planned / not here yet)

**Web Server**

1. User Authentication
2. MySQL Database to Store User Accounts, Account Information and Purchases (account info / purchases not started on yet)

**Game Server**

1. Custom Console Buffer
    - Simultaneous Input Output Functionality
    - Scrolling Behavior
    - Command History
2. Fast Serialization / Deserialization with BinaryFormatter and BinaryReader
3. Reliable UDP Networking with ENet

## Contributing

The project is no where near complete, it receives frequent updates, is on-going and all contributions are welcomed.

See [Getting Started](https://github.com/valkyrienyanko/The-MMORPG-Project/blob/master/.github/CONTRIBUTING.md#getting-started)

<a href="https://github.com/valkyrienyanko/The-MMORPG-Project/graphs/contributors">
  <img src="https://contributors-img.web.app/image?repo=valkyrienyanko/The-MMORPG-Project" />
</a>

<!--BADGES AND LINKS-->
<!--Discord Badge Image-->
[discord]: https://img.shields.io/discord/717790645900673084.svg
<!--Discord Link-->
[discord-url]: https://discord.gg/W4Nk9gt
<!--Maintenance Image-->
[maintenance]: https://img.shields.io/badge/Maintained%3F-yes-green.svg 
<!--Ask Me Anything Image-->
[ask-me-anything]: https://img.shields.io/badge/Ask%20me-anything-1abc9c.svg 
<!--Issues Image-->
[issues]: https://img.shields.io/github/issues/valkyrienyanko/The-MMORPG-Project 
<!--Issues Link-->
[issues-url]: https://github.com/valkyrienyanko/The-MMORPG-Project/issues 
<!--License Image-->
[license]: https://img.shields.io/badge/license-MIT-blue.svg
<!--License URL-->
[license-url]: https://github.com/valkyrienyanko/The-MMORPG-Project/blob/master/LICENSE
