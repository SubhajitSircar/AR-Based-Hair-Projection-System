Shader "Custom/DepthMask"
{
    SubShader
    {
        Tags {"Queue" = "Geometry-10" } // render early
        ColorMask 0                     // don’t draw colors
        ZWrite On                       // write to depth buffer

        Pass {}
    }
}
