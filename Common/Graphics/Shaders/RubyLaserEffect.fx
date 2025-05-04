#pragma warning (disable : 4717) 
sampler2D image1 : register(s1);
sampler2D image2 : register(s2);
sampler2D image3 : register(s3);
float4x4 viewWorldProjection;
float time;
float4 shaderData;
float3 color;


struct VertexShaderInput
{
    float4 pos : POSITION0;
    float4 col : COLOR0;
    float2 texCoord : TEXCOORD0;
};

struct VertexShaderOutput
{
    float4 pos : SV_POSITION;
    float4 col : COLOR0;
    float2 texCoord : TEXCOORD0;
};


VertexShaderOutput ShaderVS(VertexShaderInput input)
{
    
    VertexShaderOutput output = (VertexShaderOutput) 0;

    output.pos = mul(input.pos, viewWorldProjection);
    output.col = input.col;
    output.texCoord = input.texCoord;
    
    return output;

}


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
    float2 uv2 = Rotate(uv, t);
    float1 d = length(uv2);

    return (d * uv2 + ((t) - uv2));
    
}
float4 ShaderPS(float4 vertexColor : COLOR0, float2 texCoords : TEXCOORD0) : COLOR0
{
    float2 repeatedUV = float2(texCoords.x * (shaderData.w / 128.), texCoords.
    y * 2. - 1.);
    float2 normalUV = texCoords * 2.0 - 1.0;

    float progress = shaderData.x;
    
    float laser = length(1/normalUV.y /10);
    laser *= smoothstep(0.1, 1, laser);
    
    laser = laser * tex2D(image1, texCoords * 2 - time * 3).r;
    laser *= 3;
    laser *= step(lerp(0, 1, texCoords.x), progress);
    
    if (progress > 0.8)
        laser *= smoothstep(0.1, 1, texCoords.y - (1 - progress));
    return laser.xxxx * color.rgbr;
}

technique t0
{
    pass RubyLaserEffect
    {
        VertexShader = compile vs_2_0 ShaderVS();
        PixelShader = compile ps_2_0 ShaderPS();
    }
}