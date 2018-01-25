Shader "ImageEffect/StencilPicker" {
    Properties {
        _MainTex ("Texture", 2D) = "white" {}
        _StencilRef("Stencil Reference", Int) = 1
    }
    SubShader {

        Cull Off
        ZWrite Off
        ZTest Off

        Stencil{
            Ref [_StencilRef]
            Comp Equal
        }

        Pass {

            CGPROGRAM

            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata v) {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            sampler2D _MainTex;

            fixed4 frag (v2f i) : SV_Target {
                fixed4 col = fixed4(frac(i.uv * float2(16, 9)),1,1); // 市松模様
                return col;
            }
            ENDCG
        }
    }
}