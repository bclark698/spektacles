Shader "Custom/BlurFieldOfView"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Mask ("Blur Mask", 2D) = "white" {}
        _Bloom ("Bloom (RGB)", 2D) = "black" {}
    }


    SubShader
    {
		// Blend SrcColor OneMinusSrcColor
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
			sampler2D _Mask;
			sampler2D _Bloom;

			// this function runs once for every pixel, determining what color each should be
            fixed4 frag (v2f i) : SV_Target
            {
				
                fixed4 col = tex2D(_MainTex, i.uv);
                // just invert the colors
                //col.rgb = 1 - col.rgb;
				col = 1 - col;
				half4 maskColor = tex2D(_Mask, i.uv);
				col.a = maskColor.a;
                return col;

				/*
				fixed4 color = tex2D(_MainTex, i.uv);
				//Some calculations that give you 'color'
				half4 originalColor = tex2D(_MainTex, i.uv) ;
				half4 maskColor = tex2D(_Mask, i.uv);
 
				return lerp(originalColor,color, maskColor.a);*/
            }
            ENDCG
        }
    }
}
