Shader "Hidden/Dither"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "red" {}
        _Dither ("Texture", 2D) = "white" {}
        _ColorRamp("Color Ramp", 2D) = "white" {}

        _TL ("Direction", Vector) = (0.0, 0.0, 0.0, 0.0)
        _BL ("Direction", Vector) = (0.0, 0.0, 0.0, 0.0)
        _TR ("Direction", Vector) = (0.0, 0.0, 0.0, 0.0)
        _BR ("Direction", Vector) = (0.0, 0.0, 0.0, 0.0)
        
        //_Tiling("Tiling", Float) = 192.0
        //_Threshold("Threshold", Float) = 0.1
    }
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            sampler2D _MainTex;
            float4 _MainTex_TexelSize;

            sampler2D _Dither;
            float4 _Dither_TexelSize;

            sampler2D _ColorRamp;

            float4 _BL;
            float4 _TL;
            float4 _TR;
            float4 _BR;

            float _Tiling;

            float cubeProject(sampler2D tex, float2 texel, float3 dir){
                float3x3 rotDirMatrix = {0.9473740, -0.1985178, 0.2511438,
                                         0.2511438, 0.9473740, -0.1985178,
                                         -0.1985178, 0.2511438, 0.9473740};

                dir = mul(rotDirMatrix, dir);
                float2 uvCoords;
                if( ( abs(dir.x) > abs(dir.y) ) && ( abs(dir.x) > abs(dir.z) ) ) {
                    uvCoords = dir.yz; // x axis
                } else if( (abs(dir.z) > abs(dir.y) ) && ( abs(dir.z) > abs(dir.x) ) ){
                    uvCoords = dir.xy; // z axis
                }else{
                    uvCoords = dir.xz; // y axis
                }

                return tex2D(tex, texel * _Tiling * uvCoords).r;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                float3 dir = normalize(lerp(lerp(_BL, _TL, i.uv.y), lerp(_BR, _TR,i.uv.y), i.uv.x));

                float lum = dot(col, float3(0.299f, 0.587f, 0.114f));

                float2 ditherCoords = i.uv * _Dither_TexelSize.xy * _MainTex_TexelSize.zw;
                float ditherLum = cubeProject(_Dither, _Dither_TexelSize.xy, dir);

                float ramp = (lum <= clamp(ditherLum, 0.1f, 0.9f)) ? 0.1f : 0.9f;

                float3 output = tex2D(_ColorRamp, float2(ramp, 0.5f));
                return float4(output, 1.0f);
            }
            ENDCG
        }
    }
}
