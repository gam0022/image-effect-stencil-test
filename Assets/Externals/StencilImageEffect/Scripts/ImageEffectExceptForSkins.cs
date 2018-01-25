using UnityEngine;

[RequireComponent(typeof(Camera))]
[ExecuteInEditMode]
public class ImageEffectExceptForSkins : MonoBehaviour 
{
    [SerializeField] int stencilMask = 1;
    Material material_;

    const string shaderName = "Hidden/ImageEffectExceptForSkins";

    [ImageEffectOpaque]
    void OnRenderImage(RenderTexture src, RenderTexture dst)
    {
        if (material_ == null) {
            var shader = Shader.Find(shaderName);
            material_ = new Material(shader);
        }
        material_.SetInt("_StencilMask", stencilMask);
        Graphics.Blit(src, dst, material_, 0);
    }
}
