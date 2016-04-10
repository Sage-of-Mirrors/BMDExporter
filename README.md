# BMDExporter

BMDExporter is a tool for converting textured Wavefront OBJ (and later Collada DAE) model files into Nintendo's BMD model format. This format is used
in many popular GameCube and Wii games, such as Super Mario Sunshine, The Legend of Zelda: The Wind Waker, and The Legend of Zelda: Twilight Princess.

## Models

Currently, BMDExporter can only export textured OBJ files to the BMD model format. In future iterations, however, DAE models will be supported, as well.

## Textures

Currently, BMDExporter supports both Targa (.tga) and .png images for textures.

## Vertex Colors

BMD files support vertex colors, and these are generally used to simulate lighting. OBJ models do not support vertex colors, and as such, BMDExporter cannot
create BMD files with them at the moment. However, BMD models with vertex colors will be supported in the future when DAE models can be exported into BMDs.

## Credits

Thanks to Dmitry Brant (http://dmitrybrant.com) for Targa (TGA) image support.