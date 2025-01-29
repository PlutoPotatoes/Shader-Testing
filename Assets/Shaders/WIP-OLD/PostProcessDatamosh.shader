Shader "CustomRenderTexture/PostProcessDatamosh"
{
    Properties
    {
        _MainTex("InputTex", 2D) = "white" {}
        _VRadius("Vingette Radius", Range(0.0,1.0)) = 0.8
        _VSoft("Vingette Softness", Range(0.0,1.0)) = 0.5
     }

     SubShader
     {
        Tags{
            "RenderingPipeline" = "UniversalPipeline"
            "RenderType" = "Opaque"
        }

        Pass
        {
            Name "VingettePass"


            CGPROGRAM
            #include "UnityCG.cginc"
            #pragma vertex vert_img
            #pragma fragment frag

            sampler2D   _MainTex;
            float _VRadius;
            float _VSoft;

            float4 frag(v2f_img input) : COLOR
            {
                float4 base = tex2D(_MainTex, input.uv);
                float distFromCenter = distance(input.uv.xy, float2(0.5, 0.5));
                float vignette = smoothstep(_VRadius, _VRadius - _VSoft, distFromCenter);
                
                base = saturate(base*vignette);
                return base;
            }
            ENDCG
        }
    }
}
