shader_type spatial;

uniform vec3 waterColor: source_color;
uniform vec2 topDirection = vec2(0.1, 0.0);
uniform float topSpeed: hint_range(0.0, 1.0, 0.1) = 0.5;
uniform sampler2D topNoiseTexture: repeat_enable, filter_linear;

uniform vec2 bottomDirection = vec2(0.0, 0.1);
uniform float bottomSpeed: hint_range(0.0, 1.0, 0.1) = 0.2;
uniform sampler2D bottomNoiseTexture: repeat_enable, filter_linear;

void fragment() {
	vec2 topOffset = topDirection * topSpeed * TIME; 
	vec2 bottomOffset = bottomDirection * bottomSpeed * TIME;
	vec4 topNormal = texture(topNoiseTexture, UV  + topOffset);
	vec4 bottomNormal = texture(bottomNoiseTexture, UV + bottomOffset);
	NORMAL_MAP = mix(topNormal.rgb, bottomNormal.rgb, 0.5);
	//
	ALBEDO = waterColor;
	METALLIC = 0.0;
	ROUGHNESS = 0.01;
}