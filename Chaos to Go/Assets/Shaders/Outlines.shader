Shader "Custom/Outlines"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        Pass{
            GLSLPROGRAM

            #ifdef VERTEX
           
            varying vec2 TextureCoordinate;
           
            void main()
            {
                gl_Position = gl_ModelViewProjectionMatrix * gl_Vertex;
                TextureCoordinate = gl_MultiTexCoord0.xy;
            }
           
            #endif
           
            #ifdef FRAGMENT
                       
            uniform sampler2D _MainTex;
            varying vec2 TextureCoordinate;
           
            void main()
            {
                gl_FragColor = texture2D(_MainTex, TextureCoordinate);
            }
           
            #endif

            ENDGLSL
        }
        
    }
    FallBack "Diffuse"
}
