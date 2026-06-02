namespace PackCalculator;

internal readonly struct CString(int length, bool wide = false) : IObject
{
	private readonly int CharSize => wide ? 2 : 1;

	public readonly string Name => wide ? $"WSTRING({length})" : $"STRING({length})";

	// +1 for the null terminator
	public readonly int Size => (length + 1) * CharSize;

	public readonly int Alignment => CharSize;

	public readonly List<(MemoryType Type, string? Name, int Size)> ToMemoryView()
		=> [(MemoryType.Data, Name, Size)];
}
