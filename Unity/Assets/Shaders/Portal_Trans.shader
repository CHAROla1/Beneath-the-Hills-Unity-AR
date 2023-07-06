Shader "Custom/Portal_Trans"
{
    SubShader
    {
        
        ZWrite Off
        Cull Off
        ColorMask 0

        Stencil
        {
            Ref 1
            Pass Replace
        }

        Pass
        {
            
        }
    }
}
