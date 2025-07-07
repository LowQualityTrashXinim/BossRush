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
    centeredUV -= 0.5;
    centeredUV *= 2.;
    float2 pixelatedUV = round(centeredUV * (128.)) / 128.;
    float d = length(pixelatedUV);
    float angle = atan2(pixelatedUV.y, pixelatedUV.x);
    float2 VortexUV = float2(sin(angle + d * 5 - time * 3), d + time);
    float4 finalCol = tex2D(image1, VortexUV).r * lerp(float4(color, 15), float4(1, .0, 0, 1), smoothstep(0, 2, d * 2)) * smoothstep(1, 0., d);
    finalCol.rgb = lerp(finalCol.rgb, color, finalCol.rgb);
    finalCol = floor(finalCol * (6)) / 6;
    finalCol += smoothstep(float4(1,0,1,1),finalCol,d * 4).rrrr;
    
    return finalCol * 3;
}

technique t0
{
    pass Rift
    {
        VertexShader = compile vs_3_0 ShaderVS();
        PixelShader = compile ps_3_0 ShaderPS();
    }
}