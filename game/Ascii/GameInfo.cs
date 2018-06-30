using System.Runtime.InteropServices;

namespace game.Ascii
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct GameInfo
    {
        [MarshalAs(UnmanagedType.LPStr)]
        private string name;
        
        [MarshalAs(UnmanagedType.LPStr)]
        private string version;
        
        [MarshalAs(UnmanagedType.LPStr)]
        private string description;

        public string Name
        {
            get => name;
            set => name = value;
        }

        public string Version
        {
            get => version;
            set => version = value;
        }

        public string Description
        {
            get => description;
            set => description = value;
        }

        public GameInfo(string name, string version, string description)
        {
            this.name = name;
            this.version = version;
            this.description = description;
        }
    }
}