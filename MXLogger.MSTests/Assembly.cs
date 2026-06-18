global using System;
global using Microsoft.Extensions.Logging;
global using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics.CodeAnalysis;

[assembly: Parallelize(Workers = 0, Scope = ExecutionScope.MethodLevel)]

[assembly: SuppressMessage("Style", "IDE0290: Use primary constructor")]
