namespace PhysiXSharp.Core.Physics;

public class Constraints(bool LockX, bool LockY, bool LockRotation)
{
    public required bool LockX { get; set; } = LockX;
    public required bool LockY { get; set; } = LockY;
    public required bool LockRotation { get; set; } = LockRotation;
}