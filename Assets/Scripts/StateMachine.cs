public class StateMachine
{
    private IState _current;

    public void ChangeState(IState state)
    {
        if (null != _current)
        {
            _current.Exit();
        }

        _current = state;

        if (null != _current)
        {
            _current.Enter();
        }
    }

    public void Update(float dt)
    {
        if (null != _current)
        {
            _current.Update(dt);
        }
    }
}