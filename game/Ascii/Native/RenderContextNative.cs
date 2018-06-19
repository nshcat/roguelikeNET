using System;
using System.Runtime.InteropServices;

namespace game.Ascii.Native
{
    public static class RenderContextNative
    {    	    
	    [DllImport("libascii")]
	    public static extern bool render_context_should_close();
	    
	    [DllImport("libascii")]
	    public static extern void render_context_begin_frame();
	    
	    [DllImport("libascii")]
	    public static extern bool render_context_end_frame();
    }
}