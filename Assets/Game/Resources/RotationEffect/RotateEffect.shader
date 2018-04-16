// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Shader created with Shader Forge v1.32 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.32;sub:START;pass:START;ps:flbk:,iptp:1,cusa:True,bamd:0,lico:1,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:True,tesm:0,olmd:1,culm:2,bsrc:3,bdst:7,dpts:2,wrdp:False,dith:0,rfrpo:True,rfrpn:Refraction,coma:15,ufog:False,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False;n:type:ShaderForge.SFN_Final,id:1873,x:33229,y:32719,varname:node_1873,prsc:2|emission-9287-OUT,alpha-603-OUT;n:type:ShaderForge.SFN_Color,id:5983,x:32551,y:32923,ptovrint:False,ptlb:Color,ptin:_Color,varname:_Color,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.9485294,c2:0.1534386,c3:0.1534386,c4:0.666;n:type:ShaderForge.SFN_VertexColor,id:5376,x:32551,y:33079,varname:node_5376,prsc:2;n:type:ShaderForge.SFN_Multiply,id:603,x:32812,y:32992,cmnt:A,varname:node_603,prsc:2|A-6397-A,B-5983-A,C-5376-A,D-5355-A;n:type:ShaderForge.SFN_Rotator,id:8516,x:32143,y:32768,varname:node_8516,prsc:2|UVIN-6668-UVOUT,SPD-3436-OUT;n:type:ShaderForge.SFN_TexCoord,id:6668,x:31788,y:32802,varname:node_6668,prsc:2,uv:0;n:type:ShaderForge.SFN_Multiply,id:9287,x:32775,y:32743,varname:node_9287,prsc:2|A-6397-RGB,B-5983-RGB,C-5355-RGB;n:type:ShaderForge.SFN_Slider,id:3436,x:31979,y:32933,ptovrint:False,ptlb:Speed_1,ptin:_Speed_1,varname:_Speed_1,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:-1,cur:0,max:1;n:type:ShaderForge.SFN_Rotator,id:69,x:32186,y:33219,varname:node_69,prsc:2|UVIN-6668-UVOUT,SPD-1762-OUT;n:type:ShaderForge.SFN_Slider,id:1762,x:31847,y:33351,ptovrint:False,ptlb:Speed_2,ptin:_Speed_2,varname:_Speed_2,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:-1,cur:0.05982902,max:1;n:type:ShaderForge.SFN_Tex2d,id:5355,x:32567,y:33241,ptovrint:False,ptlb:MainTex2,ptin:_MainTex2,varname:_MainTex2,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:70ac20a40ba28dc4c9acfc284d9b57f8,ntxv:0,isnm:False|UVIN-6655-OUT;n:type:ShaderForge.SFN_Tex2d,id:3576,x:32015,y:33066,ptovrint:False,ptlb:DistoreTex,ptin:_DistoreTex,varname:_DistoreTex,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:89e1b1c005d29cf4598ea861deb35a80,ntxv:3,isnm:True|UVIN-1166-UVOUT;n:type:ShaderForge.SFN_Panner,id:1166,x:31878,y:33066,varname:node_1166,prsc:2,spu:0.2,spv:0|UVIN-6668-UVOUT;n:type:ShaderForge.SFN_Lerp,id:9126,x:32357,y:32783,varname:node_9126,prsc:2|A-8516-UVOUT,B-6452-OUT,T-2570-OUT;n:type:ShaderForge.SFN_Slider,id:2570,x:32200,y:33015,ptovrint:False,ptlb:Distore,ptin:_Distore,varname:_Distore,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0.02564103,max:1;n:type:ShaderForge.SFN_Append,id:6452,x:32200,y:33066,varname:node_6452,prsc:2|A-3576-R,B-3576-G;n:type:ShaderForge.SFN_Lerp,id:6655,x:32402,y:33209,varname:node_6655,prsc:2|A-69-UVOUT,B-6452-OUT,T-2570-OUT;n:type:ShaderForge.SFN_Tex2d,id:6397,x:32559,y:32636,ptovrint:False,ptlb:MainTex,ptin:_MainTex,varname:_MainTex,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False|UVIN-9126-OUT;proporder:5983-3436-1762-5355-3576-2570-6397;pass:END;sub:END;*/

Shader "Shader Forge/RotateEffect" {
    Properties {
        _Color ("Color", Color) = (0.9485294,0.1534386,0.1534386,0.666)
        _Speed_1 ("Speed_1", Range(-1, 1)) = 0
        _Speed_2 ("Speed_2", Range(-1, 1)) = 0.05982902
        _MainTex2 ("MainTex2", 2D) = "white" {}
        _DistoreTex ("DistoreTex", 2D) = "bump" {}
        _Distore ("Distore", Range(0, 1)) = 0.02564103
        _MainTex ("MainTex", 2D) = "white" {}
        [HideInInspector]_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
        [MaterialToggle] PixelSnap ("Pixel snap", Float) = 0
    }
    SubShader {
        Tags {
            "IgnoreProjector"="True"
            "Queue"="Transparent"
            "RenderType"="Transparent"
            "CanUseSpriteAtlas"="True"
            "PreviewType"="Plane"
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
            #pragma multi_compile _ PIXELSNAP_ON
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase
            #pragma only_renderers d3d9 d3d11 glcore gles 
            #pragma target 3.0
            uniform float4 _TimeEditor;
            uniform float4 _Color;
            uniform float _Speed_1;
            uniform float _Speed_2;
            uniform sampler2D _MainTex2; uniform float4 _MainTex2_ST;
            uniform sampler2D _DistoreTex; uniform float4 _DistoreTex_ST;
            uniform float _Distore;
            uniform sampler2D _MainTex; uniform float4 _MainTex_ST;
            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
                float4 vertexColor : COLOR;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 vertexColor : COLOR;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.vertexColor = v.vertexColor;
                o.pos = UnityObjectToClipPos(v.vertex );
                #ifdef PIXELSNAP_ON
                    o.pos = UnityPixelSnap(o.pos);
                #endif
                return o;
            }
            float4 frag(VertexOutput i, float facing : VFACE) : COLOR {
                float isFrontFace = ( facing >= 0 ? 1 : 0 );
                float faceSign = ( facing >= 0 ? 1 : -1 );
////// Lighting:
////// Emissive:
                float4 node_6538 = _Time + _TimeEditor;
                float node_8516_ang = node_6538.g;
                float node_8516_spd = _Speed_1;
                float node_8516_cos = cos(node_8516_spd*node_8516_ang);
                float node_8516_sin = sin(node_8516_spd*node_8516_ang);
                float2 node_8516_piv = float2(0.5,0.5);
                float2 node_8516 = (mul(i.uv0-node_8516_piv,float2x2( node_8516_cos, -node_8516_sin, node_8516_sin, node_8516_cos))+node_8516_piv);
                float2 node_1166 = (i.uv0+node_6538.g*float2(0.2,0));
                float3 _DistoreTex_var = UnpackNormal(tex2D(_DistoreTex,TRANSFORM_TEX(node_1166, _DistoreTex)));
                float2 node_6452 = float2(_DistoreTex_var.r,_DistoreTex_var.g);
                float2 node_9126 = lerp(node_8516,node_6452,_Distore);
                float4 _MainTex_var = tex2D(_MainTex,TRANSFORM_TEX(node_9126, _MainTex));
                float node_69_ang = node_6538.g;
                float node_69_spd = _Speed_2;
                float node_69_cos = cos(node_69_spd*node_69_ang);
                float node_69_sin = sin(node_69_spd*node_69_ang);
                float2 node_69_piv = float2(0.5,0.5);
                float2 node_69 = (mul(i.uv0-node_69_piv,float2x2( node_69_cos, -node_69_sin, node_69_sin, node_69_cos))+node_69_piv);
                float2 node_6655 = lerp(node_69,node_6452,_Distore);
                float4 _MainTex2_var = tex2D(_MainTex2,TRANSFORM_TEX(node_6655, _MainTex2));
                float3 emissive = (_MainTex_var.rgb*_Color.rgb*_MainTex2_var.rgb);
                float3 finalColor = emissive;
                return fixed4(finalColor,(_MainTex_var.a*_Color.a*i.vertexColor.a*_MainTex2_var.a));
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
