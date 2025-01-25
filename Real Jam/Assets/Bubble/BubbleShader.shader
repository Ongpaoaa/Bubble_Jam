Shader "Custom/BubbleShader"
{
    Properties
    {
        _BaseColor ("Base Color", Color) = (0.5, 0.7, 1, 0.2)
        _Smoothness ("Smoothness", Range(0, 1)) = 0.9
        _FresnelPower ("Fresnel Power", Range(0, 5)) = 2.0
        _ColorShiftSpeed ("Color Shift Speed", Range(0, 10)) = 1.0
        _GradientTexture ("Gradient Texture", 2D) = "white" {} // Texture for color shifting
        _Transparency ("Transparency", Range(0, 1)) = 0.5
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        LOD 200

        Pass
        {
            Tags { "LightMode"="UniversalForward" }
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            Cull Back

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float3 normalOS : NORMAL;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float3 normalWS : NORMAL;
                float3 worldPos : TEXCOORD0;
                float2 uv : TEXCOORD1;
            };

            float4 _BaseColor;
            float _Smoothness;
            float _FresnelPower;
            float _ColorShiftSpeed;
            sampler2D _GradientTexture;
            float _Transparency;

            Varyings vert (Attributes input)
            {
                Varyings output;
                output.positionHCS = TransformObjectToHClip(input.positionOS);
                output.normalWS = normalize(TransformObjectToWorldNormal(input.normalOS));
                output.worldPos = TransformObjectToWorld(input.positionOS).xyz;
                output.uv = input.uv;
                return output;
            }

            half4 frag (Varyings input) : SV_Target
            {
                // Fresnel Effect for edge highlights
                float3 viewDir = normalize(GetCameraPositionWS() - input.worldPos);
                float fresnel = pow(1.0 - saturate(dot(viewDir, input.normalWS)), _FresnelPower);

                // Time-based Color Shifting
                float timeShift = _Time.y * _ColorShiftSpeed;
                float2 shiftedUV = float2(frac(input.uv.x + timeShift), 0.5);
                float4 gradientColor = tex2D(_GradientTexture, shiftedUV);

                // Combine Base Color, Gradient, and Fresnel
                half4 color = _BaseColor;
                color.rgb = gradientColor.rgb * fresnel; // Gradient color + edge highlight
                color.a = _Transparency * fresnel; // Translucency controlled by Fresnel

                return color;
            }
            ENDHLSL
        }
    }
   
}