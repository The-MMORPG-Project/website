# Contributing Guide

## Setting up Visual Studio Code
I will recommend that you use Visual Studio Code as it is by far one of the best text editors for development that I've ever used, you will have no regrets when using it.

### Installing VSC
1. Download and install [Visual Studio Code](https://code.visualstudio.com)
2. Follow all steps in [Unity Development with VS Code](https://code.visualstudio.com/docs/other/unity)

### Extensions
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

### Known Issues
#### VSC Intellisense does not work for Unity Namespaces
Solution: Downgrade to `C# 1.21.16`

# Workflow
## Opening an Issue
1. Gather as much information as you can about the topic
2. Read the General Guidelines ([#1](https://github.com/valkyrienyanko/ENet-Model/issues/1))
3. Open an issue with a predefined template where possible
4. Provide as much context as possible in your issue!

## Creating a Pull Request
1. Talk to valk#3277 on Discord before you create a PR.
2. Always test the PR to see if it works as intended with no additional bugs you may be adding!
3. State all the changes you made in the PR, not everyone will understand what you've done!

# Notes
## Unity Good Practices
- Try to avoid putting assets in the resources folder as much as possible as it increases game startup time and ram usage. (Eventually we will not use the Resources folder at all)
- Try to make use of Unity's prefabs as much as possible, I found that I ran into countless production issues by trying to code everything from scratch

## C# Style & Guidelines
- Private variables should be camelCase.
- If a public variable has around 2 characters, keep everything UPPERCASE (e.g. "ID")
- Public variables should follow the PascalFormat
- All methods should follow PascalFormat
- Add informative comments to clear up obscure code

# Documentation
## Unity
- [Unity Scripting Reference](https://docs.unity3d.com/ScriptReference/)

## ENet
- [ENet-CSharp](https://github.com/nxrighthere/ENet-CSharp)

## Server-Side
- [Entity Framework](https://docs.microsoft.com/en-us/ef/) (Will eventually be scrapped and replaced with MySQL from web server)

## Web Node Modules
- [JSON Web Tokens](https://jwt.io)
- [promise-mysql](https://www.npmjs.com/package/promise-mysql)
- [bcrypt](https://www.npmjs.com/package/bcrypt)
- [Express](https://www.npmjs.com/package/express)
