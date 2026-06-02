namespace PackCalculator;

internal readonly struct Primitive(byte size) : IObject
{
	public readonly string Name => $"Primitive({size})";
	public readonly int Size => size;
	public readonly int Alignment => size;

	public readonly List<(MemoryType Type, string? Name, int Size)> ToMemoryView() => [(MemoryType.Data, Name, Size)];
}
