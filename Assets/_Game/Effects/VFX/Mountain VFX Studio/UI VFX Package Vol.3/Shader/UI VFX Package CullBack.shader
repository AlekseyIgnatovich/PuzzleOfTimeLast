
Shader "UI VFX Package/UI VFX Package CullBack" 
{
	Properties {
		_MainTex ("Particle Texture", 2D) = "white" {}
	}

	Category {
		Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
		Blend SrcAlpha One
		ColorMask RGB
		Cull Back
		ZWrite Off
		Fog { Mode Off }
	
		BindChannels {
			Bind "Color", color
			Bind "Vertex", vertex
			Bind "TexCoord", texcoord
		}
	
		SubShader {
			Pass {
				SetTexture [_MainTex] {
					combine texture * primary
				}
			}
		}
	}
}