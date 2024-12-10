#pragma warning (disable : 4717) 
sampler2D uImage1 : register(s0);

float4x4 viewWorldProjection;
float time;
float4 shaderData;
float3 uColor;


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


VertexShaderOutput MyShaderVS(VertexShaderInput input)
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
float4 MyShaderPS(float4 vertexColor : COLOR0, float2 texCoords : TEXCOORD0) : COLOR0
{
    //float2 uv = texCoords * 2.0 - 1.0;
    //float d = length(uv);
    //float m = d / 1;
    //float4 color = (255, 255, 255, 0);
    //float4 color2 = (255, 255, 255, 0);
    //float pixels = 512;
    //float dx = 15.0 * (1 / pixels);
    //float dy = 15.0 * (1 / pixels);
    //float2 pixalatedUV = float2(dx * floor(uv.x / dx), dy * floor(uv.y / dy));
    //float starShape = clamp(1 - abs(((uv.x * 0.75) * (uv.y * 0.75)) * 100), 0, 1);
    //float starShape2 = clamp(1 - abs(((Rotate(uv, 3.1415 / 4).x * 1) * (Rotate(uv, 3.1415 / 4).y * 1)) * 200), 0, 1);

    //float4 noiseTex = tex2D(uImage1, expandInsideOutside(uv));
    //noiseTex.rgb *= uColor;
    //noiseTex.a = noiseTex.r;
    //color += noiseTex;
    //color.rgb += uColor * (d + 1);
    // the magic/sauce
    //color.rgba -= m * 3;
    //color += starShape;
    //color += starShape2;
    //color.rgb *= shaderData.x * 0.01f;

    // explosion
    //color2 = noiseTex;
    //color2.rgba -= ((shaderData.z));
    //color2.rgba *= d * 3;
    //color2.a *= d * 15;
    //color2.rgba *= step(d * 0.5, 0.5);
    
    //if (shaderData.y == 0)
    //    return color;
    //else
    //    return color2;
    
    float2 uv = texCoords * 2.0 - 1.0;
    float d = length(uv);
    float4 color1 = float4(0,0,0,0);
    
    float4 noiseTex = tex2D(uImage1, expandInsideOutside(uv));
    noiseTex.a = noiseTex.r;
    color1 += noiseTex;
    return color1;
    
}

technique t0
{
    pass MyShader
    {
        VertexShader = compile vs_2_0 MyShaderVS();
        PixelShader = compile ps_2_0 MyShaderPS();
    }
}