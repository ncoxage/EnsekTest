// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "I'm a bit 'Old School' in this regard", Scope = "module")]
[assembly: SuppressMessage("Style", "IDE0063:Use simple 'using' statement", Justification = "Prefer layout offered by curlies", Scope = "module")]
[assembly: SuppressMessage("CodeQuality", "IDE0079:Remove unnecessary suppression", Justification = "Don't really understand why this one's required.", Scope = "member", Target = "~M:Data.Tests.MeterDBContextTests.DummySet.Configure(Microsoft.EntityFrameworkCore.ModelBuilder)")]
