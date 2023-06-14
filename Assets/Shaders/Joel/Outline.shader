Shader "Custom/Outline"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
        _OutlineWidth ("OutlineWidth", Range(0,1)) = 1
        _OutlineSoftness ("OutlineSoftness", Range(0,1)) = 1
        normalWS ("NormalWS", Range(0,1)) = 1
        viewWS ("ViewWS", Range(0,1)) = 1
        _OutlinePower ("OutlinePower", Range(0,1)) = 1
        _OutlineColor ("OutlineColor", Range(0,1)) = 1
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        sampler2D _MainTex;

        struct Input
        {
            float2 uv_MainTex;
        };

        half _Glossiness;
        half _Metallic;
        fixed4 _Color;
        float _OutlineWidth;
        float _OutlineSoftness;
        float normalWS;
        float viewWS;
        float _OutlinePower;
        float _OutlineColor;

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            // Albedo comes from a texture tinted by color
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
            o.Albedo = c.rgb;
            // Metallic and smoothness come from slider variables
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Alpha = c.a;

            float edge1 = 1 - _OutlineWidth;
            float edge2 = edge1 + _OutlineSoftness;
            float fresnel = pow(1.0 - saturate(dot(normalWS, viewWS)), _OutlinePower);
            o.Albedo = lerp(1, smoothstep(edge1, edge2, fresnel), step(0, edge1)) * _OutlineColor;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
