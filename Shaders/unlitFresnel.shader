Shader "MyShaders/unlitFresnel"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color ("Color", Color) = (1, 1, 1, 1)
        _FresnelColor ("Fresnel Color", Color) = (1, 1, 1, 1)
        _FresnelCutoff ("Fresnel Cutoff", Float) = 1
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" "RenderPipeline" = "UniversalPipeline" }
        LOD 300
        Cull Back

        Pass
        {
            Tags {"LightMode" = "UniversalForward"}

            HLSLPROGRAM
            
            #pragma target 5.0
            #pragma vertex vert
            #pragma fragment frag

            struct vertData
            {
                float4 positionOS : POSITION;
                float3 normalOS : NORMAL;
                float4 color : COLOR0;
                float2 uv : TEXCOORD0;
            };

            struct fragData
            {
                float4 positionCS : SV_POSITION;
                float4 positionWS : TEXCOORD1;
                float3 normalWS : TEXCOORD2;
            };

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            CBUFFER_START(UnityPerMaterial)
            Texture2D _MainTex;
            float4 _MainTex_ST;
            SamplerState sampler_MainTex;
            float4 _Color;
            float4 _FresnelColor;
            float _FresnelCutoff;
            CBUFFER_END

            //float map(float value, float low1, float high1, float low2, float high2) { return low2 + (value - low1) * (high2 - low2) / (high1 - low1); }

            fragData vert (vertData v)
            {
                fragData o;
                o.positionWS = float4(TransformObjectToWorld(v.positionOS.xyz), 0);
                o.normalWS = TransformObjectToWorldNormal(v.normalOS);

                o.positionCS = TransformWorldToHClip(o.positionWS.xyz);
                return o;
            }

            float4 frag (fragData i) : SV_Target
            {
                float edge = dot(i.normalWS, GetWorldSpaceNormalizeViewDir(i.positionWS));
                float4 col = lerp(_FresnelColor, _Color, edge);
                //if (edge > _FresnelCutoff) col = _Color;
                //else col = _FresnelColor;
                return col;
            }

            ENDHLSL
        }
    }
}
