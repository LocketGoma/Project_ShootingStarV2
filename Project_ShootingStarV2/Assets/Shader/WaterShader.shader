Shader "Custom/WaterShader"
{
    Properties
    {
        _MainTex("Albedo(RGB)",2D) = "white"{}
        _BumpMap("Water Bump",2D) = "bump"{}
    }
        SubShader
    {
        //Tags { "RenderType" = "Opaque"}
        Tags { "RenderType" = "Transparent" "Queue" = "Transparent"}
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Lambert alpha:blend

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        sampler2D _MainTex;
        sampler2D _BumpMap;

        struct Input
        {
            float2 uv_BumpMap;
            float3 worldRefl;
            float3 viewDir;

            INTERNAL_DATA
        };


        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutput o)
        {            
            float3 fNormalA = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap + float2(_Time.y * 0.005, 0.0f)));
            float3 fNormalB = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap - float2(_Time.y * 0.015, _Time.y * 0.02)));
            float3 fNormalC = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap + float2(_Time.y * -0.020, _Time.y * 0.025)));

            o.Normal = (fNormalA + fNormalB + fNormalC)*(1.0f/3.0f);
            o.Normal *= float3(0.5, 0.5, 1);
            o.Albedo = tex2D(_MainTex,WorldReflectionVector(IN,o.Normal))*0.5;


            float fRim = dot(IN.viewDir, o.Normal);
            o.Alpha = saturate(pow(1 - fRim, 2))+0.1;
            
        }




        ENDCG
    }
    FallBack "Diffuse"
}
