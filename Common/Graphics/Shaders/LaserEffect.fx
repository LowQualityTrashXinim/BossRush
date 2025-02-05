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

    
    float4 color1 = color.rgbr;
    color1.a = color1.r;
    float d = length(normalUV);
    float4 tex1 = tex2D(image1, repeatedUV - time * 3);
    tex1.a = tex1.r;
    
    tex1 *= 1 - clamp(abs(repeatedUV.y), 0, 1);
    
    
    //color1 *= middleWhitePart;
    
    color1 *= tex1;
    
    
    //bloom
    color1 += 0.5 - distance(0.5 * 2. - 1., repeatedUV.y);

    //the actual laser
    color1 *= 0.5 - distance(0.5 * 2. - 1, repeatedUV.y);
    color1 *= 0.5 - clamp(abs(repeatedUV.y), 0, 1);
    color1 += 1 - clamp(abs(repeatedUV.y) * 15, 0, 1);
    color1 = clamp(color1, 0, 1 - abs(repeatedUV.y) * 2);

    // the actual shape of the laser
    float4 tex2 = tex2D(image1, repeatedUV - float2(time * 5,0));
    tex2.a = tex2.r * 3;
    color1 *= tex2;
    color1 *= exp(normalUV*3).x;
    color1 *= step(d / 2,0.5);
    
    return color1 * 5;

}

technique t0
{
    pass LaserEffect
    {
        VertexShader = compile vs_2_0 ShaderVS();
        PixelShader = compile ps_2_0 ShaderPS();
    }
}