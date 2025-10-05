#nullable enable
namespace ModelSwapHelper;

public class SwapDetails
{
    public string ModName { get; init; } = null;
    public string BundleName { get; init; } = null;
    public string MeshAddress { get; init; } = null;
    public string TextureAddress { get; init; } = null;
    public string? NormalAddress { get; init; } = null;
    public List<string> ObjectNames { get; init; } = null;
    public List<string>? DeactivateObjectNames { get; init; } = null;


    public SwapDetailsCode Validate()
    {
        if (string.IsNullOrEmpty(ModName)) return SwapDetailsCode.MISSING_MOD_NAME;
        if (string.IsNullOrEmpty(BundleName)) return SwapDetailsCode.MISSING_BUNDLE_NAME;
        if (string.IsNullOrEmpty(MeshAddress)) return SwapDetailsCode.MISSING_MESH_ADDRESS;
        if (string.IsNullOrEmpty(TextureAddress)) return SwapDetailsCode.MISSING_TEXTURE_ADDRESS;
        if (ObjectNames == null || ObjectNames.Count == 0) return SwapDetailsCode.MISSING_OBJECT_NAME;
        
        return SwapDetailsCode.SUCCESS;
    }
}