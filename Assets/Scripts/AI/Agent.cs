public class Agent
{
    private readonly StateMachine _fsm = new StateMachine();

    public void Command(IState state)
    {
        _fsm.ChangeState(state);
    }

    public void Update(float dt)
    {
        _fsm.Update(dt);
    }
}