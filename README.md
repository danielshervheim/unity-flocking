# Flocking in Unity
A Unity implementation of Craig Reynold's boids.

[![gif](https://media.giphy.com/media/jnUMgg3GFzhwUxxwfg/giphy.gif)](https://youtu.be/JlhW6CCkrhY "Flocking in Unity")

*20,000 boids simulated entirely on the GPU, running at 60 fps [(on Youtube)](https://youtu.be/JlhW6CCkrhY).*

---

There are 2 versions, a CPU version and a GPU version.

Both versions feature the rules described in Reynold's original paper (seperation, cohesion, alignment), as well as a boundary force to keep them within a specified area.

Both are naive (i.e. O(N<sup>2</sup>)) implementations. On my computer, the CPU version can handle **150** or so boids before slowing down, while the GPU version can handle about **20,000**.

The GPU version requires a graphics card that supports Compute shaders and GPU instancing in Unity.

The CPU version also includes a visualization script to visualize some of the parameters of the boids (its neighbors, the forces acting on it, and its path over time).

## Todo

- Implement fish vertex animation and bird wing vertex animation.
- Add obstacle / enviroment detection.
- Add predator / prey boid types.