﻿Shader "Custom/Outlines"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Ambient("Ambient Strength", Range(0.0, 1.0)) = 1.0
        _Diffuse("Diffuse Strength", Range(0.0, 1.0)) = 1.0
        _Specular("Specular Strength", Range(0.0, 1.0)) = 1.0
        _SpecularExponent("Specular Exponent", Range(0.0, 128.0)) = 8.0

        _OutlineStrength("Outline Strength", Range(0.0, 1.0)) = 1.0
        _OutlineColor("Outline Color", Color) = (0, 0, 0, 1)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        Pass{
            GLSLPROGRAM

            #ifdef VERTEX
            
            uniform vec3 _WorldSpaceCameraPos; 
            uniform mat4 _Object2World;
            uniform mat4 _World2Object;

            varying vec4 WorldPos; 
            varying vec3 Normal;
            varying vec2 TextureCoordinate;

            void main(){
                WorldPos = _Object2World * gl_Vertex;
                Normal = normalize(vec3(vec4(gl_Normal, 0.0) * _World2Object));
                TextureCoordinate = gl_MultiTexCoord0.xy;
                gl_Position = gl_ModelViewProjectionMatrix * gl_Vertex;
            }

            #endif

            #ifdef FRAGMENT

            varying vec4 WorldPos; 
            varying vec3 Normal;
            varying vec2 TextureCoordinate;

            uniform sampler2D _MainTex;
            uniform vec4 _Color;
            uniform float _Ambient;
            uniform float _Diffuse;
            uniform float _Specular;
            uniform float _SpecularExponent;

            uniform vec3 _WorldSpaceCameraPos;
            uniform vec4 _WorldSpaceLightPos0; 
            uniform vec4 _LightColor0;

            void main(){
                vec3 viewDir = normalize(_WorldSpaceCameraPos - vec3(WorldPos));

                vec3 lightDir = 
                    float(_WorldSpaceLightPos0.w == 0.0) * normalize(vec3(_WorldSpaceLightPos0)) +
                    float(_WorldSpaceLightPos0.w != 0.0) * vec3(_WorldSpaceLightPos0 - WorldPos);

                float attentuation = 
                    float(_WorldSpaceLightPos0.w == 0.0) + 
                    float(_WorldSpaceLightPos0.w != 0.0) * (1.0 / length(_WorldSpaceLightPos0 - WorldPos));
                
                vec3 col = _Color.rgb;
                vec3 amb = _Ambient * _LightColor0.rgb;

                vec3 diff = _Diffuse * attentuation * vec3(_LightColor0) * col * max(0.0, dot(Normal, lightDir));

                vec3 spec = vec3(0.0, 0.0, 0.0) + 
                    float(dot(Normal, lightDir) >= 0.0) * 
                    _Specular * attentuation * vec3(_LightColor0) * col * pow(max(0.0, dot(reflect(-lightDir, Normal), viewDir)), _SpecularExponent);

                gl_FragColor = vec4((amb + diff + spec) * col * texture2D(_MainTex, TextureCoordinate).rgb, 1.0f);
            }

            #endif

            ENDGLSL
        }

        Cull Front
        Pass{
            GLSLPROGRAM

            #ifdef VERTEX

            uniform float _OutlineStrength;

            uniform vec4 _WorldSpaceCameraPos;

            void main(){
                float dist = distance(gl_Vertex, _WorldSpaceCameraPos);
                float thickness = 0.1f * _OutlineStrength;
                float scale = 1.0f + thickness * 0.05 * dist;
                vec3 scaledPos = gl_Vertex.xyz * scale;
                gl_Position = gl_ModelViewProjectionMatrix * vec4(scaledPos, 1.0f);
            }

            #endif


            #ifdef FRAGMENT

            uniform vec4 _OutlineColor;

            void main(){
                gl_FragColor = _OutlineColor;
            }

            #endif

            ENDGLSL
        }
    }
    FallBack "Diffuse"
}
