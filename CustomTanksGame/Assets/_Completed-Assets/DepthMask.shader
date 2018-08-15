
/**
* This shader is used for the plane mask material in order to simulate a split view camera
* 
* The splitted view effect would have been better with a Voronoi Shader but im not used to work with unity's HLSL/Cg language yet :(
*
*/

Shader "Masked/Mask" {
 
	SubShader {
		// Don't draw in the RGBA channels; just the depth buffer
 
		ColorMask 0
		ZWrite On
 
		// Do nothing specific in the pass:
 
		Pass {}
	}
}