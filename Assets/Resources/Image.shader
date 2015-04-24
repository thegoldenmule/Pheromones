Shader "SpaceAnts/Image"
{
	Properties
	{
		_MainTex ("Base (RGB)", 2D) = "white" {}
	}
	
	CGINCLUDE

	#include "UnityCG.cginc"

	uniform sampler2D _MainTex;

	fixed4 frag(v2f_img v) : COLOR
	{
		return tex2D(_MainTex, v.uv);
	}

	ENDCG

	SubShader
	{
		Pass
		{
			CGPROGRAM
			
			#pragma vertex vert_img
			#pragma fragment frag

			ENDCG
		}
	}
}
