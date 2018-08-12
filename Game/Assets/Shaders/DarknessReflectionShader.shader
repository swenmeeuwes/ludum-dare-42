Shader "Camera/DarknessReflectionShader"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {} // Screen
		_DarknessColor ("Darkness color", Color) = (1, 1, 1, 1)
		_Opacity ("Opacity", Range(0, 1)) = 0.5
		_Darkness("Darkness", Range(0, 1)) = 0.5
	}
	SubShader
	{
		// No culling or depth
		Cull Off ZWrite Off ZTest Always

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}
			
			sampler2D _MainTex;
			float4 _DarknessColor;			
			float _Opacity;
			float _Darkness;

			fixed4 frag (v2f i) : SV_Target
			{
				float4 col = tex2D(_MainTex, i.uv);

				col = lerp(col, _DarknessColor, _Darkness * _Opacity);
				
				return col;
			}
			ENDCG
		}
	}
}
