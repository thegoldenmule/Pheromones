public class StateMachine
{
    public IState Current
    {
        get;
        private set;
    }

    public void ChangeState(IState state)
    {
        if (null != Current)
        {
            Current.Exit();
        }

        Current = state;

        if (null != Current)
        {
            Current.Enter();
        }
    }

    public void Update(float dt)
    {
        if (null != Current)
        {
            Current.Update(dt);
        }
    }
}