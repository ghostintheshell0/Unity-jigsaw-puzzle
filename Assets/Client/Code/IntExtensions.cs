public static class IntExtensions
{
	public static bool Contains(this int value, int mask)
	{
		return (value & mask) == mask;
	}
}