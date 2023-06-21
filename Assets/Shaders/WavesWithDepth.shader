Shader "Custom/WavesWithDepth"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
        
        //_Amplitude ("Amplitude", Float) = 1
        //_Speed ("Speed", Float) = 1
        
        _WaterFogColor("Water Fog Color", Color) = (0,0,0,0)
        _WaterFogDensity("Water Fog Density", Range(0,1)) = 0.5
        _RefractionStrength("Refraction Strength", Range(0,1)) = 0.25
        
        //_WaveLength ("WaveLength", Float) = 10
        //_Steepness ("Steepness", Float) = 1
        //_Direction ("Direction (2D)", Vector) = (1,0,0,0)
        _WaveA("WaveA (Direction, Steepness, Wavelength)", Vector) = (1,0,5,10)
        _WaveB("WaveB", Vector) = (0,1,0.25,20)
        _WaveC("WaveC", Vector) = (1,1,0.15,10)
        
        _AnimSpeedX ("Anim Speed (X)", Range(0,4)) = 1.3
		_AnimSpeedY ("Anim Speed (Y)", Range(0,4)) = 2.7
		_AnimScale ("Anim Scale", Range(0,1)) = 0.03
		_AnimTiling ("Anim Tiling", Range(0,20)) = 8
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        //ZWrite Off
        //Blend One One
        //Blend DstColor Zero
        LOD 200
        
        //take a "screenshot" of the scene so we can keep track of it and then deform it
        GrabPass {"_WaterBackground"}

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard alpha vertex:vert finalcolor:ResetAlpha // addshadow fullforwardshadows

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        #include "LookingThroughWater.cginc"

        sampler2D _MainTex;

        struct Input
        {
            float2 uv_MainTex;
            float4 screenPos;
        };
        
        //make the texture MOVE
        float _AnimSpeedX;
        float _AnimSpeedY;
        float _AnimScale;
        float _AnimTiling;

        //Base properties
        float _Glossiness;
        float _Metallic;
        float4 _Color;

        //Make the wave
        //float _Speed;
        //float _WaveLength;
        //float _Steepness;
        //float2 _Direction;
        //_WaveA("WaveA (Direction, Steepness, Wavelength)", Vector) = (1,0,5,10)
        //_WaveB("WaveB (Direction, Steepness, Wavelength)", Vector) = (0,1,0.25,20)
        //_WaveC("WaveC (Direction, Steepness, Wavelength)", Vector) = (1,1,0.15,10)
        float4 _WaveA, _WaveB, _WaveC;

        float3 GerstnerWave(float4 wave, float3 pnt, inout float3 tangent, inout float3 binormal)
        {
            //decontruct the wave
            float2 direction = normalize(wave.xy);
            float steepness = wave.z;
            float waveLength = wave.w;


            waveLength = 2 * UNITY_PI / waveLength;
            float speed = sqrt(9.8/waveLength);
            float f = waveLength * (dot(direction, pnt.xz) - speed * _Time.y);
            float amplitude = steepness / waveLength;

            tangent += float3(- direction.x * direction.x       * (steepness * sin(f)),
                                direction.x                     * (steepness * cos(f)),
                               -direction.x * direction.y       * (steepness * sin(f)));
            
            binormal += float3(-direction.x * direction.y       * (steepness * sin(f)),
                                direction.y                     * (steepness * cos(f)),
                               -direction.y * direction.y       * (steepness * sin(f)));
            
            return float3 (direction.x * (amplitude * cos(f)), amplitude * sin(f), direction.y * (amplitude * cos(f)));
        }
        
        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling

        void ResetAlpha(Input IN, SurfaceOutputStandard o, inout fixed4 color)
        {
            color.a = 1;
        }

        void vert (inout appdata_full vertexData)
        {
            
            float3 vert = vertexData.vertex.xyz;
            float3 tangent = float3(1,0,0);
            float3 binormal = float3(0,0,1);
            float3 pnt = vert;
            pnt += GerstnerWave(_WaveA, vert, tangent, binormal);
            pnt += GerstnerWave(_WaveB, vert, tangent, binormal);
            pnt += GerstnerWave(_WaveC, vert, tangent, binormal);

            vertexData.vertex.xyz = pnt;
            vertexData.normal = normalize(cross(binormal, tangent));
            
            //float waveLength = 2 * UNITY_PI / _WaveLength;

            //float speed = sqrt(9.8/waveLength);

            //float2 direction = normalize(_Direction);
                                    //cross product is the calculation to find the perpendicular direction of the two vectors
                                    //DOT the more the two numbers are aligned / the same then the higher the DOT product
            //float f = waveLength * (dot(direction, vert.xz) - speed * _Time.y);

            //float amplitude = _Steepness / waveLength;
            
            
            // make wave go
            //vert.x += direction.x * (amplitude * cos(f));
            
            //vert.y = amplitude * sin(f);

            //vert.z += direction.y * (amplitude * cos(f));
            
            
            //Get lighting by calculating normal direction
            //float3 tangent = float3(1 - direction.x * (_Steepness * cos(f)), direction.x * (_Steepness * cos(f)), -direction.x *direction.y * (_Steepness * sin(f)));
            //float3 binormal = float3(-direction.x, * direction.y * (_Steepness * sin(f)), direction.y * (_Steepness * cos(f)), 1 -direction.y * direction.y * (_Steepness * sin(f)));
            //tangent = normalize(tangent);
            //float3 normal = float3(cross(binormal, tangent));
            
            //set the values
            //vertexData.vertex.xyz = vert;
            //vertexData.normal = normal;
        }
        
        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            IN.uv_MainTex.x += sin((IN.uv_MainTex.x + IN.uv_MainTex.y) * _AnimTiling + _Time.y * _AnimSpeedX) * _AnimScale;
            IN.uv_MainTex.y += cos((IN.uv_MainTex.x - IN.uv_MainTex.y) * _AnimTiling + _Time.y * _AnimSpeedY) * _AnimScale;
            
            // Albedo comes from a texture tinted by color
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
            o.Albedo = c.rgb;
            // Metallic and smoothness come from slider variables
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Alpha = c.a;
            //the further down the foggier and the less light affects the fog
            o.Emission = ColourBelowWater(IN.screenPos) * (1-c.a);
        }
        ENDCG
    }
    //FallBack "Diffuse"
}
