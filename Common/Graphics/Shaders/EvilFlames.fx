#pragma warning (disable : 4717) 
sampler2D image1 : register(s1);
sampler2D image2 : register(s2);
sampler2D image3 : register(s3);

float4x4 viewWorldProjection;
float time;
float4 shaderData;
float3 color;


float4 EvilFlame(float4 sampleColor : COLOR0, float2 coords : TEXCOORD0) : COLOR0
{
    float2 centeredUV = coords;
    
    // make it near the hitbox
    centeredUV.x += 0.25;
    
    centeredUV *= 2;
    centeredUV -= 0.5;
    
    float2 distanceUV = 1 / distance(centeredUV,
    float2(0.5, 0.5));
    float tex = tex2D(image1, centeredUV - float2(time,0)).r;
      
    float pulse = sin(time * 45) * 0.3 + 1;
    
    float2 bolt = (distanceUV);
    bolt *= smoothstep(0.1, 1, length(bolt / 10));
    bolt *= pulse;
    float4 finalCol = tex * bolt.y * (float4(lerp(color.rgb, float3(1, 0, 0), centeredUV.x), 5));
    
    //remove the cut
    finalCol *= smoothstep(0,1,centeredUV.x * 10);
    return floor(finalCol * 5) / 5;
}
    

    
technique Technique1
{
    pass EvilFlame
    {
        
        PixelShader = compile ps_3_0 EvilFlame();
    }
}