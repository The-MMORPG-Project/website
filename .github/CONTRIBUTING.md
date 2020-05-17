<h1>Contributing Guide</h1>

<h2>Table of Contents</h2>

1. [Introduction](#introduction)
2. [Setup](#setup)
3. [VSC](#visual-studio-code)
4. [Workflow](#workflow)
5. [Notes](#notes)
6. [Documentation](#documentation)

<h1 align="center">Introduction</h1>

The project is intended to be a boilerplate for all Unity MMORPG projects. The client authenticates through the web server then connects to the game server with all the other players.

<h1 align="center">Setup</h1>
<h2 align="center">Client</h2>

Use [Unity Hub](https://unity3d.com/get-unity/download) on version `2019.3.13f1` or later to launch the project.

**IMPORTANT** Do not use Unity 2020 versions or you will run into many issues!

<h2 align="center">ENet Server</h2>

Setup the database (Note that Entity Framework will eventually be scrapped, talk to valk#3277 for more info)
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

<h2 align="center">Web Server</h2>

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

<h1 align="center">Visual Studio Code</h1>

I will recommend that you use Visual Studio Code as it is by far one of the best text editors for development that I've ever used, you will have no regrets when using it.

<h2 align="center">VSC Installation</h2>

1. Download and install [Visual Studio Code](https://code.visualstudio.com)
2. Follow all steps in [Unity Development with VS Code](https://code.visualstudio.com/docs/other/unity)

<h2 align="center">VSC Extensions</h2>

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

<h2 align="center">VSC Known Issues</h2>
<h3>VSC Intellisense does not work for Unity Namespaces</h3>

Solution: Downgrade to `C# 1.21.16`

<h1 align="center">Workflow</h1>

Fork the repository and create a pull request when you want to merge a change.

<h2 align="center">Opening an Issue</h2>

1. Gather as much information as you can about the topic
2. Read the General Guidelines ([#1](https://github.com/valkyrienyanko/ENet-Model/issues/1))
3. Open an issue with a predefined template where possible
4. Provide as much context as possible in your issue!

<h2 align="center">Creating a Pull Request</h2>

1. Talk to valk#3277 on Discord before you create a PR.
2. Always test the PR to see if it works as intended with no additional bugs you may be adding!
3. State all the changes you made in the PR, not everyone will understand what you've done!

<h1 align="center">Notes</h1>
<h2 align="center">Unity Good Practices</h2>

- Try to avoid putting assets in the resources folder as much as possible as it increases game startup time and ram usage. (Eventually we will not use the Resources folder at all)
- Try to make use of Unity's prefabs as much as possible, I found that I ran into countless production issues by trying to code everything from scratch

<h2 align="center">CSharp Formatting</h2>

- Private variables should be camelCase.
- If a public variable has around 2 characters, keep everything UPPERCASE (e.g. "ID")
- Public variables should follow the PascalFormat
- All methods should follow PascalFormat
- Add informative comments to clear up obscure code

<h1 align="center">Documentation</h1>
<h2>Unity</h2>

- [Unity Scripting Reference](https://docs.unity3d.com/ScriptReference/)

<h2>ENet</h2>

- [ENet-CSharp](https://github.com/nxrighthere/ENet-CSharp)

<h2>Server-Side</h2>

- [Entity Framework](https://docs.microsoft.com/en-us/ef/) (will eventually be scrapped and replaced with MySQL from web server)

<h2>Web Node Modules</h2>

- [JSON Web Tokens](https://jwt.io)
- [promise-mysql](https://www.npmjs.com/package/promise-mysql)
- [bcrypt](https://www.npmjs.com/package/bcrypt)
- [Express](https://www.npmjs.com/package/express)
