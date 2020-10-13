// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Shaders101/Basic"
{
  Properties
  {
    _MainTex("Texture", 2D) = "white"
    _MudTex("Mudkip", 2D) = "white"
  }


	SubShader
	{
		Tags
		{
			"PreviewType" = "Plane"
      "Queue" = "Transparent"
		}
		Pass
		{
      Blend SrcAlpha OneMinusSrcAlpha

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
				float4 vertex : SV_POSITION;
        float2 uv : TEXCOORD0;
			};

			v2f vert(appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
        o.uv = v.uv;
				return o;
			}

      sampler2D _MainTex;
      sampler2D _MudTex;

			float4 frag(v2f i) : SV_Target
			{
        //getting the texture, for us, the camera
				float4 color = tex2D(_MainTex, i.uv);
        color *= float4(1, 0, 1, 1);

        color *= tex2D(_MudTex, i.uv);
        color += tex2D(_MainTex, i.uv);
        //float4 color = float4(i.uv.g, 1, 1, 0);
				return color;
			}
			ENDCG
		}
	}
}
