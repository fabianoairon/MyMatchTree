using UnityEngine;

public class ParticleMover : MonoBehaviour
{
	private float _speed;
	private Vector3 _direction;

	void Start()
	{
		transform.position = new Vector3(transform.position.x, transform.position.y, -2);
	}

	private void Update()
	{
		if (_direction != null && _speed > 0)
		{
			transform.position += _direction * _speed * Time.deltaTime;
		}
	}

	public void SetSpeed(float speed)
	{
		_speed = speed;
	}


	public void SetDirection(Vector3 direction)
	{
		_direction = direction;
	}
}