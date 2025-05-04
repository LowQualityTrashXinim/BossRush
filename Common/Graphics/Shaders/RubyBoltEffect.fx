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
float4 ShaderPS(float4 vertexColor : COLOR0, float2 texCoords : TEXCOORD0) : COLOR0
{
    float2 centeredUV = texCoords;
    
    float2 distanceUV = 1 / distance(centeredUV, 
    float2(0.5, 0.5));
    
    float pulse = sin(time * 45) * 0.3 + 1;
    
    float2 bolt = distanceUV;
    bolt *= smoothstep(0.1, 1, length(bolt / 20));
    bolt *= pulse;
    return (bolt.xxxx * float4(color.rgb, 1));
}

technique t0
{
    pass RubyBoltEffect
    {
        VertexShader = compile vs_2_0 ShaderVS();
        PixelShader = compile ps_2_0 ShaderPS();
    }
}