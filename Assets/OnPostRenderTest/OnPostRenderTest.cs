using UnityEngine;
using UnityStandardAssets.CinematicEffects;

public class OnPostRenderTest : MonoBehaviour
{
    private Camera camera;

    private Camera Camera
    {
        get
        {
            if (!camera)
            {
                camera = GetComponent<Camera>();
            }

            return camera;
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

    private Material blitMaterial;

    private Material BlitMaterial
    {
        get
        {
            if (!blitMaterial)
            {
                var blitShader = Shader.Find("Hidden/Blit");
                maskMaterial = ImageEffectHelper.CheckShaderAndCreateMaterial(blitShader);
            }

            return maskMaterial;
        }
    }

    private RenderTextureFormat Format
    {
        get
        {
            RenderTextureFormat format = RenderTextureFormat.Default;
            if (SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.ARGBHalf))
            {
                format = RenderTextureFormat.ARGBHalf;
            }

            return format;
        }
    }

    private RenderTexture targetRenderTexture;

    private RenderTexture TargetRenderTexture
    {
        get
        {
            if (!targetRenderTexture)
            {
                targetRenderTexture = new RenderTexture(Screen.width, Screen.height, 24, Format);
                targetRenderTexture.Create();
            }

            return targetRenderTexture;
        }
    }

    private RenderTexture maskRenderTexture;

    public RenderTexture MaskRenderTexture
    {
        get
        {
            if (!maskRenderTexture)
            {
                maskRenderTexture = new RenderTexture(Screen.width, Screen.height, 0, Format);
                maskRenderTexture.Create(); //Ensure these two RT have the same size
            }

            return maskRenderTexture;
        }
    }

    // Use this for initialization
    void Start()
    {
        // RenderTexture に対して、オフスクリーンにシーンを描画する
        // Start() ではなく OnPostRender() の先頭に書くと画面に何も出力されなくなる
        Camera.targetTexture = TargetRenderTexture;
    }

    // http://qiankanglai.me/2015/03/07/unity-posteffect-stencil/index.html
    // OnPostRender で描画対象を RenderTexture にすれば、Stencil が参照できる
    public void OnPostRender()
    {
        // MaskRenderTexture を背景色で初期化する
        Graphics.SetRenderTarget(MaskRenderTexture);
        GL.Clear(true, true, new Color(0, 1, 0, 0));

        // MaskRenderTexture に対して、Stencil を用いたマスク画像を描画するシェーダ MaskMaterial で描画する
        // TargetRenderTexture.depthBuffer を指定する点がポイント
        Graphics.SetRenderTarget(MaskRenderTexture.colorBuffer, TargetRenderTexture.depthBuffer);
        Graphics.Blit(TargetRenderTexture, MaskMaterial, 0);

        // オフスクリーンレンダリングした結果を画面にコピーする
        Camera.targetTexture = null;
        Graphics.Blit(TargetRenderTexture, (RenderTexture) null);

        // Materialを指定した場合、maskRenderTexture がフレームデバッガーで参照されなくなる
        //Graphics.Blit(RenderTexture, (RenderTexture)null, BlitMaterial);
    }

//    // OnRenderImage では Stencil の情報が失われる
//    [ImageEffectOpaque]
//    void OnRenderImage(RenderTexture src, RenderTexture dest)
//    {
//        MaskMaterial.SetPass(0);
//
//        MaskRenderTexture.DiscardContents();
//        Graphics.SetRenderTarget(MaskRenderTexture);
//        GL.Clear(true, true, new Color(0,0,0,0));	// clear the full RT
//
//        // *KEY POINT*: Draw with the camera's depth buffer
//        Graphics.SetRenderTarget(MaskRenderTexture.colorBuffer, src.depthBuffer);
//        Graphics.Blit(src, MaskMaterial, 0);
//
//        Graphics.Blit (src, dest);
//    }
}
