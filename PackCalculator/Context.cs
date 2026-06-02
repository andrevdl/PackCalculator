namespace PackCalculator;

/// <summary>
/// Holds layout configuration shared by objects, primarily the maximum packing alignment
/// (the <c>pack</c> value, equivalent to <c>#pragma pack</c> or <c>{attribute 'pack_mode'}</c>).
/// </summary>
public readonly struct Context
{
	private readonly byte _defaultPack;

	/// <summary>
	/// Initializes a new <see cref="Context"/> with the given default packing alignment.
	/// </summary>
	/// <param name="defaultPack">The maximum alignment in bytes; must be between 1 and 8 inclusive.</param>
	/// <exception cref="ArgumentOutOfRangeException">
	/// Thrown when <paramref name="defaultPack"/> is less than 1 or greater than 8.
	/// </exception>
	public Context(byte defaultPack)
	{
		if (defaultPack <= 0)
			throw new ArgumentOutOfRangeException(nameof(defaultPack), "Default pack must be greater than 0.");

		if (defaultPack > 8)
			throw new ArgumentOutOfRangeException(nameof(defaultPack), "Default pack must be less than or equal to 8.");

		_defaultPack = defaultPack;
	}

	/// <summary>
	/// Calculates the effective pack value for a member, combining the member's own pack
	/// request with the context default.
	/// </summary>
	/// <param name="memberPack">
	/// The member's requested pack value, or <c>0</c> to use the context default.
	/// </param>
	/// <returns>
	/// The context default when <paramref name="memberPack"/> is <c>0</c>; otherwise the smaller
	/// of <paramref name="memberPack"/> and the context default.
	/// </returns>
	public byte CalcPack(byte memberPack)
	{
		if (memberPack == 0)
			return _defaultPack;

		return Math.Min(memberPack, _defaultPack);
	}
}
