namespace PackCalculator;

/// <summary>
/// Identifies the kind of region in a memory view produced by <see cref="IObject.ToMemoryView"/>.
/// </summary>
public enum MemoryType
{
	/// <summary>Actual data occupied by a field.</summary>
	Data,

	/// <summary>Padding inserted between members to satisfy a member's alignment.</summary>
	DataPadding,

	/// <summary>Trailing padding added so the object's size is a multiple of its alignment.</summary>
	ObjectPadding,

	/// <summary>Marks the beginning of a composite element (object or array); increases nesting depth.</summary>
	Start,

	/// <summary>Marks the end of a composite element (object or array); decreases nesting depth.</summary>
	End,
}
