shader_type spatial;
render_mode unshaded;

uniform sampler2D floor_texture;
uniform float scroll_speed = 0.3;
uniform vec2 uv_scale = vec2(5.0, 50.0);

void fragment() {
    vec2 uv = UV * uv_scale;
    uv.y += TIME * scroll_speed;

    vec4 tex_color = texture(floor_texture, uv);

    // Edge fade calculation
    float fade_left = smoothstep(0.05, 0.15, UV.x);
    float fade_right = smoothstep(0.95, 0.85, UV.x);
    float edge_fade = fade_left * fade_right;

    ALBEDO = tex_color.rgb * edge_fade;
}
