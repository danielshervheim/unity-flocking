using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GPUFlock : MonoBehaviour {

	private struct Boid {
		public Vector3 position;
		public Vector3 velocity;
		public Vector3 acceleration;
	}

	private const int BOID_SIZE = 36;  // 3*3*sizeof(float)



	[Header("Boid Assets")]
	public Mesh mesh;
	public int submeshIndex = 0;  // for multi-part meshes
	public Material material;
	public ComputeShader compute;
	private int computeKernel;

	[Header("Simulation Settings")]
	public int count = 100000;
	public float boundaryRadius = 1000f;
	public float spawnRadius = 100f;

	[Header("Boid Settings")]
	public float maxVelocity = 1.75f;
	public float maxSteeringForce = 0.03f;
	public float seperationDistance = 35.0f;
	public float neighborDistance = 50.0f;
	[Range(0f, 360f)]
	public float fieldOfView = 300f;

	[Header("Force Weight Adjustments")]
	public float seperationScale = 1.5f;
	public float alignmentScale = 1.0f;
	public float cohesionScale = 1.0f;
	public float boundaryScale = 1.0f;

	private ComputeBuffer argsBuffer;
	private ComputeBuffer boidBuffer;

	// the private cached variables to detect changes
	private float cachedBoundaryRadius;
	private float cachedMaxVelocity;
	private float cachedMaxSteeringForce;
	private float cachedSeperationDistance;
	private float cachedNeighborDistance;
	private float cachedFieldOfView;
	private float cachedSeperationScale;
	private float cachedAlignmentScale;
	private float cachedCohesionScale;
	private float cachedBoundaryScale;


	// Use this for initialization
	void Start () {
		// verify the count is valid
		if (count < 1) {
			count = 1;
		}

		// create and set the args array
		uint[] args = new uint[5] {0, 0, 0, 0, 0};
		if (mesh != null) {
			args[0] = (uint)mesh.GetIndexCount(0);
			args[1] = (uint)count;
			args[2] = (uint)mesh.GetIndexStart(0);
			args[3] = (uint)mesh.GetBaseVertex(0);
		}

		// transfer the args array to the args buffer
		argsBuffer = new ComputeBuffer(1, args.Length * sizeof(uint), ComputeBufferType.IndirectArguments);
		argsBuffer.SetData(args);

		// create and set the boids array
		Boid[] boids = new Boid[count];
		for (int i = 0; i < count; i++) {
			boids[i].position = Random.insideUnitSphere * spawnRadius;
			boids[i].velocity = Random.insideUnitSphere;
			boids[i].acceleration = Vector3.zero;
		}

		// transfer the boids array to the boids buffer
		boidBuffer = new ComputeBuffer(count, BOID_SIZE);
		boidBuffer.SetData(boids);

		// upload the buffer to the shaders
		computeKernel = compute.FindKernel("CSMain");
		compute.SetBuffer(computeKernel, "boidBuffer", boidBuffer);
		material.SetBuffer("boidBuffer", boidBuffer);

		// initialize the cached variables
		cachedBoundaryRadius = boundaryRadius;
		cachedMaxVelocity = maxVelocity;
		cachedMaxSteeringForce = maxSteeringForce;
		cachedSeperationDistance = seperationDistance;
		cachedNeighborDistance = neighborDistance;
		cachedFieldOfView = fieldOfView;
		cachedSeperationScale = seperationScale;
		cachedAlignmentScale = alignmentScale;
		cachedCohesionScale = cohesionScale;
		cachedBoundaryScale = boundaryScale;

		// warmup shader
		Shader.WarmupAllShaders();
	}
	
	// Update is called once per frame
	void Update () {
		// update parameters if they have changed
		if (cachedBoundaryRadius != boundaryRadius) {
			compute.SetFloat("boundaryRadius", boundaryRadius);
			cachedBoundaryRadius = boundaryRadius;
		}

		if (cachedMaxVelocity != maxVelocity) {
			compute.SetFloat("maxVelocity", maxVelocity);
			cachedMaxVelocity = maxVelocity;
		}

		if (cachedMaxSteeringForce != maxSteeringForce) {
			compute.SetFloat("maxSteeringForce", maxSteeringForce);
			cachedMaxSteeringForce = maxSteeringForce;
		}

		if (cachedSeperationDistance != seperationDistance) {
			compute.SetFloat("seperationDistance", seperationDistance);
			cachedSeperationDistance = seperationDistance;
		}

		if (cachedNeighborDistance != neighborDistance) {
			compute.SetFloat("neighborDistance", neighborDistance);
			cachedNeighborDistance = neighborDistance;
		}

		if (cachedFieldOfView != fieldOfView) {
			compute.SetFloat("fieldOfView", fieldOfView);
			cachedFieldOfView = fieldOfView;
		}

		if (cachedSeperationScale != seperationScale) {
			compute.SetFloat("seperationScale", seperationScale);
			cachedSeperationScale = seperationScale;
		}

		if (cachedAlignmentScale != alignmentScale) {
			compute.SetFloat("alignmentScale", alignmentScale);
			cachedAlignmentScale = alignmentScale;
		}

		if (cachedCohesionScale != cohesionScale) {
			compute.SetFloat("cohesionScale", cohesionScale);
			cachedCohesionScale = cohesionScale;
		}

		if (cachedBoundaryScale != boundaryScale) {
			compute.SetFloat("boundaryScale", boundaryScale);
			cachedBoundaryScale = boundaryScale;
		}

		// dispatch compute shader
		compute.Dispatch(computeKernel, count, 1, 1);
	}

	void LateUpdate () {  // void OnRenderObject () {
		material.SetPass(0);
		Graphics.DrawMeshInstancedIndirect(mesh, submeshIndex, material,
			new Bounds(transform.position, Vector3.one * 2f * boundaryRadius), argsBuffer);
	}

	void OnDestroy () {
		if (argsBuffer != null) {
			argsBuffer.Release();
		}

		if (boidBuffer != null) {
			boidBuffer.Release();
		}
	}
}
