Shader "Custom/DataMoshImageEffect"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "Queue" = "Transparent" }

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
                float4 uvgrab : TEXCOORD1;

            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.uvgrab = ComputeGrabScreenPos(o.vertex);
                return o;
            }

            sampler2D _MainTex;
            sampler2D _CameraMotionVectorsTexture;
            sampler2D _MotionVectorTexture;
            sampler2D _PR;
            int _Button;
            float _DMOSHSTR;
            sampler2D _FrameBuffer;
            float _VariableButton;


            float nrand(float x, float y)
            {
                return frac(sin(dot(float2(x, y), float2(12.9898, 78.233))) * 43758.5453);
            }   

            fixed4 frag (v2f i) : SV_Target
            { 
                float2 uvr = round(i.uv*(_ScreenParams.xy/(32*nrand(_Time.x, _Time.y))))/(_ScreenParams.xy/(32*nrand(_Time.x, _Time.y)));
                float4 mot = tex2D(_CameraMotionVectorsTexture,uvr);
                float n = nrand(_Time.x, uvr.x+uvr.y*_ScreenParams.x);
 
                #if UNITY_UV_STARTS_AT_TOP
                float2 mvuv = float2(i.uv.x+2*mot.r,i.uv.y-2*mot.g);
                #else
                float2 mvuv = float2(i.uv.x+2*mot.r,1-i.uv.y+2*mot.g);
                #endif
                fixed4 col = lerp(tex2D(_MainTex, i.uv), tex2D(_FrameBuffer, mvuv), lerp(0,round(1-(n)/1.4),_VariableButton));
                return col;

                //
                // Frame Buffer works but only buffering one frame to offput. 
                //next step smear the frame accross the frame;

            }
            ENDCG
        }
    }
}
