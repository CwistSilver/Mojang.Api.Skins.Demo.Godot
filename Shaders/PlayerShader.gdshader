shader_type spatial;

render_mode blend_mix, depth_draw_opaque, cull_back, diffuse_lambert, specular_schlick_ggx, depth_prepass_alpha;

uniform sampler2D Texture2DParameter : hint_default_black, filter_nearest;



void fragment() {
	vec4 n_out2p0;
// Texture2D:2
	n_out2p0 = texture(Texture2DParameter, UV);


// FloatConstant:7
	float n_out7p0 = 1.000000;


// FloatConstant:4
	float n_out4p0 = 0.000000;


// FloatConstant:5
	float n_out5p0 = 1.000000;


if (n_out2p0.a == 0.0) {
        ALBEDO = vec3(0.0, 0.0, 0.0);
    } else {
        ALBEDO = n_out2p0.rgb;
    }
	METALLIC = 0.0;
ROUGHNESS = 1.0;
// Output:0
	//ALBEDO = vec3(n_out2p0.xyz);
	//ALPHA = n_out7p0;
	//METALLIC = n_out4p0;
	//ROUGHNESS = n_out5p0;


}