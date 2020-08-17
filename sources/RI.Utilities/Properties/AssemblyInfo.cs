using System;
using System.Reflection;
using System.Runtime.InteropServices;




[assembly: AssemblyTitle("RI.Utilities"),]
[assembly: AssemblyDescription("Assorted utilities and extensions for .NET"),]
[assembly: Guid("D2FF2609-DB2D-4696-93C4-85C3E5AF03E9"),]

[assembly: AssemblyProduct("RotenInformatik/UtilitiesDotNet"),]
[assembly: AssemblyCompany("Roten Informatik"),]
[assembly: AssemblyCopyright("Copyright (c) 2011-2020 Roten Informatik"),]
[assembly: AssemblyTrademark(""),]
[assembly: AssemblyCulture(""),]
[assembly: CLSCompliant(true),]
[assembly: AssemblyVersion("0.0.0.0"),]
[assembly: AssemblyFileVersion("0.0.0.0"),]
[assembly: AssemblyInformationalVersion("0.0.0.0"),]

#if DEBUG
[assembly: AssemblyConfiguration("DEBUG"),]
#else
[assembly: AssemblyConfiguration("RELEASE")]
#if !RELEASE
#warning "RELEASE not specified"
#endif
#endif
