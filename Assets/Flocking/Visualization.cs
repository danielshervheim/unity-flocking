using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum TrailType { NONE, INFINITE, DURATION };

public enum TrailContent { NEITHER, VELOCITY, ACCELERATION };

[RequireComponent(typeof(Flock))]
public class Visualization : MonoBehaviour {
	[Header("Global On/Off")]
	public bool visualize = false;

	[Header("Which Boids")]
	public int startIndex = 0;
	public int endIndex = 0;

	[Header("Boundary Options")]
	public bool showBoundary = false;
	public Color boundaryColor = Color.gray;

	[Header("Trail Options")]
	public TrailType trailType = TrailType.NONE;
	public float trailDuration = 10f;
	public TrailContent trailContent = TrailContent.NEITHER;
	public Color trailColor = Color.red;

	[Header("Neighbor Options")]
	public bool showNeighbors = false;
	public Color neighborColor = Color.blue;

	[Header("Force Options")]
	public bool showSeperationForce = false;
	public bool showAlignmentForce = false;
	public bool showCohesionForce = false;
	public bool showBoundaryForce = false;
	public float forceLength = 10f;

	private Flock flock;

	// Use this for initialization
	void Start () {
		flock = GetComponent<Flock>();
	}
	
	// Update is called once per frame
	void Update () {
		if (visualize) {
			Boid[] boids = flock.boids;
			for (int i = 0; i < boids.Length; i++) {
				if (i >= startIndex && i <= endIndex) {
					Visualize(ref boids, i);
				}
			}
		}
	}

	void Visualize(ref Boid[] boids, int i) {
		if (trailType != TrailType.NONE) {
			Color color = trailColor;
			if (trailContent == TrailContent.ACCELERATION) {
				color = Color.Lerp(Color.black, trailColor, Mathf.Clamp(boids[i].acceleration.magnitude / flock.maxAcceleration, 0f, 1f));
			}
			else if (trailContent == TrailContent.VELOCITY) {
				color = Color.Lerp(Color.black, trailColor, Mathf.Clamp(boids[i].velocity.magnitude / flock.maxVelocity, 0f, 1f));
			}

			float dur = trailType == TrailType.INFINITE ? Mathf.Infinity : trailDuration;
			Debug.DrawLine(boids[i].position, boids[i].position - boids[i].velocity, color, dur, true);
		}

		if (showNeighbors && boids[i].neighborIndices != null) {
			foreach (int j in boids[i].neighborIndices) {
				Debug.DrawLine(boids[i].position, boids[j].position, neighborColor, 0f, false);
			}
		}

		if (showSeperationForce) {
			Debug.DrawLine(boids[i].position, boids[i].position + Vector3.Normalize(boids[i].seperation)*forceLength, Color.red, 0f, false);
		}

		if (showAlignmentForce) {
			Debug.DrawLine(boids[i].position, boids[i].position + Vector3.Normalize(boids[i].alignment)*forceLength, Color.blue, 0f, false);
		}

		if (showCohesionForce) {
			Debug.DrawLine(boids[i].position, boids[i].position + Vector3.Normalize(boids[i].cohesion)*forceLength, Color.green, 0f, false);
		}

		if (showBoundaryForce) {
			if (boids[i].boundary != Vector3.zero) {
				Debug.DrawLine(boids[i].position, boids[i].position + Vector3.Normalize(boids[i].boundary)*forceLength, Color.black, 0f, false);
			}
		}
	}

	void OnDrawGizmosSelected() {
		if (visualize) {
			if (flock == null) {
				flock = GetComponent<Flock>();
			}
			if (showBoundary) {
       	 		Gizmos.color = boundaryColor;
        		Gizmos.DrawWireSphere(transform.position, flock.boundaryRadius);
			}
		}
    }	
}
