namespace PhysiXSharp.Core.Physics;

public class PhysicsManager
{
    private static PhysicsManager _instance = null!;
    private static readonly object InstanceLock = new ();

    internal static PhysicsManager Instance
    {
        get
        {
            lock (InstanceLock)
            {
                return _instance ??= new PhysicsManager();
            }
        }
    }
    
    private PhysicsManager() {}

    private int _physicsObjectIdTracker = 0;
    private readonly List<PhysicsObject> _physicsObjects = new List<PhysicsObject>();
    private readonly List<Rigidbody> _rigidbodies = new List<Rigidbody>();
    
    public void AddPhysicsObject(PhysicsObject physicsObject)
    {
        _physicsObjects.Add(physicsObject);
    }

    public void RemovePhysicsObject(PhysicsObject physicsObject)
    {
        _physicsObjects.Remove(physicsObject);
    }

    public void AddRigidbody(Rigidbody rigidbody)
    {
        _rigidbodies.Add(rigidbody);
    }

    public void RemoveRigidbody(Rigidbody rigidbody)
    {
        _rigidbodies.Remove(rigidbody);
    }

    public List<PhysicsObject> GetPhysicsObjects()
    {
        return _physicsObjects;
    }
    
    public int GetUniquePhysicsObjectId()
    {
        //Return an id and post-increment the id tracker
        return _physicsObjectIdTracker++;
    }

    public void Update()
    {
        foreach (Rigidbody rigidbody in _rigidbodies)
        {
            rigidbody.Update();
        }
    }
    
}