Shader <"Outline">
{
    SubShader
    {
        HLSLINCLUDE
        #include "Packages/com.unity.postprocessing/PostProcessing/Shaders/StdLib.hlsl"

        TEXTURE2D_SAMPLER2D(_MainTex , sampler_MainTex);
        float4 _MainTex_TexelSize;

        TEXTURE2D_SAMPLER2D(_CameraDepthTexture, sampler_CameraDepthTexture);
        float4 unity_MatrixMVP;

        half _MinDepth;
        half _MaxDepth;
        half _Thickness;
        half _EdgeColor;
    
        struct v2f
        {
            float2 uv : TEXCOORD0;
            float4 vertex : SV_POSITION;
            float3 screen_pos : TEXCOORD2;
        };

        inline float4 ComputeScreenPos(float4 pos)
        {
            float4 o = pos * 0.5f;
            o.xy = float(o.x, o.y * _ProjectionParams.x) + o.w;
            o.zw = pos.zw;
            return o;
        };

  
        ENDHLSL
        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            ENDHLSL
        }
    }
}
