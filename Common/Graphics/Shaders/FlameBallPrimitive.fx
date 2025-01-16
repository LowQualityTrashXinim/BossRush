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
    
    VertexShaderOutput output = (VertexShaderOutput)0;

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
    float2 uv2 = Rotate(uv,t);
    float1 d = length(uv2);

    return (d * uv2 + ((t) - uv2));
    
}
float4 ShaderPS(float4 vertexColor : COLOR0, float2 texCoords : TEXCOORD0) : COLOR0
{
    float progress1 = shaderData.z;
    float2 uv = texCoords * 2.0 - 1.0;
    float d = length(uv);
    float4 color1 = (255, 255, 255, 0);
    float pixels = 512;
    float dx = 15.0 * (1 / pixels);
    float dy = 15.0 * (1 / pixels);
    float2 pixalatedUV = float2(dx * floor(uv.x / dx), dy * floor(uv.y / dy));
    float starShape = clamp(1 - abs(((uv.x * 0.5) * (uv.y * 0.5f)) * 125), 0, 1);
    float starShape2 = clamp(1 - abs(((Rotate(uv, 3.1415/4).x * 0.1) * (Rotate(uv, 3.1415/4).y * 0.1f)) * 100), 0, 0.75);
    float4 noiseTex = tex2D(image1, expandInsideOutside(uv));
    noiseTex.rgb *= color;
    noiseTex.a = noiseTex.r;
    color1 += noiseTex;
    color1.rgb += color * (d + 0.5);
    // the magic/sauce
    color1.rgba -= d * 3;
    color1.rgba *= d * 5;
    color1 += starShape;
    color1 += (starShape2);
    color1 += d * 2;
    color1.rgb /= d / 0.3;
    color1.rgb *= (shaderData.x * 0.01);

    if (shaderData.y == 1)
    {
    
        color1.rgba *= step(d, step(progress1, d));
        color1.rgb = lerp(0,color * color1.r,shaderData.z);
    }
    
    
    return color1;

}

technique t0
{
    pass FlameBallPrimitive
    {
        VertexShader = compile vs_2_0 ShaderVS();
        PixelShader = compile ps_2_0 ShaderPS();
    }
}