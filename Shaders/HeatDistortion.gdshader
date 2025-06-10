shader_type spatial;
render_mode unshaded;

uniform sampler2D floor_texture;
uniform float scroll_speed = 0.3;

void fragment() {
    vec2 uv = UV;
    uv.y += TIME * scroll_speed;
    ALBEDO = texture(floor_texture, uv).rgb;
}
