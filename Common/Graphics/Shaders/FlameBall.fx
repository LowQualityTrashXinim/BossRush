#pragma warning (disable : 4717) 
sampler2D image1 : register(s1);
sampler2D image2 : register(s2);
sampler2D image3 : register(s3);
float4x4 viewWorldProjection;
float time;
float4 shaderData;
float3 color;



float2 Rotate(float2 uv, float amount)
{
    float2 uv2 = uv;
    float s = sin(amount);
    float c = cos(amount);
    uv2.x = (uv.x * c) + (uv.y * -s);
    uv2.y = (uv.x * s) + (uv.y * c);

    return uv2;
    
}

float2 expandInsideOutside(float2 uv)
{
    float1 t = time + shaderData.w;
    float2 uv2 = Rotate(uv,t);
    float1 d = length(uv2);

    return (d * uv2 + ((t) - uv2));
    
}


float4 ballOfire(float4 sampleColor : COLOR0, float2 coords : TEXCOORD0) : COLOR0
{
    float2 uv = coords * 2.0 - 1.0;
    float d = length(uv);
    float m = d / 1;
    float4 color1 = (255,255, 255, 0);
    float4 color2 = (255, 255, 255, 0);
    float pixels = 512;
    float dx = 15.0 * (1 / pixels);
    float dy = 15.0 * (1 / pixels);
    float2 pixalatedUV = float2(dx * floor(uv.x / dx), dy * floor(uv.y / dy));
    float starShape = clamp(1 - abs(((pixalatedUV.x * 0.5) * (pixalatedUV.y * 0.5f)) * 125), 0, 1);
    float4 noiseTex = tex2D(image1, expandInsideOutside(uv));
    noiseTex.rgb *= color;
    noiseTex.a = noiseTex.r;
    color1 += noiseTex;
    color1.rgb += color * (m + 0.5);
    // the magic/sauce
    color1.rgba -= d * 3;
    color1.rgba *= d * 2;
    color1 += starShape;

    color1.rgb /= d / 0.3;
    color1.rgb *= shaderData.x * 0.01f;

    // explosion
    color2 = noiseTex;
    color2.rgba -= ((shaderData.z));
    color2.rgba *= d * 3;
    color2.a *= d * 15;
    color2.rgba *= step(d * 0.5, 0.5);
    
    if (shaderData.y == 0)
        return color1;
    else
        return color2;
   
}
    
technique Technique1
{
    pass ballOfire
    {
        
        PixelShader = compile ps_2_0 ballOfire();
    }
}