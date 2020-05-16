![PreviewServer](https://i.gyazo.com/acf5808f64dbf0971198c6b01ec0433c.png)

[![Discord][discord]][discord-url]
[![Maintenance](https://img.shields.io/badge/Maintained%3F-yes-green.svg)][discord-url]
[![Ask Me Anything!](https://img.shields.io/badge/Ask%20me-anything-1abc9c.svg)][discord-url]
[![Issues](https://img.shields.io/github/issues/valkyrienyanko/Unity-ENet-Model)](https://github.com/valkyrienyanko/Unity-MMORPG-Boilerplate/issues)

<h1>Unity MMORPG Boilerplate</h1>
I found out the hard way that UNet and the Unity Transport Layer were really only mean't for LAN and P2P. In light of this I discovered a reliable UDP networking library called ENet which provides a middle line between sending unreliable and reliable packets. This project is intended to be a boilerplate for all your Unity MMORPGs.

<h2>Table of Contents</h2>

1. [Setup Client](#setup-client)
2. [Setup ENet Server](#setup-enet-server)
3. [Setup Web Server](#setup-web-server)
4. [Features](#features)
5. [Releases](#releases)
6. [Stress Tests](#stress-tests)
7. [Issues](#issues)
8. [Contributing](#contributing)
9. [Contributors](#contributors)

<h2 align="center">Setup Client</h2>

Use [Unity Hub](https://unity3d.com/get-unity/download) on version `2019.3.13f1` or later to launch the project.

**IMPORTANT** Do not use Unity 2020 versions or you will run into many issues!

<h2 align="center">Setup ENet Server</h2>

Setup the database
```
dotnet tool install --global dotnet-ef
dotnet add package Microsoft.EntityFrameworkCore.Design
dotnet ef migrations add InitialCreate
dotnet ef database update
```
Run the server
```
dotnet run
```

<h2 align="center">Setup Web Server</h2>

1. Install and setup [MySQL](https://dev.mysql.com/downloads/installer/)
2. Install yarn 
```
npm i -g yarn
```
3. Install dependencies 
```
yarn install
```
4. Run server
```
yarn dev:server
```

<h2 align="center">Features</h2>

- Reliable UDP Networking with ENet
- Fast Serialization / Deserialization with BinaryFormatter and BinaryReader
- Server Database for Users
- Client-Server Account Management
- Headless Server
- Wrapper Classes to Simplify Networking Send / Receive
- Straight Forward Friendly Client UX
- Server Console Command Input Functionality
- Custom Server Input / Output Console Buffers / Controls

<h2 align="center">Releases</h2>

The project is still in its alpha stages, not everything may work as you expect. 

[Releases](https://github.com/valkyrienyanko/Unity-ENet-Model/releases)

<h2 align="center">Stress Tests</h2>

Works fine with 3 CCU's (Concurrently Connected Users)

Would test with more but I currently only have 2 alpha testers.

Join the [Discord](https://discord.gg/thMupbv) and ask **valk#3277** for the Alpha Tester role if you want to become a Alpha Tester.

<h2 align="center">Issues</h2>

Have a look at the projects current [issues](https://github.com/valkyrienyanko/Unity-ENet-Model/issues)

Please make time to read and follow the [Issue Guidelines](https://github.com/valkyrienyanko/Unity-ENet-Model/issues/1)

<h2 align="center">Contributing</h2>

Please talk to **valk#3277** over Discord to learn more on how you can contribute to the project.

You can also read the [Contributing Guide](https://github.com/valkyrienyanko/Unity-ENet-Model/blob/master/.github/CONTRIBUTING.md)

And take a look at the [Roadmap to v0.2](https://github.com/valkyrienyanko/Unity-ENet-Model/issues/12)

<h2 align="center">Contributors</h2>

<a href="https://github.com/valkyrienyanko/Unity-ENet-Model/graphs/contributors">
  <img src="https://contributors-img.web.app/image?repo=valkyrienyanko/Unity-ENet-Model" />
</a>

[discord]: https://img.shields.io/discord/453710350454620160.svg
[discord-url]: https://discord.gg/thMupbv
