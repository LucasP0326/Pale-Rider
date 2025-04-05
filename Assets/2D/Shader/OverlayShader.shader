Shader "Custom/OverlayShader"
{
    SubShader
    {
        Tags { "Queue" = "Overlay" }
        Pass
        {
            ZTest Always
            ZWrite Off
            ColorMask RGBA
        }
    }
}
