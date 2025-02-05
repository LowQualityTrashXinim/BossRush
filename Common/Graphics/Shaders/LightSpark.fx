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

float4 ShaderPS(float4 vertexColor : COLOR0, float2 texCoords : TEXCOORD0) : COLOR0
{
	
	float2 uv = texCoords * 2. - 1.;
	
	float4 Color1 = color.rgbr;
    float starShape = clamp(1 - abs(((uv.x * 0.5) * (uv.y * 0.5f)) * 255), 0, 1);
    starShape *= smoothstep(0., 1, starShape);
	
	
	float d = length(uv);

    Color1 += starShape;
    Color1 *= smoothstep(0.8 * lerp(0, 1, shaderData.x), 0, d) * 5;

	
	return Color1;
}

technique t0
{
	pass LaserEffect
	{
		VertexShader = compile vs_2_0 ShaderVS();
		PixelShader = compile ps_2_0 ShaderPS();
	}
}
