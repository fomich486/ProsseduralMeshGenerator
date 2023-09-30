Shader "Custom/NewSurfaceShader"
{
    SubShader
    {
        //start 
        CGPROGRAM
        #pragma surface surf Lambert

        //#pragma indicates a directive (https://docs.unity3d.com/Manual/SL-SurfaceShaders.html)
        //for a surface shader, need to use #pragma surface namOfSurfaceFunction nameOfLightingModel [optional parameters]

        //calling this Input is just convention
        struct Input
        {
            //input variable for the vertex colour - rgba so float4
            float4 vertColour : COLOR;
        };

        void surf(Input IN, inout SurfaceOutput o)
        {
            //set the mesh's albedo to be the vertex colour.
            o.Albedo = IN.vertColour; //.rgb;
        }
        ENDCG
    }
    FallBack "Diffuse"
}