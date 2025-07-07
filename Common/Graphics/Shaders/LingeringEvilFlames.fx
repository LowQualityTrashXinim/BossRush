#pragma warning (disable : 4717) 
sampler2D image1 : register(s1);
sampler2D image2 : register(s2);
sampler2D image3 : register(s3);

float4x4 viewWorldProjection;
float time;
float4 shaderData;
float3 color;


float4 EvilFlameLinger(float4 sampleColor : COLOR0, float2 coords : TEXCOORD0) : COLOR0
{
    float2 uv =  coords * 2.0 - 1.;
    float d = distance(uv, float2(0.5,0.5));
    float angle = atan2(uv.y, uv.x);
    float2 polar = float2(angle,d - time + shaderData.x);
    
    float tex1 = tex2D(image1, coords);
    float tex2 = tex2D(image1, polar);
    
    float4 finalCol = tex1.rrrr + tex2.rrrr;
    finalCol *= float4(lerp(float3(1, 0, 0), color, length(uv * 2)), 1);
    finalCol *= smoothstep(0, 1,1 / length(uv * 5));
    finalCol *= 2;
    
    

    return floor(finalCol * 2) / 2;
}
    

    
technique Technique1
{
    pass EvilFlameLinger
    {
        
        PixelShader = compile ps_3_0 EvilFlameLinger();
    }
}