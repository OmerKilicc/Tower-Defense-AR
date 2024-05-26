using System;
using UnityEngine;
using UnityEngine.UIElements;

public class NewEnemy : GameBehavior
{
	[SerializeField]
	Transform _model = default;

	EnemyFactory _originFactory;
	GameTile _tileFrom, _tileTo;

	Direction _direction;
	DirectionChange _directionChange;
	Vector3 _positionFrom, _positionTo;

	float _progress, _progressFactor;
	float _directionAngleFrom, _directionAngleTo;
	float _pathOffset;
	float _speed;
    float Health { get; set; }

    public float Scale { get; private set; }

    public EnemyFactory OriginFactory
	{
		get => _originFactory;
		set
		{
			Debug.Assert(_originFactory == null, "Redefined origin factory!");
			_originFactory = value;
		}
	}

	// Enemy alive check, if alive update the progress
	public override bool GameUpdate()
	{
        if (Health <= 0f)
        {
            OriginFactory.Reclaim(this);
            return false;
        }

        _progress += Time.deltaTime * _progressFactor;
		while (_progress >= 1f)
		{
			// Checks if we reached the destination if so kills the enemy returns false
			if (_tileTo == null)
			{
				OriginFactory.Reclaim(this);
				return false;
			}

			_progress = (_progress - 1f) / _progressFactor;
			PrepareNextState();
			_progress *= _progressFactor;
		}

		if (_directionChange == DirectionChange.None)
		{
			transform.localPosition = Vector3.LerpUnclamped(_positionFrom, _positionTo, _progress);
		}

		else
		{
			float angle = Mathf.LerpUnclamped(_directionAngleFrom, _directionAngleTo, _progress);
			transform.localRotation = Quaternion.Euler(0f, angle, 0f);
		}
		return true;
	}

	public void Initialize(float scale, float speed, float pathOffset)
	{
        Scale = scale;
        _model.localScale = new Vector3(scale, scale, scale);
		this._speed = speed;
		this._pathOffset = pathOffset;
        Health = 100f * scale;
    }

    public void ApplyDamage(float damage)
    {
        Debug.Assert(damage >= 0f, "Negative damage applied.");
        Health -= damage;
    }

    // Handles what to do when spawned at point
    // progress is zero because we just spawned
    // init path for enemy when spawned
    public void SpawnOn(GameTile tile)
	{
		Debug.Assert(tile.NextTileOnPath != null, "Nowhere to go!", this);

		_tileFrom = tile;
		_tileTo = tile.NextTileOnPath;

		_progress = 0f;
		PrepareIntro();
	}


	private void PrepareNextState()
	{
		_tileFrom = _tileTo;
		_tileTo = _tileTo.NextTileOnPath;

		_positionFrom = _positionTo;

		// if we are in destination tile
		if (_tileTo == null)
		{
			PrepareOutro();
			return;
		}

		_positionTo = _tileFrom.ExitPoint;

		_directionChange = _direction.GetDirectionChangeTo(_tileFrom.PathDirection);
		_direction = _tileFrom.PathDirection;
		_directionAngleFrom = _directionAngleTo;

		switch (_directionChange)
		{
			case DirectionChange.None: PrepareForward(); break;
			case DirectionChange.TurnRight: PrepareTurnRight(); break;
			case DirectionChange.TurnLeft: PrepareTurnLeft(); break;
			default: PrepareTurnAround(); break;
		}
	}

	void PrepareForward()
	{
		transform.localRotation = _direction.GetRotation();
		_directionAngleTo = _direction.GetAngle();
		_model.localPosition = new Vector3(_pathOffset, 0f);
		_progressFactor = _speed;
	}
	void PrepareTurnRight()
	{
		_directionAngleTo = _directionAngleFrom + 90f;
		_model.localPosition = new Vector3(_pathOffset - 0.5f, 0f);
		transform.localPosition = _positionFrom + _direction.GetHalfVector();
		_progressFactor = _speed / (Mathf.PI * 0.5f * (0.5f - _pathOffset));
	}
	void PrepareTurnLeft()
	{
		_directionAngleTo = _directionAngleFrom - 90f;
		_model.localPosition = new Vector3(_pathOffset + 0.5f, 0f);
		transform.localPosition = _positionFrom + _direction.GetHalfVector();
		_progressFactor = _speed / (Mathf.PI * 0.5f * (0.5f + _pathOffset));
	}
	void PrepareTurnAround()
	{
		_directionAngleTo = _directionAngleFrom + (_pathOffset < 0f ? 180f : -180f);
		_model.localPosition = new Vector3(_pathOffset, 0f);
		transform.localPosition = _positionFrom;
		_progressFactor = _speed / (Mathf.PI * Mathf.Max(Mathf.Abs(_pathOffset), 0.2f));
	}
	private void PrepareIntro()
	{
		_positionFrom = _tileFrom.transform.localPosition;
		_positionTo = _tileFrom.ExitPoint;

		_direction = _tileFrom.PathDirection;
		_directionChange = DirectionChange.None;
		_directionAngleFrom = _directionAngleTo = _direction.GetAngle();

		_model.localPosition = new Vector3(_pathOffset, 0f);
		transform.localRotation = _tileFrom.PathDirection.GetRotation();

		_progressFactor = 2f * _speed;
	}

	void PrepareOutro()
	{
		_positionTo = _tileFrom.transform.localPosition;

		_directionChange = DirectionChange.None;
		_directionAngleTo = _direction.GetAngle();

		_model.localPosition = new Vector3(_pathOffset, 0f);
		transform.localRotation = _direction.GetRotation();

		_progressFactor = 2f * _speed;
	}


}
