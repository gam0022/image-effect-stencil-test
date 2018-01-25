using UnityEngine;
using UnityStandardAssets.CinematicEffects;

public class ImageEffectOpaqueTest : MonoBehaviour
{
   private Material maskMaterial;

    private Material MaskMaterial
    {
        get
        {
            if (!maskMaterial)
            {
                var maskShader = Shader.Find("Hidden/Mask");
                maskMaterial = ImageEffectHelper.CheckShaderAndCreateMaterial(maskShader);
            }

            return maskMaterial;
        }
    }

    // Stencilテストを常にPassしてしまい、マスク画像を得られない
    [ImageEffectOpaque]
    void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        Graphics.Blit(src, dest, MaskMaterial);
    }
}
