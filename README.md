# Flocking in Unity
A Unity implementation of Craig Reynolds famous boid literature.

![screenshot](https://i.imgur.com/CaDNBkm.png)

### This readme is a WIP!

The basic CPU implementation is done. It features the 3 rules described in Reynolds original literature (seperation, alignment, cohesion) as well as a repellant force to keep the boids contrained within a radius.

To see the flock in action, just load up the `FlockScene` and press the play button. Additionally, there are parameters you can change to alter the behavior of the flock.

The **Simulation Settings** should not be modified during runtime.

The **Boid Settings** and **Force Multipliers** can be modified freely at runtime.

Finally, there is also an included script to visualize (in the editor window, not the play view) the forces acting on the boids, as well as their neighbor relations, and paths over time.

![options](https://i.imgur.com/uDXdKGe.png)

## todo

- Write readme! :)
- Make a video or gif demo of a flock in action and add to readme.
- Add predator/prey boid types.
- Add obstacles / enviroment detection.
- GPU / Compute shader version. Perhaps particle system (?).