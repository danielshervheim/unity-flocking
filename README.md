# Flocking in Unity
A Unity implementation of Craig Reynold's boids.

[![gif](https://media.giphy.com/media/jnUMgg3GFzhwUxxwfg/giphy.gif)](https://youtu.be/JlhW6CCkrhY "Flocking in Unity")

*20,000 boids simulated entirely on the GPU, running at 60 fps ([watcb on Youtube](https://youtu.be/JlhW6CCkrhY)).*

## About

Includes CPU and GPU implementations.

Both are naïve O(N<sup>2</sup>) implementations. The CPU version can handle about **150** boids at 60 fps, while the GPU version can handle about **20,000**. (As tested on a Ryzen 5, Nvidia GTX 1060 machine. Your mileage may vary).

The GPU version requires a graphics card that supports Compute shaders and GPU instancing in Unity.

The CPU version includes a script to visualize some of the parameters of the boids. (Nearest neighbors, forces, and path over time).

## To-do

- Implement vertex animations for swimming fish and flying birds.
- Improve performance with some kind of spatial data structure.
- Add environmental awareness / obstacle avoidance.