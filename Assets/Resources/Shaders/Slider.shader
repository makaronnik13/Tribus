// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Shader created with Shader Forge v1.32 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.32;sub:START;pass:START;ps:flbk:,iptp:1,cusa:True,bamd:0,lico:1,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:True,tesm:0,olmd:1,culm:2,bsrc:3,bdst:7,dpts:2,wrdp:False,dith:0,rfrpo:True,rfrpn:Refraction,coma:15,ufog:False,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False;n:type:ShaderForge.SFN_Final,id:1873,x:34066,y:32640,varname:node_1873,prsc:2|emission-1800-OUT,alpha-4054-OUT;n:type:ShaderForge.SFN_Tex2d,id:4805,x:32651,y:32627,ptovrint:False,ptlb:MainTex,ptin:_MainTex,varname:_MainTex_copy,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:True,tagnsco:False,tagnrm:False,tex:a05584e00fb7ff147bc0670e8cc05402,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Multiply,id:1086,x:32939,y:32734,cmnt:RGB,varname:node_1086,prsc:2|A-4805-RGB,B-8988-OUT,C-4922-OUT,D-4805-A;n:type:ShaderForge.SFN_Color,id:5983,x:33056,y:33385,ptovrint:False,ptlb:Color,ptin:_Color,varname:_Color_copy,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:1,c2:1,c3:1,c4:1;n:type:ShaderForge.SFN_VertexColor,id:5376,x:33056,y:33534,varname:node_5376,prsc:2;n:type:ShaderForge.SFN_Slider,id:5115,x:31153,y:33241,ptovrint:False,ptlb:Value,ptin:_Value,varname:node_5115,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0.6265076,max:1;n:type:ShaderForge.SFN_Slider,id:976,x:31169,y:33342,ptovrint:False,ptlb:LeftOffset,ptin:_LeftOffset,varname:node_976,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:-1,cur:-0.04281434,max:1;n:type:ShaderForge.SFN_Slider,id:7292,x:31169,y:33432,ptovrint:False,ptlb:RightOffset,ptin:_RightOffset,varname:node_7292,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:-1,cur:0.01266506,max:1;n:type:ShaderForge.SFN_Add,id:6466,x:31659,y:33214,varname:node_6466,prsc:2|A-5115-OUT,B-976-OUT;n:type:ShaderForge.SFN_RemapRangeAdvanced,id:2080,x:31954,y:33268,varname:node_2080,prsc:2|IN-6466-OUT,IMIN-8131-OUT,IMAX-7730-OUT,OMIN-976-OUT,OMAX-37-OUT;n:type:ShaderForge.SFN_Subtract,id:37,x:31718,y:33481,varname:node_37,prsc:2|A-2950-OUT,B-2150-OUT;n:type:ShaderForge.SFN_Vector1,id:2950,x:31544,y:33525,varname:node_2950,prsc:2,v1:1;n:type:ShaderForge.SFN_Add,id:2150,x:31544,y:33377,varname:node_2150,prsc:2|A-976-OUT,B-7292-OUT;n:type:ShaderForge.SFN_Vector1,id:8131,x:31766,y:33286,varname:node_8131,prsc:2,v1:0;n:type:ShaderForge.SFN_Vector1,id:7730,x:31766,y:33335,varname:node_7730,prsc:2,v1:1;n:type:ShaderForge.SFN_Set,id:6493,x:32353,y:33260,varname:FadeValue,prsc:2|IN-8168-OUT;n:type:ShaderForge.SFN_Set,id:8778,x:33383,y:32089,varname:Fade,prsc:2|IN-8573-OUT;n:type:ShaderForge.SFN_Tex2d,id:2384,x:33162,y:32413,ptovrint:False,ptlb:Edge,ptin:_Edge,varname:node_2384,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:0000000000000000f000000000000000,ntxv:2,isnm:False|UVIN-1255-OUT;n:type:ShaderForge.SFN_TexCoord,id:5484,x:31848,y:32420,varname:node_5484,prsc:2,uv:0;n:type:ShaderForge.SFN_Append,id:1255,x:33008,y:32413,varname:node_1255,prsc:2|A-4340-OUT,B-5484-V;n:type:ShaderForge.SFN_Slider,id:2835,x:31430,y:32321,ptovrint:False,ptlb:EdgeWidth,ptin:_EdgeWidth,varname:node_2835,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0.03376705,max:1;n:type:ShaderForge.SFN_Subtract,id:7239,x:32396,y:32277,varname:node_7239,prsc:2|A-5484-U,B-7939-OUT;n:type:ShaderForge.SFN_Get,id:8464,x:31901,y:32201,varname:node_8464,prsc:2|IN-6493-OUT;n:type:ShaderForge.SFN_Subtract,id:7939,x:32166,y:32179,varname:node_7939,prsc:2|A-8464-OUT,B-2753-OUT;n:type:ShaderForge.SFN_Multiply,id:2753,x:31969,y:32267,varname:node_2753,prsc:2|A-2835-OUT,B-7223-OUT;n:type:ShaderForge.SFN_Vector1,id:7223,x:31693,y:32420,varname:node_7223,prsc:2,v1:0.5;n:type:ShaderForge.SFN_Get,id:5664,x:31933,y:33411,varname:node_5664,prsc:2|IN-9841-OUT;n:type:ShaderForge.SFN_Set,id:9841,x:32168,y:32328,varname:HalfEdgeWidth,prsc:2|IN-2753-OUT;n:type:ShaderForge.SFN_Add,id:8168,x:32190,y:33260,varname:node_8168,prsc:2|A-2080-OUT,B-5664-OUT;n:type:ShaderForge.SFN_Get,id:8988,x:32630,y:32788,varname:node_8988,prsc:2|IN-8778-OUT;n:type:ShaderForge.SFN_RemapRangeAdvanced,id:8812,x:32599,y:32277,varname:node_8812,prsc:2|IN-7239-OUT,IMIN-908-OUT,IMAX-8272-OUT,OMIN-908-OUT,OMAX-4294-OUT;n:type:ShaderForge.SFN_Vector1,id:908,x:32347,y:32521,varname:node_908,prsc:2,v1:0;n:type:ShaderForge.SFN_Vector1,id:4294,x:32347,y:32576,varname:node_4294,prsc:2,v1:1;n:type:ShaderForge.SFN_Multiply,id:8272,x:32168,y:32376,varname:node_8272,prsc:2|A-2835-OUT,B-2644-OUT;n:type:ShaderForge.SFN_Vector1,id:2644,x:32067,y:32522,varname:node_2644,prsc:2,v1:0.5;n:type:ShaderForge.SFN_Lerp,id:4340,x:32886,y:32293,varname:node_4340,prsc:2|A-8812-OUT,B-5801-OUT,T-7387-OUT;n:type:ShaderForge.SFN_Tex2d,id:7075,x:32602,y:31861,ptovrint:False,ptlb:EdgeDistortion,ptin:_EdgeDistortion,varname:node_7075,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:28c7aad1372ff114b90d330f8a2dd938,ntxv:3,isnm:True|UVIN-8221-UVOUT;n:type:ShaderForge.SFN_Slider,id:372,x:32523,y:31748,ptovrint:False,ptlb:DistoreValue,ptin:_DistoreValue,varname:node_372,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:5,max:5;n:type:ShaderForge.SFN_TexCoord,id:2315,x:32199,y:31757,varname:node_2315,prsc:2,uv:0;n:type:ShaderForge.SFN_Panner,id:8221,x:32422,y:31861,varname:node_8221,prsc:2,spu:-1,spv:0|UVIN-2315-UVOUT,DIST-8986-OUT;n:type:ShaderForge.SFN_Slider,id:4539,x:31813,y:31875,ptovrint:False,ptlb:DistSpeed,ptin:_DistSpeed,varname:node_4539,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0.09084379,max:1;n:type:ShaderForge.SFN_Multiply,id:8986,x:32199,y:31913,varname:node_8986,prsc:2|A-4539-OUT,B-7047-T;n:type:ShaderForge.SFN_Time,id:7047,x:32025,y:31949,varname:node_7047,prsc:2;n:type:ShaderForge.SFN_Color,id:8965,x:33162,y:32243,ptovrint:False,ptlb:EdgeColor,ptin:_EdgeColor,varname:node_8965,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.5310346,c2:1,c3:0,c4:1;n:type:ShaderForge.SFN_Multiply,id:4521,x:33360,y:32316,varname:node_4521,prsc:2|A-8965-RGB,B-2384-RGB;n:type:ShaderForge.SFN_Set,id:4313,x:33379,y:33385,varname:ColorModificator,prsc:2|IN-751-OUT;n:type:ShaderForge.SFN_Set,id:5836,x:33379,y:33534,varname:AlphaModificator,prsc:2|IN-1260-OUT;n:type:ShaderForge.SFN_Multiply,id:751,x:33234,y:33385,varname:node_751,prsc:2|A-5983-RGB,B-5376-RGB;n:type:ShaderForge.SFN_Multiply,id:1260,x:33234,y:33534,varname:node_1260,prsc:2|A-5983-A,B-5376-A;n:type:ShaderForge.SFN_Get,id:4922,x:32630,y:32851,varname:node_4922,prsc:2|IN-4313-OUT;n:type:ShaderForge.SFN_Multiply,id:1931,x:33133,y:32904,varname:node_1931,prsc:2|A-4805-A,B-8988-OUT,C-5968-OUT;n:type:ShaderForge.SFN_Get,id:5968,x:32630,y:32931,varname:node_5968,prsc:2|IN-5836-OUT;n:type:ShaderForge.SFN_Set,id:8877,x:33096,y:32798,varname:BaseColor,prsc:2|IN-1086-OUT;n:type:ShaderForge.SFN_Get,id:1,x:33573,y:32666,varname:node_1,prsc:2|IN-8877-OUT;n:type:ShaderForge.SFN_Multiply,id:7245,x:33549,y:32411,varname:node_7245,prsc:2|A-4521-OUT,B-2384-A;n:type:ShaderForge.SFN_Set,id:9200,x:33759,y:32411,varname:EdgeColor,prsc:2|IN-7245-OUT;n:type:ShaderForge.SFN_Get,id:351,x:33573,y:32718,varname:node_351,prsc:2|IN-9200-OUT;n:type:ShaderForge.SFN_Set,id:7362,x:32983,y:31851,varname:DistV,prsc:2|IN-4961-OUT;n:type:ShaderForge.SFN_Vector1,id:7387,x:32703,y:32423,varname:node_7387,prsc:2,v1:0.5;n:type:ShaderForge.SFN_Multiply,id:4961,x:32831,y:31851,varname:node_4961,prsc:2|A-372-OUT,B-7075-R;n:type:ShaderForge.SFN_Get,id:5801,x:32703,y:32329,varname:node_5801,prsc:2|IN-7362-OUT;n:type:ShaderForge.SFN_Add,id:4054,x:33510,y:32873,varname:node_4054,prsc:2|A-1313-OUT,B-1931-OUT;n:type:ShaderForge.SFN_Multiply,id:1313,x:33340,y:32666,varname:node_1313,prsc:2|A-2384-A,B-9013-OUT,C-5078-OUT;n:type:ShaderForge.SFN_Get,id:5078,x:33112,y:32702,varname:node_5078,prsc:2|IN-5836-OUT;n:type:ShaderForge.SFN_Round,id:9013,x:33133,y:32590,varname:node_9013,prsc:2|IN-4805-A;n:type:ShaderForge.SFN_Lerp,id:1800,x:33850,y:32640,varname:node_1800,prsc:2|A-1-OUT,B-351-OUT,T-2384-A;n:type:ShaderForge.SFN_OneMinus,id:9940,x:33057,y:32075,varname:node_9940,prsc:2|IN-4340-OUT;n:type:ShaderForge.SFN_Clamp01,id:8573,x:33199,y:32075,varname:node_8573,prsc:2|IN-9940-OUT;proporder:4805-5983-5115-976-7292-2384-2835-7075-372-4539-8965;pass:END;sub:END;*/

Shader "Tribus/Slider" {
    Properties {
        [PerRendererData]_MainTex ("MainTex", 2D) = "white" {}
        _Color ("Color", Color) = (1,1,1,1)
        _Value ("Value", Range(0, 1)) = 0.6265076
        _LeftOffset ("LeftOffset", Range(-1, 1)) = -0.04281434
        _RightOffset ("RightOffset", Range(-1, 1)) = 0.01266506
        _Edge ("Edge", 2D) = "black" {}
        _EdgeWidth ("EdgeWidth", Range(0, 10)) = 0.03376705
        _EdgeDistortion ("EdgeDistortion", 2D) = "bump" {}
        _DistoreValue ("DistoreValue", Range(0, 5)) = 5
        _DistSpeed ("DistSpeed", Range(0, 1)) = 0.09084379
        _EdgeColor ("EdgeColor", Color) = (0.5310346,1,0,1)
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
            uniform sampler2D _MainTex; uniform float4 _MainTex_ST;
            uniform float4 _Color;
            uniform float _Value;
            uniform float _LeftOffset;
            uniform float _RightOffset;
            uniform sampler2D _Edge; uniform float4 _Edge_ST;
            uniform float _EdgeWidth;
            uniform sampler2D _EdgeDistortion; uniform float4 _EdgeDistortion_ST;
            uniform float _DistoreValue;
            uniform float _DistSpeed;
            uniform float4 _EdgeColor;
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
                float4 _MainTex_var = tex2D(_MainTex,TRANSFORM_TEX(i.uv0, _MainTex));
                float node_8131 = 0.0;
                float node_2753 = (_EdgeWidth*0.5);
                float HalfEdgeWidth = node_2753;
                float FadeValue = ((_LeftOffset + ( ((_Value+_LeftOffset) - node_8131) * ((1.0-(_LeftOffset+_RightOffset)) - _LeftOffset) ) / (1.0 - node_8131))+HalfEdgeWidth);
                float node_908 = 0.0;
                float4 node_7047 = _Time + _TimeEditor;
                float2 node_8221 = (i.uv0+(_DistSpeed*node_7047.g)*float2(-1,0));
                float3 _EdgeDistortion_var = UnpackNormal(tex2D(_EdgeDistortion,TRANSFORM_TEX(node_8221, _EdgeDistortion)));
                float DistV = (_DistoreValue*_EdgeDistortion_var.r);
                float node_4340 = lerp((node_908 + ( ((i.uv0.r-(FadeValue-node_2753)) - node_908) * (1.0 - node_908) ) / ((_EdgeWidth*0.5) - node_908)),DistV,0.5);
                float node_9940 = (1.0 - node_4340);
                float Fade = saturate(node_9940);
                float node_8988 = Fade;
                float3 ColorModificator = (_Color.rgb*i.vertexColor.rgb);
                float3 node_1086 = (_MainTex_var.rgb*node_8988*ColorModificator*_MainTex_var.a); // RGB
                float3 BaseColor = node_1086;
                float3 node_1 = BaseColor;
                float2 node_1255 = float2(node_4340,i.uv0.g);
                float4 _Edge_var = tex2D(_Edge,TRANSFORM_TEX(node_1255, _Edge));
                float3 node_7245 = ((_EdgeColor.rgb*_Edge_var.rgb)*_Edge_var.a);
                float3 EdgeColor = node_7245;
                float3 node_351 = EdgeColor*_EdgeColor.a;
                float3 emissive = lerp(node_1,node_351,_Edge_var.a);
                float3 finalColor = emissive;
                float AlphaModificator = (_Color.a*i.vertexColor.a);
                return fixed4(finalColor,((_Edge_var.a*round(_MainTex_var.a)*AlphaModificator)+(_MainTex_var.a*node_8988*AlphaModificator)));
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
