namespace game.Ascii
{
    public static class CommonMath
    {
	    public static double LMap(double val, double minA, double maxA, double minB, double maxB)
	    {
		    return (val - minA) * (maxB - minB) / (maxA - minA) + minB;
	    }
	    
	    public static float LMap(float val, float minA, float maxA, float minB, float maxB)
	    {
		    return (val - minA) * (maxB - minB) / (maxA - minA) + minB;
	    }
    }
}