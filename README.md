<p align="center">
  <a href="" rel="noopener">
 <img width=600 height=225 src="https://i.imgur.com/FsnlF8g.png" alt="Project logo"></a>
</p>

<div align="center">

  [![Discord][discord]][discord-url]

</div>

<p align="center">The purpose of this project is to make multiplayer in Unity a piece of cake (mostly targeting dedicated MMORPG servers). The project consists of a launcher, a web server, a game server and a client. The idea is the user loads up the launcher, updates the client, launches the client, logs in to an account through the web server and then connects to the game server.</p>

## NOTICE
**This project has been abandoned** as the ENet code violates the rules of thread safety, the game server console uses a library that causes all sorts of issues for what I was trying to accomplish and I don't have the time to work on this. Please refer to [Kittens Rise Up](https://github.com/Kittens-Rise-Up), this project uses The MMORPG Project template, it is more up-to-date, does not depend on a third party library for the game server console and ensures thread safety. Kittens Rise Up is a upcoming resource management / long-term progression MMORPG. The game (and everything else connected to it) will remain free and open source forever.

## How it Works
The launcher fetches the latest client releases from web server and then launches that release. The web server handles user authentication. The game server handles all game related networking events.

## Repositories
https://github.com/The-MMORPG-Project/Launcher

https://github.com/The-MMORPG-Project/Client

https://github.com/The-MMORPG-Project/Web-Server

https://github.com/The-MMORPG-Project/Game-Server

## Contributing

The project is on-going, all contributors are welcomed.

If you are interested in contributing, please join [The MMORPG Project](https://discord.gg/W4Nk9gt) Discord and talk to valk#3277.

<!-- BADGES -->
[discord]: https://img.shields.io/discord/717790645900673084.svg
<!-- Discord Link -->
[discord-url]: https://discord.gg/W4Nk9gt
