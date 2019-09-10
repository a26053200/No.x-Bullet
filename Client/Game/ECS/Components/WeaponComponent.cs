using Unity.Entities;

namespace ECS
{

    public struct Weapon : IComponentData
    {
        
    }
    
    public class WeaponComponent : ComponentDataProxy<Weapon>
    {
        
    }
}