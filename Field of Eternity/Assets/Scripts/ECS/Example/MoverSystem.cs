using Unity.Entities;
using Unity.Transforms;
public class MoverSystem : ComponentSystem
{
    protected override void OnUpdate()
    {
        Entities.ForEach((ref Translation translation, ref MoveSpeedComponent moveSpeed) => {
            translation.Value.y += moveSpeed.moveSpeed * Time.DeltaTime;
        });
    }
}
