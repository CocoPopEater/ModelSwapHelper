using MelonLoader.Utils;
using UnityEngine;

namespace ModelSwapLib.Managers;

public class BundleManager
{
    private static BundleManager _instance;

    private BundleManager()
    {
        _instance = this;
    }

    public static BundleManager GetInstance()
    {
        if (_instance == null)
        {
            _instance = new BundleManager();
        }
        return _instance;
    }
    
    private readonly Dictionary<string, Il2CppAssetBundle> _bundles = new();

    internal void InitializeBundles()
    {
        _bundles.Clear();
        string rootDir = MelonEnvironment.ModsDirectory;
        FindBundlesRecursive(rootDir);
    }

    private void FindBundlesRecursive(string rootDir)
    {
        foreach (string file in Directory.GetFiles(rootDir))
        {
            if (file.EndsWith(".bundle"))
            {
                string fileName = Path.GetFileName(file);
                ConsoleUtils.Msg($"Found Bundle: {fileName}");
                _bundles.Add(fileName, Il2CppAssetBundleManager.LoadFromFile(file));
            }
        }
        
        foreach (var folder in Directory.GetDirectories(rootDir))
        {
            var dirInfo = new DirectoryInfo(folder);
            if (dirInfo.LinkTarget != null)
            {
                // Symlink and can cause SOF
                // Should ignore for safety
                continue;
            }
            FindBundlesRecursive(folder);
        }
    }
    
    
    
    
    internal Il2CppAssetBundle GetBundle(Swapper.Swapper swapper)
    {
        // Verifying bundle name format
        string validBundleName;
        if (swapper.BundleName.EndsWith(".bundle"))
        {
            validBundleName = swapper.BundleName;
        }
        else
        {
            validBundleName = string.Concat(swapper.BundleName, ".bundle");
        }
        
        _bundles.TryGetValue(validBundleName, out var bundle);
        if (bundle != null)
        {
            // Bundle was loaded previously
            return bundle;
        }

        return null;
    }

    internal void Shutdown()
    {
        foreach (var bundle in _bundles.Values)
        {
            try
            {
                bundle?.Unload(true);
            }
            catch (Exception ignored) { }

        }
        _bundles.Clear();
    }
}