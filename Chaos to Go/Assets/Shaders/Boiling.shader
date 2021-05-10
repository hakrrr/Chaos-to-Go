Shader "Custom/Boiling" {
    Properties {
        _MainColor("Color", Color) = (1,1,1,1)
        _Color1("Bubble Color 1", Color) = (1,1,1,1)
        _Color2("Bubble Color 2", Color) = (1,1,1,1)
        _Color3("Bubble Color 3", Color) = (1,1,1,1)
        _DotTex1("Dotted Tex", 2D) = "black"{}
        _DotTex2("Dotted Tex 2", 2D) = "black"{}
        _DotTex3("Dotted Tex 3", 2D) = "black"{}
        _Freq1("Frequency 1", float) = 1.0
        _Freq2("Frequency 2", float) = 0.5
        _Freq3("Frequency 3", float) = 2.0
    }
   
    SubShader {
        Tags { "Queue" = "Geometry" }
        
        Pass {
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
   
            uniform vec4 _MainColor;
            uniform vec4 _Color1;
            uniform vec4 _Color2;
            uniform vec4 _Color3;
            uniform sampler2D _DotTex1;
            uniform sampler2D _DotTex2;
            uniform sampler2D _DotTex3;
            uniform float _Freq1;
            uniform float _Freq2;
            uniform float _Freq3;
            uniform float _Time;
            varying vec2 TextureCoordinate;
            
            const float PI = 3.14159f;


            float wave1(float x, float freq){
                return (0.5f * sin(freq * x) + 0.5f);
            }


            float wave1_ceiled_derivate(float x, float freq){
                return float(ceil(cos(freq * x)));
            }


            vec2 rotated2D(vec2 v, float angle){
                return vec2(v.x * cos(angle) - v.y * sin(angle), v.x * sin(angle) + v.y * cos(angle));
            }


            float radial_expansion(sampler2D tex, vec2 r){
                float val = 0.0f;
                for(int i = 0; i < 360 && val == 0.0f; i++){
                    float angle = (float(i) / 360.0f) * 2.0f * PI;
                    vec2 v = rotated2D(r, angle);
                    val += texture2D(tex, TextureCoordinate + v).r;
                }
                if(val > 0.0f){
                    val = 1.0f;
                }
                return val;
            }


            void main()
            {
                float freq1 = 100.0f * _Freq1;
                vec2 r1 = wave1_ceiled_derivate(_Time, freq1) * wave1(_Time, freq1) * vec2(0.1f, 0.0f);
                float val1 = radial_expansion(_DotTex1, r1);

                float freq2 = 100.0f * _Freq2;
                vec2 r2 = wave1_ceiled_derivate(_Time + 0.5f, freq2) * wave1(_Time + 0.5f, freq2) * vec2(0.05f, 0.0f);
                float val2 = radial_expansion(_DotTex2, r2);

                float freq3 = 100.0f * _Freq3;
                vec2 r3 = wave1_ceiled_derivate(_Time + 0.25f, freq3) * wave1(_Time + 0.25f, freq3) * vec2(0.15f, 0.0f);
                float val3 = radial_expansion(_DotTex3, r3);

                // Ifs im Shader, Hat keiner gesehen...
                vec3 col = _MainColor.rgb;
                if(val1 > 0.0f){
                    col = mix(_Color1.rgb, _MainColor.rgb, wave1(_Time, freq1));
                }
                if(val2 > 0.0f){
                    col = mix(_Color2.rgb, col, wave1(_Time + 0.5f, freq2));
                }
                if(val3 > 0.0f){
                    col = mix(_Color3.rgb, col, wave1(_Time + 0.25f, freq3));
                }

                gl_FragColor = vec4(col, 1.0f);
            }
            
            #endif
            
            ENDGLSL
        }
    }
}