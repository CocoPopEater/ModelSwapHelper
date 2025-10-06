# Model Swap Mod Lib

This is an abstraction based on this asset swap template: https://github.com/SamGarratt17/ModelSwapTemplate-JumpSpace/tree/master for the game "Jump Space".

This is intended for use by mod developers and will have no effect if installed alone.

To use this as a developer you will need to add ModelSwapLib.dll as a project reference

```
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
                    new MeshModule
                        {
                            AssetPath = "Path/To/Your/Model.fbx",
                        },
                    new Texture2DModule("Path/To/Your/Texture.png")
                },
                Deactivations = new List<string>([
                    "SomeObjectNameYouWantDeactivated"
                    ])
            };
            Guid guid1 = SwapperManager.GetInstance().AddSwapper(swapper);
            
            MelonLogger.Msg($"YourMod Initialized");
        }
    }
}
```

# Required Properties:
- string ModName
  - This ensures that if there are issues, then logging can point out which mod is failing
- string BundleName
  - The name of the bundle you would like this Swapper to operate with. The swapper will automatically validate the format of this to ensure it ends with ".bundle", and if it doesn't then it will implicitly concat ".bundle". 
- List&lt;string&gt; ObjectNames
  - A list of object names that this swapper should swap.
- List&lt;IModule&gt; Modules
  - A list of module implementations that will run one after the other to affect each Object. As shown in the code example above you may instantiate a module using either a parameter or a parameterless constructor.

If any of the above properties are null or empty, Validate() will fail and the swapper will not function.

# Optional Parameters:
- List&lt;string&gt; Deactivations
  - A list of object names that you would like to deactivate.

Currently, this project only functions with objects that are loaded on scene initialization and does not currently support moving meshes. There are plans to improve this project to rectify both of these shortcomings.

Many thanks to https://github.com/SamGarratt17 @graciouscub5622 on discord for his Asset Swap Template, upon which this entire project is based
