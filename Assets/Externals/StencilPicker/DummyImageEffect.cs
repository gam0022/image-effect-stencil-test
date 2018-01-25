using UnityEngine;
using UnityStandardAssets.CinematicEffects;

public class DummyImageEffect : MonoBehaviour
{
    // AO shader
    Shader aoShader
    {
        get
        {
            if (_aoShader == null)
                _aoShader = Shader.Find("Hidden/Image Effects/Cinematic/AmbientOcclusion");
            return _aoShader;
        }
    }

    [SerializeField]
    Shader _aoShader;

    // Temporary aterial for the AO shader
    Material aoMaterial
    {
        get
        {
            if (_aoMaterial == null)
                _aoMaterial = ImageEffectHelper.CheckShaderAndCreateMaterial(aoShader);
            return _aoMaterial;
        }
    }

    Material _aoMaterial;
    
    public RenderTexture SilhouetteRenderTexture
    {
        get
        {
            if (silhouetteRenderTexture == null)
            {
                silhouetteRenderTexture = GetComponent<RimlightImageEffect>().SilhouetteRenderTexture;
            }

            return silhouetteRenderTexture;
        }
    }
    
    RenderTexture silhouetteRenderTexture;

    [ImageEffectOpaque]
    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        var m = aoMaterial;
        
        //Graphics.Blit(source, SilhouetteRenderTexture, m, 7);
        
        Graphics.Blit(source, destination, m, 7);
        //Graphics.Blit(source, destination);
    }
}