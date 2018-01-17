uniform extern texture ScreenTexture;

sampler ScreenS = sampler_state
{
	Texture = < ScreenTexture >;
};

float brightness;

float4 PixelShaderFunction ( float2 curCoord : TEXCOORD0 ) : COLOR
{
	float4 color = tex2D ( ScreenS , curCoord );
	color [0] *= brightness;
	color [1] *= brightness;
	color [2] *= brightness;
	return color;
}

technique
{
	pass P0
	{
		PixelShader = compile ps_2_0 PixelShaderFunction () ;
	}
}