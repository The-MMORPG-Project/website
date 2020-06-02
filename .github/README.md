<p align="center">
  <a href="" rel="noopener">
 <img width=600 height=225 src="https://i.imgur.com/AlP8aFy.png" alt="Project logo"></a>
</p>

<div align="center">
  
  [![Discord][discord]][discord-url]
  [![Maintenance][maintenance]][discord-url]
  [![Ask Me Anything!][ask-me-anything]][discord-url]
  [![Issues][issues]][issues-url]
  [![License][license]][license-url]
  
</div>

---

<p align="center"> The purpose of this boilerplate is to make multiplayer in Unity a piece of cake, specifically for MMORPGs. The boilerplate consists of a launcher, a web server, a game server and a client. The idea is the user loads up the launcher, updates the client, launches the client, logs in to an account through the web server and then connects to the game server.
    <br> 
</p>

## Table of Contents

1. [Features](#features)
2. [Contributing](#contributing)
3. [Contributors](#contributors)

## Features

**ENet Server**

- Multi-Threaded Console Input Ouput Buffers (currently very bugged - [#2](/../../issues/2) [#4](/../../issues/4) [#5](/../../issues/5) [#6](/../../issues/6) [#15](/../../issues/15))
- Fast Serialization / Deserialization with BinaryFormatter and BinaryReader
- Reliable UDP Networking with ENet

**Web Server**

- MySQL Database to Store User Accounts, Account Information and Purchases (account info / purchases not started on yet)

**Client**

- UI Flow to Demonstrate Communication with Web Server / ENet Server

**Launcher**

- Handles Auto-Updating Client (not complete)
- Grabs Release Notes (not complete)

## Contributing

This project is no where near complete, all contributions are highly appreciated and welcomed.

Please talk to me over Discord (**valk#3277**) to learn more on how you can contribute to the project. If you don't want to wait for me to accept your friend request, you can join [my discord](https://discord.gg/thMupbv) and then you'll be able to send me messages without a friend request.

Also see [CONTRIBUTING.md](https://github.com/valkyrienyanko/Unity-ENet-Model/blob/master/.github/CONTRIBUTING.md)

And take a look at the [Roadmap to v0.2](https://github.com/valkyrienyanko/Unity-ENet-Model/issues/12) and [Roadmap to v0.3](https://github.com/valkyrienyanko/Unity-MMORPG-Boilerplate/issues/31)

## Contributors

<a href="https://github.com/valkyrienyanko/Unity-ENet-Model/graphs/contributors">
  <img src="https://contributors-img.web.app/image?repo=valkyrienyanko/Unity-ENet-Model" />
</a>

<!--BADGES AND LINKS-->
<!--Discord Badge Image-->
[discord]: https://img.shields.io/discord/453710350454620160.svg
<!--Discord Link-->
[discord-url]: https://discord.gg/thMupbv
<!--Maintenance Image-->
[maintenance]: https://img.shields.io/badge/Maintained%3F-yes-green.svg 
<!--Ask Me Anything Image-->
[ask-me-anything]: https://img.shields.io/badge/Ask%20me-anything-1abc9c.svg 
<!--Issues Image-->
[issues]: https://img.shields.io/github/issues/valkyrienyanko/Unity-ENet-Model 
<!--Issues Link-->
[issues-url]: https://github.com/valkyrienyanko/Unity-MMORPG-Boilerplate/issues 
<!--License Image-->
[license]: https://img.shields.io/badge/license-MIT-blue.svg
<!--License URL-->
[license-url]: https://github.com/valkyrienyanko/Unity-MMORPG-Boilerplate/blob/master/LICENSE
