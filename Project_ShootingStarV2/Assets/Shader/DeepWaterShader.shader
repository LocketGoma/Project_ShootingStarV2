Shader "Custom/DeepWaterShader"
{
    Properties
    {
        _MainTex("Albedo(RGB)",2D) = ""{}
        _BumpMap("Water Bump",2D) = "bump"{}
    }
        SubShader
    {
        Tags { "RenderType" = "Opaque"}
        //Tags { "RenderType" = "Transparent" "Queue" = "Transparent"}
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Lambert

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        sampler2D _MainTex;
        sampler2D _BumpMap;

        struct Input
        {
            float2 uv_BumpMap;
            float3 worldRefl;

            INTERNAL_DATA
        };


        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf(Input IN, inout SurfaceOutput o)
        {
            
            o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap));
            o.Albedo = tex2D(_MainTex,WorldReflectionVector(IN,o.Normal));

        }




        ENDCG
    }
        FallBack "Diffuse"
}
