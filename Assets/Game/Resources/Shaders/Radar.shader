// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Shader created with Shader Forge v1.32 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.32;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,lico:1,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:2,bsrc:3,bdst:7,dpts:2,wrdp:False,dith:0,rfrpo:True,rfrpn:Refraction,coma:15,ufog:False,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False;n:type:ShaderForge.SFN_Final,id:3138,x:33616,y:32681,varname:node_3138,prsc:2|emission-7219-OUT,alpha-1984-OUT,clip-34-OUT;n:type:ShaderForge.SFN_Color,id:7241,x:32799,y:32621,ptovrint:False,ptlb:Color,ptin:_Color,varname:node_7241,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0,c2:1,c3:0.7103448,c4:1;n:type:ShaderForge.SFN_Slider,id:5872,x:31486,y:32540,ptovrint:False,ptlb:speed,ptin:_speed,varname:node_5872,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0.1446393,max:1;n:type:ShaderForge.SFN_Slider,id:3030,x:32074,y:32612,ptovrint:False,ptlb:frequency,ptin:_frequency,varname:node_3030,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0.974359,max:2;n:type:ShaderForge.SFN_Slider,id:3844,x:31681,y:33266,ptovrint:False,ptlb:angle,ptin:_angle,varname:node_3844,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:1,max:1;n:type:ShaderForge.SFN_TexCoord,id:7755,x:31552,y:33084,varname:node_7755,prsc:2,uv:0;n:type:ShaderForge.SFN_RemapRange,id:3458,x:31706,y:33084,varname:node_3458,prsc:2,frmn:0,frmx:1,tomn:-1,tomx:1|IN-7755-UVOUT;n:type:ShaderForge.SFN_ComponentMask,id:8846,x:31889,y:33084,varname:node_8846,prsc:2,cc1:0,cc2:1,cc3:-1,cc4:-1|IN-3458-OUT;n:type:ShaderForge.SFN_ArcTan2,id:4358,x:32344,y:33524,varname:node_4358,prsc:2,attp:0|A-8846-R,B-8846-G;n:type:ShaderForge.SFN_Add,id:34,x:33195,y:33500,varname:node_34,prsc:2|A-4358-OUT,B-684-OUT;n:type:ShaderForge.SFN_RemapRangeAdvanced,id:684,x:32148,y:33684,varname:node_684,prsc:2|IN-3844-OUT,IMIN-6213-OUT,IMAX-7491-OUT,OMIN-1226-OUT,OMAX-1179-OUT;n:type:ShaderForge.SFN_Pi,id:1179,x:31681,y:33459,varname:node_1179,prsc:2;n:type:ShaderForge.SFN_Negate,id:1226,x:31838,y:33490,varname:node_1226,prsc:2|IN-1179-OUT;n:type:ShaderForge.SFN_Vector1,id:6213,x:31838,y:33352,varname:node_6213,prsc:2,v1:0;n:type:ShaderForge.SFN_Vector1,id:7491,x:31838,y:33424,varname:node_7491,prsc:2,v1:0.9;n:type:ShaderForge.SFN_Multiply,id:9723,x:31889,y:32947,varname:node_9723,prsc:2|A-3458-OUT,B-3458-OUT;n:type:ShaderForge.SFN_Add,id:1123,x:32198,y:32967,varname:node_1123,prsc:2|A-1244-R,B-1244-G;n:type:ShaderForge.SFN_ComponentMask,id:1244,x:32041,y:32947,varname:node_1244,prsc:2,cc1:0,cc2:1,cc3:-1,cc4:-1|IN-9723-OUT;n:type:ShaderForge.SFN_Sqrt,id:7169,x:32378,y:32905,varname:node_7169,prsc:2|IN-1123-OUT;n:type:ShaderForge.SFN_Multiply,id:790,x:32559,y:33025,varname:node_790,prsc:2|A-7169-OUT,B-1179-OUT,C-4810-OUT;n:type:ShaderForge.SFN_Sin,id:4606,x:32799,y:32802,varname:node_4606,prsc:2|IN-7320-OUT;n:type:ShaderForge.SFN_RemapRange,id:4810,x:32424,y:32602,varname:node_4810,prsc:2,frmn:0,frmx:1,tomn:0.5,tomx:30|IN-3030-OUT;n:type:ShaderForge.SFN_Add,id:7320,x:32651,y:32802,varname:node_7320,prsc:2|A-8992-OUT,B-790-OUT;n:type:ShaderForge.SFN_Time,id:1468,x:31616,y:32678,varname:node_1468,prsc:2;n:type:ShaderForge.SFN_Multiply,id:8992,x:31946,y:32676,varname:node_8992,prsc:2|A-8363-OUT,B-1468-TTR;n:type:ShaderForge.SFN_RemapRange,id:8363,x:31797,y:32540,varname:node_8363,prsc:2,frmn:0,frmx:1,tomn:0,tomx:-10|IN-5872-OUT;n:type:ShaderForge.SFN_OneMinus,id:445,x:32738,y:33070,varname:node_445,prsc:2|IN-7169-OUT;n:type:ShaderForge.SFN_Multiply,id:785,x:33054,y:32950,varname:node_785,prsc:2|A-4606-OUT,B-4507-OUT;n:type:ShaderForge.SFN_Slider,id:601,x:32587,y:33444,ptovrint:False,ptlb:FadeDistance,ptin:_FadeDistance,varname:node_601,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0.1367521,max:1;n:type:ShaderForge.SFN_Power,id:4507,x:32864,y:32943,varname:node_4507,prsc:2|VAL-445-OUT,EXP-7046-OUT;n:type:ShaderForge.SFN_RemapRange,id:7046,x:32935,y:33461,varname:node_7046,prsc:2,frmn:0,frmx:1,tomn:0.01,tomx:25|IN-601-OUT;n:type:ShaderForge.SFN_Power,id:5038,x:32624,y:33291,varname:node_5038,prsc:2|VAL-445-OUT,EXP-6966-OUT;n:type:ShaderForge.SFN_Slider,id:6966,x:32295,y:33291,ptovrint:False,ptlb:gradient,ptin:_gradient,varname:node_6966,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:1,cur:2.094017,max:5;n:type:ShaderForge.SFN_Add,id:5809,x:33219,y:33103,varname:node_5809,prsc:2|A-3689-OUT,B-5947-OUT;n:type:ShaderForge.SFN_Clamp01,id:5947,x:33219,y:32950,varname:node_5947,prsc:2|IN-785-OUT;n:type:ShaderForge.SFN_Multiply,id:9699,x:33152,y:32832,varname:node_9699,prsc:2|A-7241-RGB,B-4606-OUT;n:type:ShaderForge.SFN_Add,id:2107,x:33361,y:32787,varname:node_2107,prsc:2|A-481-OUT,B-9699-OUT;n:type:ShaderForge.SFN_Multiply,id:481,x:33152,y:32694,varname:node_481,prsc:2|A-7219-OUT,B-3689-OUT;n:type:ShaderForge.SFN_Sqrt,id:7219,x:33004,y:32559,varname:node_7219,prsc:2|IN-7241-RGB;n:type:ShaderForge.SFN_Divide,id:3689,x:33011,y:33291,varname:node_3689,prsc:2|A-5038-OUT,B-7046-OUT;n:type:ShaderForge.SFN_Multiply,id:1984,x:33385,y:33052,varname:node_1984,prsc:2|A-7241-A,B-5809-OUT;proporder:7241-3844-3030-5872-601-6966;pass:END;sub:END;*/

Shader "CarEffects/Radar" {
    Properties {
        _Color ("Color", Color) = (0,1,0.7103448,1)
        _angle ("angle", Range(0, 1)) = 1
        _frequency ("frequency", Range(0, 2)) = 0.974359
        _speed ("speed", Range(0, 1)) = 0.1446393
        _FadeDistance ("FadeDistance", Range(0, 1)) = 0.1367521
        _gradient ("gradient", Range(1, 5)) = 2.094017
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
            uniform float _angle;
            uniform float _FadeDistance;
            uniform float _gradient;
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
                float2 node_3458 = (i.uv0*2.0+-1.0);
                float2 node_8846 = node_3458.rg;
                float node_6213 = 0.0;
                float node_1179 = 3.141592654;
                float node_1226 = (-1*node_1179);
                clip((atan2(node_8846.r,node_8846.g)+(node_1226 + ( (_angle - node_6213) * (node_1179 - node_1226) ) / (0.9 - node_6213))) - 0.5);
////// Lighting:
////// Emissive:
                float3 node_7219 = sqrt(_Color.rgb);
                float3 emissive = node_7219;
                float3 finalColor = emissive;
                float2 node_1244 = (node_3458*node_3458).rg;
                float node_7169 = sqrt((node_1244.r+node_1244.g));
                float node_445 = (1.0 - node_7169);
                float node_7046 = (_FadeDistance*24.99+0.01);
                float node_3689 = (pow(node_445,_gradient)/node_7046);
                float4 node_1468 = _Time + _TimeEditor;
                float node_4606 = sin((((_speed*-10.0+0.0)*node_1468.a)+(node_7169*node_1179*(_frequency*29.5+0.5))));
                return fixed4(finalColor,(_Color.a*(node_3689+saturate((node_4606*pow(node_445,node_7046))))));
            }
            ENDCG
        }
        Pass {
            Name "ShadowCaster"
            Tags {
                "LightMode"="ShadowCaster"
            }
            Offset 1, 1
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_SHADOWCASTER
            #include "UnityCG.cginc"
            #include "Lighting.cginc"
            #pragma fragmentoption ARB_precision_hint_fastest
            #pragma multi_compile_shadowcaster
            #pragma only_renderers d3d9 d3d11 glcore gles 
            #pragma target 3.0
            uniform float _angle;
            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                V2F_SHADOW_CASTER;
                float2 uv0 : TEXCOORD1;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.pos = UnityObjectToClipPos(v.vertex );
                TRANSFER_SHADOW_CASTER(o)
                return o;
            }
            float4 frag(VertexOutput i, float facing : VFACE) : COLOR {
                float isFrontFace = ( facing >= 0 ? 1 : 0 );
                float faceSign = ( facing >= 0 ? 1 : -1 );
                float2 node_3458 = (i.uv0*2.0+-1.0);
                float2 node_8846 = node_3458.rg;
                float node_6213 = 0.0;
                float node_1179 = 3.141592654;
                float node_1226 = (-1*node_1179);
                clip((atan2(node_8846.r,node_8846.g)+(node_1226 + ( (_angle - node_6213) * (node_1179 - node_1226) ) / (0.9 - node_6213))) - 0.5);
                SHADOW_CASTER_FRAGMENT(i)
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
