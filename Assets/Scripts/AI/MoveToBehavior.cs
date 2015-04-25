using UnityEngine;

public class MoveToBehavior : IState
{
    private Ant _ant;
    private Vector3 _target;

    public bool HasArrived
    {
        get
        {
            var position = _ant.transform.position;
            return Mathf.Abs(position.x - _target.x) < Mathf.Epsilon
                && Mathf.Abs(position.y - _target.y) < Mathf.Epsilon
                && Mathf.Abs(position.z - _target.z) < Mathf.Epsilon;
        }
    }

    public MoveToBehavior(
        Ant ant,
        Vector3 target)
    {
        _ant = ant;
        _target = target;
    }

    public void Enter()
    {

    }

    public void Update(float dt)
    {
        var delta = _target - _ant.transform.position;
        var distance = delta.magnitude;
        var direction = delta / distance;

        _ant.transform.position += Mathf.Min(_ant.Speed * dt, distance) * direction;
    }

    public void Exit()
    {

    }
}