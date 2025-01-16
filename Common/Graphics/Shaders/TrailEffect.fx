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

float4 FadeTrail(float4 sampleColor : COLOR0, float2 coords : TEXCOORD0) : COLOR0
{
    float2 uv = coords;
    
    float4 noiseTex = tex2D(image1, uv - time);
    noiseTex.rgb *= color;
    noiseTex.a = noiseTex.r;

    float4 color1 = noiseTex;
    color1.rgba -= uv.x;
    
    
    return color1;
}
    
technique Technique1
{
    pass FadeTrail
    {
        VertexShader = compile vs_2_0 ShaderVS();
        PixelShader = compile ps_2_0 FadeTrail();
    }
}