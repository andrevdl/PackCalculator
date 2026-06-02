# PackCalculator

> Model and visualize C / TwinCAT–style struct memory layout — size, alignment, and padding, byte by byte.

![.NET](https://img.shields.io/badge/.NET-10-512BD4?logo=dotnet&logoColor=white)
![License](https://img.shields.io/github/license/andrevdl/PackCalculator)
![Last commit](https://img.shields.io/github/last-commit/andrevdl/PackCalculator)

A small tool for modeling C / TwinCAT–style memory layout and computing struct sizes,
alignment, and padding. You compose objects from primitives, arrays, strings, and nested
objects, then render a human-readable memory map with `MemoryViewer.DisplayMemoryView`.

## Features

- 📐 **Accurate layout** — computes member offsets, struct size, and alignment the way C / TwinCAT / `StructLayout` do.
- 🧱 **Composable building blocks** — primitives, arrays, `STRING`/`WSTRING`, and arbitrarily nested objects.
- 🎚️ **Configurable packing** — set a pack value (1–8) to cap alignment, just like `#pragma pack` / `{attribute 'pack_mode'}`.
- 🔍 **Readable memory map** — renders data, data padding, and trailing object padding as an indented tree.

## Concepts

| Type | Represents | Size | Alignment |
|------|------------|------|-----------|
| `Primitive(size)` | A scalar field (e.g. `BYTE`, `INT`, `LREAL`) | `size` | `size` |
| `CArray(length, element)` | A contiguous array | `length * element.Size` | `element.Alignment` |
| `CString(length)` | `STRING(length)` | `length + 1` (null terminator) | `1` |
| `CString(length, wide: true)` | `WSTRING(length)` | `(length + 1) * 2` | `2` |
| `CObject(name, context, pack)` | A struct | sum of members + padding | largest member alignment |

`Context(defaultPack)` sets the maximum alignment (the *pack* value, 1–8). Each member is
aligned to `min(member.Alignment, pack)`. The struct's total size is rounded up to a
multiple of its largest member alignment (trailing **object padding**), and gaps inserted
to align members appear as **data padding**.

## Usage

```csharp
var context = new Context(8); // pack = 8

var obj = new CObject("Example", context, 0);
obj.AddMember(new Primitive(1));
obj.AddMember(new Primitive(8));

MemoryViewer.DisplayMemoryView(obj);
Console.WriteLine($"Size: {obj.Size} bytes");
```

## Examples

### 1. Alignment padding

A `byte` followed by an 8-byte field forces the larger field onto an 8-byte boundary.

```csharp
var obj = new CObject("Simple", new Context(8), 0);
obj.AddMember(new Primitive(1));
obj.AddMember(new Primitive(8));
MemoryViewer.DisplayMemoryView(obj);
```

```
Simple
    Primitive(1):	1 bytes
    Data Padding:	7 bytes
    Primitive(8):	8 bytes
End:	16 bytes
```

### 2. Packing (`pack = 1`)

The same members under `Context(1)` cap every member's alignment at 1, removing all padding.

```csharp
var obj = new CObject("Packed", new Context(1), 0);
obj.AddMember(new Primitive(1));
obj.AddMember(new Primitive(8));
MemoryViewer.DisplayMemoryView(obj);
```

```
Packed
    Primitive(1):	1 bytes
    Primitive(8):	8 bytes
End:	9 bytes
```

### 3. Strings (`STRING` vs `WSTRING`)

`STRING` is byte-aligned; `WSTRING` is word-aligned (2 bytes/char), so it can introduce a
padding byte after an odd-sized field. Both reserve space for a null terminator.

```csharp
var obj = new CObject("Message", new Context(8), 0);
obj.AddMember(new Primitive(1));            // byte flag
obj.AddMember(new CString(5, wide: true));  // WSTRING(5)
MemoryViewer.DisplayMemoryView(obj);
```

```
Message
    Primitive(1):	1 bytes
    Data Padding:	1 bytes
    WSTRING(5):	12 bytes
End:	14 bytes
```

### 4. Arrays and nested objects

Arrays are laid out contiguously, and nested objects align by their own largest member.

```csharp
var inner = new CObject("Inner", new Context(8), 0);
inner.AddMember(new Primitive(3));

var obj = new CObject("Test", new Context(8), 0);
obj.AddMember(new Primitive(1));
obj.AddMember(new Primitive(2));
obj.AddMember(new Primitive(8));
obj.AddMember(new CArray(3, new Primitive(1)));
obj.AddMember(new CString(10));
obj.AddMember(new CString(10, wide: true));
obj.AddMember(inner);
MemoryViewer.DisplayMemoryView(obj);
```

```
Test
    Primitive(1):	1 bytes
    Data Padding:	1 bytes
    Primitive(2):	2 bytes
    Data Padding:	4 bytes
    Primitive(8):	8 bytes
    Array[3] of Primitive(1)
        Primitive(1):	1 bytes
        Primitive(1):	1 bytes
        Primitive(1):	1 bytes
    End:	3 bytes
    STRING(10):	11 bytes
    WSTRING(10):	22 bytes
    Data Padding:	2 bytes
    Inner
        Primitive(3):	3 bytes
    End:	3 bytes
    Object Padding:	7 bytes
End:	64 bytes
```
