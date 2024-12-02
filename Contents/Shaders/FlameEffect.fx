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

float4 FlamethrowerFlame(float4 sampleColor : COLOR0, float2 coords : TEXCOORD0) : COLOR0
{
    float2 uv = coords;

    float4 noiseTex = tex2D(uImage1, uv - uTime);
    noiseTex.rgb *= uColor;
    noiseTex.a = noiseTex.r;

    float4 color = noiseTex;
    color.rgb += uColor * (uv.x + 1);
    color.rgba -= uv.x * 3;
    
    color.a *= lerp(0, 1, clamp(uShaderSpecificData.r / uShaderSpecificData.g,0,1));
    
    return color;
}
    
technique Technique1
{
    pass FlamethrowerFlame
    {
        
        PixelShader = compile ps_2_0 FlamethrowerFlame();
    }
}