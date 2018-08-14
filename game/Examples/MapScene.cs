using game.Ascii;

namespace game
{
    public enum MovementAction
    {
        ActionUp,
        ActionDown,
        ActionLeft,
        ActionRight
    }
    
    public class MapScene : IScene
    {
        private TestMap map = new TestMap();
        private MapView view;
        private InputMapper testMapping = new InputMapper("test");

        public MapScene()
        {
            view = new MapView(map, Position.Origin, new Dimensions(50, 30));
            view.Center = new Position(10, 10);
        }
        
        public void update(long elapsedTicks)
        {
            if(testMapping.HasInput(MovementAction.ActionDown))
                view.Center += new Position(0, 1);
              
            if(testMapping.HasInput(MovementAction.ActionUp))
                view.Center -= new Position(0, 1);
            
            if(testMapping.HasInput(MovementAction.ActionLeft))
                view.Center -= new Position(1, 0);
              
            if(testMapping.HasInput(MovementAction.ActionRight))
                view.Center += new Position(1, 0);
        }

        public void render()
        {
            view.Render();
            Screen.SetTile(new Position(50/2, 30/2), new Tile(Color.Black, Color.Red, 1));
        }
    }
}