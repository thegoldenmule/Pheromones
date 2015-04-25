public class Agent
{
    private readonly StateMachine _commands = new StateMachine();

    private readonly StateMachine _behaviors = new StateMachine();

    public void Command(ICommand command)
    {
        if (null != command)
        {
            command.Initialize(_behaviors);
        }

        _commands.ChangeState(command);
    }

    public void Update(float dt)
    {
        _commands.Update(dt);
        _behaviors.Update(dt);
    }
}