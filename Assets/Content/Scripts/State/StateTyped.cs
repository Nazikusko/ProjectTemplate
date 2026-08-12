public abstract class StateTyped<T> : State
{
    protected T MachineObject;

    protected StateTyped(T machineObject)
    {
        MachineObject = machineObject;
    }
}
