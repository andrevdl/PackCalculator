namespace PackCalculator;

/// <summary>
/// A scalar field of a fixed size (for example <c>BYTE</c>, <c>INT</c>, or <c>LREAL</c>).
/// A primitive's alignment equals its size.
/// </summary>
/// <param name="size">The size of the primitive in bytes, which is also its alignment.</param>
public readonly struct Primitive(byte size) : IObject
{
	/// <inheritdoc/>
	public readonly string Name => $"Primitive({size})";

	/// <inheritdoc/>
	public readonly int Size => size;

	/// <inheritdoc/>
	public readonly int Alignment => size;

	/// <inheritdoc/>
	public readonly List<(MemoryType Type, string? Name, int Size)> ToMemoryView() => [(MemoryType.Data, Name, Size)];
}
