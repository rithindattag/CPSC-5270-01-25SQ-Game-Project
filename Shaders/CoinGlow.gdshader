shader_type spatial;
render_mode unshaded, cull_disabled;

uniform vec4 glow_color : source_color = vec4(1.0, 0.8, 0.0, 1.0); // bright gold
uniform float glow_strength = 3.0;

void fragment() {
    ALBEDO = glow_color.rgb;
    EMISSION = glow_color.rgb * glow_strength;
}
