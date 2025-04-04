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
    
    float4 noiseTex = tex2D(image1, uv - time);
    noiseTex.a = noiseTex.r;
    float4 mainSlash = noiseTex * color.rgbr * 2;
    float upperPart = saturate(lerp(1, 0, length((uv.y))) * lerp(1, 0, uv.x));
    float upperPart2 = saturate(lerp(1, 0, length((uv.y) * 35)));

    float4 finalResult = (mainSlash * upperPart + saturate(upperPart2 * color.rgbr * 5)) * lerp(1, 0, uv.x * 1.1) * lerp(0, 1, uv.x * 5);
    return finalResult;
    
}
    
technique Technique1
{
    pass SwordTrail
    {
        
        PixelShader = compile ps_3_0 SwordTrail();
    }
}