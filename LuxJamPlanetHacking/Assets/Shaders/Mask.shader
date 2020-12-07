Shader "LuxJam/Mask"
{
    Properties
    {
        _Sref("Stencil ref", int) = 1
        [Enum(UnityEngine.Rendering.CompareFunction)] _SComp("Stencil Comp", Float) = 8
        [Enum(UnityEngine.Rendering.StencilOp)] _SOp("Stencil Op", Float) = 2
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" "Queue"="Geometry-100"}
        LOD 200
        ZWrite Off
        ColorMask 0

        Pass
        {
            Stencil
            {
                Ref [_Sref]
                Comp [_SComp]
                Pass [_SOp]
            }
            Cull back
        }

        CGPROGRAM
        #pragma surface surf Lambert

        sampler2D _MainTex;

        struct Input
        {
            float2 uv_MainTex;
        };

        void surf (Input IN, inout SurfaceOutput o)
        {
            
        }
        ENDCG
    }
    FallBack "Diffuse"
}
