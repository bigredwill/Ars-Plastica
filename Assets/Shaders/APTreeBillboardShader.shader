﻿// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Ars Plastica/APTree Billboard Shader"
{
	Properties
	{
		_Color("Main Color", Color) = (1,1,1,1)
		_SpecColor("Specular Color", Color) = (0.5, 0.5, 0.5, 0)
		_HueVariation("Hue Variation", Color) = (1.0,0.5,0.0,0.1)
		_Shininess("Shininess", Range(0.01, 1)) = 0.078125
		_MainTex("Base (RGB)", 2D) = "white" {}
		_IllumMap("Illum Map (RGBA)", 2D) = "white" {}
		_BumpMap("Normalmap", 2D) = "bump" {}
		_Cutoff("Alpha cutoff", Range(0,1)) = 0.5
		[MaterialEnum(None,0,Fastest,1)] _WindQuality("Wind Quality", Range(0,1)) = 0
	}

	// targeting SM3.0+
	SubShader
	{
		Tags
		{
			"Queue" = "AlphaTest"
			"IgnoreProjector" = "True"
			"RenderType" = "TransparentCutout"
			"DisableBatching" = "LODFading"
		}
		LOD 400
		Cull Off

		CGPROGRAM
		#pragma surface surf Lambert vertex:SpeedTreeBillboardVert nolightmap addshadow
		#pragma target 3.0
		#pragma multi_compile __ LOD_FADE_CROSSFADE
		#pragma multi_compile __ BILLBOARD_FACE_CAMERA_POS
		#pragma shader_feature EFFECT_BUMP
		#pragma shader_feature EFFECT_HUE_VARIATION
		#define ENABLE_WIND
		#include "SpeedTreeBillboardCommonWithEmission.cginc"

		void surf(Input IN, inout SurfaceOutput OUT)
		{
			SpeedTreeFragOut o;
			SpeedTreeFrag(IN, o);
			SPEEDTREE_COPY_FRAG(OUT, o)
		}
		ENDCG

		Pass
		{
			Tags{ "LightMode" = "Vertex" }

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0
			#pragma multi_compile_fog
			#pragma multi_compile __ LOD_FADE_CROSSFADE
			#pragma multi_compile __ BILLBOARD_FACE_CAMERA_POS
			#pragma shader_feature EFFECT_HUE_VARIATION
			#define ENABLE_WIND
			#include "SpeedTreeBillboardCommonWithEmission.cginc"

			struct v2f
			{
				float4 vertex	: SV_POSITION;
				UNITY_FOG_COORDS(0)
					Input data : TEXCOORD1;
			};

			v2f vert(SpeedTreeBillboardData v)
			{
				v2f o;
				SpeedTreeBillboardVert(v, o.data);
				o.data.color.rgb *= ShadeVertexLightsFull(v.vertex, v.normal, 4, true);
				o.vertex = UnityObjectToClipPos(v.vertex);
				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				SpeedTreeFragOut o;
				SpeedTreeFrag(i.data, o);
				fixed4 c = fixed4(o.Albedo, o.Alpha);
				UNITY_APPLY_FOG(i.fogCoord, c);
				return c;
			}
			ENDCG
		}
	}

	// targeting SM2.0: Cross-fading, Hue variation and Camera-facing billboard are turned off for less instructions
	SubShader
	{
		Tags
		{
			"Queue" = "AlphaTest"
			"IgnoreProjector" = "True"
			"RenderType" = "TransparentCutout"
		}
		LOD 400
		Cull Off

		CGPROGRAM
		#pragma surface surf Lambert vertex:SpeedTreeBillboardVert nolightmap
		#pragma shader_feature EFFECT_BUMP
		#include "SpeedTreeBillboardCommonWithEmission.cginc"

		void surf(Input IN, inout SurfaceOutput OUT)
		{
			SpeedTreeFragOut o;
			SpeedTreeFrag(IN, o);
			SPEEDTREE_COPY_FRAG(OUT, o)
		}
		ENDCG

		Pass
		{
			Tags{ "LightMode" = "Vertex" }

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_fog
			#include "SpeedTreeBillboardCommonWithEmission.cginc"

			struct v2f
			{
				float4 vertex	: SV_POSITION;
				UNITY_FOG_COORDS(0)
					Input data : TEXCOORD1;
			};

			v2f vert(SpeedTreeBillboardData v)
			{
				v2f o;
				SpeedTreeBillboardVert(v, o.data);
				o.data.color.rgb *= ShadeVertexLightsFull(v.vertex, v.normal, 2, false);
				o.vertex = UnityObjectToClipPos(v.vertex);
				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				SpeedTreeFragOut o;
				SpeedTreeFrag(i.data, o);
				fixed4 c = fixed4(o.Albedo, o.Alpha);
				UNITY_APPLY_FOG(i.fogCoord, c);
				return c;
			}
			ENDCG
		}
	}

FallBack "Transparent/Cutout/VertexLit"
}