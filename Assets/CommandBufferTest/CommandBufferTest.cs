using UnityEngine;
using UnityEngine.Rendering;
using UnityStandardAssets.CinematicEffects;

// https://forum.unity.com/threads/unity-5-6-0f3-empty-stencil-buffer-onrenderimage.473444/
public class CommandBufferTest : MonoBehaviour
{
    private CommandBuffer commandBuffer;

    private Texture2D m_Texture;

    private Texture2D texture
    {
        get
        {
            if (m_Texture == null)
            {
                m_Texture = new Texture2D(16, 16);
            }

            return m_Texture;
        }
    }

    private Camera m_Camera;

    public Camera camera
    {
        get
        {
            if (m_Camera == null)
            {
                m_Camera = GetComponent<Camera>();
            }

            return m_Camera;
        }
    }

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

    private RenderTexture maskRenderTexture;

    public RenderTexture MaskRenderTexture
    {
        get
        {
            if (!maskRenderTexture)
            {
                RenderTextureFormat format = RenderTextureFormat.Default;
                if (SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.ARGBHalf))
                {
                    format = RenderTextureFormat.ARGBHalf;
                }

                maskRenderTexture = new RenderTexture(Screen.width, Screen.height, 0, format);
                maskRenderTexture.Create(); //Ensure these two RT have the same size
            }

            return maskRenderTexture;
        }
    }

    private void Start()
    {
        if (commandBuffer == null)
        {
            commandBuffer = new CommandBuffer();
            commandBuffer.name = "commandBuffer";

            int stencilTextureID = Shader.PropertyToID("_StencilTexture");
            commandBuffer.GetTemporaryRT(stencilTextureID, -1, -1, 24);

            commandBuffer.Blit(BuiltinRenderTextureType.None, stencilTextureID, MaskMaterial);

            commandBuffer.SetGlobalTexture("_StencilTexture", stencilTextureID);
            camera.AddCommandBuffer(CameraEvent.AfterForwardAlpha, commandBuffer);
        }
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        RenderTexture stencilRT = Shader.GetGlobalTexture("_StencilTexture") as RenderTexture;
        RenderTexture activeRT = RenderTexture.active;
        RenderTexture.active = stencilRT;

        texture.ReadPixels(new Rect(Screen.width / 2, Screen.height / 2, texture.width, texture.height), 0, 0, false);
        //Debug.Log (texture.GetPixel(0, 0));

        //Graphics.Blit(stencilRT, MaskRenderTexture, MaskMaterial);

        RenderTexture.active = activeRT;

        Graphics.Blit(source, destination);
    }
}
