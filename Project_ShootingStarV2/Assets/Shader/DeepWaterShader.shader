Shader "Custom/DeepWaterShader"
{
    Properties
    {
        _MainTex("Albedo(RGB)",2D) = "white"{}
        _CubeMap ("Water Cube",CUBE) = ""{}
    }
        SubShader
    {
        Tags { "RenderType" = "Opaque"}
        //Tags { "RenderType" = "Transparent" "Queue" = "Transparent"}
        LOD 200
        

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Lambert
        #pragma target 3.0

        sampler2D _MainTex;
        samplerCUBE _CubeMap;

        struct Input
        {
            float2 uv_MainTex;
            float3 worldRefl;
            float3 viewDir;

            INTERNAL_DATA
        };

        void surf(Input IN, inout SurfaceOutput o)
        {                    
            fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
            float4 reflection = texCUBE(_CubeMap, WorldReflectionVector(IN, o.Normal));
            o.Emission = reflection*0.75;
            o.Alpha = 1;
        }
        float4 LightingWater(SurfaceOutput s, float3 lightDir, float3 viewDir, float atten)
        {
            float rim = saturate(dot(s.Normal, viewDir));
            rim = pow(1 - rim, 3);

            return rim * _LightColor0;

        }



        ENDCG
    }
        FallBack "Diffuse"
}
