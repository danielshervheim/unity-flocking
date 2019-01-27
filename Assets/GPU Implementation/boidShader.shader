Shader "Custom/boidShader" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200

		CGPROGRAM

		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard vertex:vert addshadow nolightmap 
		
		// Add instancing support for this shader.
		// You need to check 'Enable Instancing' on materials that use the shader.
		#pragma instancing_options procedural:setup assumeuniformscaling

		// Use shader model 5.0 target, which is required for instancing
		// #pragma target 5.0

		#include "boidUtilities.cginc"

		half _Glossiness;
		half _Metallic;
		fixed4 _Color;

		struct Input {
			float3 position;
			float3 velocity;
			float3 acceleration;
		};

		#ifdef UNITY_PROCEDURAL_INSTANCING_ENABLED
		struct Boid {
			float3 position;
			float3 velocity;
			float3 acceleration;
		};

       	StructuredBuffer<Boid> boidBuffer;
    	#endif

       	void vert(inout appdata_full v, out Input data) {
       		UNITY_INITIALIZE_OUTPUT(Input, data);
            #ifdef UNITY_PROCEDURAL_INSTANCING_ENABLED
            // set the frag parameters
       		data.position = boidBuffer[unity_InstanceID].position;
       		data.velocity = boidBuffer[unity_InstanceID].velocity;
       		data.acceleration = boidBuffer[unity_InstanceID].acceleration;

       		// todo: something is causing jerky rotations - must investigate
       		// rotate to face direction
       		float4 quat = rotationTo(boidBuffer[unity_InstanceID].velocity, float3(0.0, 0.0, 1.0));
       		float3 positionRotated = rotateVector(quat, v.vertex.xyz);

       		// offset vertex by boid position
       		v.vertex.xyz = positionRotated + boidBuffer[unity_InstanceID].position;
			#endif
		}

		//UNITY_INSTANCING_BUFFER_START(Props)
			// put more per-instance properties here
		//UNITY_INSTANCING_BUFFER_END(Props)

		// required for DrawMeshInstancedIndirect(). Don't know why, but them's the ropes.
		void setup () { }

		void surf (Input IN, inout SurfaceOutputStandard o) {
			o.Albedo = float3(1,1,1);  // lerp(float3(0,0,0), float3(1,1,1), length(IN.velocity)/1.75);
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
		}
		ENDCG
	}
	FallBack "Diffuse"
}