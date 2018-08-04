using System.Collections.Generic;
using game.Ascii;
using game.EntityComponent;
using game.EntityComponent.Components;

namespace game
{
    public class TestObserverSystem : ObserverSystem
    {
        public TestObserverSystem()
            : base(new List<IEntityFilter>{
                EntityFilter<LifetimeComponent>.ByComponent()
            })
        {
        }

        protected override void Update(long elapsedTicks, EntityQueryResult entities)
        {
            // Do nothing
        }

        protected override void OnEntityRemoved(Entity e)
        {
            Logger.PostMessageTagged(SeverityLevel.Debug, "TestObserverSystem", $"Entity of type {e.TypeName} got removed");
        }

        protected override void OnEntityAdded(Entity e)
        {
            Logger.PostMessageTagged(SeverityLevel.Debug, "TestObserverSystem", $"Entity of type {e.TypeName} got added");
        }
    }
}