using System.Collections;
using System.Text;
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
        // Need to find SkinnedMeshRenderers
        
        var renderers = obj.GetComponentsInChildren<SkinnedMeshRenderer>(true)
            .Where(smr => ObjectActions.ContainsKey(smr.gameObject.name))
            .ToList();
        
        if (renderers == null || renderers.Count == 0)
        {
            // this object has no SkinnedMeshRenderers
            SkipCache.Add(obj.name);
            yield break;
        }
        
        // the list of SkinnedMeshRenderers is not null or empty
        foreach (var renderer in renderers)
        {
            if(renderer == null) continue;

            if (!ObjectActions.TryGetValue(renderer.gameObject.name, out List<Swapper> actions))
            {
                // no swapper list associated with this smr name
                continue;
            }

            if (actions == null || actions.Count == 0)
            {
                continue;
            }

            yield return new WaitForSeconds(0.01f);

            foreach (Swapper swapper in actions)
            {
                swapper.RunAllModules();
            }
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
    /// Will clear the SkipCache and cause a re-evaluation of all newly instantiated objects.
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
}