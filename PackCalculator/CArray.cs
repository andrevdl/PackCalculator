namespace PackCalculator;

internal readonly struct CArray(int length, IObject element) : IObject
{
	public readonly string Name => $"Array[{length}] of {element.Name}";

	public readonly int Size => length * element.Size;

	public readonly int Alignment => element.Alignment;

	public readonly List<(MemoryType Type, string? Name, int Size)> ToMemoryView()
	{
		List<(MemoryType Type, string? Name, int Size)> view = [];
		
		view.Add((MemoryType.Start, Name, 0));
		for (int i = 0; i < length; i++)
			view.AddRange(element.ToMemoryView());
		view.Add((MemoryType.End, null, Size));

		return view;
	}
}
