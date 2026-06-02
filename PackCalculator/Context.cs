namespace PackCalculator;

internal readonly struct Context
{
	private readonly byte _defaultPack;

	public Context(byte defaultPack)
	{
		if (defaultPack <= 0)
			throw new ArgumentOutOfRangeException(nameof(defaultPack), "Default pack must be greater than 0.");

		if (defaultPack > 8)
			throw new ArgumentOutOfRangeException(nameof(defaultPack), "Default pack must be less than or equal to 8.");

		_defaultPack = defaultPack;
	}

	public byte CalcPack(byte memberPack)
	{
		if (memberPack == 0)
			return _defaultPack;

		return Math.Min(memberPack, _defaultPack);
	}
}
