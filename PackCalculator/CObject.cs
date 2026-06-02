using System.Diagnostics;

namespace PackCalculator;

/// <summary>
/// A composite object (struct) built from ordered members. Computes member offsets, inter-member
/// (data) padding, trailing (object) padding, total size, and alignment following C / TwinCAT /
/// <see cref="System.Runtime.InteropServices.StructLayoutAttribute"/> rules.
/// </summary>
/// <param name="name">The display name of the object.</param>
/// <param name="context">The layout context providing the default packing alignment.</param>
/// <param name="objectPack">
/// The object's own pack value, or <c>0</c> to use the context default.
/// </param>
[DebuggerDisplay("Name = {Name}, Size = {Size}, Pack = {objectPack}")]
public struct CObject(string name, Context context, byte objectPack) : IObject
{
	/// <inheritdoc/>
	public readonly string Name => name;

	private int _size;
	private int _maxAlignment = 1;

	/// <inheritdoc/>
	public readonly int Size => _size + CalcPaddingTo(_size, _maxAlignment);

	/// <inheritdoc/>
	public readonly int Alignment => _maxAlignment;

	private readonly List<(int ByteOffset, IObject Member)> _members = [];

	/// <summary>
	/// Gets the members added so far, each paired with its computed byte offset within the object.
	/// </summary>
	public readonly IReadOnlyList<(int ByteOffset, IObject Member)> Members => _members;

	/// <summary>
	/// Appends a member to the object, inserting any leading padding required to satisfy the
	/// member's alignment and updating the object's size and overall alignment.
	/// </summary>
	/// <param name="member">The member to add.</param>
	public void AddMember(IObject member)
	{
		int alignment = Math.Min(member.Alignment, context.CalcPack(objectPack));
		if (alignment > _maxAlignment)
			_maxAlignment = alignment;

		int padding = CalcPaddingTo(_size, alignment);

		_members.Add((_size + padding, member));
		_size += member.Size + padding;
	}

	private static int CalcPaddingTo(int currentSize, int alignment)
		=> (alignment - (currentSize % alignment)) % alignment;

	/// <inheritdoc/>
	public readonly List<(MemoryType Type, string? Name, int Size)> ToMemoryView()
	{
		List<(MemoryType Type, string? Name, int Size)> view = [];
		int offset = 0;
		
		view.Add((MemoryType.Start, Name, 0));
		foreach (var (ByteOffset, Member) in _members)
		{
			if (ByteOffset > offset)
				view.Add((MemoryType.DataPadding, null, ByteOffset - offset));

			view.AddRange(Member.ToMemoryView());
			offset = ByteOffset + Member.Size;
		}

		int finalPadding = CalcPaddingTo(_size, _maxAlignment);
		if (finalPadding > 0)
			view.Add((MemoryType.ObjectPadding, null, finalPadding));
		
		view.Add((MemoryType.End, null, Size));
		return view;
	}
}