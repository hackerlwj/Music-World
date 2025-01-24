Shader "Custom/RemoveWhiteBackground" {
    Properties {
        _MainTex ("Base (RGB)", 2D) = "white" {}
        _Threshold ("Threshold", Range(0.0,1.0)) = 0.95
    }
    SubShader {
        Tags { "RenderType"="Transparent" "Queue"="Transparent"}
        LOD 200
        Lighting Off
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha
        AlphaTest Greater 0.5
        Cull Off

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
                float4 pos : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _Threshold;

            v2f vert (appdata v) {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target {
                fixed4 col = tex2D(_MainTex, i.uv);
                // 如果颜色接近白色，则设置为透明
                if (col.r > _Threshold && col.g > _Threshold && col.b > _Threshold) {
                    col.a = 0.0;
                }
                return col;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}