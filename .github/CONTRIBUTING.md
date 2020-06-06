# Contributing Guide

## Table of Contents

1. [Getting Started](#getting-started)
    - [Welcome](#welcome)
    - [Issue Labels](#issue-labels)
2. [Setup Workspace](#setup-workspace)
    - [VSC](#visual-studio-code)
3. [Setup Project](#setup-project)
    - [Launcher](#launcher)
    - [Client](#client)
    - [Web Server](#web-server)
    - [Game Server](#game-server)
4. [Workflow](#workflow)
    - [Working on Active Issues](#working-on-active-issues)
    - [Opening an Issue](#opening-an-issue)
    - [Creating a Pull Request](#creating-a-pull-request)
5. [Notes](#notes)
6. [Libraries](#libraries)

## Getting Started
### Welcome
All contributors please join [The MMORPG Project Discord](https://discord.com/invite/W4Nk9gt) and talk to **valk#3277** before you make any contributions.

### Issue Labels
There are many labels that help organize the projects issues. If you can't find any issues open for a specific label, then it probably means that more issues need to be created with that label to help contributors like you figure out how to contribute to the project further.

You can also view the projects milestones [here](https://github.com/valkyrienyanko/The-MMORPG-Project/milestones). Milestones help tell contributors what needs to be done in order more or less.

**Roadmaps**
- [Roadmaps](https://github.com/valkyrienyanko/The-MMORPG-Project/issues?q=is%3Aopen+is%3Aissue+label%3Aroadmap)

**Generic**
- [Good First Issues](https://github.com/valkyrienyanko/The-MMORPG-Project/issues?q=is%3Aopen+is%3Aissue+label%3A"good+first+issue")
- [Help Wanted](https://github.com/valkyrienyanko/The-MMORPG-Project/issues?q=is%3Aopen+is%3Aissue+label%3A"help+wanted")
- [Bug](https://github.com/valkyrienyanko/The-MMORPG-Project/issues?q=is%3Aopen+is%3Aissue+label%3Abug)
- [Question](https://github.com/valkyrienyanko/The-MMORPG-Project/issues?q=is%3Aopen+is%3Aissue+label%3Aquestion)
- [Request](https://github.com/valkyrienyanko/The-MMORPG-Project/issues?q=is%3Aopen+is%3Aissue+label%3Arequest)

**Specific**
- [Launcher](https://github.com/valkyrienyanko/The-MMORPG-Project/issues?q=is%3Aopen+is%3Aissue+label%3Alauncher)
- [Client](https://github.com/valkyrienyanko/The-MMORPG-Project/issues?q=is%3Aopen+is%3Aissue+label%3Aclient)
- [Game Server](https://github.com/valkyrienyanko/The-MMORPG-Project/issues?q=is%3Aopen+is%3Aissue+label%3A"game+server")
- [Web Server](https://github.com/valkyrienyanko/The-MMORPG-Project/issues?q=is%3Aopen+is%3Aissue+label%3A"web+server")

Scroll up a bit for [Table of Contents](https://github.com/valkyrienyanko/The-MMORPG-Project/blob/master/.github/CONTRIBUTING.md#table-of-contents)

## Setup Workspace

### Visual Studio Code

I will recommend that you use Visual Studio Code as it is by far one of the best text editors for development that I've ever used, you will have no regrets when using it. You will need this editor when working with the launcher, client, web server and game server.

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

## Setup Project
### Launcher

1. Navigate to the Launcher directory
2. Install dependencies with `yarn install`
3. Start launcher with `yarn dev:start` (protip: use `yarn start` to run without developer tools)
    - Open debugger with `Ctrl + Shift + I`
    - Reload page with `Ctrl + R`
    - Run linter with `yarn dev:lint`
4. Optional: Build the project's binaries with `yarn build:win` (`win` can be replaced with `linux` or `mac`, you can only build a platform if you're currently on that OS, for e.g. you can't build mac or linux on windows)
5. Note that you will need to setup the web server when testing out the 'Play' button (or temporarily comment out the communication to the web server and provide your own latest.zip test file)

### Client

Use [Unity Hub](https://unity3d.com/get-unity/download) on version `2019.3.13f1` or later to launch the project.

**IMPORTANT** Do not use Unity 2020 versions or you will run into many issues!

### Web Server

1. Install and setup [MySQL](https://dev.mysql.com/downloads/installer/) (preferably setup on a remote dedicated machine)
2. Navigate to the web directory
3. Install dependencies with `yarn install`
4. Create `.env` file in Web root folder and fill following variables inside
```
DB_HOST=xxx.xxx.xxx.xxx
DB_USER=xxxxx
DB_PASSWORD=xxxxxxx
```
5. Populate the `src/releases` folder with build(s) from the Unity standalone. (The folder must be called `latest` and must be compressed to a zip called `latest.zip` in the respective platform folder under releases in order for the Launcher to correctly retrieve the standalone)
6. Run server with `yarn dev:start` (protip: just use `yarn start` if you want to start without compiling tsc)

### Game Server

1. Clone `https://github.com/SoftwareGuy/ENet-CSharp`
2. Go into the `Source/Managed` folder and run `dotnet build`
3. Grab the `ENet-CSharp.dll` from `Source\Managed\bin\Debug\netcoreapp3.1` and add it to your .vscode references in the game server directory
4. Go to the game server directory and run the server with `dotnet run`

## Workflow

Fork the repository and create a pull request when you want to merge a change.

### Working on Active Issues

1. Remember to pull from master often to avoid merge conflicts
2. If you have contributor permissions, assign yourself to the issue you are working on

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

- [Electron](https://www.electronjs.org/docs)

### Unity Client

- [Unity Scripting Reference](https://docs.unity3d.com/ScriptReference/)

### ENet Server

- [ENet-CSharp](https://github.com/nxrighthere/ENet-CSharp)
- [gui.cs](https://github.com/migueldeicaza/gui.cs)

### Web Server

- [MySQL](https://dev.mysql.com/downloads/installer/)
- [JSON Web Tokens](https://jwt.io)
- [promise-mysql](https://www.npmjs.com/package/promise-mysql)
- [bcrypt](https://www.npmjs.com/package/bcrypt)
- [Express](https://www.npmjs.com/package/express)
