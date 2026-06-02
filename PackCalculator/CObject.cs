using System.Diagnostics;

namespace PackCalculator;

[DebuggerDisplay("Name = {Name}, Size = {Size}, Pack = {objectPack}")]
internal struct CObject(string name, Context context, byte objectPack) : IObject
{
	public readonly string Name => name;

	private int _size;
	private int _maxAlignment = 1;

	public readonly int Size => _size + CalcPaddingTo(_size, _maxAlignment);

	public readonly int Alignment => _maxAlignment;

	private readonly List<(int ByteOffset, IObject Member)> _members = [];

	public readonly IReadOnlyList<(int ByteOffset, IObject Member)> Members => _members;

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