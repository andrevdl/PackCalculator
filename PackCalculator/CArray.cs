namespace PackCalculator;

/// <summary>
/// A fixed-length array of a single element type. Elements are laid out contiguously, so the
/// array's size is <c>length * element.Size</c> and its alignment matches the element's alignment.
/// </summary>
/// <param name="length">The number of elements in the array.</param>
/// <param name="element">The element type that is repeated.</param>
public readonly struct CArray(int length, IObject element) : IObject
{
	/// <inheritdoc/>
	public readonly string Name => $"Array[{length}] of {element.Name}";

	/// <inheritdoc/>
	public readonly int Size => length * element.Size;

	/// <inheritdoc/>
	public readonly int Alignment => element.Alignment;

	/// <inheritdoc/>
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
