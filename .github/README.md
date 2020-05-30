![PreviewServer](https://i.gyazo.com/acf5808f64dbf0971198c6b01ec0433c.png)

[![Discord][discord]][discord-url]
[![Maintenance][maintenance]][discord-url]
[![Ask Me Anything!][ask-me-anything]][discord-url]
[![Issues][issues]][issues-url]

<h1>Unity MMORPG Boilerplate</h1>
I found out the hard way that UNet and the Unity Transport Layer were really only mean't for LAN and P2P. In light of this I discovered a reliable UDP networking library called ENet which provides a middle line between sending unreliable and reliable packets. This project is intended to be a boilerplate for all your Unity MMORPGs.

<h2>Table of Contents</h2>

1. [Features](#features)
2. [Contributing](#contributing)
3. [Contributors](#contributors)

<h2 align="center">Features</h2>

*Note: Not all of these features have been implemented or fully completed.

<h3 align="left">ENet Server</h3>

- Multi-Threaded Console Input Ouput Buffers
- Fast Serialization / Deserialization with BinaryFormatter and BinaryReader
- Reliable UDP Networking with ENet

<h3 align="left">Web Server</h3>

- MySQL Database to Store User Accounts, Account Information and Purchases

<h3 align="left">Client</h3>

- UI Flow to Demonstrate Communication with Web Server / ENet Server

<h3 align="left">Launcher</h3>

- Handles Auto-Updating Client from GitHub Releases
- Grabs Release Notes from GitHub Releases

<h2 align="center">Contributing</h2>

This project is no where near complete, all contributions are highly appreciated and welcomed.

Please talk to me over Discord (**valk#3277**) to learn more on how you can contribute to the project. If you don't want to wait for me to accept your friend request, you can join [my discord](https://discord.gg/thMupbv) and then you'll be able to send me messages without a friend request.

Also see [CONTRIBUTING.md](https://github.com/valkyrienyanko/Unity-ENet-Model/blob/master/.github/CONTRIBUTING.md)

And take a look at the [Roadmap to v0.2](https://github.com/valkyrienyanko/Unity-ENet-Model/issues/12)

<h2 align="center">Contributors</h2>

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
