Shader "MyShaders/HLSL Template"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
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
            Texture2D _MainTex;
            float4 _MainTex_ST;
            SamplerState sampler_MainTex;
            CBUFFER_END

            fragData vert (vertData v)
            {
                fragData o;
                o.positionCS = TransformObjectToHClip(v.positionOS.xyz);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            float4 frag (fragData i) : SV_Target
            {
                float4 col = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv);
                return col;
            }

            ENDHLSL
        }
    }
}
