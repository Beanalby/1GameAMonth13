import bpy
from math import radians

print("running!")

for letter in range(32,127):
# for letter in range(50,51):    
    bpy.ops.object.text_add()
    obj = bpy.context.object
    obj.name = str(letter)
    obj.rotation_euler=[radians(90),0,radians(180)]
    text = obj.data
    text.body=chr(letter)
    text.bevel_depth=.01
    text.extrude=.05
    text.align='CENTER'
    
