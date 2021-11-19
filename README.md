# Optical-flow-triggered-granulated-rest-frames-in-360-degree-video
Research Project (Unity): Optical-flow-triggered-granulated-rest-frames-in-360-degree-video

## Optical Flow Pre-calculate (Optical_FLow(Net2)_v4.ipynb in ./VideoPreprocess)
### Equirectangular view to perspective view
Class Equirec2Perpec: Function: GetPerspective(FOV, THETA, PHI, height, width).
FOV: the field of view of each eye in the headset.
THETA: left/right angle in degrees of view center (right direction is positive, left direction is negative)
PHI: up/down angle in degrees of view center (up is positive, down is negative)
height, width: height/width of the output viewport image, should fit the resolution of each eye's viewport in the headset

### Calculate the optical flow
Upload the video via cell Uploading smaple video. It should be a video in your google drive. Then run the subcells in Optical flow calculation


https://user-images.githubusercontent.com/25666019/142665487-bd5edbd6-d16b-483c-8056-f9d457d944fb.mp4


## Run the Scene

Run the scene GRF.
