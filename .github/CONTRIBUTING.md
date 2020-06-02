# Contributing Guide

## Table of Contents

1. [Getting Started](#getting-started)
2. [Setup Workspace](#setup-workspace)
    - [VSC](#visual-studio-code)
    - [VS](#visual-studio)
3. [Setup Project](#setup-project)
    - [Launcher](#launcher)
    - [Client](#client)
    - [Web Server](#web-server)
    - [Game Server](#game-server)
4. [Workflow](#workflow)
    - [Opening an Issue](#opening-an-issue)
    - [Creating a Pull Request](#creating-a-pull-request)
5. [Notes](#notes)
6. [Libraries](#libraries)

## Getting Started

More will eventually be put here. For now take a look at the roadmaps in the projects [issues](https://github.com/valkyrienyanko/Unity-MMORPG-Boilerplate/issues) and talk to me on Discord (**valk#3277**).

## Setup Workspace

### Visual Studio Code

I will recommend that you use Visual Studio Code as it is by far one of the best text editors for development that I've ever used, you will have no regrets when using it. You will need this editor when working with the client, web server and ENet game server.

#### VSC Installation

1. Download and install [Visual Studio Code](https://code.visualstudio.com)
2. Follow all steps in [Unity Development with VS Code](https://code.visualstudio.com/docs/other/unity)

#### VSC Extensions

Not all extensions are required for development but I highly recommend you at least read through them all. All extensions marked with an asterick (*) are required.
- [C#](https://marketplace.visualstudio.com/items?itemName=ms-dotnettools.csharp) (Lightweight development tools for .NET Core)*
- [Debugger for Unity](https://marketplace.visualstudio.com/items?itemName=Unity.unity-debug) (Debug Unity in the editor and the compiled build)*
- [Unity Meta Files Watcher](https://marketplace.visualstudio.com/items?itemName=PTD.vscode-unitymeta) (Watches Unity all meta files and changes them accordingly)*
- [Unity Tools](https://marketplace.visualstudio.com/items?itemName=Tobiah.unity-tools) (View the Unity Scripting Reference through a keybind)
- [Auto-Using for C#](https://marketplace.visualstudio.com/items?itemName=Fudge.auto-using) (Auto-imports and provides intellisense for references that were not yet imported in a C# file.)
- [Unity Code Snippets](https://marketplace.visualstudio.com/items?itemName=kleber-swf.unity-code-snippets) (Provides you with useful Unity Code Snippets on the fly)
- [Live Share](https://marketplace.visualstudio.com/items?itemName=MS-vsliveshare.vsliveshare) (Real-time collaboration through the VSC editor)
- [Discord Presence](https://marketplace.visualstudio.com/items?itemName=icrawl.discord-vscode) (Shows others what your working on)
- [Material Theme](https://marketplace.visualstudio.com/items?itemName=Equinusocio.vsc-material-theme) (Legacy material themes work the best in my opinion)

#### VSC Known Issues
##### VSC Intellisense does not work for Unity Namespaces

Solution: Downgrade to `C# 1.21.16`

### Visual Studio

Visual Studio setup info will eventually go here. You will need this IDE when working with the launcher.

## Setup Project
### Launcher

NOTE: This area is lacking information and will be updated sooner or later, please be patient..

1. Install dependencies with `yarn install`
2. OPTIONAL: Build the app through electron `yarn build:win` (`win` can be replaced with `mac` or `linux`) (note: you must be on the specified platform to build for that OS)

### Client

Use [Unity Hub](https://unity3d.com/get-unity/download) on version `2019.3.13f1` or later to launch the project.

**IMPORTANT** Do not use Unity 2020 versions or you will run into many issues!

### Web Server

1. Install and setup [MySQL](https://dev.mysql.com/downloads/installer/) (preferably setup on a remote dedicated machine)
2. Install dependencies with `yarn install`
3. Create `.env` file in Web root folder and fill following variables inside
```
DB_HOST=xxx.xxx.xxx.xxx
DB_USER=xxxxx
DB_PASSWORD=xxxxxxx
```
4. Run server with `yarn dev:start` (protip: just use `yarn dev:start` if you want to start without compiling tsc)

### Game Server

1. Clone `https://github.com/SoftwareGuy/ENet-CSharp`
2. Go into the `Source/Managed` folder and run `dotnet build`
3. Grab the `ENet-CSharp.dll` from `Source\Managed\bin\Debug\netcoreapp3.1` and add it to your VSCode references
4. Go to the game server directory and run the server with `dotnet run`

## Workflow

Fork the repository and create a pull request when you want to merge a change.

### Opening an Issue

1. Gather as much information as you can about the topic
2. Read the General Guidelines ([#1](https://github.com/valkyrienyanko/ENet-Model/issues/1))
3. Open an issue with a predefined template where possible
4. Provide as much context as possible in your issue!

### Creating a Pull Request

1. Talk to **valk#3277** on Discord before you create a PR.
2. Always test the PR to see if it works as intended with no additional bugs you may be adding!
3. State all the changes you made in the PR, not everyone will understand what you've done!

## Notes
### Unity Good Practices

- Try to avoid putting assets in the resources folder as much as possible as it increases game startup time and ram usage. (Eventually we will not use the Resources folder at all)
- Try to make use of Unity's prefabs as much as possible, I found that I ran into countless production issues by trying to code everything from scratch

### CSharp Formatting

- Private variables should be camelCase.
- If a public variable has around 2 characters, keep everything UPPERCASE (e.g. "ID")
- Public variables should follow the PascalFormat
- All methods should follow PascalFormat
- Add informative comments to clear up obscure code

## Libraries
### Launcher

- [Avalonia](https://avaloniaui.net)

### Unity Client

- [Unity Scripting Reference](https://docs.unity3d.com/ScriptReference/)

### ENet Server

- [ENet-CSharp](https://github.com/nxrighthere/ENet-CSharp)

### Web Server

- [MySQL](https://dev.mysql.com/downloads/installer/)
- [JSON Web Tokens](https://jwt.io)
- [promise-mysql](https://www.npmjs.com/package/promise-mysql)
- [bcrypt](https://www.npmjs.com/package/bcrypt)
- [Express](https://www.npmjs.com/package/express)
