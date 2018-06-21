using System.Runtime.InteropServices;

namespace game.Ascii.Native
{
    [StructLayout(LayoutKind.Explicit)]
    public struct RenderCommand
    {
        [FieldOffset(0)]
        private RenderCommandType type;

        [FieldOffset(4)]
        private Position position;

        [FieldOffset(12)]
        private Color color;
        
        [FieldOffset(12)]
        private byte value;

        public RenderCommand(RenderCommandType t, Position p)
        {
            this.type = t;
            this.position = p;

            color = Color.White;
            value = 0;
        }
        

        public RenderCommandType Type
        {
            get => type;
            set => type = value;
        }

        public Position Position
        {
            get => position;
            set => position = value;
        }

        public Color Color
        {
            get => color;
            set => color = value;
        }

        public byte Value
        {
            get => value;
            set => this.value = value;
        }

        public static RenderCommand ClearTileCommand(Position p)
        {
            return new RenderCommand(RenderCommandType.ClearTile, p);
        }
        
        public static RenderCommand SetBackgroundCommand(Position p, Color c)
        {
            return new RenderCommand(RenderCommandType.SetBackground, p)
            {
               Color = c
            };
        }
        
        public static RenderCommand SetForegroundCommand(Position p, Color c)
        {
            return new RenderCommand(RenderCommandType.SetForeground, p)
            {
                Color = c
            };
        }
        
        public static RenderCommand SetGlyphCommand(Position p, byte g)
        {
            return new RenderCommand(RenderCommandType.SetGlyph, p)
            {
                Value = g
            };
        }
        
        public static RenderCommand SetDepthCommand(Position p, byte d)
        {
            return new RenderCommand(RenderCommandType.SetDepth, p)
            {
                Value = d
            };
        }
    }
}