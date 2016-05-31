Shader "Custom/Wire over" 
 {
	Properties 
	{
		// properties for wireframe shader
		_Color ("Line Color", Color) = (1,1,1,1)
		_MainTex ("Wireframe Main Texture", 2D) = "white" {}
		_baseMainTex ("Color (RGB) Alpha (A)", 2D) = "white" {}
		_Thickness ("Thickness", Float) = 1
		
		
		// texture transition properties
		_DetailTex ("Detail (RGB)", 2D) = "white" {}
        _Guide ("Guide (RGB)", 2D) = "white" {}
        _Threshold ("Threshold",Range(0,1)) = 0.0
        
        // outline texture properties
        _OutlineThickness("Outline Thickness", Range(0,50)) = 0.0
	}
	
	
	SubShader {
		//////////////////////////////////////////////////////////////
		// clear object Pass
		//
		//	-	This pass will ensure the vertices are not drawn on the
		//		interior of the object, so that the outline pass will
		//		work.
		//////////////////////////////////////////////////////////////	
		 Pass
         {

		 Tags { "Queue"="Geometry" "IgnoreProjector"="True" "RenderType"="Transparent" }
         LOD 200
         Blend SrcAlpha OneMinusSrcAlpha
		 Cull Back
		 ZTest Always
		 ZWrite Off

             Stencil
			 {
                 Ref 1
                 Comp always
                 Pass replace
             }
             CGPROGRAM
             #pragma vertex vert
             #pragma fragment frag
             #pragma multi_compile_fog
             
             #include "UnityCG.cginc"
             
             struct v2g 
             {
                 float4  pos : SV_POSITION;
                 float2  uv : TEXCOORD0;
                 float3 viewT : TANGENT;
                 float3 normals : NORMAL;
             };
             
             struct g2f 
             {
                 float4  pos : SV_POSITION;
                 float2  uv : TEXCOORD0;
                 float3  viewT : TANGENT;
                 float3  normals : NORMAL;
             };
	
			// Vertex Shader
             v2g vert(appdata_base v)
             {
                 v2g OUT;
                 OUT.pos = mul(UNITY_MATRIX_MVP, v.vertex);
                 OUT.uv = v.texcoord; 
                  OUT.normals = v.normal;
                 OUT.viewT = ObjSpaceViewDir(v.vertex);
                 
                 return OUT;
             }  
          
			// Fragment Shader
             fixed4 frag(g2f IN) : SV_Target
             {				
				return 0;           
             }
             ENDCG
         }
		
		  //////////////////////////////////////////////////////////////
		// transparent shader	
		//
		//	-	This pass takes two textures and, using a third texture,
		//		maps them. The "detail texture" is set to 0.0f Alpha 
		//		and Albedo, meaning it is completely transparent.
		//////////////////////////////////////////////////////////////	
		 CGPROGRAM
         #pragma surface surf Lambert alpha
		 
         sampler2D _baseMainTex;
         sampler2D _DetailTex;
         sampler2D _Guide;
         float _Threshold;
 
 
         struct Input {
             float2 uv_baseMainTex;
             float2 uv_DetailTex;
             float2 uv_Guide;
         };

         void surf (Input IN, inout SurfaceOutput o) 
         {
             half4 c = tex2D (_baseMainTex, IN.uv_baseMainTex);
             half4 d = tex2D (_DetailTex, IN.uv_DetailTex);
             half4 g = tex2D (_Guide, IN.uv_Guide);
             
             if((g.r+g.g+g.b)*0.33333f<_Threshold)
             {
                 o.Albedo = d.rgb;
                 o.Alpha = d.a;
             }
                     else
                     {
                         o.Albedo = 0.0f;
                         o.Alpha = 0.0f;
                     }            
         }
         ENDCG

		//////////////////////////////////////////////////////////////
		// Outline shader
		//
		//	-	This pass takes the vertices and creates an outline
		//		by taking every edge of the mesh and turns them into
		//		quads facing the camera.
		//////////////////////////////////////////////////////////////	

         Pass 
         {
			ZWrite On // used for drawing semi-transparent effects
			
		 
             Stencil {
                 Ref 0
                 Comp equal
             }
             CGPROGRAM
             #include "UnityCG.cginc"
             #pragma target 4.0
             #pragma vertex vert
             #pragma geometry geom
             #pragma fragment frag
             
             
             half4 _Color;
             float _OutlineThickness;
         
             struct v2g  // vertex to geometry data
             {
                 float4 pos : SV_POSITION; // vertex position
                 float2 uv : TEXCOORD0; // texture coordinates
                 float3 viewT : TANGENT; // tangent
                 float3 normals : NORMAL; // normals
             };
             
             struct g2f // geometry to fragment
             {
                 float4 pos : SV_POSITION; 
                 float2 uv : TEXCOORD0;
                 float3 viewT : TANGENT;
                 float3 normals : NORMAL;
             };
 
             v2g vert(appdata_base v) // converts vertex data to geometry data
             {
                 v2g OUT;
                 OUT.pos = mul(UNITY_MATRIX_MVP, v.vertex);
                 
                 OUT.uv = v.texcoord;
                  OUT.normals = v.normal;
                 OUT.viewT = ObjSpaceViewDir(v.vertex);
                 
                 return OUT;
             }
             
             void geom2(v2g start, v2g end, inout TriangleStream<g2f> triStream)
             {
                 float thisWidth = _OutlineThickness/100; // define thickness
                 float4 parallel = start.pos-end.pos; // define parallel
                 normalize(parallel);
                 parallel *= thisWidth;
                 
                 float4 perpendicular = float4(parallel.y,-parallel.x, 0, 0); // define perpendicular
                 perpendicular = normalize(perpendicular) * thisWidth;
                 float4 v1 = start.pos-parallel;
                 float4 v2 = end.pos+parallel;

                 g2f OUT; // define output
                 OUT.pos = v1-perpendicular; 
                 OUT.uv = start.uv;
                 OUT.viewT = start.viewT;
                 OUT.normals = start.normals;
                 triStream.Append(OUT);
                 
                 OUT.pos = v1+perpendicular;
                 triStream.Append(OUT);
                 
                 OUT.pos = v2-perpendicular;
                 OUT.uv = end.uv;
                 OUT.viewT = end.viewT;
                 OUT.normals = end.normals;
                 triStream.Append(OUT);
                 
                 OUT.pos = v2+perpendicular;
                 OUT.uv = end.uv;
                 OUT.viewT = end.viewT;
                 OUT.normals = end.normals;
                 triStream.Append(OUT);
             }
             
             [maxvertexcount(12)]
             void geom(triangle v2g IN[3], inout TriangleStream<g2f> triStream) // generate the triangles for fragment shader
             {
                 geom2(IN[0],IN[1],triStream);
                 geom2(IN[1],IN[2],triStream);
                 geom2(IN[2],IN[0],triStream);
             }
             
             half4 frag(g2f IN) : COLOR
             {
                 _Color.a = 1; // render the geometry
                 return _Color;
             }
             
             ENDCG
 
         }	
				
		//////////////////////////////////////////////////////////////
		// wireframe shader
		//
		//	-	
		//////////////////////////////////////////////////////////////
		Pass {
			Tags { "RenderType"="Transparent" }
			
			Blend SrcAlpha OneMinusSrcAlpha 
			ZWrite On
			ZTest Less
			LOD 200

			
		CGPROGRAM
			
			#pragma target 5.0
			//include the vertex data
			#pragma vertex vert
			#pragma fragment frag
			#pragma geometry geom
			#include "UnityCG.cginc"
			
			
			// PARAMETERS //
			float _Thickness = 1;		// Thickness of the wireframe line rendering
			float4 _Color = {0,0,0,1};	// Color of the line
			float4 _MainTex_ST;			// For the Main Tex UV transform
			sampler2D _MainTex;			// Texture used for the line
			
			// create vertex struct
			struct v2g
			{
			float4	pos		: POSITION;		// vertex position
			float2  uv		: TEXCOORD0;	// vertex uv coordinate
			};
		
			//create geometry struct
			struct g2f
			{
			float4	pos		: POSITION;		// fragment position
			float2	uv		: TEXCOORD0;	// fragment uv coordinate
			float3  dist	: TEXCOORD1;	// distance to each edge of the triangle
			};
		
			// Vetex Shader
			v2g vert(appdata_base v)
			{
			v2g output;
			output.pos =  mul(UNITY_MATRIX_MVP, v.vertex);
			output.uv = TRANSFORM_TEX (v.texcoord, _MainTex);//v.texcoord;

			return output;
			}
			
			// geometry shader
			[maxvertexcount(3)]
			void geom(triangle v2g p[3], inout TriangleStream<g2f> triStream)
			{
				//points in screen space
				float2 p0 = _ScreenParams.xy * p[0].pos.xy / p[0].pos.w;
				float2 p1 = _ScreenParams.xy * p[1].pos.xy / p[1].pos.w;
				float2 p2 = _ScreenParams.xy * p[2].pos.xy / p[2].pos.w;
				
				//edge vectors
				float2 v0 = p2 - p1;
				float2 v1 = p2 - p0;
				float2 v2 = p1 - p0;

				//area of the triangle
			 	float area = abs(v1.x*v2.y - v1.y * v2.x);

				//values based on distance to the edges
				float dist0 = area / length(v0);
				float dist1 = area / length(v1);
				float dist2 = area / length(v2);
				
				g2f pIn;
				
				//add the first point
				pIn.pos = p[0].pos;
				pIn.uv = p[0].uv;
				pIn.dist = float3(dist0,0,0);
				triStream.Append(pIn);

				//add the second point
				pIn.pos =  p[1].pos;
				pIn.uv = p[1].uv;
				pIn.dist = float3(0,dist1,0);
				triStream.Append(pIn);
				
				//add the third point
				pIn.pos = p[2].pos;
				pIn.uv = p[2].uv;
				pIn.dist = float3(0,0,dist2);
				triStream.Append(pIn);
			}
			
			// Fragment Shader
			float4 frag(g2f input) : COLOR
			{			
				//find the smallest distance
				float val = min( input.dist.x, min( input.dist.y, input.dist.z));
				
				//calculate power to 2 to thin the line
				val = exp2( -1/_Thickness * val * val );
					
				//blend between the lines and the negative space to give illusion of anti aliasing
				float4 targetColor = _Color * tex2D( _MainTex, input.uv);
				float4 transCol = _Color * tex2D( _MainTex, input.uv);
				transCol.a = 0;
				return val * targetColor + ( 1 - val ) * transCol;
			}
			ENDCG
		 }	


		
		 
		 
	} 
} 