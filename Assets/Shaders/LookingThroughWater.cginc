#if !defined (LOOKING_THROUGH_WATER_INCLUDED)
#define LOOKING_THROUGH_WATER_INCLUDED

//Filled out by unity
sampler2D _CameraDepthTexture;
float4 _CameraDepthTexture_TexelSize;

//filled by grab pass
sampler2D _WaterBackground;

//filled by our properties
float3 _WaterFogColor;
float _WaterFogDensity;

float3 ColourBelowWater(float4 screenPos)
{
    //do this to get correct screen position
    float2 uv = screenPos.xy / screenPos.w;
    //in some platforms the v in uv is flipped
    #if UNITY_UV_STARTS_AT_TOP
    if (_CameraDepthTexture_TexelSize.y < 0)
    {
        uv.y = 1 - uv.y;
    }
    #endif
    
    //Depth relative to the screen
    float backgroundDepth = LinearEyeDepth(SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, uv));
    //how far away the surface of the water is from the ground
    float surfaceDepth = UNITY_Z_0_FAR_FROM_CLIPSPACE(screenPos.z);
    //depth from water to background
    float depthDifference = backgroundDepth - surfaceDepth;
    
    //return depthDifference/20;
    float3 backGroundColour = tex2D(_WaterBackground, uv).rgb;
    float fogFactor = exp2(-_WaterFogDensity * depthDifference);
    return lerp(_WaterFogColor, backGroundColour, fogFactor);// * depthDifference;
}

#endif