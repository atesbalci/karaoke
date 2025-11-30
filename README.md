## Instructions
1. Open the project using Godot 4.5 (Mono Version)
2. Make sure that the song(s) you want to run are in the `songs` directory with the `.txt` extension
3. Run the application by clicking the play button

## Features
* Loads lyrics and their timings from a custom text file format
  * `>>` prefix on a line represents the start of a group which either has lines of lyrics or nothing if we want a cleared screen. It only can have the parameter `t` for the timing (in seconds)
  * `<>` (with parameters between) initiates a new segment of text (a line can have multiple segments in order to support parts of the lyrics progressing slower or have different visual effects):
    * `ts` (mandatory) specifies the start time of the segment (to be sung)
    * `te` (mandatory) specifies the end time of the segment
    * `style` (optional) specifies the shader effect to be applied to the segment. For the sake of demonstration, the available styles are:
      * `Regular` (default): No additional effect
      * `Wiggle`: Wiggles the letters in the segment randomly up and down
      * `ColorWave`: Runs a rainbow font color effect across the segment
* Application supports a maximum of 2 lines on the screen at a time.
* A dot visual (cursor) jumps across the words in the lyrics as the song progresses
* A fastforward button (`>>`) in the top right corner to speed up the song progression for some ease of testing
* A pause button `||` to pause the playback
* A non-functional visualization effect running in the background
* A countdown in the middle of the screen when the empty screen state is longer than 3 seconds

## AI Prompts
* `I'm working on a karaoke app prototype using Godot. Can you draft me a shader for some cool visualization effects for the background? There won't be any audio on this stage of development` <br> 
The result of this prompt was used to generate the background visual effect of this prototype. But I do need to disclaim that the generated code performs quite a lot of calculations per fragment (pixel), so it definitely needs to be simplified/optimized for a final product. For the purposes of this MVP, I left it as is.

## Demo Video
https://raw.githubusercontent.com/atesbalci/karaoke/develop/demo.mp4
