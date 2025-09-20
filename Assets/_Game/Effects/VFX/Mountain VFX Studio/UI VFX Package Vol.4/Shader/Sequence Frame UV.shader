Shader "UI VFX Package/Sequence Frame UV"
{
    Properties
    {
        _Color("Color Tint",color) = (1,1,1,1)
        _MainTex("Texture", 2D) = "white" {}
        _CutX("CutX Amount",float) = 4
        _CutY("CutY Amount",float) = 4
        _Speed("Speed",range(1,100)) = 30
    }
        SubShader
        {
            Tags { "RenderType" = "transparent" "queue" = "transparent" "ignoreprojector" = "true" }
            ZWrite off
            blend srcalpha oneminussrcalpha

            Pass
            {
                CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag
                #pragma multi_compile_fog

                #include "UnityCG.cginc"

                struct appdata
                {
                    float4 vertex : POSITION;
                    float2 uv : TEXCOORD0;
                };

                struct v2f
                {
                    float2 uv : TEXCOORD0;
                    UNITY_FOG_COORDS(1)
                    float4 vertex : SV_POSITION;
                };

                sampler2D _MainTex;
                float4 _MainTex_ST;
                fixed4 _Color;
                float _CutX;
                float _CutY;
                float _Speed;

                v2f vert(appdata v)
                {
                    v2f o;
                    o.vertex = UnityObjectToClipPos(v.vertex);
                    o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                    UNITY_TRANSFER_FOG(o,o.vertex);
                    return o;
                }

                fixed4 frag(v2f i) : SV_Target
                {
                    float time = floor(_Time.y * _Speed);
                float row = floor(time / _CutX);
                float column = time - row * _CutX;
                half2 uv = i.uv + half2(column,-row);
                uv.x /= _CutX;
                uv.y /= _CutY;

                fixed4 col = tex2D(_MainTex,uv);
                col.rgb *= _Color;
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
        }
}