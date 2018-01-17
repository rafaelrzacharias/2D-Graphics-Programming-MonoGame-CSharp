# 2D Graphics Programming MonoGame CSharp

This was my first project.

Please visit: http://www.monogame.net/2017/03/01/monogame-3-6/

Download and install MonoGame for Visual Studio.

Download the complete project with solution and placement art, build and enjoy!


NOTES:

- In this project I aimed to create the basic building blocks for a 2D sidescrolling game.

- The project is coded in C# and uses MonoGame (old Microsoft XNA) framework.

- I built a sprite class, an animation class to use spritesheets in a configurable way.

- Sprites can be automatically reordered to overlay each other based on y-coordinate.

- Move character using ARROW keys. Drag, acceleration and inertia are present in the movement.

- There is a particle system with the ability to be expanded to include many effects.

- Press Numpad 0 to 9 to see some example effects. I explored the ability to use rendering effects such as alphablend, non-multiplied, premultiplied and additive modes.

- The Controller class was created to teach myself about controlling a character on screen, it can be controlled using keyboard and also switch automatically to an Xbox One Gamepad is connected.

- Simple physics engine was created to showcase concepts such as mass, air drag, floor friction, velocity and acceleration for characters and also used in the interaction between particles in the particle system.

- A Camera system was created to be customizable, zoom level, locked/unlocked, and follow player.
