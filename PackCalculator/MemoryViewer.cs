using System.Text;

namespace PackCalculator;

internal static class MemoryViewer
{
	public static void DisplayMemoryView(IObject @object)
	{
		List<(MemoryType Type, string? Name, int Size)> view = @object.ToMemoryView();

		StringBuilder sb = new();
		int indent = 0;
		foreach (var (Type, Name, Size) in view)
		{
			string postfix = Size > 0 ? $":\t{Size} bytes" : string.Empty;
			switch (Type)
			{
				case MemoryType.Data:
					sb.AppendLine($"{new('\t', indent)}{Name ?? "Data"}{postfix}");
					break;
				case MemoryType.DataPadding:
					sb.AppendLine($"{new('\t', indent)}{Name ?? "Data Padding"}{postfix}");
					break;
				case MemoryType.ObjectPadding:
					sb.AppendLine($"{new('\t', indent)}{Name ?? "Object Padding"}{postfix}");
					break;
				case MemoryType.Start:
					sb.AppendLine($"{new('\t', indent)}{Name ?? "Start"}{postfix}");
					indent++;
					break;
				case MemoryType.End:
					indent--;
					sb.AppendLine($"{new('\t', indent)}{Name ?? "End"}{postfix}");
					break;
				default:
					sb.AppendLine($"{new('\t', indent)}{Name ?? "Unknown"}{postfix}");
					break;
			}
		}

		Console.WriteLine(sb.ToString());
	}
}
