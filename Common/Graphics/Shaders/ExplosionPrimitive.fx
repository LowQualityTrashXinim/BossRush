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

float2 expandInsideOutside(float2 uv, float dir)
{
    float1 t = (time + shaderData.w) * dir;
    float2 uv2 = Rotate(uv, t);
    float1 d = length(uv2);

    return (d * uv2 + (Rotate(uv, t + shaderData.w)) * 0.1);
    
}

float4 ShaderPS(float4 vertexColor : COLOR0, float2 texCoords : TEXCOORD0) : COLOR0
{
    float2 uv = texCoords * 2.0 - 1.0;
    float d = length(uv);
    float progress = shaderData.x;
    float4 noise1 = tex2D(image1, uv + time);
    float4 noise2 = tex2D(image1, expandInsideOutside(uv,-1));
    float circle1 = clamp(smoothstep(0, 1, d / progress) * 15,0,1);
    circle1 *= lerp(0, noise1.r,  1/d);
    float circle2 = smoothstep(0.1,1,progress / d);
    float circle3 = clamp(-1 * (0.5 - distance(float2(0.5 * 2.0 - 1.0, 0.5 * 2.0 - 1.0), uv) * (lerp(0,1,1/progress))), 0, 1);
    circle3 *= 1;
    circle3 *= smoothstep(0.9, 1, clamp(progress,0,0.75) / d);


    float4 color1 = float4(color.rgb, 1);
    color1 *= 15;
    color1 += noise1;
    color1.a = color1.r;
    color1.rgb += color * (d - 8);
    color1 /= noise1.r * 1.5;
    color1.a = color.r;

    
    //step(d,0.75) removes the corners since full circles only take 3/4 of the rendertarget 
        return (color1 * circle3);
}


technique t0
{
    pass explodingStar
    {
        VertexShader = compile vs_2_0 ShaderVS();
        PixelShader = compile ps_2_0 ShaderPS();
    }
}