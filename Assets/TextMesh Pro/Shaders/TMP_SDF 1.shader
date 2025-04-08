Shader "TextMeshPro/Distance Field Overlay" {

Properties {
    _FaceTex            ("Face Texture", 2D) = "white" {}
    _FaceUVSpeedX       ("Face UV Speed X", Range(-5, 5)) = 0.0
    _FaceUVSpeedY       ("Face UV Speed Y", Range(-5, 5)) = 0.0
    _FaceColor          ("Face Color", Color) = (1,1,1,1)
    _FaceDilate         ("Face Dilate", Range(-1,1)) = 0

    _OutlineColor       ("Outline Color", Color) = (0,0,0,1)
    _OutlineTex         ("Outline Texture", 2D) = "white" {}
    _OutlineUVSpeedX    ("Outline UV Speed X", Range(-5, 5)) = 0.0
    _OutlineUVSpeedY    ("Outline UV Speed Y", Range(-5, 5)) = 0.0
    _OutlineWidth       ("Outline Thickness", Range(0, 1)) = 0
    _OutlineSoftness    ("Outline Softness", Range(0,1)) = 0

    _GlowColor          ("Color", Color) = (0, 1, 0, 0.5)
    _GlowOffset         ("Offset", Range(-1,1)) = 0
    _GlowInner          ("Inner", Range(0,1)) = 0.05
    _GlowOuter          ("Outer", Range(0,1)) = 0.05
    _GlowPower          ("Falloff", Range(1, 0)) = 0.75

    _MainTex            ("Font Atlas", 2D) = "white" {}
    _GradientScale      ("Gradient Scale", float) = 5.0
    _PerspectiveFilter  ("Perspective Correction", Range(0, 1)) = 0.875
    _Sharpness          ("Sharpness", Range(-1,1)) = 0
}

SubShader {

    Tags
    {
        "Queue"="Overlay" // Render on top of everything
        "IgnoreProjector"="True"
        "RenderType"="Transparent"
    }

    Stencil
    {
        Ref [_Stencil]
        Comp [_StencilComp]
        Pass [_StencilOp]
        ReadMask [_StencilReadMask]
        WriteMask [_StencilWriteMask]
    }

    Cull Off
    ZWrite Off // Disable writing to the depth buffer
    ZTest Always // Always render, ignoring depth testing
    Blend SrcAlpha OneMinusSrcAlpha // Enable transparency blending
    ColorMask RGBA

    Pass {
        CGPROGRAM
        #pragma target 3.0
        #pragma vertex VertShader
        #pragma fragment PixShader
        #pragma shader_feature __ BEVEL_ON
        #pragma shader_feature __ UNDERLAY_ON UNDERLAY_INNER
        #pragma shader_feature __ GLOW_ON

        #pragma multi_compile __ UNITY_UI_CLIP_RECT
        #pragma multi_compile __ UNITY_UI_ALPHACLIP

        #include "UnityCG.cginc"
        #include "UnityUI.cginc"
        #include "TMPro_Properties.cginc"
        #include "TMPro.cginc"

        struct vertex_t
        {
            UNITY_VERTEX_INPUT_INSTANCE_ID
            float4  position        : POSITION;
            float3  normal          : NORMAL;
            fixed4  color           : COLOR;
            float4  texcoord0       : TEXCOORD0;
            float2  texcoord1       : TEXCOORD1;
        };

        struct pixel_t
        {
            UNITY_VERTEX_INPUT_INSTANCE_ID
            UNITY_VERTEX_OUTPUT_STEREO
            float4  position        : SV_POSITION;
            fixed4  color           : COLOR;
            float2  atlas           : TEXCOORD0;        // Atlas
            float4  param           : TEXCOORD1;        // alphaClip, scale, bias, weight
        };

        float4 _FaceTex_ST;

        pixel_t VertShader(vertex_t input)
        {
            pixel_t output;

            UNITY_INITIALIZE_OUTPUT(pixel_t, output);
            UNITY_SETUP_INSTANCE_ID(input);
            UNITY_TRANSFER_INSTANCE_ID(input,output);
            UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(output);

            float4 vert = input.position;
            float4 vPosition = UnityObjectToClipPos(vert);

            output.position = vPosition;
            output.color = input.color;
            output.atlas = input.texcoord0;

            return output;
        }

        fixed4 PixShader(pixel_t input) : SV_Target
        {
            UNITY_SETUP_INSTANCE_ID(input);

            float c = tex2D(_MainTex, input.atlas).a;

            fixed4 faceColor = _FaceColor;
            faceColor.rgb *= input.color.rgb;

            return faceColor * input.color.a;
        }
        ENDCG
    }
}

Fallback "TextMeshPro/Mobile/Distance Field"
CustomEditor "TMPro.EditorUtilities.TMP_SDFShaderGUI"
}
