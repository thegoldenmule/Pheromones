public interface IState
{
    void Enter();
    void Update(float dt);
    void Exit();
}