namespace PackCalculator;

internal interface IObject
{
	string Name { get; }
	int Size { get; }
	int Alignment { get; }

	List<(MemoryType Type, string? Name, int Size)> ToMemoryView();
}
