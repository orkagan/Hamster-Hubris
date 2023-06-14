Shader "Unlit/DEATH"
{
    Properties
    {
        _ColorA ("Color A", Color) = (1,1,1,1)
        _ColorB ("Color B", Color) = (1,1,1,1)
        _ColorC ("Color C", Color) = (1,1,1,1)
        
        _ColorStart ("Color Start", Range(0,1)) = 1
        _ColorEnd ("Color End", Range(0,1)) = 0
        _FreeRange ("Free Range", Range(0,100)) = 50
        _FreeRangeB ("Free Range B", Range(0,1)) = 0.5
        _AnimSpeed ("Anim Speed", Range(-100,100)) = 1


        

    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue" = "Transparent"}

        Pass
        {
            Cull Back
            ZWrite Off
            Blend One One
            //Blend DstColor Zero
            
            CGPROGRAM
            //vertex reference is called vert function same with the fragment
            #pragma vertex vert
            #pragma fragment frag
            #pragma Standard alpha

            //Include any library here basically pastes it in because HLSL is not object oriented its procedural
            #include "UnityCG.cginc"

            struct appdata // Mesh data
            {
                //position of the mesh
                float4 vertex : POSITION;
                //direction of the normal
                float3 normals : NORMAL;
                //xy of the 2D uv
                float2 uv : TEXCOORD0;
            };

            //v2f is an interpolator rasterizing the vertices in the mesh to fragments which kind of are pixels
            struct Interpolator_v2f
            {
                //textcoord0 isnt the same as appdata where it is the 1st uv channel of the texture
                //its basically just a pre defines stream for information and the different indexes are just for data separations 
                float2 uv : TEXCOORD1;
                float3 normal : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            //making the color property instance / variable in the sub shader / pass


            float4 _ColorA;
            float4 _ColorB;
            float4 _ColorC;

            
            float _ColorStart;
            float _ColorEnd;
            float _FreeRange;
            float _FreeRangeB;
            float _AnimSpeed;

            #define TAU 6.2831855

            
            Interpolator_v2f vert (appdata v)
            {
                Interpolator_v2f o;
                //Allows the shader to render the color on the clip view coords instead of screen coordinates so it moves with the object instead of static position
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.normal = UnityObjectToWorldNormal(v.normals);
                o.uv = v.uv;
                
                //o.normal = v.normals;
                return o;
            }

            float InverseLerp(float a, float b, float v)
            {
                return (v - a) / (b - a);
            }

            float4 frag (Interpolator_v2f i) : SV_Target
            {
                //float t = InverseLerp(_ColorStart, _ColorEnd, i.uv.x);
                //float t = abs(frac(i.uv.x * 5) * 2 - 1);
                // float t = tan(i.uv.x * 50);
                //float t = cos(i.uv.x * 50);
                //const float t  = tanh(i.uv.x * TAU * _FreeRange) * _FreeRangeB + _FreeRangeB;

                
                
                float xOffset = cos(i.uv.x * TAU -22) - _AnimSpeed * 10;
                float t  = tan((i.uv.y * TAU + xOffset * _Time.zy * _AnimSpeed) * _FreeRange / 20) * _FreeRangeB + _FreeRangeB;
                t *= 1-i.uv.y;
               // t /= 3+i.uv.y;

                float TopBottomRemover = abs(i.normal.y < 0.999);
                float waves = t * TopBottomRemover;
                float4 color = (_ColorA + _ColorB - _ColorC);
                return waves + color / _Time.x;
                
                //float4 o = ((t * _ColorA) + (t * _ColorB) / _FreeRangeB);
                
                
                //float4 outColor = lerp(_ColorA, _ColorB);
                //return outColor;
            }
            ENDCG
        }
    }
}
