Shader "Custom/Toon"
{
    Properties
    {
        [Header(Base Parameters)]
        _Color ("Tint", Color) = (0, 0, 0, 1)
        _MainTex ("Texture", 2D) = "white" {}
        _Specular ("Specular Color", Color) = (1,1,1,1)
        [HDR] _Emission ("Emission", color) = (0 ,0 ,0 , 1)

        [Header(Lighting Parameters)]
        _ShadowTint ("Shadow Color", Color) = (0.5, 0.5, 0.5, 1)
        [IntRange]_StepAmount ("Shadow Steps", Range(1,16)) = 2
        _StepWidth ("Step Size", Range(0.05, 1)) = 0.25
        _SpecularSize ("Specular Size", Range(0,1)) = 0.1
        _SpecularFalloff ("Specular Falloff", Range(0,2)) = 1
        
        [Header(Wibbles)]
        _WibbleFactor ("Wibble Factor", float) = 1
        _WibbleRate ("Wibble Rate", float) = 1
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" "Queue" = "Geometry"}

        CGPROGRAM
        //the shader is a surface shader, meaning that it will be extended by unity in the background to have fancy lighting and other features
        //our surface shader function is called surf and we use our custom lighting model
        //fullforwardshadows makes sure unity adds the shadow passes the shader might need
        #pragma surface surf Stepped fullforwardshadows
        #pragma vertex vert
        #pragma target 3.0

        sampler2D _MainTex;
        fixed4 _Color;
        half3 _Emission;
        fixed4 _Specular;

        float3 _ShadowTint;
        float _StepWidth;
        float _StepAmount;
        float _SpecularSize;
        float _SpecularFalloff;
        float _WibbleFactor;
        float _WibbleRate;

        struct ToonSurfaceOutput
        {
            fixed3 Albedo;
            half3 Emission;
            fixed3 Specular;
            fixed Alpha;
            fixed3 Normal;
        };

        float4 LightingStepped(ToonSurfaceOutput s, float3 lightDir, half3 viewDir, float shadowAttenuation)
        {
            float towardsLight = dot(s.Normal, lightDir);
            towardsLight /= _StepWidth;
            float lightIntensity = floor(towardsLight);

            float change = fwidth(towardsLight);
            float smoothing = smoothstep(0,change,frac(towardsLight));
            lightIntensity += smoothing;
            lightIntensity /= _StepAmount;
            lightIntensity = saturate(lightIntensity);

            #ifdef USING_DIRECTIONAL_LIGHT
                float attenuationChange = fwidth(shadowAttenuation) * 0.5;
                float shadow = smoothstep(0.5 - attenuationChange, 0.5 + attenuationChange, shadowAttenuation);
            #else
                float attenuationChange = fwidth(shadowAttenuation);
                float shadow = smoothstep(0,attenuationChange,shadowAttenuation);
            #endif
                lightIntensity *= shadow;
            
            //calculate how much the surface points points towards the reflection direction
            float3 reflectionDirection = reflect(lightDir, s.Normal);
            float towardsReflection = dot(viewDir, -reflectionDirection);

            //make specular highlight all off towards outside of model
            float specularFalloff = dot(viewDir, s.Normal);
            specularFalloff = pow(specularFalloff, _SpecularFalloff);
            towardsReflection = towardsReflection * specularFalloff;

            //make specular intensity with a hard corner
            float specularChange = fwidth(towardsReflection);
            float specularIntensity = smoothstep(1 - _SpecularSize, 1 - _SpecularSize + specularChange, towardsReflection);
            //factor inshadows
            specularIntensity = specularIntensity * shadow;

            float4 color;
            //calculate final color
            color.rgb = s.Albedo * lightIntensity * _LightColor0.rgb;
            color.rgb = lerp(color.rgb, s.Specular * _LightColor0.rgb, saturate(specularIntensity));

            color.a = s.Alpha;
            return color;
        }
        
        //input struct which is automatically filled by unity
        struct Input
        {
            float2 uv_MainTex;
        };
        void vert (inout appdata_full v)
        {
            //v.vertex.xyz += v.normal * (cos(_Time.y*5)+1);
            v.vertex.y += sin(_Time.y * _WibbleRate + v.vertex.x) *_WibbleFactor;
        }

        //the surface shader function which sets parameters the lighting function then uses
        void surf (Input i, inout ToonSurfaceOutput o)
        {
            //sample and tint albedo texture
            fixed4 col = tex2D(_MainTex, i.uv_MainTex);
            col *= _Color;
            o.Albedo = col.rgb;

            o.Specular = _Specular;

            float3 shadowColor = col.rgb * _ShadowTint;
            o.Emission = _Emission + shadowColor;
        }

        ENDCG
    }
    FallBack "Standard"
}