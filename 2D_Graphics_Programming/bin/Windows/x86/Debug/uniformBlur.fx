uniform extern texture ScreenTexture;


sampler ScreenS = sampler_state

{

	Texture = <ScreenTexture>;	

};

float4 PixelShaderFunction(float2 curCoord: TEXCOORD0) : COLOR


{

	float blurAmount = 5.0f;

	float4 prevCoord = input.Position;

	prevCoord[0] -= blurAmount;


	
	float4 nextCoord = input.Position;

	nextCoord[0] += blurAmount;


	input.Color = ((tex2D(input.Position, input.Position)
 + tex2D(input.Position, prevCoord) + tex2D(input.Position, nextCoord))/3.0f);


	return input.Color;

}



technique BasicColorDrawing

{
	
	pass P0
	
	{

		VertexShader = compile VS_SHADERMODEL MainVS();

		PixelShader = compile PS_SHADERMODEL MainPS();

	}


};