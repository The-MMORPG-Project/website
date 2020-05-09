![Preview](https://i.gyazo.com/acf5808f64dbf0971198c6b01ec0433c.png)

[![GitHub license][license]][license-url]
[![Issues][issues]][issues-url]
[![Discord][discord]][discord-url]
[![GitHub stars][stars]][stars-url]
[![GitHub forks][forks]][forks-url]

<h1>Unity ENet Model</h1>
Unity Client-Server model for reliable UDP networking.

## Table of Contents
1. [Setup Client](#setup-client)
2. [Setup Server](#setup-server)
3. [Features](#features)
4. [Contributing](#contributing)
5. [Contributors](#contributors)

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
- Wrapper Classes to Simplify Networking Send / Receive
- Straight Forward Friendly Client UX
- Server Console Command Input Functionality
- Custom Server Input / Output Console Buffers / Controls

<h2 align="center">Contributing</h2>

1. Please talk to **valk#3277** over Discord if you're interested in contributing.
2. Read the [Contributing Guide](https://github.com/valkyrienyanko/Unity-ENet-Model/blob/master/.github/CONTRIBUTING.md)

<h2 align="center">Contributors</h2>

<a href="https://github.com/valkyrienyanko/Unity-ENet-Model/graphs/contributors">
  <img src="https://contributors-img.web.app/image?repo=valkyrienyanko/Unity-ENet-Model" />
</a>

[license]: https://img.shields.io/github/license/valkyrienyanko/Unity-ENet-Model?color=brightgreen
[license-url]: https://github.com/valkyrienyanko/Unity-ENet-Model/blob/master/LICENSE
[issues]: https://img.shields.io/github/issues/valkyrienyanko/Unity-ENet-Model
[issues-url]: https://github.com/valkyrienyanko/Unity-ENet-Model/issues
[discord]: https://img.shields.io/discord/453710350454620160.svg
[discord-url]: https://discord.gg/thMupbv
[stars]: https://img.shields.io/github/stars/valkyrienyanko/Unity-ENet-Model?color=brightgreen
[stars-url]: https://github.com/valkyrienyanko/Unity-ENet-Model/stargazers
[forks]: https://img.shields.io/github/forks/valkyrienyanko/Unity-ENet-Model?color=brightgreen
[forks-url]: https://github.com/valkyrienyanko/Unity-ENet-Model/network
