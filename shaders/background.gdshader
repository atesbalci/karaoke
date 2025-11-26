shader_type canvas_item;

// Uniforms for customization
uniform float time_scale = 1.0;
uniform float wave_speed = 0.5;
uniform float wave_intensity = 0.3;
uniform vec3 color1 : source_color = vec3(0.8, 0.2, 0.9); // Purple
uniform vec3 color2 : source_color = vec3(0.2, 0.6, 1.0); // Blue
uniform vec3 color3 : source_color = vec3(1.0, 0.3, 0.5); // Pink

void fragment() {
    vec2 uv = UV;
    float time = TIME * time_scale;
    
    // Create flowing waves
    float wave1 = sin(uv.x * 3.0 + time * wave_speed) * cos(uv.y * 2.0 - time * wave_speed * 0.7);
    float wave2 = sin(uv.x * 2.0 - time * wave_speed * 0.8) * sin(uv.y * 3.0 + time * wave_speed);
    float wave3 = cos(uv.x * 4.0 + uv.y * 4.0 - time * wave_speed * 1.2);
    
    // Combine waves
    float combined = (wave1 + wave2 + wave3) * wave_intensity;
    
    // Create gradient based on position and waves
    float gradient = length(uv - vec2(0.5)) + combined;
    
    // Animated color mixing
    vec3 color = mix(color1, color2, sin(gradient * 3.0 + time) * 0.5 + 0.5);
    color = mix(color, color3, cos(gradient * 2.0 - time * 0.7) * 0.5 + 0.5);
    
    // Add some sparkle/energy spots
    float sparkle = 0.0;
    for(float i = 0.0; i < 3.0; i++) {
        vec2 sparkle_pos = vec2(
            sin(time * (0.5 + i * 0.3) + i * 2.0) * 0.3 + 0.5,
            cos(time * (0.4 + i * 0.2) + i * 3.0) * 0.3 + 0.5
        );
        float dist = length(uv - sparkle_pos);
        sparkle += 0.02 / (dist * dist + 0.01);
    }
    
    // Add sparkle glow
    color += vec3(sparkle) * 0.5;
    
    // Pulsing vignette effect
    float vignette = 1.0 - length(uv - vec2(0.5)) * 0.8;
    vignette *= (0.8 + sin(time * 2.0) * 0.2);
    color *= vignette;
    
    COLOR = vec4(color, 1.0);
}