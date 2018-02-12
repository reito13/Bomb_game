// @takashicompany (http://takashicompany.com)

using UnityEngine;
using System.Collections;

namespace TakashiCompany.Unity.Util
{
	/// <summary>
	/// Trajectory calculate.
	/// </summary>
	public static class TrajectoryCalculate
	{

		/// <summary>
		/// Calculates the trajectory by force.
		/// </summary>
		/// <returns>The trajectory position at time.</returns>
		/// <param name="start">Start Position.</param>
		/// <param name="force">Force.(e.g. rigidbody.AddRelativeForce(force))</param>
		/// <param name="mass">Mass.(e.g. rigidbody.mass)</param>
		/// <param name="gravity">Gravity.(e.g. Physics.gravity)</param>
		/// <param name="gravityScale">Gravity scale.(e.g. rigidbody2D.gravityScale)</param>
		/// <param name="time">Time.</param>
		public static Vector3 Force(
			Vector3 start,
			Vector3 force,
			float mass,
			Vector3 gravity,
			float gravityScale,
			float time
		)
		{
			var speedX = force.x / mass;
			var speedY = force.y / mass;
			var speedZ = force.z / mass;

			var halfGravityX = gravity.x * 0.5f * gravityScale;
			var halfGravityY = gravity.y * 0.5f * gravityScale;
			var halfGravityZ = gravity.z * 0.5f * gravityScale;

			var positionX = speedX * time + halfGravityX * Mathf.Pow(time, 2);
			var positionY = speedY * time + halfGravityY * Mathf.Pow(time, 2);
			var positionZ = speedZ * time + halfGravityZ * Mathf.Pow(time, 2);

			return start + new Vector3(positionX, positionY, positionZ);
		}

		/// <summary>
		/// Calculates the trajectory by velocity.
		/// </summary>
		/// <returns>The trajectory position at time.</returns>
		/// <param name="start">Start Position.</param>
		/// <param name="velocity">Velocity.(e.g. rigidbody.velocity)</param>
		/// <param name="gravity">Gravity.(e.g. Physics.gravity)</param>
		/// <param name="gravityScale">Gravity scale.(e.g. rigidbody2D.gravityScale)</param>
		/// <param name="time">Time.</param>
		public static Vector3 Velocity(
			Vector3 start,
			Vector3 velocity,
			Vector3 gravity,
			float gravityScale,
			float time
		)
		{
			var halfGravityX = gravity.x * 0.5f * gravityScale;
			var halfGravityY = gravity.y * 0.5f * gravityScale;
			var halfGravityZ = gravity.z * 0.5f * gravityScale;

			var positionX = velocity.x * time + halfGravityX * Mathf.Pow(time, 2);
			var positionY = velocity.y * time + halfGravityY * Mathf.Pow(time, 2);
			var positionZ = velocity.z * time + halfGravityZ * Mathf.Pow(time, 2);

			return start + new Vector3(positionX, positionY, positionZ);
		}
	}
}