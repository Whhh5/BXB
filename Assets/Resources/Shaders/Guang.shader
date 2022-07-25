//���ߣ�taecg
//���ӣ�https ://zhuanlan.zhihu.com/p/102190139
//��Դ��֪��
//����Ȩ���������С���ҵת������ϵ���߻����Ȩ������ҵת����ע��������

Shader "taecg/ShaderToy/Rotary Star"
{
	SubShader
	{
		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			//��������
			#define NUM_PARTICLES 200.0
			//���Ӱ뾶
			#define RADIUS 0.015
			//�����ⷢ��
			#define GLOW 0.5

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

			v2f vert(appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}

			float3 Orb(float2 uv, float3 color, float radius, float offset)
			{
				float2 position = float2(sin(offset * (_Time.y + 30.)),cos(offset * (_Time.y + 30.)));
				position *= sin((_Time.y) - offset) * cos(offset);
				radius = radius * offset;
				//ͨ������UV����������Position��ľ���������Բ��,������radius�������ǿ���Բ�εĴ�С
				float dist = radius / distance(uv, position);
				return color * pow(dist, 1.0 / GLOW);
			}

			fixed4 frag(v2f i) : SV_Target
			{
				//�������Ļ��߱�(��Ļ��_ScreenParams.x������Ļ��_ScreenParams.y)_ScreenParamsΪ���ñ���
				half ratio = _ScreenParams.x / _ScreenParams.y;
			//ͨ��*2-1��UV��0,0���Ƶ���Ļ������
			half2 centerUV = i.uv * 2 - 1;
			//ͬʱ����UV������Ļ��������
			centerUV.x *= ratio;

			float3 pixel,color = 0;

			//��ʱ����仯����ɫ
			color.r = (sin(_Time.y * 0.55) + 1.5) * 0.4;
			color.g = (sin(_Time.y * 0.34) + 2.0) * 0.4;
			color.b = (sin(_Time.y * 0.31) + 4.5) * 0.3;

			for (int i = 0.0; i < NUM_PARTICLES; i++)
			{
				pixel += Orb(centerUV, color, RADIUS, i / NUM_PARTICLES);
			}

			float4 fragColor = lerp(float4(centerUV,0.8 + 0.5 * sin(_Time.y),1.0), float4(pixel, 1.0), 0.8);
			return fragColor;
		}

		ENDCG
	}
	}
}