using System.Collections;
using MelonLoader;
using UnityEngine;

namespace ModelSwapLib.Swapper;

public class ObjectActionManager
{
    private static ObjectActionManager instance;

    private ObjectActionManager()
    {
        instance = this;
    }

    public static ObjectActionManager GetInstance()
    {
        if (instance == null)
        {
            instance = new ObjectActionManager();
        }
        return instance;
    }
    
    private Dictionary<string, List<Swapper>> ObjectActions = new Dictionary<string, List<Swapper>>();
    private HashSet<string> SkipCache = new HashSet<string>();
    
    internal IEnumerator HandleObject(GameObject obj)
    {
        if (SkipCache.Contains(obj.name)) yield break;
        // The instantiated object is not listed as skip
        // Need to find SkinnedMeshRenderer

        var skinnedMeshRenderer = obj.GetComponentInChildren<SkinnedMeshRenderer>();
        if(skinnedMeshRenderer == null) yield break;
        if (!ObjectActions.TryGetValue(skinnedMeshRenderer.gameObject.name, out List<Swapper> swappers))
        {
            // the object is not in the swap list
            // we can skip it on subsequent instantiations
            SkipCache.Add(obj.name);
            yield break;
        }
        
        if (swappers.Count == 0)
        {
            // if the swapper count is 0, it might as well be null
            SkipCache.Add(obj.name);
            yield break;
        }
        yield return new WaitForSeconds(0.01f);
        
        foreach (Swapper swapper in swappers)
        {
            swapper.RunAllModules();
        }
    }

    public Guid RegisterSwapper(Swapper swapper)
    {
        if (!swapper.Validate()) return Guid.Empty;

        List<string> names = swapper.ObjectNames;
        foreach (string name in names)
        {
            ObjectActions.TryGetValue(name, out List<Swapper> objectSwappers);
            // List might be null, meaning no swappers were registered for this object
            if (objectSwappers == null)
            {
                objectSwappers = new List<Swapper>();
                objectSwappers.Add(swapper);
                ObjectActions[name] = objectSwappers;
                continue;
            }
            // List not null, might already contain the swapper instance
            // Don't want to add again
            if(objectSwappers.Contains(swapper)) continue;
            
            // List does not contain the swapper instance
            // We can add without duplicating
            objectSwappers.Add(swapper);
        }

        return swapper.SwapperGuid;
    }
    
    /// <summary>
    /// Will clear the SkipCache. Should prefer SyncSkipCache(Swapper swapper)
    /// But if you add a swapper and proceed to lose the object reference before calling SyncSkipCache
    /// This method can be used in lieu
    /// </summary>
    public void ClearSkipCache()
    {
        SkipCache.Clear();
    }

    /// <summary>
    /// Used to Unregister a swapper
    /// </summary>
    /// <remarks>
    /// This will loop through every entry in the Dicionary
    /// It would be preferable to never need to call this method however it exists
    /// In the event that you want to update the event list at runtime
    /// </remarks>
    /// <param name="swapper">The swapper</param>
    internal void UnregisterSwapper(Swapper swapper)
    {
        if(!swapper.Validate()) return;
        foreach (KeyValuePair<string, List<Swapper>> kvp in ObjectActions)
        {
            List<Swapper> eventSwappers = kvp.Value;
            if (eventSwappers.Contains(swapper)) eventSwappers.Remove(swapper);
        }
    }

    /// <summary>
    /// Loops through the Swappers ObjectNames and ensures they are removed from the SkipCache
    /// This should be called whenever you register a new swapper
    /// </summary>
    /// <param name="swapper">The Swapper object to sync with</param>
    public void SyncSkipCache(Swapper swapper)
    {
        foreach (string objectName in swapper.ObjectNames)
        {
            if(SkipCache.Contains(objectName)) SkipCache.Remove(objectName);
        }
    }
}