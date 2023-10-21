Shader "MyShaders/terrainBlend"
{
    Properties
    {
        _SplatMap ("SplatMap", 2D) = "white" {}
        _ColorN ("Color N", Color) = (1, 1, 1, 1)
        _ColorR ("Color R", Color) = (1, 0, 0, 1)
        _ColorG ("Color G", Color) = (0, 1, 0, 1)
        _ColorB ("Color B", Color) = (0, 0, 1, 1)
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

            #pragma vertex vert
            #pragma fragment frag

            struct vertData
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct fragData
            {
                float2 uv : TEXCOORD0;
                float4 positionCS : SV_POSITION;
            };

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            CBUFFER_START(UnityPerMaterial)

            Texture2D _SplatMap;
            float4 _SplatMap_ST;
            SamplerState sampler_SplatMap;

            float4 _ColorN;
            float4 _ColorR;
            float4 _ColorG;
            float4 _ColorB;

            CBUFFER_END

            fragData vert (vertData v)
            {
                fragData o;
                o.positionCS = TransformObjectToHClip(v.positionOS.xyz);
                o.uv = TRANSFORM_TEX(v.uv, _SplatMap);
                return o;
            }

            float4 frag (fragData i) : SV_Target
            {
                float4 splat = SAMPLE_TEXTURE2D(_SplatMap, sampler_SplatMap, i.uv);
                float4 col = lerp(_ColorN, _ColorR, splat.r);
                col = lerp(col, _ColorG, splat.g);
                col = lerp(col, _ColorB, splat.b);
                return col;
            }

            ENDHLSL
        }
    }
}
