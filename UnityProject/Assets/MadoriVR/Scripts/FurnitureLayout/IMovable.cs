namespace MadoriVR.Scripts.FurnitureLayout
{
    public interface IMovable
    {
        void ChangePosition(MoveCommand command);
        void ChangeRotation(float angle);
        float GetRotation();
    }
}