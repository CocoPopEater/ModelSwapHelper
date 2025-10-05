# Jump Space Model Swap Mod Guide

This is an abstraction based on this asset swap template: https://github.com/SamGarratt17/ModelSwapTemplate-JumpSpace/tree/master

```
[assembly: MelonAdditionalDependencies("ModelSwapHelper")]

namespace YourMod
{
    public class Core : MelonMod
    {
        public override void OnInitializeMelon()
        {
            SwapDetailsCode code = ModelSwapHelper.Helper.GetInstance().AddSwapDetails(new SwapDetails
            {
                ModName = "YourModName",
                BundleName = "BundleName",
                MeshAddress = "Path/To/Your/Model.fbx",
                TextureAddress = "Path/To/Your/Texture.png",
                ObjectNames = new List<string>
                {
                    "BuddyBot",
                    "PF_BuddyBot"
                },
                DeactivateObjectNames = new List<string>
                {
                    "BuddyBotThruster"
                }
            });
            MelonLogger.Msg($"YourMod Initialized with code {code.ToString()}");
        }
    }
}
```
The above code will request that ModelSwapHelper swaps the model for BuddyBot and PF_BuddyBot, while deactivating the BuddyBotThruster GameObject.

Required parameters for SwapDetails are:
- string ModName
- string BundleName
- string MeshAddress
- string TextureAddress
- List&lt;string&gt; ObjectNames

Optional Parameters for SwapDetails are:
- string NormalAddress
- List&lt;string&gt; DeactivateObjects

SwapDetailsCode's are:
- SUCCESS - Your request succeeded with no issues
- MISSING_MOD_NAME - You didn't provide your mod name
- MISSING_BUNDLE_NAME - You didn't provide your bundle name
- MISSING_MESH_ADDRESS - You didn't provide the address to your model within the bundle
- MISSING_TEXTURE_ADDRESS - You didn't provide the address to your model within the bundle
- MISSING_OBJECT_NAME - The ObjectNames list provided was either null or empty
- SWAP_DETAILS_NULL - The object provided to "AddSwapDetails()" was null
- DUPLICATE_SWAP_DETAILS - You tried to register the same object multiple times

In all of the above responses (except SUCCESS and DUPLICATE_SWAP_DETAILS), ModelSwapHelper will not swap assets
