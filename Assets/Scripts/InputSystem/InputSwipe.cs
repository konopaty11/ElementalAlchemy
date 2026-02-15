using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.UIElements;

public class InputSwipe : MonoBehaviour
{
	[SerializeField] GeneralConfig generalConfig;

	public static UnityAction<Direction> OnSwipeTrack;

	InputSystem_Actions _inputSystem;

	Vector2 _startPosition;
	bool _isTracking;
	Direction _direction;

	void Awake()
	{
		_inputSystem = new();
	}

	void OnEnable()
	{
		_inputSystem.Enable();
		_inputSystem.UI.Touch.started += OnTouchStarted;
		_inputSystem.Player.Look.performed += OnLookPerformed;
	}

	void OnDisable()
	{
        _inputSystem.Disable();
        _inputSystem.UI.Touch.started -= OnTouchStarted;
        _inputSystem.Player.Look.performed -= OnLookPerformed;
    }

	void OnTouchStarted(InputAction.CallbackContext _context)
	{
		if (Touchscreen.current == null) return;

		_startPosition = GetPrimaryPosition();
		_isTracking = true;
	}

	void OnLookPerformed(InputAction.CallbackContext _context)
	{
		if (!_isTracking) return;

		Vector2 _delta = _context.ReadValue<Vector2>();
		Direction _currentDirection = GetDirection(_delta);
		Vector2 _currentPosition = GetPrimaryPosition();

		//Debug.Log(_delta);

		if (_direction == Direction.None)
		{
			_direction = _currentDirection;
		}
		else if (_direction != _currentDirection)
		{
			//Debug.Log("change direction");
			_direction = _currentDirection;
			_startPosition = _currentPosition;
		}

		float _distance = Vector2.Distance(_startPosition, _currentPosition);
		if (_distance >= generalConfig.MinSwipeLenght)
		{
			_isTracking = false;
			OnSwipeTrack?.Invoke(_direction);
            _direction = Direction.None;
        }
    }

	Vector2 GetPrimaryPosition()
	{
		TouchControl _touch = Touchscreen.current.primaryTouch;
		return _touch.ReadValue().position;
	}

	Direction GetDirection(Vector2 _delta)
	{
		if (Mathf.Abs(_delta.x) > Mathf.Abs(_delta.y))
		{
			if (_delta.x > 0)
				return Direction.Right;
			else if (_delta.x < 0)
				return Direction.Left;
		}
		else if (Mathf.Abs(_delta.x) < Mathf.Abs(_delta.y))
		{
			if (_delta.y > 0)
				return Direction.Top;
			else if (_delta.y < 0)
				return Direction.Bottom;
		}

		return Direction.None;
	}
}
