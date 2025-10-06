using System.Collections;
using MelonLoader;
using UnityEngine;

namespace ModelSwapHelper.Swapper;

public class SwapperManager
{
    private static SwapperManager instance;

    private SwapperManager()
    {
        instance = this;
    }

    public static SwapperManager GetInstance()
    {
        if (instance == null)
        {
            instance = new SwapperManager();
        }
        return instance;
    }
    
    private Dictionary<Guid, Swapper> Swappers = new Dictionary<Guid, Swapper>();

    public Guid AddSwapper(Swapper swapper)
    {
        if (!swapper.Validate()) return Guid.Empty;
        Guid id = Guid.NewGuid();
        Swappers.Add(id, swapper);
        return id;
    }

    public Swapper GetSwapper(Guid id)
    {
        return Swappers[id];
    }

    public void RemoveSwapper(Guid id)
    {
        Swappers.Remove(id);
    }
    
    internal void StartActivate()
    {
        MelonCoroutines.Start(DelayedActivate());
    }
    
    private IEnumerator DelayedActivate()
    {
        yield return new WaitForSeconds(3f);
        foreach (Swapper swapper in Swappers.Values)
        {
            swapper.RunAllModules();
        }
    }
}