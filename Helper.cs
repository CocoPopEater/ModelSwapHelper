using System.Collections;
using Il2CppSystem.Text;
using MelonLoader;
using UnityEngine;

namespace ModelSwapHelper;

public class Helper
{
    
    private static Helper instance;

    private Helper()
    {
        instance = this;
    }

    public static Helper GetInstance()
    {
        if (instance == null)
        {
            instance = new Helper();
        }
        return instance;
    }
    
    private List<SwapDetails> SwapDetailsList = new List<SwapDetails>();

    public SwapDetailsCode AddSwapDetails(SwapDetails swapDetails)
    {
        if (swapDetails == null) return SwapDetailsCode.SWAP_DETAILS_NULL;
        
        SwapDetailsCode code = swapDetails.Validate();
        if (code != SwapDetailsCode.SUCCESS) return code;
        
        if (SwapDetailsList.Contains(swapDetails))
        {
            return SwapDetailsCode.DUPLICATE_SWAP_DETAILS;
        }
        SwapDetailsList.Add(swapDetails);

        return code;
    }

    public void RemoveSwapDetails(SwapDetails swapDetails)
    {
        SwapDetailsList.Remove(swapDetails);
    }

    public void StartActivate()
    {
        /*MelonLogger.Msg("StartActivate called, list of all models to be swapped:");
        StringBuilder sb = new StringBuilder();
        foreach (SwapDetails swapDetails in SwapDetailsList)
        {
            foreach (string objectName in swapDetails.ObjectNames)
            {
                sb.AppendLine($"{swapDetails.ModName} : {objectName}");
            }
        }
        MelonLogger.Msg(sb.ToString());*/
        MelonCoroutines.Start(DelayedActivate());
    }
    
    private IEnumerator DelayedActivate()
    {
        yield return new WaitForSeconds(3f);
        foreach (SwapDetails swapDetails in SwapDetailsList)
        {
            MeshSwap.SwapMesh(swapDetails);
        }
    }
}