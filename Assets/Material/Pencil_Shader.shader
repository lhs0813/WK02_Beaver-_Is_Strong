Shader "Custom/Pencil_Shader"
{
    Properties
    {
        _MainTex ("Base Texture", 2D) = "white" {}
        _Offset ("Offset", Vector) = (0,0,0,0)
        _Scale ("Scale", Vector) = (1,1,1,1)
    }
    SubShader
    {
        Tags { "Queue" = "Overlay" }
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
                float4 pos : POSITION;
                float2 uv : TEXCOORD0;
            };

            float4 _Offset;
            float4 _Scale;
            sampler2D _MainTex;

            v2f vert(appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv * _Scale.xy + _Offset.xy; // 텍스처 오프셋과 스케일 적용
                return o;
            }

            half4 frag(v2f i) : SV_Target
            {
                return tex2D(_MainTex, i.uv);
            }
            ENDCG
        }
    }
}
