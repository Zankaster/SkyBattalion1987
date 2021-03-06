Shader "Blazefrost/EnemyShip"{
	Properties{
		_Color("Tint", Color) = (0, 0, 0, 1)
		_MainTex("MainTex", 2D) = "white" {}
		_FlashColor("FlashColor", Color) = (0.86, 0.86, 0.86, 1)
		_Flash("Flash", Float) = 0
	}

		SubShader{
			Tags{
				"RenderType" = "Transparent"
				"Queue" = "Transparent"
			}

			Blend SrcAlpha OneMinusSrcAlpha

			ZWrite off
			Cull off

			Pass{

				CGPROGRAM

				#include "UnityCG.cginc"

				#pragma vertex vert
				#pragma fragment frag

				sampler2D _MainTex;
				float4 _MainTex_ST;
				float _Flash;
				fixed4 _FlashColor;

				fixed4 _Color;

				struct appdata {
					float4 vertex : POSITION;
					float2 uv : TEXCOORD0;
					fixed4 color : COLOR;
				};

				struct v2f {
					float4 position : SV_POSITION;
					float2 uv : TEXCOORD0;
					fixed4 color : COLOR;
				};

				v2f vert(appdata v) {
					v2f o;
					o.position = UnityObjectToClipPos(v.vertex);
					o.uv = TRANSFORM_TEX(v.uv, _MainTex);
					o.color = v.color;
					return o;
				}

				fixed4 frag(v2f i) : SV_TARGET{
					fixed4 col = tex2D(_MainTex, i.uv);
					col *= _Color;
					col *= i.color;

					if (_Flash == 1) {
						col.rgb = _FlashColor;
						col.rgb *= col.a;
					}
					/*fixed4 c = tex2D(_MainTex, i.uv);
					c.rgb = (1,1,1);
					c.rgb *= c.a;*/
					return col;
				}

				ENDCG
			}
		}
}
