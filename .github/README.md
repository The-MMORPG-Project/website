![Preview](https://i.gyazo.com/acf5808f64dbf0971198c6b01ec0433c.png)

[![GitHub license][license]](license-url)
[![Issues][issues]](issues-url)
[![Discord][discord]](discord-url)
[![GitHub stars][stars]](stars-url)
[![GitHub forks][forks]](forks-url)

<h1>Unity ENet Model</h1>
Unity Client-Server model for reliable UDP networking.

## Table of Contents
1. [Setup](#setup)
2. [Contributing](#contributing)

<h2 align="center">Setup</h2>
<h3 align="center">Server</h3>

1. Navigate to the server directory
2. Run the following commands to setup the database
```
dotnet tool install --global dotnet-ef
dotnet add package Microsoft.EntityFrameworkCore.Design
dotnet ef migrations add InitialCreate
dotnet ef database update
```
3. Run the server with `dotnet run`

<h3 align="center">Client</h3>

1. Use [Unity Hub](https://unity3d.com/get-unity/download) to launch the project
2. Make sure you're using `Unity 2019.3.12f1` or later

<h2 align="center">Contributing</h2>

1. Please talk to **valk#3277** over Discord if you're interested in contributing.
2. Read the [Contributing Guide](https://github.com/valkyrienyanko/Unity-ENet-Model/blob/master/.github/CONTRIBUTING.md)

[license]: https://img.shields.io/github/license/valkyrienyanko/Unity-ENet-Model?color=brightgreen
[license-url]: https://github.com/valkyrienyanko/Unity-ENet-Model/blob/master/LICENSE
[issues]: https://img.shields.io/github/issues/valkyrienyanko/Unity-ENet-Model
[issues-url]: https://github.com/valkyrienyanko/Unity-ENet-Model/issues
[discord]: https://img.shields.io/discord/453710350454620160.svg
[discord-url]: https://discordapp.com/invite/N9QVxbM
[stars]: https://img.shields.io/github/stars/valkyrienyanko/Unity-ENet-Model?color=brightgreen
[stars-url]: https://github.com/valkyrienyanko/Unity-ENet-Model/stargazers
[forks]: https://img.shields.io/github/forks/valkyrienyanko/Unity-ENet-Model?color=brightgreen
[forks-url]: https://github.com/valkyrienyanko/Unity-ENet-Model/network
