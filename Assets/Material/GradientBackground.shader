Shader "Custom/GradientBackground"
 {
     Properties
     {
         _Color1 ("Color 1", Color) = (1.0, 1.0, 1.0, 1.0)
         _Color2 ("Color 2", Color) = (0.75, 0.75, 0.75, 1.0)
         _Color3 ("Color 3", Color) = (0.25, 0.25, 0.25, 1.0)
         _Color4 ("Color 4", Color) = (0.0, 0.0, 0.0, 1.0)
         _Pos1 ("Gradient Position 1", Range (0,1)) = 0.33
         _Pos2 ("Gradient Position 2", Range (0,1)) = 0.66
     }
     SubShader
     {
         Tags { "RenderType"="Opaque" "Queue"="Background" }
         LOD 100
 
         ZWrite Off
         Cull Off
 
         Pass
         {
             CGPROGRAM
             #pragma vertex vert
             #pragma fragment frag
 
             #include "UnityCG.cginc"
 
             fixed4 _Color1;
             fixed4 _Color2;
             fixed4 _Color3;
             fixed4 _Color4;
 
             half _Pos1;
             half _Pos2;
 
             struct appdata
             {
                 float4 vertex : POSITION;
                 float2 uv : TEXCOORD0;
             };
 
             struct v2f
             {
                 float4 pos : SV_POSITION;
                 float4 uv : TEXCOORD0;
             };
 
             v2f vert (appdata v)
             {
                 v2f o;
                 o.pos = UnityObjectToClipPos (v.vertex);
                 o.uv = ComputeScreenPos (o.pos);
                 return o;
             }
 
             //This is a slightly cheaper version of smoothstep for linear gradients
             float linstep (float a, float b, float x)
             {
                 return saturate ((x - a) / (b - a));
             }
 
             fixed4 frag (v2f i) : SV_Target
             {
                 //These are screen-space UVs
                 float2 uv = i.uv.xy / i.uv.w;
 
                 //Make sure the gradient always travels in the same direction
                 float p1 = min (_Pos1, _Pos2);
                 float p2 = max (_Pos1, _Pos2);
 
                 //Here is a simple 4-colour gradient on the y-axis, using smoothstep to get a more continuous derivative
                 return lerp 
                 (
                     _Color1, 
                     lerp 
                     (
                         _Color2, 
                         lerp 
                         (
                             _Color3, 
                             _Color4, 
                             smoothstep (p2, 1.0, uv.y)
                         ), 
                         smoothstep (p1, p2, uv.y)
                     ), 
                     smoothstep (0.0, p1, uv.y)
                 );
             }
             ENDCG
         }
     }
 }