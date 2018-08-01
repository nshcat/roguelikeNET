using System;
using System.Collections.Generic;
using System.Linq;
using game.Ascii;
using game.EntityComponent;
using game.EntityComponent.Components;

namespace game
{
    /// <summary>
    /// Just a test.
    /// </summary>
    public class LifetimeSystem : EntityComponent.System
    {
        public LifetimeSystem()
            : base(new List<IEntityFilter> {
                EntityFilter<LifetimeComponent>.ByComponent()
            })
        {
            
        }
        
        protected override void Update(long elapsedTicks, EntityQueryResult entities)
        {          
            foreach(var (entity, component) in entities.GetPairs<LifetimeComponent>())
            {
                if(component.MaximumLifetime <= 50)
                    entity.ReplaceComponent(new LifetimeComponent{ MaximumLifetime = 300 });
                
                if (component.CurrentLifetime >= component.MaximumLifetime)
                {
                    EntityManager.Destroy(entity.UniqueID);
                    
                    Logger.PostMessageTagged(SeverityLevel.Debug,
                        "LifetimeSystem", $"Destroyed entity of type \"{entity.TypeName}\" because maximum lifetime was reached");
                }
                else
                {
                    component.CurrentLifetime += (int)elapsedTicks;
                }
            }
        }
    }
}