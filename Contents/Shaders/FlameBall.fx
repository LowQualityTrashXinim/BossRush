sampler uImage0 : register(s0); 
sampler uImage1 : register(s1); 
sampler uImage2 : register(s2);
sampler uImage3 : register(s3);
float3 uColor;
float uOpacity;
float uTime;
float uProgress;
float2 uImageSize1;
float2 uImageSize2;
float uSaturation;
float4 uShaderSpecificData;
float2 uWorldPosition;
float4 uSourceRect;
float3 uSecondaryColor;


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
    float1 t = uTime + uShaderSpecificData.w;
    float2 uv2 = Rotate(uv,t);
    float1 d = length(uv2);

    return (d * uv2 + ((t) - uv2));
    
}


float4 ballOfire(float4 sampleColor : COLOR0, float2 coords : TEXCOORD0) : COLOR0
{
    float2 uv = coords * 2.0 - 1.0;
    float d = length(uv);
    float m = d / 1;
    float4 color = (255,255, 255, 0);
    float4 color2 = (255, 255, 255, 0);
    float pixels = 512;
    float dx = 15.0 * (1 / pixels);
    float dy = 15.0 * (1 / pixels);
    float2 pixalatedUV = float2(dx * floor(uv.x / dx), dy * floor(uv.y / dy));
    float starShape = clamp(1 - abs(((pixalatedUV.x * 0.5) * (pixalatedUV.y * 0.5f)) * 125), 0, 1);
    float4 noiseTex = tex2D(uImage1, expandInsideOutside(uv));
    noiseTex.rgb *= uColor;
    noiseTex.a = noiseTex.r;
    color += noiseTex;
    color.rgb += uColor * (m + 0.5);
    // the magic/sauce
    color.rgba -= d * 3;
    color.rgba *= d * 2;
    color += starShape;

    color.rgb /= d / 0.3;
    color.rgb *= uShaderSpecificData.x * 0.01f;

    // explosion
    color2 = noiseTex;
    color2.rgba -= ((uShaderSpecificData.z));
    color2.rgba *= d * 3;
    color2.a *= d * 15;
    color2.rgba *= step(d * 0.5, 0.5);
    
    if (uShaderSpecificData.y == 0)
        return color;
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