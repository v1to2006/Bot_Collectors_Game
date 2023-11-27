using UnityEngine;

public class CameraMovement : MonoBehaviour
{
	[SerializeField] private float _movementSpeed;
	[SerializeField] private float _verticalMovementSpeed;

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

		if (Input.GetKey(KeyCode.E))
		{
			transform.Translate(Vector3.up * _verticalMovementSpeed * Time.deltaTime, Space.World);
		}
		else if (Input.GetKey(KeyCode.Q))
		{
			transform.Translate(Vector3.down * _verticalMovementSpeed * Time.deltaTime, Space.World);
		}
	}
}
