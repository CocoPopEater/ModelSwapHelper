using MelonLoader.Utils;
using UnityEngine;

namespace ModelSwapLib.Swapper;

public class BundleManager
{
    private static BundleManager instance;

    private BundleManager()
    {
        instance = this;
    }

    public static BundleManager GetInstance()
    {
        if (instance == null)
        {
            instance = new BundleManager();
        }
        return instance;
    }
    
    private Dictionary<string, AssetBundle> bundles = new();

    internal AssetBundle GetBundle(string bundleName)
    {
        /*
         * Info:
         * In this class I want to store the keys as  a full file name, e.g "Banana.bundle"
         * 
         * NOT "Banana"
         * NOT "C://Whatever/The/Path/Is.bundle"
         *
         * returns:
         * An AssetBundle object, if it could be found/loaded
         * null, if the bundle could not be found and the load failed
         * 
         */
        
        // Verifying bundle name format
        string validBundleName;
        if (bundleName.EndsWith(".bundle"))
        {
            validBundleName = bundleName;
        }
        else
        {
            validBundleName = string.Concat(bundleName, ".bundle");
        }
        
        bundles.TryGetValue(validBundleName, out var bundle);
        if (bundle != null)
        {
            return bundle;
        }
        // If we get here, bundle has not been loaded yet and should be registered with the manager
        
        string fullPath = Path.Combine(MelonEnvironment.ModsDirectory, validBundleName);
        if (!File.Exists(fullPath))
        {
            return null;
        }
        bundle = AssetBundle.LoadFromFile(fullPath);
        if (bundle == null)
        {
            return null;
        }
        bundles.Add(validBundleName, bundle);
        return bundle;
    }

    internal void Shutdown()
    {
        foreach (var Bundle in bundles.Values)
        {
            Bundle?.Unload(true);
        }
        bundles.Clear();
    }
}