//blur code from https://www.ronja-tutorials.com/2018/08/27/postprocessing-blur.html
//

Shader "Custom/BlurFieldOfView"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Mask ("Blur Mask", 2D) = "white" {}
      //  _Bloom ("Bloom (RGB)", 2D) = "black" {}
      _BlurSize("Blur Size", Range(0,0.5)) = 0
  		[KeywordEnum(Low, Medium, High)] _Samples ("Sample amount", Float) = 0
  		[Toggle(GAUSS)] _Gauss ("Gaussian Blur", float) = 0
  		[PowerSlider(3)]_StandardDeviation("Standard Deviation (Gauss only)", Range(0.00, 0.3)) = 0.02
    }


    SubShader
    {
		    // Blend SrcColor OneMinusSrcColor
        // No culling or depth
        Cull Off ZWrite Off ZTest Always
        Tags
        {
          "Queue" = "Transparent"
        }
        Pass
        {
          Blend SrcAlpha OneMinusSrcAlpha
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #pragma multi_compile _SAMPLES_LOW _SAMPLES_MEDIUM _SAMPLES_HIGH
			      #pragma shader_feature GAUSS

            float _BlurSize;
			      float _StandardDeviation;

            #include "UnityCG.cginc"

            #define PI 3.14159265359
			      #define E 2.71828182846

            #if _SAMPLES_LOW
			         #define SAMPLES 10
		        #elif _SAMPLES_MEDIUM
		          #define SAMPLES 30
	          #else
			         #define SAMPLES 100
	          #endif

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
      float4 _MainTex_TexelSize;

      float4 box(sampler2D tex, float2 uv, float4 size)
			{
				float4 c = tex2D(tex, uv + float2(-size.x, size.y)) + tex2D(tex, uv + float2(0, size.y)) + tex2D(tex, uv + float2(size.x, size.y)) +
							tex2D(tex, uv + float2(-size.x, 0)) + tex2D(tex, uv + float2(0, 0)) + tex2D(tex, uv + float2(size.x, 0)) +
							tex2D(tex, uv + float2(-size.x, -size.y)) + tex2D(tex, uv + float2(0, -size.y)) + tex2D(tex, uv + float2(size.x, -size.y));

				return c / 9;
			}

			// this function runs once for every pixel, determining what color each pixel should be
            fixed4 frag (v2f i) : SV_Target
            {
            //get the texture from the camera
            float4 color = tex2D(_MainTex, i.uv);





            //run a process on our editing bit, in our case, blur

            color *= float4(1, .3, .3, 1);


            color = box(_MainTex, i.uv, _MainTex_TexelSize);

            //Gaussian Blur!
            #if GAUSS
            				//failsafe so we can use turn off the blur by setting the deviation to 0
            				if(_StandardDeviation == 0)
            				return tex2D(_MainTex, i.uv);
            			#endif
            				//init color variable
            			//	float4 color = 0;
            			#if GAUSS
            				float sum = 0;
            			#else
            				float sum = SAMPLES;
            			#endif
            				//iterate over blur samples
            				for(float index = 0; index < SAMPLES; index++){
            					//get the offset of the sample
            					float offset = (index/(SAMPLES-1) - 0.5) * _BlurSize;
            					//get uv coordinate of sample
            					float2 uv = i.uv + float2(0, offset);
            				#if !GAUSS
            					//simply add the color if we don't have a gaussian blur (box)
            					color += tex2D(_MainTex, uv);
            				#else
            					//calculate the result of the gaussian function
            					float stDevSquared = _StandardDeviation*_StandardDeviation;
            					float gauss = (1 / sqrt(2*PI*stDevSquared)) * pow(E, -((offset*offset)/(2*stDevSquared)));
            					//add result to sum
            					sum += gauss;
            					//multiply color with influence from gaussian function and add it to sum color
            					color += tex2D(_MainTex, uv) * gauss;
            				#endif
            				}
            				//divide the sum of values by the amount of samples
            				color = color / sum;
            			//	return color;

            color *= float4(.05, .25, .05, 1);


            //lerp between the mask and the original texture, to add the masking
            half4 originalColor = tex2D(_MainTex, i.uv) ;
            half4 maskColor = tex2D(_Mask, i.uv);
            return lerp(originalColor,color,maskColor.a);


            }
            ENDCG
        }
    }
}
