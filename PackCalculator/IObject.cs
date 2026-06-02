namespace PackCalculator;

/// <summary>
/// Represents an element that occupies memory and can describe its own layout —
/// for example a primitive, array, string, or composite object (struct).
/// </summary>
public interface IObject
{
	/// <summary>Gets the human-readable name of the element (used in the memory view).</summary>
	string Name { get; }

	/// <summary>Gets the total size of the element in bytes, including any internal padding.</summary>
	int Size { get; }

	/// <summary>Gets the alignment requirement of the element in bytes.</summary>
	int Alignment { get; }

	/// <summary>
	/// Produces a flat memory map of the element as an ordered list of regions.
	/// Each entry describes a region's <see cref="MemoryType"/>, an optional name, and its size in bytes.
	/// </summary>
	/// <returns>An ordered list of memory regions describing the element's layout.</returns>
	List<(MemoryType Type, string? Name, int Size)> ToMemoryView();
}
