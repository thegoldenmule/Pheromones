using UnityEngine;

[RequireComponent(typeof(Ant))]
public class AntMover : MonoBehaviour
{
    private enum LookMode
    {
        Scan,
        LookAt
    };

    public float MaxAntennaeAngle = 35f;
    public float HeadSpeed = 1f;
    public float TurnSpeed = 1f;

    private Ant _ant;

    private LookMode _lookMode = LookMode.Scan;
    private Vector3 _lookAtTarget;

    public Vector3 BodyForward
    {
        get;
        private set;
    }

    public Vector3 HeadForward
    {
        get;
        private set;
    }

    private void Start()
    {
        _ant = GetComponent<Ant>();
    }

    public void Scan()
    {
        _lookMode = LookMode.Scan;
    }

    public void LookAt(Vector3 position)
    {
        _lookMode = LookMode.LookAt;

        _lookAtTarget = position;
    }

    private void Update()
    {
        var dt = Time.deltaTime;

        UpdateBody(dt);
        UpdateHead(dt);
    }

    private void UpdateBody(float dt)
    {

    }

    private void UpdateHead(float dt)
    {
        if (_lookMode == LookMode.LookAt)
        {
            if (_lookAtTarget.Approximately(transform.position))
            {
                Scan();

                return;
            }

            // calculate quaternion
            var direction = _lookAtTarget - transform.position;
            var angle = Mathf.Atan2(direction.z, direction.x);
            var lookAtQuat = Quaternion.AngleAxis(
                angle * Mathf.Rad2Deg,
                Vector3.up);

            // turn head toward target
            transform.rotation = Quaternion.RotateTowards(
                transform.rotation,
                lookAtQuat,
                TurnSpeed * dt);
        }
        else if (_lookMode == LookMode.Scan)
        {
            
        }
    }
}