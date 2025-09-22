# Jump Space Model Swap Mod Guide

This is primarily designed for importing models and textures, but may also work for other assets.

Part 1: Creating an assetbundle
1. Create a new unity environment in version 2022.3.47f1
2. Install the "Adressables" Package from unity's package manager
3. Import your asset of choice into unity as an fbx for models, and png for textures
4. Assign the asset same name as the asset used by Jump Space (This can be checked by opening "steamapps\common\JumpSpace\Jump Space_Data\StreamingAssets\aa\StandaloneWindows64\ag_managed_default_assets_all.bundle" in [UABEANext](https://github.com/nesrak1/UABEANext?tab=readme-ov-file))
5. Using the inspector in unity, select your asset, if its a model you must enable Read/Write and set the index format to 16 bits
6. Again in the inspector, enable the Addresable check box on your assets (located top of the inspector), and assign them to a bundle (located bottom of the inspector)
7. Build your unity project and locate your .bundle, copy this to your Jump Space Mods folder

Part 2: Setting up the mod
1. Install the Template and open the .csproject in an IDE of your choice
2. Swap out the following details:
   
  "ModName" - Your mod's name

  "version" - The Version of your mod
  
  "Author" - Your name
  
  "Mod Inititialized." - I recommend you add your mods name here
  
  "BundleName.bundle" - The name of your custom assetbundle that has been copied to your Mods folder
  
  "Assets/Mesh.fbx" - The path of your mesh within the bundle, this can be found beside the Adressable checkbox in unity
  
  "Assets/Texture.png" - The path of your texture within the bundle, this can be found beside the Adressable checkbox in unity
  
  "ObjectName" - The name of the GameObject which contains the SkinnedMeshRenderer (This can be found with the [Unity Explorer Mod](https://github.com/sinai-dev/UnityExplorer) - it must contain a skinnedmeshrenderer)

Optional:
Within the template i have include many commented out debugging loggers, if your mod isnt working i would reccomend you re-enable these to locate the issue.
For more advanced textures, you may also want to include other maps, this can be done by reenabling a commented out portion, and replace ("_T2", null) with ("_T2", variableName) where variableName is another loaded texture (see line 86)

Common Issues:
Incorrect model rotation - The model has likely been exported with differant transform settings to Jump Space. Import the model into blender, and export it as an fbx setting forward to -Y and up to Z making sure to enable the "Apply Transforms" checkbox.

For any other issues, feel free to dm me on Discord @GraciousCub5622
