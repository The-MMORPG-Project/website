# Contributing Guide

## Setting up Visual Studio Code
Visual Studio Code is by far one of the best text editors I've ever used, you will have no regrets when using it.

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

# Issues and Pull Requests
## Opening an Issue
1. Gather as much information as you can about the topic
2. Read the General Guidelines ([#1](https://github.com/valkyrienyanko/ENet-Model/issues/1))
3. Open an issue with a predefined template where possible
4. Provide as much context as possible in your issue!

## Creating a Pull Request
1. Talk to a project manager to get your PR approved in advance.
2. Always test the PR to see if it works as intended with no additional bugs you may be adding!
3. State all the changes you made in the PR, not everyone will understand what you've done!

# Notes
## Unity Good Practices
- Try to avoid putting assets in the resources folder as much as possible as it increases game startup time and ram usage.
- When comparing gameObject tags use `.CompareTag` instead of `==` to avoid garbage collection.
- Objects at the very bottom of the Unity Game Window Hierarchy will be layered over objects above. (e.g. If a panel is above a button, the panel will block the raycast to the button)
- Avoid searching for inactive gameObjects, instead create a public variable in the inspector for the gameObject.

## C# Style & Guidelines
- Private variables should be camelCase.
- If a public variable has around 2 characters, keep everything UPPERCASE
- Public variables should follow the PascalFormat
- Methods should follow PascalFormat
- Add informative comments to clear up obscure code

## Sprites
[Gimp](https://www.gimp.org) is open-source free software great for creating Sprites. Make sure when finished creating a Sprite, export both the `.xcf` and image file into the Unity assets folder.

## Animations
### Mixamo Humanoids
1. Keep the format .FBX for Unity at 30 Frames with no keyframe reduction and check off "In Place" if applicable. 
2. Import the .FBX into Blender, then delete the imported armature. 
3. Apply the new animation to the original armature. Make sure your applying the animation to the "Armature" and not the "Cube". 
4. Export the Blender file to .FBX, import into Unity, set the scale factor to 0.1, set the animation type to Humanoid, go into configure, sample Bind-Pose, then enforce T-Pose, and finally enable Loop Time for all animations.

## Documentation
- [Unity Scripting Reference](https://docs.unity3d.com/ScriptReference/)
- [ENet-CSharp](https://github.com/nxrighthere/ENet-CSharp)
- [C#](https://docs.microsoft.com/en-us/dotnet/csharp/)
