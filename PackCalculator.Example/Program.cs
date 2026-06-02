using PackCalculator;

Primitive n1 = new(1);
Primitive n2 = new(2);
Primitive n8 = new(8);

CObject inner = new("Inner", new Context(8), 0);
inner.AddMember(new Primitive(3));

CObject obj = new("Test", new Context(8), 0);

obj.AddMember(n1);
obj.AddMember(n2);
obj.AddMember(n8);
obj.AddMember(new CArray(3, n1));
obj.AddMember(new CString(10));
obj.AddMember(new CString(10, wide: true));
obj.AddMember(inner);

MemoryViewer.DisplayMemoryView(obj);

Console.WriteLine($"Size of {obj.Name}: {obj.Size} bytes");
