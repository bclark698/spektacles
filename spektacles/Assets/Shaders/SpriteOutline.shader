Shader "Sprites/Outline"
{
    Properties
    {
		[PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
		_Color ("Main texture Tint", Color) = (1,1,1,1)

		[Header(General Settings)]
		[MaterialToggle] _OutlineEnabled ("Outline Enabled", Float) = 1
		[MaterialToggle] _ConnectedAlpha ("Connected Alpha", Float) = 0
        [HideInInspector] _AlphaThreshold ("Alpha clean", Range (0, 1)) = 0
        _Thickness ("Width (Max recommended 100)", float) = 10
		[KeywordEnum(Solid)] _OutlineMode("Outline mode", Float) = 0
		[KeywordEnum(Contour)] _OutlineShape("Outline shape", Float) = 0

		[Header(Solid Settings)]
		_SolidOutline ("Outline Color Base", Color) = (1,1,1,1)
    }

	SubShader
	{
		Tags
		{ 
			"Queue"="Transparent" 
			"IgnoreProjector"="True" 
			"RenderType"="Transparent" 
			"PreviewType"="Plane"
			"CanUseSpriteAtlas"="True"
		}

		Cull Off
		Lighting Off
		ZWrite Off
		Blend One OneMinusSrcAlpha

		Pass
		{
		CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile _ PIXELSNAP_ON
			#pragma exclude_renderers d3d11_9x

			#include "UnityCG.cginc"
			
			struct appdata_t
			{
				float4 vertex   : POSITION;
				float4 color    : COLOR;
				float2 texcoord : TEXCOORD0;
			};

			struct v2f
			{
				float4 vertex   : SV_POSITION;
				fixed4 color    : COLOR;
				float2 texcoord  : TEXCOORD0;
			};

			fixed4 _Color;
            fixed _Thickness;
            fixed _OutlineEnabled;
            fixed _ConnectedAlpha;
            fixed _OutlineShape;
            fixed _OutlinePosition;
            fixed _OutlineMode;

			fixed4 _SolidOutline;

            fixed _AlphaThreshold;

			v2f vert(appdata_t IN)
			{
				v2f OUT;
				OUT.vertex = UnityObjectToClipPos(IN.vertex);
				OUT.texcoord = IN.texcoord;
				OUT.color = IN.color * _Color;
				#ifdef PIXELSNAP_ON
				OUT.vertex = UnityPixelSnap (OUT.vertex);
				#endif

				return OUT;
			}

			sampler2D _MainTex;
			sampler2D _AlphaTex;
			float _AlphaSplitEnabled;
            uniform float4 _MainTex_TexelSize;

			fixed4 SampleSpriteTexture (float2 uv)
			{
				float2 offsets;
				if((_OutlinePosition != 2 && _OutlineShape == 1) || _OutlineEnabled == 0) // not outside and frame
				{
					offsets = float2(0, 0);
				}
				else
				{
					offsets = float2(_Thickness * 2, _Thickness * 2);
				}
				float2 bigsize = float2(_MainTex_TexelSize.z, _MainTex_TexelSize.w);
				float2 smallsize = float2(_MainTex_TexelSize.z - offsets.x, _MainTex_TexelSize.w - offsets.y);

				float2 uv_changed = float2
				(
					uv.x * bigsize.x / smallsize.x - 0.5 * offsets.x / smallsize.x,
					uv.y * bigsize.y / smallsize.y - 0.5 * offsets.y / smallsize.y
				);

				if(uv_changed.x < 0 || uv_changed.x > 1 || uv_changed.y < 0 || uv_changed.y > 1)
				{
					return float4(0, 0, 0, 0);
				}

				fixed4 color = tex2D (_MainTex, uv_changed);

#if UNITY_TEXTURE_ALPHASPLIT_ALLOWED
				if (_AlphaSplitEnabled)
					color.a = tex2D (_AlphaTex, uv).r;
#endif //UNITY_TEXTURE_ALPHASPLIT_ALLOWED

				return color;
			}

			bool CheckOriginalSpriteTexture (float2 uv, bool ifZero)
			{
				float thicknessX = _Thickness / _MainTex_TexelSize.z;
				float thicknessY = _Thickness / _MainTex_TexelSize.w;
				int steps = 100;
				float angle_step = 360.0 / steps;

				float alphaThreshold = _AlphaThreshold / 10;
				float alphaCount = _AlphaThreshold * 10;

				// check if the basic points has an alpha to speed up the process and not use the for loop
				bool outline = false;
				float alphaCounter = 0;

				if(ifZero)
				{
					
				}
				else
				{
					outline = 	SampleSpriteTexture(uv + fixed2(0, +thicknessY)).a > alphaThreshold ||
								SampleSpriteTexture(uv + fixed2(0, -thicknessY)).a > alphaThreshold ||
								SampleSpriteTexture(uv + fixed2(+thicknessX, 0)).a > alphaThreshold ||
								SampleSpriteTexture(uv + fixed2(-thicknessX, 0)).a > alphaThreshold ||
								SampleSpriteTexture(uv + fixed2(+thicknessX * cos (3.14 / 4), -thicknessY * sin (3.14 / 4))).a > alphaThreshold ||
								SampleSpriteTexture(uv + fixed2(-thicknessX * cos (3.14 / 4), +thicknessY * sin (3.14 / 4))).a > alphaThreshold ||
								SampleSpriteTexture(uv + fixed2(-thicknessX * cos (3.14 / 4), -thicknessY * sin (3.14 / 4))).a > alphaThreshold ||
								SampleSpriteTexture(uv + fixed2(+thicknessX * cos (3.14 / 4), +thicknessY * sin (3.14 / 4))).a > alphaThreshold;
				}
				if(outline) return outline;

				for(int i = 0; i < steps; i++) // high number and not a variable to avoid stupid compiler bugs
				{
					float angle = i * angle_step * 2 * 3.14 / 360;
					if(ifZero && SampleSpriteTexture(uv + fixed2(thicknessX * cos(angle), thicknessY * sin(angle))).a == 0)
					{
						alphaCounter++;
						if(alphaCounter >= alphaCount)
						{
							outline = true;
							break;
						}
					}
					else if(!ifZero && SampleSpriteTexture(uv + fixed2(thicknessX * cos(angle), thicknessY * sin(angle))).a > alphaThreshold)
					{
						outline = true;
						break;
					}
				}

				return outline;
			}

			fixed4 frag(v2f IN) : SV_Target
			{
				float thicknessX = _Thickness / _MainTex_TexelSize.z;
				float thicknessY = _Thickness / _MainTex_TexelSize.w;

				fixed4 c = SampleSpriteTexture (IN.texcoord) * IN.color;

				c.rgb *= c.a;

				fixed alpha;

				fixed4 outlineC = fixed4(0, 0, 0, 1);

				if(_OutlineEnabled != 0)
				{
					if(_OutlineMode == 0) // Solid
					{
						outlineC = _SolidOutline;

						if(_ConnectedAlpha != 0)
						{
							outlineC.a *= _Color.a;
						}
						outlineC.rgb *= outlineC.a;
					}
					if(_OutlineShape == 0 && _Thickness > 0) // Contour
					{
						if((_OutlinePosition != 2 && _OutlineShape == 1) && c.a != 0 && // inside and frame
							(
								IN.texcoord.y + thicknessY > 1 ||
								IN.texcoord.y - thicknessY < 0 ||
								IN.texcoord.x + thicknessX > 1 ||
								IN.texcoord.x - thicknessX < 0 || 
								CheckOriginalSpriteTexture(IN.texcoord, true)
							)
						)
						{
							return outlineC;
						}
						else if((_OutlinePosition == 2 || _OutlineShape != 1) && c.a == 0 && // outside orcontour
								(
									CheckOriginalSpriteTexture(IN.texcoord, false)
								)
							)
						{
							return outlineC;
						}
						else
						{
							return c;
						}
					}
					else
					{
						return c;
					}
				}
				else
				{
					return c;
				}

				return c;
			}
		ENDCG
		}
	}
}