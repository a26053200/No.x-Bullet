using Unity.Entities;

namespace Game
{

    public struct Weapon : IComponentData
    {
        
    }
    
    public class WeaponComponent : ComponentDataProxy<Weapon>
    {
        
    }
}