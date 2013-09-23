

float PiOver2 = 1.5707963267948966192313216916398;

uniform extern float4 BaseColor = float4(0.25,1,0,1);
uniform extern float Thickness = 0.05;

uniform extern float4x4 xView;
uniform extern float4x4 xProjection;
uniform extern float4x4 xWorld;

uniform extern float3 Center;
uniform extern float Radius;



struct VertexShaderInput
{
	float4 Position : POSITION0;
	float2 Tex0 : TexCoord0;
};

struct VertexShaderOutput
{
	float4 Position : POSITION0;
	float2 Tex0 : TexCoord0;
};



//////////////////////////////////////////////////////////////////////////////////////////////////////////

// common vertex shader, simple passthrough
VertexShaderOutput VertexShaderFunction(VertexShaderInput input)
{
	VertexShaderOutput output;
	
	// transform matrix
	float4x4 preViewProjection = mul (xView, xProjection);
	float4x4 preWorldViewProjection = mul (xWorld, preViewProjection);

	// circle point
	float4 circlePoint = input.Position;
	circlePoint.x = circlePoint.x  * Radius + Center.x;
	circlePoint.y = circlePoint.y * Radius + Center.y;
	circlePoint.z = Center.z;

	output.Position = mul (circlePoint, preWorldViewProjection);

	output.Tex0 = input.Tex0;

	return output;
}


// simple pixel shader to render the texture
float4 PixelShaderRender(VertexShaderOutput input) : COLOR0
{	
	// remap
	float x = 2 * input.Tex0.x - 1;
	float y = 2 * input.Tex0.y - 1;
	
	// calculate implicit circle formula for x + y = 1;
	float implicit =  x * x + y * y;
	
	// Calculate our upper and lower bounds based on thickness.
	float lower = 1 - Thickness;
	float upper = 1;	
	
	// remap [lower;upper] to [-1,1] and effectively clamp to [lower;upper] 	
	implicit = 2 * (implicit - lower) / Thickness - 1;
	implicit = clamp(implicit, -1, 1);
	
	// distribute for smoothing/antialiassing using 1 - implicit²
	float density = 1 - (implicit * implicit * implicit * implicit);
	
	// UNUSED - this alternative fades out more gradually, but is more costly
	// distribute for antialiassing using cos(implicit * 0.5 * Pi)
	// density = cos(implicit * PiOver2);
	
	float4 color = BaseColor;
	color.a *= density;
	
	return color;
}

// basic technique to render the texture
technique Render
{
	pass Pass1
	{        
		VertexShader = compile vs_2_0 VertexShaderFunction();
		PixelShader = compile ps_2_0 PixelShaderRender();
	}
}
