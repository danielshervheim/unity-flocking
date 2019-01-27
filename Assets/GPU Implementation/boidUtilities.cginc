#define ZERO 1e-15
#define PI 3.14159265359
#define RAD2DEG (180.0/PI)



// the following functions were found from
// https://github.com/Unity-Technologies/UnityCsReference/blob/master/Runtime/Export/Vector3.cs

float3 setMagnitude(float3 dir, float len) {
	if (length(dir) < ZERO) {
		return float3(0.0, 0.0, 0.0);
	}
	else {
		return normalize(dir) * len;
	}
}

float3 clampMagnitude(float3 dir, float max) {
	if (length(dir) > max) {
		return setMagnitude(dir, max);
	}
	else {
		return dir;
	}
}

float sqrMagnitude(float3 vec) {
	return vec.x*vec.x + vec.y*vec.y + vec.z*vec.z;
}

float angleBetween(float3 from, float3 to) {
	float denominator = sqrt(sqrMagnitude(from) * sqrMagnitude(to));

	if (denominator < ZERO) {
		return 0.0;
	}

	float dotted = clamp(dot(from, to) / denominator, -1.0, 1.0);
	return acos(dotted) * RAD2DEG;
}



// the following functions were found from
// https://gist.github.com/nkint/7449c893fb7d6b5fa83118b8474d7dcb

float4 setAxisAngle(float3 axis, float rad) {
	rad = rad * 0.5;
	float s = sin(rad);
	return float4(s*axis.x, s*axis.y, s*axis.z, cos(rad));
}

float4 rotationTo(float3 a, float3 b) {
	float vecDot = dot(a, b);
	float3 tmp = float3(0, 0, 0);
	if (vecDot < -1.0 + ZERO) {
		tmp = cross(float3(1, 0, 0), a);
		if (length(tmp) < ZERO) {
			tmp = cross(float3(0, 1, 0), a);
		}
		tmp = normalize(tmp);
		return setAxisAngle(tmp, PI);
	}
	else if (vecDot > 1.0 - ZERO) {
		return float4(0, 0, 0, 1);
	}
	else {
		tmp = cross(a, b);
		float4 tmp2 = float4(tmp, 1.0 + vecDot);
		return normalize(tmp2);
	}
}

float4 multQuat(float4 a, float4 b) {
	return float4(
		a.w * b.x + a.x * b.w + a.z * b.y - a.y * b.z,
	    a.w * b.y + a.y * b.w + a.x * b.z - a.z * b.x,
	    a.w * b.z + a.z * b.w + a.y * b.x - a.x * b.y,
	    a.w * b.w - a.x * b.x - a.y * b.y - a.z * b.z
	);
}

float3 rotateVector(float4 quat, float3 vec) {
	float4 quatv = multQuat(quat, float4(vec, 0.0));
	return multQuat(quatv, float4(-1.0 * quat.xyz, quat.w)).xyz;
}
