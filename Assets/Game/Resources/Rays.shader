// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Shader created with Shader Forge v1.32 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.32;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,lico:1,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:2,bsrc:3,bdst:7,dpts:2,wrdp:False,dith:0,rfrpo:True,rfrpn:Refraction,coma:15,ufog:False,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False;n:type:ShaderForge.SFN_Final,id:3138,x:33616,y:32681,varname:node_3138,prsc:2|emission-7219-OUT,alpha-1984-OUT;n:type:ShaderForge.SFN_Color,id:7241,x:32799,y:32621,ptovrint:False,ptlb:Color,ptin:_Color,varname:node_7241,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0,c2:1,c3:0.7103448,c4:1;n:type:ShaderForge.SFN_Slider,id:5872,x:31486,y:32540,ptovrint:False,ptlb:speed,ptin:_speed,varname:node_5872,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0.2393163,max:1;n:type:ShaderForge.SFN_Slider,id:3030,x:32073,y:32541,ptovrint:False,ptlb:frequency,ptin:_frequency,varname:node_3030,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0.6155411,max:2;n:type:ShaderForge.SFN_TexCoord,id:7755,x:31604,y:32998,varname:node_7755,prsc:2,uv:0;n:type:ShaderForge.SFN_Pi,id:1179,x:31914,y:33227,varname:node_1179,prsc:2;n:type:ShaderForge.SFN_Multiply,id:790,x:32422,y:33163,varname:node_790,prsc:2|A-7755-U,B-1179-OUT,C-4810-OUT;n:type:ShaderForge.SFN_Sin,id:4606,x:32552,y:32794,varname:node_4606,prsc:2|IN-7320-OUT;n:type:ShaderForge.SFN_RemapRange,id:4810,x:32477,y:32608,varname:node_4810,prsc:2,frmn:0,frmx:1,tomn:0.5,tomx:30|IN-3030-OUT;n:type:ShaderForge.SFN_Add,id:7320,x:32247,y:32760,varname:node_7320,prsc:2|A-8992-OUT,B-790-OUT,C-1468-TTR,D-3726-OUT;n:type:ShaderForge.SFN_Time,id:1468,x:31564,y:32699,varname:node_1468,prsc:2;n:type:ShaderForge.SFN_Multiply,id:8992,x:32024,y:32635,varname:node_8992,prsc:2|A-8363-OUT,B-1468-TTR;n:type:ShaderForge.SFN_RemapRange,id:8363,x:31823,y:32518,varname:node_8363,prsc:2,frmn:0,frmx:1,tomn:0,tomx:-10|IN-5872-OUT;n:type:ShaderForge.SFN_Multiply,id:785,x:33054,y:32950,varname:node_785,prsc:2|A-4606-OUT,B-4507-OUT;n:type:ShaderForge.SFN_Slider,id:601,x:32500,y:33500,ptovrint:False,ptlb:FadeDistance,ptin:_FadeDistance,varname:node_601,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:1.062215,max:3;n:type:ShaderForge.SFN_Power,id:4507,x:32882,y:33045,varname:node_4507,prsc:2|VAL-1773-OUT,EXP-7046-OUT;n:type:ShaderForge.SFN_RemapRange,id:7046,x:32684,y:33294,varname:node_7046,prsc:2,frmn:0,frmx:1,tomn:0.01,tomx:25|IN-601-OUT;n:type:ShaderForge.SFN_Clamp01,id:5947,x:33219,y:32950,varname:node_5947,prsc:2|IN-785-OUT;n:type:ShaderForge.SFN_Multiply,id:9699,x:33152,y:32832,varname:node_9699,prsc:2|A-7241-RGB,B-4606-OUT;n:type:ShaderForge.SFN_Add,id:2107,x:33361,y:32787,varname:node_2107,prsc:2|A-481-OUT,B-9699-OUT;n:type:ShaderForge.SFN_Multiply,id:481,x:33152,y:32694,varname:node_481,prsc:2|A-7219-OUT;n:type:ShaderForge.SFN_Sqrt,id:7219,x:33004,y:32559,varname:node_7219,prsc:2|IN-7241-RGB;n:type:ShaderForge.SFN_Multiply,id:1984,x:33438,y:33227,varname:node_1984,prsc:2|A-7241-A,B-5947-OUT;n:type:ShaderForge.SFN_Multiply,id:7519,x:31914,y:33023,varname:node_7519,prsc:2|A-7755-V,B-1179-OUT;n:type:ShaderForge.SFN_Sin,id:1773,x:32585,y:33034,varname:node_1773,prsc:2|IN-7519-OUT;n:type:ShaderForge.SFN_Multiply,id:3726,x:31900,y:32831,varname:node_3726,prsc:2|A-1468-T,B-1638-OUT;n:type:ShaderForge.SFN_Multiply,id:1638,x:31681,y:32870,varname:node_1638,prsc:2|A-6447-OUT,B-1773-OUT;n:type:ShaderForge.SFN_Slider,id:6447,x:31333,y:32869,ptovrint:False,ptlb: Curving,ptin:_Curving,varname:node_6447,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:-0.02820513,max:-0.3;proporder:7241-3030-5872-601-6447;pass:END;sub:END;*/

Shader "Shader Forge/Rays" {
    Properties {
        _Color ("Color", Color) = (0,1,0.7103448,1)
        _frequency ("frequency", Range(0, 2)) = 0.6155411
        _speed ("speed", Range(0, 1)) = 0.2393163
        _FadeDistance ("FadeDistance", Range(0, 3)) = 1.062215
        _Curving (" Curving", Range(0, -0.3)) = -0.02820513
        [HideInInspector]_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
    }
    SubShader {
        Tags {
            "IgnoreProjector"="True"
            "Queue"="Transparent"
            "RenderType"="Transparent"
        }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            Blend SrcAlpha OneMinusSrcAlpha
            Cull Off
            ZWrite Off
			ZTest Always

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase
            #pragma only_renderers d3d9 d3d11 glcore gles 
            #pragma target 3.0
            uniform float4 _TimeEditor;
            uniform float4 _Color;
            uniform float _speed;
            uniform float _frequency;
            uniform float _FadeDistance;
            uniform float _Curving;
            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.pos = UnityObjectToClipPos(v.vertex );
                return o;
            }
            float4 frag(VertexOutput i, float facing : VFACE) : COLOR {
                float isFrontFace = ( facing >= 0 ? 1 : 0 );
                float faceSign = ( facing >= 0 ? 1 : -1 );
////// Lighting:
////// Emissive:
                float3 node_7219 = sqrt(_Color.rgb);
                float3 emissive = node_7219;
                float3 finalColor = emissive;
                float4 node_1468 = _Time + _TimeEditor;
                float node_1179 = 3.141592654;
                float node_1773 = sin((i.uv0.g*node_1179));
                float node_4606 = sin((((_speed*-10.0+0.0)*node_1468.a)+(i.uv0.r*node_1179*(_frequency*29.5+0.5))+node_1468.a+(node_1468.g*(_Curving*node_1773))));
                return fixed4(finalColor,(_Color.a*saturate((node_4606*pow(node_1773,(_FadeDistance*24.99+0.01))))));
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
