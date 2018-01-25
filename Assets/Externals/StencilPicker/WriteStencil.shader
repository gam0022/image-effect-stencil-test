Shader "Stencil/WriteStencil" {
    Properties{
        _StencilRef("Stencil Reference", Int) = 1
    }
    SubShader{
        Tags{ "RenderType" = "Opaque" "Queue" = "Geometry" }

        Stencil{
            Ref [_StencilRef]
            Comp Always
            Pass Replace
        }

        Pass{

            CGPROGRAM

            #pragma target 3.0
            #pragma vertex vert
            #pragma fragment frag



            struct appdata {
                float4 vertex : POSITION;
            };

            struct v2f {
                float4 pos : SV_POSITION;
            };

            v2f vert(appdata v) {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                return o;
            }

            half4 frag(v2f i) : SV_Target{
                return half4(1,0,0,1);
            }
            ENDCG
        }
    }
}