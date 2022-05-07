# ToApexClass

A `dotnet tool` that reads the text of a C# POCO class and outputs a valid Apex (Salesforce) class. The objective is to make it easier to define a class in C# that can be copied into Salesforce without modification. This could be useful for interop scenarios where the equivalent class needs to exist on both platforms.

```
using System;

namespace Example
{
	public class Something
	{
		public string Detail { get; set; }
		public bool IsTest { get; set; }
		public DateTime? Timestamp { get; set; }
	}
}
```
is converted to
```
public class Something
{
	public string Detail { get; set; }
	public Boolean IsTest { get; set; } { this.IsTest = false; }
	public DateTime Timestamp { get; set; }
}
```

This does not require additional library dependencies in your .NET code but places some limits on how a C# class can be defined.

## Install / Use

A standalone executable is not provided.

If you have the .NET SDK installed, ToApexClass can be run from source by cloning this repository, then from root:

`dotnet run -p .\src\ToApexClass\ -f net5.0 -- .\path\to\Class.cs`

Otherwise, the .NET runtime (6.0 or higher) is required.

`TODO tool install instructions pending Nuget publication`

## Language Differences & Limitations

Namespaces are a common and vital part of programming in .NET; they are almost nonexistent in Apex.

Directives to open namespaces is also common in .NET; all non-local types in Apex must be fully qualified.

Apex class definitions can only be 2 layers deep.

C# is case-sensitive; Apex is case-*in*sensitive.

Apex only supports 3 collection types: List/Array, Map, and Set.

## Build

`dotnet build`

## Alternatives

- https://github.com/apexsharp/apexparser

## Key Dependencies

- [Argu](https://github.com/fsprojects/Argu) for command line argument parsing
- [cecil](https://github.com/jbevain/cecil) for reading types from .NET assemblies
- [Expecto](https://github.com/haf/expecto) for testing