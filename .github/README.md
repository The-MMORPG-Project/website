![Preview](https://i.gyazo.com/acf5808f64dbf0971198c6b01ec0433c.png)

[![Discord][discord]][discord-url]
[![Maintenance](https://img.shields.io/badge/Maintained%3F-yes-green.svg)][discord-url]
[![Ask Me Anything!](https://img.shields.io/badge/Ask%20me-anything-1abc9c.svg)][discord-url]
[![Issues](https://img.shields.io/github/issues/valkyrienyanko/Unity-ENet-Model)](https://github.com/valkyrienyanko/Unity-ENet-Model/issues)

<h1>Unity ENet Model</h1>
ENet-CSharp provides support for sending both unreliable and reliable packets over the network, this means we can handle things such as user authentication and player position updates at the same time with no problems. This project was designed with the intent for one headless server to provide connections to many clients. If you are creating a MMORPG in Unity then this project is most likely in your interests.

## Table of Contents
1. [Setup Client](#setup-client)
2. [Setup Server](#setup-server)
3. [Features](#features)
4. [Releases](#releases)
5. [Tests](#tests)
6. [Issues](#issues)
7. [Contributing](#contributing)
8. [Contributors](#contributors)

<h2 align="center">Setup Client</h2>

Use [Unity Hub](https://unity3d.com/get-unity/download) on version `2019.3.12f1` or later to launch the project

<h2 align="center">Setup Server</h2>

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

<h2 align="center">Features</h2>

- Reliable UDP Networking with ENet
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

<h2 align="center">Tests</h2>

Works fine with 3 CCU's (Concurrently Connected Users)

Would test with more but I currently only have 2 alpha testers.

<h2 align="center">Issues</h2>

Have a look at the projects current [issues](https://github.com/valkyrienyanko/Unity-ENet-Model/issues)

Please make time to read and follow the [Issue Guidelines](https://github.com/valkyrienyanko/Unity-ENet-Model/issues/1)

<h2 align="center">Contributing</h2>

Please talk to **valk#3277** over Discord to learn more on how you can contribute to the project.

You can also read the [Contributing Guide](https://github.com/valkyrienyanko/Unity-ENet-Model/blob/master/.github/CONTRIBUTING.md)

<h2 align="center">Contributors</h2>

<a href="https://github.com/valkyrienyanko/Unity-ENet-Model/graphs/contributors">
  <img src="https://contributors-img.web.app/image?repo=valkyrienyanko/Unity-ENet-Model" />
</a>

[discord]: https://img.shields.io/discord/453710350454620160.svg
[discord-url]: https://discord.gg/thMupbv
