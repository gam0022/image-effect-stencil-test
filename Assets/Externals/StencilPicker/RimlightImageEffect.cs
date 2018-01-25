using UnityEngine;
using UnityStandardAssets.CinematicEffects;

public class RimlightImageEffect : MonoBehaviour
{
    // AO shader
    Shader RimlightShader
    {
        get
        {
            if (rimlightShader == null)
                rimlightShader = Shader.Find("Hidden/Rimlight");
            return rimlightShader;
        }
    }

    [SerializeField]
    Shader rimlightShader;

    // Temporary aterial for the AO shader
    Material RimlightMaterial
    {
        get
        {
            if (rimlightMaterial == null)
                rimlightMaterial = ImageEffectHelper.CheckShaderAndCreateMaterial(RimlightShader);
            return rimlightMaterial;
        }
    }

    Material rimlightMaterial;

    public RenderTexture SilhouetteRenderTexture
    {
        get
        {
            if (silhouetteRenderTexture == null)
                silhouetteRenderTexture = new RenderTexture(1920, 1080, 24);

            return silhouetteRenderTexture;
        }
    }

    RenderTexture silhouetteRenderTexture;

    [ImageEffectOpaque]
    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        var m = RimlightMaterial;
        
        Graphics.SetRenderTarget(SilhouetteRenderTexture.colorBuffer, source.depthBuffer);
        Graphics.Blit(source, SilhouetteRenderTexture, m, 0);
        
        Graphics.Blit(source, destination, m, 0);
        //Graphics.Blit(source, destination);
    }
}