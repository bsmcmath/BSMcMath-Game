Shader "MyShaders/geoGrass"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color1 ("Color", Color) = (1, 1, 1, 1)
        _Color2 ("Color2", Color) = (0.7, 0.7, 0.7, 0.7)
        _Height1 ("Height1", Float) = 0.2
        _Height2 ("Height2", Float) = 0.2
        _Taper ("Taper", Float) = 0.4
        _WindTex ("WindTexture", 2D) = "white" {}
        _WindAmp ("WindAmplitude", Float) = 0.2
        _WindScale ("WindScale", Float) = 1
        _WindSpeed ("WindSpeed", Float) = 0.1
        _WindOffset ("Wind Offset", Vector) = (-0.1, -0.1, -0.1, -0.1)
        _LOD0Distance ("Lod0Distance", Float) = 20
        _LOD1Distance ("Lod1Distance", Float) = 40
        _LOD2WidthAdd ("Lod2WidthAdd", Float) = 0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" "RenderPipeline" = "UniversalPipeline" }
        LOD 300
        Cull Off

        Pass
        {
            Tags {"LightMode" = "UniversalForward"}

            HLSLPROGRAM

            #pragma vertex vert
            #pragma fragment frag
            #pragma require geometry
            #pragma geometry geom

            struct vertData
            {
                float4 positionOS : POSITION;
                float3 normalOS : NORMAL;
                float2 uv : TEXCOORD0;
            };

            struct geomData
            {
                float3 positionWS : TEXCOORD1;
                //float3 positionVS : TEXCOORD2;
                float3 normal : TEXCOORD3;
                float2 uv : TEXCOORD0;
            };

            struct fragData
            {
                float4 positionCS : SV_POSITION;
                float3 positionWS : TEXCOORD1;
                float3 normal : TEXCOORD3;
                float2 uv : TEXCOORD0;
            };

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            CBUFFER_START(UnityPerMaterial)
            Texture2D _MainTex;
            float4 _MainTex_ST;
            float4 _Color1;
            float4 _Color2;
            float _Height1;
            float _Height2;
            float _Taper;
            Texture2D _NoiseTex;
            SamplerState sampler_NoiseTex;
            float _NoiseAmp;
            float _NoiseScale;  
            Texture2D _WindTex;
            SamplerState sampler_WindTex;
            float _WindAmp;
            float _WindScale;
            float _WindSpeed;
            float4 _WindOffset;
            float _LOD0Distance;
            float _LOD1Distance;
            float _LOD2WidthAdd;
            CBUFFER_END

            struct interaction
            {
                float3 pos;
                float size;
            };

            uniform Texture2D interactionTex;
            uniform SamplerState samplerinteractionTex;

            uniform float3 lodCenter;
            uniform float3 extentsMin, extentsMax;

            uniform StructuredBuffer<interaction> interactions;
            uniform int interactionCount;

            float map(float value, float low1, float high1, float low2, float high2) { return low2 + (value - low1) * (high2 - low2) / (high1 - low1); }
            float map01(float value, float low2, float high2) { return low2 + value * (high2 - low2); }

            geomData vert (vertData v)
            {
                geomData g;
                g.positionWS = TransformObjectToWorld(v.positionOS.xyz);
                g.normal = TransformObjectToWorldNormal(v.normalOS);
                g.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return g;
            }

            [maxvertexcount(5)]
            void geom (triangle geomData input[3], inout TriangleStream<fragData> triStream)
            {
                //_WorldSpaceCameraPos
                float3 toCamera = lodCenter - input[0].positionWS;
                float distance = dot(toCamera, toCamera);
                
                if (distance < _LOD0Distance)
                {
                    fragData f0, f1, f2, f3, f4;

                    float3 center = input[0].positionWS + input[1].positionWS + input[2].positionWS;
                    center *= 0.33333;

                    float4 wind = _WindTex.SampleLevel(sampler_WindTex, center.xz * _WindScale + float2(_Time.y * _WindSpeed, 0), 0);
                    wind += _WindOffset;
                    wind *= _WindAmp;

                    float3 centermid = (input[0].positionWS + input[1].positionWS) * 0.5 + input[0].normal * _Height1;

                    float2 uv;
                    uv.x = map(centermid.x, extentsMin.x, extentsMax.x, 0, 1);
                    uv.y = map(centermid.z, extentsMin.z, extentsMax.z, 0, 1);

                    float3 push = float3(0, 0, 0);
                    float4 inter = interactionTex.SampleLevel(samplerinteractionTex, uv, 0);
                    inter.y = map01(inter.y, extentsMax.y, extentsMin.y);
                    push.y = clamp(center.y + _Height2 - inter.y, 0, _Height2) * 0.5 * inter.w * inter.w;
                    push.x = inter.x * 2 - 1;
                    push.x *= push.y;
                    push.z = inter.z * 2 - 1;
                    push.z *= push.y;
                    push.y *= -1;

                    float3 immediatePush = float3(0, 0, 0);
                    for (uint i = 0; i < interactionCount; i++)
                    {
                        float3 dir = centermid - interactions[i].pos;
                        float mag = dot(dir, dir);
                        //if (mag < 1) continue;

                        mag = sqrt(mag);

                        float size = interactions[i].size * 2.5;
                        dir = dir / (mag * size) * (size - min(mag, size));
                        immediatePush += dir;
                    }
                    //float projection = dot(push, input[0].normal);
                    //projection = min(0, projection);
                    //push += input[0].normal * projection;
                    immediatePush -= input[0].normal * dot(immediatePush, immediatePush) * (1 - abs(dot(immediatePush, input[0].normal))) * 0.5;
                    push += immediatePush;

                    push += wind;

                    f0.positionWS = input[0].positionWS;
                    f0.positionCS = TransformWorldToHClip(f0.positionWS);
                    f0.normal = normalize(f0.positionWS - center);
                    f0.uv = float2(0, 0);

                    f1.positionWS = input[1].positionWS;
                    f1.positionCS = TransformWorldToHClip(f1.positionWS);
                    f1.normal = normalize(f1.positionWS - center);
                    f1.uv = float2(0, 0);

                    f2.positionWS = input[0].positionWS + input[0].normal * _Height1;
                    f2.positionWS = lerp(f2.positionWS, centermid, _Taper);
                    f2.positionWS += push * 0.6;
                    f2.positionCS = TransformWorldToHClip(f2.positionWS);
                    f2.normal = normalize(f2.positionWS - center);
                    f2.uv = float2(0, 0.5);

                    f3.positionWS = input[1].positionWS + input[1].normal * _Height1;
                    f3.positionWS = lerp(f3.positionWS, centermid, _Taper);
                    f3.positionWS += push * 0.6;
                    f3.positionCS = TransformWorldToHClip(f3.positionWS);
                    f3.normal = normalize(f3.positionWS - center);
                    f3.uv = float2(0, 0.5);

                    f4.positionWS = center + input[0].normal * _Height2;
                    f4.positionWS += push;
                    f4.positionCS = TransformWorldToHClip(f4.positionWS);
                    f4.normal = normalize(f4.positionWS - center);
                    f4.uv = float2(0, 1);

                    triStream.Append(f0); triStream.Append(f1); triStream.Append(f2); triStream.Append(f3); triStream.Append(f4); 
                    triStream.RestartStrip();
                }
                else if (distance < _LOD1Distance)
                {
                    fragData f0, f1, f2, f3, f4;

                    float3 center = input[0].positionWS + input[1].positionWS + input[2].positionWS;
                    center *= 0.33333;

                    float4 wind = _WindTex.SampleLevel(sampler_WindTex, center.xz * _WindScale + float2(_Time.y * _WindSpeed, 0), 0);
                    wind += _WindOffset;
                    wind *= _WindAmp;

                    float3 centermid = (input[0].positionWS + input[1].positionWS) * 0.5 + input[0].normal * _Height1;

                    f0.positionWS = input[0].positionWS;
                    f0.positionCS = TransformWorldToHClip(f0.positionWS);
                    f0.normal = normalize(f0.positionWS - center);
                    f0.uv = float2(0, 0);

                    f1.positionWS = input[1].positionWS;
                    f1.positionCS = TransformWorldToHClip(f1.positionWS);
                    f1.normal = normalize(f1.positionWS - center);
                    f1.uv = float2(0, 0);

                    f2.positionWS = input[0].positionWS + input[0].normal * _Height1;
                    f2.positionWS = lerp(f2.positionWS, centermid, _Taper);
                    f2.positionWS += wind * 0.6;
                    f2.positionCS = TransformWorldToHClip(f2.positionWS);
                    f2.normal = normalize(f2.positionWS - center);
                    f2.uv = float2(0, 0.5);

                    f3.positionWS = input[1].positionWS + input[1].normal * _Height1;
                    f3.positionWS = lerp(f3.positionWS, centermid, _Taper);
                    f3.positionWS += wind * 0.6;
                    f3.positionCS = TransformWorldToHClip(f3.positionWS);
                    f3.normal = normalize(f3.positionWS - center);
                    f3.uv = float2(0, 0.5);

                    f4.positionWS = center + input[0].normal * _Height2;
                    f4.positionWS += wind;
                    f4.positionCS = TransformWorldToHClip(f4.positionWS);
                    f4.normal = normalize(f4.positionWS - center);
                    f4.uv = float2(0, 1);

                    triStream.Append(f0); triStream.Append(f1); triStream.Append(f2); triStream.Append(f3); triStream.Append(f4); 
                    triStream.RestartStrip();
                }
                else
                {
                    fragData f0, f1, f2;

                    float3 center = input[0].positionWS + input[1].positionWS + input[2].positionWS;
                    center *= 0.33333;

                    float3 widthAdd = input[0].positionWS - input[1].positionWS;
                    widthAdd *= _LOD2WidthAdd;

                    f0.positionWS = input[0].positionWS + widthAdd;
                    f0.positionCS = TransformWorldToHClip(f0.positionWS);
                    f0.normal = normalize(f0.positionWS - center);
                    f0.uv = float2(0, 0);

                    f1.positionWS = input[1].positionWS - widthAdd;
                    f1.positionCS = TransformWorldToHClip(f1.positionWS);
                    f1.normal = normalize(f1.positionWS - center);
                    f1.uv = float2(0, 0);

                    f2.positionWS = center + input[0].normal * _Height2;
                    f2.positionCS = TransformWorldToHClip(f2.positionWS);
                    f2.normal = normalize(f2.positionWS - center);
                    f2.uv = float2(0, 1);

                    triStream.Append(f0); triStream.Append(f1); triStream.Append(f2);
                    triStream.RestartStrip();
                }
            }

            #include "Assets/Lux URP Essentials/Shader Graphs/Includes/Lux_Lighting_Toon_V2.hlsl"

            float4 frag (fragData f) : SV_Target
            {
                float4 zero = float4(1, 1, 1, 1);
                half3 lighting, albedo, specular, normal;
                half smoothness, occlusion;

                //Lighting_half(f.positionWS, zero, zero.xyz, 
                //    f.normal, zero.xyz, zero.xyz, zero.xyz, 
                //    _Color1.xyz, _Color2.xyz, zero.xyz, 0, false, zero.xyz, 0, 1,
                //    _Steps, 0, .01, .45, .01, .01, 0, 0,
                //    0, 0, 1, 1, 
                //    1, 0, zero, 0,
                //    zero.xy, zero.xy, false, false,
                //    _MainTex, 1, sampler_NoiseTex, sampler_NoiseTex,
                //    lighting, albedo, specular, smoothness, occlusion, normal);

                Light light = GetMainLight();
                lighting = lerp(_Color1, _Color2, saturate(dot(f.normal, light.direction)));
                return float4(lighting, 1);
            }

            ENDHLSL
        }
    }
}
