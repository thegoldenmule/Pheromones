using UnityEngine;

[RequireComponent(typeof(Ant))]
public class AntMover : MonoBehaviour
{
    public enum LookMode
    {
        Scan,
        LookAt
    };

    public float MaxAntennaeDegrees = 35f;

    public float WalkSpeed = 1f;
    public float RunSpeed = 1.5f;
    public float HeadSpeed = 1f;
    public float TurnSpeed = 1f;
    
    private Ant _ant;

    private Vector3 _moveToTarget;

    private LookMode _lookMode = LookMode.Scan;
    private Vector3 _lookAtTarget;
    private float _scanParameter = 0f;

    public void Scan()
    {
        _lookMode = LookMode.Scan;
    }

    public void LookAt(Vector3 position)
    {
        _lookMode = LookMode.LookAt;

        _lookAtTarget = position;
    }

    public void MoveTo(
        Vector3 moveToTarget,
        LookMode lookMode = LookMode.Scan)
    {
        _moveToTarget = moveToTarget;

        if (lookMode == LookMode.Scan)
        {
            Scan();
        }
        else if (lookMode == LookMode.LookAt)
        {
            LookAt(moveToTarget);
        }
    }

    private void Start()
    {
        _ant = GetComponent<Ant>();
    }

    private void Update()
    {
        var dt = Time.deltaTime;

        UpdateBody(dt);
        UpdateHead(dt);
    }

    private void UpdateBody(float dt)
    {
        if (_ant.transform.position.Approximately(_moveToTarget))
        {
            return;
        }

        var direction = _moveToTarget - _ant.transform.position;
        var distance = direction.magnitude;
        direction /= distance;

        // rotate body towards target
        _ant.transform.forward =
            Vector3.RotateTowards(
                _ant.transform.forward,
                direction,
                dt * TurnSpeed,
                0f);

        // move towards
        _ant.transform.position += Mathf.Min(distance, dt * WalkSpeed) * direction;
    }

    private void UpdateHead(float dt)
    {
        if (null == _ant.Head)
        {
            return;
        }

        if (_lookMode == LookMode.LookAt)
        {
            if (_lookAtTarget.Approximately(transform.position))
            {
                // go back to scanning
                Scan();

                return;
            }

            // calculate quaternion
            var direction = _lookAtTarget - _ant.Head.position;
            var angle = Mathf.Atan2(direction.z, direction.x);
            var lookAtQuat = Quaternion.AngleAxis(
                angle * Mathf.Rad2Deg,
                Vector3.up);

            // turn head toward target
            _ant.Head.rotation = Quaternion.RotateTowards(
                transform.rotation,
                lookAtQuat,
                dt * TurnSpeed);
        }
        else if (_lookMode == LookMode.Scan)
        {
            var delta = dt * TurnSpeed;
            _scanParameter = (_scanParameter + delta) % 2f;

            var integer = Mathf.FloorToInt(_scanParameter);
            var mantissa = _scanParameter - integer;
            var p = integer % 2 == 0
                ? mantissa
                : 1f - mantissa;

            // ping pong in [-MaxAntennaeDegrees, MaxAntennaeDegrees]
            var degrees = (2f * p  - 1f) * MaxAntennaeDegrees;
            _ant.Head.localRotation = Quaternion.Euler(
                0f,
                degrees,
                0f);
        }
    }
}