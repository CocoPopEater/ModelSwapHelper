# Model Swap Lib

Model Swap Lib is an abstraction based on [this Asset Swap Template](https://github.com/SamGarratt17/ModelSwapTemplate-JumpSpace/tree/master) for the game *Jump Space*.

This library is intended for use by mod developers and will have no effect if installed alone.

To use this as a developer, add `ModelSwapLib.dll` as a project reference:
```csharp
[assembly: MelonAdditionalDependencies("ModelSwapLib")]

namespace YourMod
{
    public class Core : MelonMod
    {
        public override void OnInitializeMelon()
        {
            Swapper swapper = new Swapper
            {
                ModName = "YourModName",
                SwapperName = "Any Name You Want",
                ObjectNames = new List<string>([
                    "ObjectName1",
                    "ObjectName2"
                ]),
                BundleName = "YourBundle.bundle",
                Modules = new List<IModule>
                {
                    new MeshModule("Path/To/Your/Model.fbx"),
                    new Texture2DModule("Path/To/Your/Texture.png")
                },
                Deactivations = new List<string>([
                    "SomeObjectNameYouWantDeactivated"
                    ])
            };
            Guid guid = ObjectActionManager.GetInstance().RegisterSwapper(swapper);

            if(guid == Guid.Empty) // If you receive a Guid.Empty then the Swapper.Validate() failed
            {
                MelonLogger.Warning($"Failed to register swapper: {swapper.SwapperName}");
            } else
            {
                MelonLogger.Msg($"Successfully registered swapper: {swapper.SwapperName} with guid: {swapper.SwapperGuid}");
                ObjectActionManager.GetInstance().ClearSkipCache(swapper); // Call this after registering each swapper/batch of swappers as it
                                                                          // ensures the SkipCache doesnt contain objects you want swapped
            }
            
            MelonLogger.Msg($"YourMod Initialized");
        }
    }
}
```

# Required Properties:
- string ModName
  - This ensures that if there are issues, then logging can point out which mod is failing
- string BundleName
  - The name of the bundle you would like this Swapper to operate with. The swapper will automatically validate the format of this to ensure it ends with ".bundle", and if it doesn't then it will implicitly concatenate ".bundle". 
- List&lt;string&gt; ObjectNames
  - A list of object names that this swapper should swap.
- List&lt;IModule&gt; Modules
  - A list of module implementations that will run one after the other to affect each Object. As shown in the code example above you may instantiate a module using either a parameter or a parameterless constructor.

If any of the above properties are null or empty, Validate() will return false and the swapper will not be registered.

# Optional Parameters:
- List&lt;string&gt; Deactivations
  - A list of object names that you would like to deactivate.


Many thanks to [GraciousCub5622](https://github.com/SamGarratt17) @graciouscub5622 on discord for his Asset Swap Template, upon which this entire project is based.<br>
Many thanks to [ScribbleTAS](https://github.com/ScribbleTAS) for MeshUtils and also for his help in debugging and brainstorming.