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
// https://gist.github.com/aeroson/043001ca12fe29ee911e

float3 safeNormalize(float3 vec) {
	if (length(vec) < ZERO) {
		return float3(0, 0, 0);
	}
	else {
		return normalize(vec);
	}
}

float4 lookRotation(float3 forward, float3 up) {
	forward = safeNormalize(forward);
	float3 right = safeNormalize(cross(up, forward));
	up = cross(forward, right);

	float m00 = right.x;
	float m01 = right.y;
	float m02 = right.z;
	float m10 = up.x;
	float m11 = up.y;
	float m12 = up.z;
	float m20 = forward.x;
	float m21 = forward.y;
	float m22 = forward.z;

	float num8 = (m00 + m11) + m22;

	float4 quaternion = float4(0, 0, 0, 0);

	if (num8 > 0) {
		float num = sqrt(num8 + 1);
		quaternion.w = num * 0.5;
		num = 0.5 / num;
		quaternion.x = (m12 - m21) * num;
		quaternion.y = (m20 - m02) * num;
		quaternion.z = (m01 - m10) * num;
		return quaternion;
	}
	if ((m00 >= m11) && (m00 >= m22)) {
		float num7 = sqrt(((1 + m00) - m11) - m22);
		float num4 = 0.5 / num7;
		quaternion.x = 0.5 * num7;
		quaternion.y = (m01 + m10) * num4;
		quaternion.z = (m02 + m20) * num4;
		quaternion.w = (m12 - m21) * num4;
		return quaternion;
	}
	if (m11 > m22) {
		float num6 = sqrt(((1 + m11) - m00) - m22);
		float num3 = 0.5 / num6;
		quaternion.x = (m10 + m01) * num3;
		quaternion.y = 0.5 * num6;
		quaternion.z = (m21 + m12) * num3;
		quaternion.w = (m20 - m02) * num3;
		return quaternion;
	}
	float num5 =sqrt(((1 + m22) - m00) - m11);
	float num2 = 0.5 / num5;
	quaternion.x = (m20 + m02) * num2;
	quaternion.y = (m21 + m12) * num2;
	quaternion.z = 0.5 * num5;
	quaternion.w = (m01 - m10) * num2;
	return quaternion;
}



// the following functions were found from
// https://pastebin.com/fAFp6NnN

float3 rotateVector(float3 vec, float4 quat) {
	float3 result = float3(0, 0, 0);
	float num12 = quat.x + quat.x;
    float num2 = quat.y + quat.y;
    float num = quat.z + quat.z;
    float num11 = quat.w * num12;
    float num10 = quat.w * num2;
    float num9 = quat.w * num;
    float num8 = quat.x * num12;
    float num7 = quat.x * num2;
    float num6 = quat.x * num;
    float num5 = quat.y * num2;
    float num4 = quat.y * num;
    float num3 = quat.z * num;
    float num15 = ((vec.x * ((1 - num5) - num3)) + (vec.y * (num7 - num9))) + (vec.z * (num6 + num10));
    float num14 = ((vec.x * (num7 + num9)) + (vec.y * ((1 - num8) - num3))) + (vec.z * (num4 - num11));
    float num13 = ((vec.x * (num6 - num10)) + (vec.y * (num4 + num11))) + (vec.z * ((1 - num8) - num5));
    result.x = num15;
    result.y = num14;
    result.z = num13;
    return result;
}
