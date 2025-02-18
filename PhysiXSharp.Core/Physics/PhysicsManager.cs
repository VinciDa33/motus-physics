namespace PhysiXSharp.Core.Physics;

internal class PhysicsManager
{
    private static PhysicsManager _instance;
    private static readonly object _instanceLock = new ();

    public static PhysicsManager Instance
    {
        get
        {
            lock (_instanceLock)
            {
                return _instance ??= new PhysicsManager();
            }
        }
    }
    
    private PhysicsManager() {}

    private int _physicsObjectIdTracker = 0;
    private readonly List<PhysicsObject> _physicsObjects = new List<PhysicsObject>();

    public void AddPhysicsObject(PhysicsObject physicsObject)
    {
        _physicsObjects.Add(physicsObject);
    }

    public int GetUniquePhysicsObjectId()
    {
        //Return an id and post-increment the id tracker
        return _physicsObjectIdTracker++;
    }

    public void Update()
    {
        
    }
    
}