using PhysiXSharp.Core.Physics;

namespace PhysiXSharp.Core.Modularity;

public interface IPhysiXModule
{
    public void Initialize();
    public void Update();

    public void Shutdown();
}