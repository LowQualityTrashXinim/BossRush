#pragma warning (disable : 4717) 
sampler2D image1 : register(s1);
sampler2D image2 : register(s2);
sampler2D image3 : register(s3);

float4x4 viewWorldProjection;
float time;
float4 shaderData;
float3 color;


float4 SwordTrail(float4 sampleColor : COLOR0, float2 coords : TEXCOORD0) : COLOR0
{
    float2 uv = coords;
    float blurEdge = saturate(lerp(1, 0, 1. / abs(uv.y * 100 - 1)));
    float4 noiseTex = tex2D(image1, uv * 2 - time);
    float4 gradient = tex2D(image2, uv);
    float4 pingpongGradient = tex2D(image3, uv);
    noiseTex.rgb *= color;
    noiseTex.a = noiseTex.r;
    noiseTex.a *= gradient.r;
    float4 color1 = noiseTex;
    color1.rgb += color * (uv.x + 1);
    color1.rgba -= uv.x * 3;
    color1.rgba *= pingpongGradient.r;
   

    return color1;
}
    
technique Technique1
{
    pass SwordTrail
    {
        
        PixelShader = compile ps_2_0 SwordTrail();
    }
}