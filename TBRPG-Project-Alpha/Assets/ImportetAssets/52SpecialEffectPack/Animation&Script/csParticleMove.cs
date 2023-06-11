using UnityEngine;
using System.Collections;

public class csParticleMove : MonoBehaviour
{
    private float speed = 0.1f;

	private void Update () {
        transform.Translate(Vector3.back * speed);
	}
}
