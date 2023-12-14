using UnityEngine;

public class CameraMovement : MonoBehaviour
{
	[SerializeField] private float _movementSpeed;
	[SerializeField] private float _verticalMovementSpeed;

	[SerializeField] private KeyCode _upKey;
	[SerializeField] private KeyCode _downKey;

	private void Update()
	{
		Move();
	}

	private void Move()
	{
		const string AxisHorizontal = "Horizontal";
		const string AxisVertical = "Vertical";

		float horizontalInput = Input.GetAxisRaw(AxisHorizontal);
		float verticalInput = Input.GetAxisRaw(AxisVertical);

		Vector3 movement = new Vector3(horizontalInput, 0f, verticalInput).normalized;

		transform.Translate(movement * _movementSpeed * Time.deltaTime, Space.World);

		if (Input.GetKey(_upKey))
		{
			transform.Translate(Vector3.up * _verticalMovementSpeed * Time.deltaTime, Space.World);
		}
		else if (Input.GetKey(_downKey))
		{
			transform.Translate(Vector3.down * _verticalMovementSpeed * Time.deltaTime, Space.World);
		}
	}
}
