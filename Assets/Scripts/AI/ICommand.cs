public interface ICommand : IState
{
    void Initialize(StateMachine behaviors);
}