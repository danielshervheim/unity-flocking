using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flock : MonoBehaviour {

	[Header("Simulation Settings")]  // these should not be changed at runtime
	public Boid boidPrefab;
	public int numBoids = 150;
	public float spawnRadius = 1f;
	public float boundaryRadius = 100.0f;

	[Header("Boid Settings")]
	public float maxVelocity = 1.75f;
	public float maxAcceleration = 0.03f;
	public float seperationDistance = 35.0f;
	public float neighborDistance = 50.0f;
	[Range(0f, 360f)]
	public float fieldOfView = 300f;

	[Header("Force Multipliers")]
	public float seperationMultiplier = 1.5f;
	public float alignmentMultiplier = 1.0f;
	public float cohesionMultiplier = 1.0f;
	public float boundaryMultiplier = 1.0f;

	public Boid[] boids { get; private set; }

	void Start () {
		boids = new Boid[numBoids];

		for (int i = 0; i < boids.Length; i++) {
			boids[i] = Instantiate(boidPrefab, transform.position, transform.rotation, transform);
			boids[i].SetupSimulation(this);
		}
	}
	
	void Update () {
		foreach (Boid boid in boids) {
			boid.UpdateSimulation();
		}
	}
}