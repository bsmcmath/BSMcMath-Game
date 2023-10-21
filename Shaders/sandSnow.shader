Shader "MyShaders/sandSnow"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _DispTex ("DisplacedTexture", 2D) = "black" {}
        _Color ("Color", Color) = (1, 1, 1, 1)
        _DispColor ("DisplacedColor", Color) = (1, 1, 1, 1)
        _Height ("Height", Float) = 0.3
        //_TessMinDistance ("Tessellation Minimum Distance", Float) = 6000
        //_TessMaxDistance ("Tessellation Maximum Distance", Float) = 1500
        //_TessMax ("Tessellation Max", Float) = 20
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
                float3 normalWS : TEXCOORD2;
                float disp : TEXCOORD3;
            };

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            CBUFFER_START(UnityPerMaterial)
            Texture2D _MainTex;
            float4 _MainTex_ST;
            SamplerState sampler_MainTex;
            Texture2D _DispTex;
            SamplerState sampler_DispTex;
            float4 _Color;
            float4 _DispColor;
            float _Height;
            CBUFFER_END

            uniform Texture2D interactionTex;
            uniform SamplerState samplerinteractionTex;
            uniform float3 extentsMin, extentsMax;

            float map(float value, float low1, float high1, float low2, float high2) { return low2 + (value - low1) * (high2 - low2) / (high1 - low1); }

            fragData vert (vertData v)
            {
                fragData o;
                o.positionCS = float4(TransformObjectToWorld(v.positionOS.xyz), 0);
                o.normalWS = TransformObjectToWorldNormal(v.normalOS);

                float2 uv;
                uv.x = map(o.positionCS.x, extentsMin.x, extentsMax.x, 0, 1);
                uv.y = map(o.positionCS.z, extentsMin.z, extentsMax.z, 0, 1);

                float4 inter = interactionTex.SampleLevel(samplerinteractionTex, uv, 0);
                inter.y = map(inter.y, 1, 0, extentsMin.y, extentsMax.y);
                inter.y = clamp(inter.y, o.positionCS.y, o.positionCS.y + _Height);
                o.disp = map(inter.y, o.positionCS.y, o.positionCS.y + _Height, 1, 0) * v.color.x;
                o.positionCS.y = lerp(o.positionCS.y, inter.y, v.color.x);
                o.positionCS = TransformWorldToHClip(o.positionCS.xyz);
                return o;
            }

            float4 frag (fragData i) : SV_Target
            {
                float4 col = lerp(_Color, _DispColor, i.disp);
                return col;
            }

            ENDHLSL
        }
    }
}
