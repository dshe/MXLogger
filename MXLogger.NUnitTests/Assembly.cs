global using Microsoft.Extensions.Logging;
global using NUnit.Framework;
using System.Diagnostics.CodeAnalysis;

[assembly: Parallelizable(ParallelScope.Children)]

[assembly: SuppressMessage("Style", "IDE0290: Use primary constructor")]
