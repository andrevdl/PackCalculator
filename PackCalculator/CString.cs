namespace PackCalculator;

/// <summary>
/// A TwinCAT/PLC-style string field. Models <c>STRING(length)</c> (1 byte per character) or
/// <c>WSTRING(length)</c> (2 bytes per character) and reserves one extra character for the
/// null terminator.
/// </summary>
/// <param name="length">The declared character capacity, excluding the null terminator.</param>
/// <param name="wide">
/// <see langword="true"/> for a wide <c>WSTRING</c> (2-byte characters, 2-byte alignment);
/// <see langword="false"/> for a single-byte <c>STRING</c> (1-byte alignment).
/// </param>
public readonly struct CString(int length, bool wide = false) : IObject
{
	private readonly int CharSize => wide ? 2 : 1;

	/// <inheritdoc/>
	public readonly string Name => wide ? $"WSTRING({length})" : $"STRING({length})";

	/// <inheritdoc/>
	// +1 for the null terminator
	public readonly int Size => (length + 1) * CharSize;

	/// <inheritdoc/>
	public readonly int Alignment => CharSize;

	/// <inheritdoc/>
	public readonly List<(MemoryType Type, string? Name, int Size)> ToMemoryView()
		=> [(MemoryType.Data, Name, Size)];
}
