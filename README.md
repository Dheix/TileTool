# TileTool
BIN to OBJ converter for Stormworks

Simply type the directory of the file or drag&drop it to the command line and convert it.

BIN file contains vertex data for layers and vertex + uv for Tlayers.\
When converted to OBJ each layer has add color.
* layer_1 #d0d0c5 
* layer_2 #a4b874 
* layer_3 #e3d08c
* layer_4 #52b8d1
* layer_5 #ffffff
* layer_6 #8b6d5b
* layer_7 #583e2c
* layer_8 #52b8d1
* layer_9 #47a3b8
* layer_10 #3d8d9f
* layer_11 #327985

Tlayer has two textures solid line or dashed line repeating in this order.
* Tlayer_1 solid line
* Tlayer_2 dashed line
* Tlayer_3 solid line
* ...

color and texture are not converted to BIN, so chaning them has no effect. Instead, change what layer they are on.\
Layers must be triangulated and Tlayer must be quad.\
When exporting to OBJ in Blender, export only UV cordinates

![image](https://user-images.githubusercontent.com/122700205/212558027-0bab27d8-e28d-4e33-9687-313cfdcc4910.png)


