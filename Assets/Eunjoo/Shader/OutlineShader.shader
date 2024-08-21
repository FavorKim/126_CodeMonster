Shader "Custom/Outline"
{
    Properties
    {
        _Color("Main Color", Color) = (1,1,1,1)
        _MainTex("Main Texture", 2D) = "white" {}
        _Outline("Outline Width", Float) = 0.03
        _OutlineColor("Outline Color", Color) = (1,0,0,1)
        _SpecularPower("Specular Power", Range(1, 256)) = 16 // 스페큘러 파워 추가
        _SpecularIntensity("Specular Intensity", Range(0, 1)) = 0.5 // 스페큘러 강도 추가
    }

    SubShader
    {
        Tags { "Queue" = "Transparent" }

        // 외곽선 그리기
        Pass
        {
            Name "Outline"
            Tags { "LightMode" = "Always" }
            Cull Front

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            uniform float _Outline;
            uniform float4 _OutlineColor;

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float4 pos : POSITION;
            };

            v2f vert(appdata v)
            {
                // Normal을 따라 일정 거리만큼 확장해서 윤곽선을 그립니다.
                v2f o;
                float3 norm = normalize(v.normal);
                o.pos = UnityObjectToClipPos(v.vertex + norm * _Outline);
                return o;
            }

            fixed4 frag(v2f i) : COLOR
            {
                return _OutlineColor;
            }
            ENDCG
        }

        // 오브젝트 본체를 정상적으로 그리기
        Pass
        {
            Name "Main Pass"
            Tags { "LightMode" = "ForwardBase" }

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fwdbase
            #include "UnityCG.cginc"
            #include "Lighting.cginc" // 기본 조명 모델 추가

            struct appdata_t
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 texcoord : TEXCOORD0;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 normalDir : TEXCOORD1;
                float3 worldPos : TEXCOORD2;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            fixed4 _Color;
            float _SpecularPower;
            float _SpecularIntensity;

            v2f vert(appdata_t v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
                o.normalDir = normalize(mul((float3x3)unity_WorldToObject, v.normal));
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                return o;
            }

            fixed4 frag(v2f i) : COLOR
            {
                // 표준 조명 모델 사용
                fixed3 normal = normalize(i.normalDir);
                fixed3 lightDir = normalize(_WorldSpaceLightPos0.xyz);
                fixed3 viewDir = normalize(UnityWorldSpaceViewDir(i.worldPos));

                // 기본적인 램버트 확산 조명 계산
                fixed diff = max(0.0, dot(normal, lightDir));

                // 표준 조명 모델 적용
                fixed3 halfDir = normalize(lightDir + viewDir);
                fixed spec = pow(max(dot(normal, halfDir), 0.0), _SpecularPower);

                fixed4 texColor = tex2D(_MainTex, i.uv) * _Color;

                fixed4 finalColor = texColor * (diff + spec * _SpecularIntensity);

                return finalColor;
            }
            ENDCG
        }
    }
}
