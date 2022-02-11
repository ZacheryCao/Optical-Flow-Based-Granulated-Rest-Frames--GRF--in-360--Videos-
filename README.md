# Optical-flow-triggered-granulated-rest-frames-in-360-degree-video
Research Project (Unity): Optical-flow-triggered-granulated-rest-frames-in-360-degree-video

## Optical Flow Pre-calculate
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

Put all calculated optical flow csv files (named in the format "horizontal_##(horizontal angle in degrees)_vertical_##(vertical angle in degrees)") in a folder with the name of your video the move that folder to ./Assets/Resources. Open the scene GRF. Update the videos in the script "Video Controller" attached to the videoclip in the Hierarchy. Run the scene.

https://user-images.githubusercontent.com/25666019/142672422-9230eb50-618d-4aad-97eb-480f68b7c587.mp4

You also can change the size and density of the GRF via the script "Rest Frames Generator" attached to the RFGenerator under Player in the Hierarchy. The variable "Radius" in the script is used to change the distance between the viewport and the rest frames.



