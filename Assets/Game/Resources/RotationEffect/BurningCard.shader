// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Shader created with Shader Forge v1.32 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.32;sub:START;pass:START;ps:flbk:,iptp:1,cusa:True,bamd:0,lico:1,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:True,tesm:0,olmd:1,culm:2,bsrc:3,bdst:7,dpts:2,wrdp:False,dith:0,rfrpo:True,rfrpn:Refraction,coma:15,ufog:False,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False;n:type:ShaderForge.SFN_Final,id:1873,x:33229,y:32719,varname:node_1873,prsc:2|emission-9287-OUT,alpha-603-OUT;n:type:ShaderForge.SFN_Color,id:5983,x:32551,y:32923,ptovrint:False,ptlb:Color,ptin:_Color,varname:_Color,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:1,c2:1,c3:1,c4:0.779;n:type:ShaderForge.SFN_VertexColor,id:5376,x:32551,y:33079,varname:node_5376,prsc:2;n:type:ShaderForge.SFN_Multiply,id:603,x:32812,y:32992,cmnt:A,varname:node_603,prsc:2|A-6397-A,B-5983-A,C-5376-A,D-5355-A;n:type:ShaderForge.SFN_TexCoord,id:6668,x:31231,y:32723,varname:node_6668,prsc:2,uv:0;n:type:ShaderForge.SFN_Multiply,id:9287,x:32841,y:32739,varname:node_9287,prsc:2|A-6397-RGB,B-5983-RGB,C-5355-RGB;n:type:ShaderForge.SFN_Tex2d,id:5355,x:32456,y:33254,ptovrint:False,ptlb:MainTex2,ptin:_MainTex2,varname:_MainTex2,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:13d82a56ca8d36c45aad466a7fec54ea,ntxv:0,isnm:False|UVIN-1166-UVOUT;n:type:ShaderForge.SFN_Tex2d,id:3576,x:31749,y:33132,ptovrint:False,ptlb:DistoreTex,ptin:_DistoreTex,varname:_DistoreTex,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:89e1b1c005d29cf4598ea861deb35a80,ntxv:3,isnm:True|UVIN-1166-UVOUT;n:type:ShaderForge.SFN_Panner,id:1166,x:31309,y:33225,varname:node_1166,prsc:2,spu:0,spv:-0.2|UVIN-6668-UVOUT;n:type:ShaderForge.SFN_Lerp,id:9126,x:32026,y:32759,varname:node_9126,prsc:2|A-6668-UVOUT,B-3576-R,T-2570-OUT;n:type:ShaderForge.SFN_Slider,id:2570,x:31485,y:33032,ptovrint:False,ptlb:Distore,ptin:_Distore,varname:_Distore,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0.06912237,max:0.1;n:type:ShaderForge.SFN_Tex2d,id:6397,x:32559,y:32636,ptovrint:False,ptlb:MainTex,ptin:_MainTex,varname:_MainTex,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:70672cf8b522b0f4bb082aa529088ff3,ntxv:0,isnm:False|UVIN-9126-OUT;proporder:5983-5355-3576-2570-6397;pass:END;sub:END;*/

Shader "Shader Forge/DistortionCardLight" {
    Properties {
        _Color ("Color", Color) = (1,1,1,0.779)
        _MainTex2 ("MainTex2", 2D) = "white" {}
        _DistoreTex ("DistoreTex", 2D) = "bump" {}
        _Distore ("Distore", Range(0, 0.1)) = 0.06912237
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
                float4 node_2735 = _Time + _TimeEditor;
                float2 node_1166 = (i.uv0+node_2735.g*float2(0,-0.2));
                float3 _DistoreTex_var = UnpackNormal(tex2D(_DistoreTex,TRANSFORM_TEX(node_1166, _DistoreTex)));
                float2 node_9126 = lerp(i.uv0,float2(_DistoreTex_var.r,_DistoreTex_var.r),_Distore);
                float4 _MainTex_var = tex2D(_MainTex,TRANSFORM_TEX(node_9126, _MainTex));
                float4 _MainTex2_var = tex2D(_MainTex2,TRANSFORM_TEX(node_1166, _MainTex2));
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
