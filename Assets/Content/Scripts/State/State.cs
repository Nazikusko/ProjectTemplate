public abstract class State
{
    public abstract void Enter();

    public abstract void Exit();

    public abstract void Update();

    public virtual void FixedUpdate()
    {

    }
}
