namespace PhysiXSharp.Core.Modularity;

public interface IPhysiXModule
{
    public void Initialize(); //May need some objects
    public void Update(); //May need some delta time
}