shader_type spatial;

uniform float speed = 10.0;
uniform float distortion = 10.0;
uniform float stretch: hint_range(0.0, 0.1, 0.01) = 0.05;

void vertex() {
	VERTEX.x += sin(TIME * speed + VERTEX.y * distortion) * stretch;
}
