using UnityEngine;
using System.Collections;

[System.Serializable]
public enum Direction {
    north, east, south, west
}

[System.Serializable]
public struct IntVector2 {
	public int x, y;

	public IntVector2 (int x, int y)
	{
		this.x = x;
		this.y = y;
	}


	public override bool Equals(object obj)
	{
		return (x == ((IntVector2)obj).x && (y == ((IntVector2)obj).y));
	}
	public override int GetHashCode()
	{
		return 0;
	}
	static public explicit operator Vector3(IntVector2 intVec2)
	{
		return new Vector3(intVec2.x, 0, intVec2.y);
	}
	static public explicit operator Vector2(IntVector2 intVec2)
	{
		return new Vector2(intVec2.x, intVec2.y);
	}

	public static IntVector2 operator +(IntVector2 a, IntVector2 b)
	{
		return new IntVector2(a.x + b.x, a.y + b.y);
	}
	public static IntVector2 operator -(IntVector2 a, IntVector2 b)
	{
		return new IntVector2(a.x - b.x, a.y - b.y);
	}
	public static bool operator ==(IntVector2 a, IntVector2 b)
	{
		return (a.x == b.x && a.y == b.y);
	}
	public static bool operator !=(IntVector2 a, IntVector2 b)
	{
		return !(a.x == b.x && a.y == b.y);
	}

    public static int ManDist(IntVector2 a, IntVector2 b) {
        return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y);
    }

    public static IntVector2 zero = new IntVector2(0, 0);
	public static IntVector2 up = new IntVector2(0, 1);
	public static IntVector2 right = new IntVector2(1, 0);
	public static IntVector2 down = new IntVector2(0, -1);
    public static IntVector2 left = new IntVector2(-1, 0);
    public static IntVector2 error = new IntVector2(-420420, -420420);

    public static IntVector2 fromDirection(Direction d)
	{
		if (d == Direction.north)
			return up;
		else if (d == Direction.east)
			return right;
		else if (d == Direction.south)
			return down;
		else
			return left;
	}

    public override string ToString() {
        return "(" + x.ToString() + "," +y.ToString() + ")";
    }
}
