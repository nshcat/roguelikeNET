using System.Runtime.InteropServices;

namespace game.Ascii
{
    /// <summary>
    /// A structure that is used to send information about the game to libascii.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct GameInfo
    {
        [MarshalAs(UnmanagedType.LPStr)]
        private string name;
        
        [MarshalAs(UnmanagedType.LPStr)]
        private string version;
        
        [MarshalAs(UnmanagedType.LPStr)]
        private string description;
        
        [MarshalAs(UnmanagedType.LPStr)]
        private string windowTitle;

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

        public string WindowTitle
        {
            get => windowTitle;
            set => windowTitle = value;
        }

        public GameInfo(string name, string version, string description, string windowTitle)
        {
            this.name = name;
            this.version = version;
            this.description = description;
            this.windowTitle = windowTitle;
        }
    }
}