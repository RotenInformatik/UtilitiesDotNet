using System;
using System.Reflection;

[assembly: AssemblyProduct("UtilitiesDotNet"),]
[assembly: AssemblyCompany("Roten Informatik"),]
[assembly: AssemblyCopyright("Apache 2.0 license; 2011-2022 Franziska Roten"),]
[assembly: AssemblyTrademark(""),]
[assembly: AssemblyCulture(""),]
[assembly: AssemblyVersion("1.3.0.0"),]
[assembly: AssemblyFileVersion("1.3.0.0"),]
[assembly: AssemblyInformationalVersion("1.3.0.0"),]
[assembly: CLSCompliant(true),]

#if DEBUG
[assembly: AssemblyConfiguration("DEBUG"),]
#else
[assembly: AssemblyConfiguration("RELEASE")]
#if !RELEASE
#warning "RELEASE not specified"
#endif
#endif
