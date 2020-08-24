using System;
using System.Reflection;
using System.Runtime.InteropServices;




[assembly: AssemblyTitle("RI.Utilities"),]
[assembly: AssemblyDescription("Assorted utilities and extensions for .NET"),]
[assembly: Guid("D61215F8-02EC-443E-B3D9-61E29752860F"),]

[assembly: AssemblyProduct("RotenInformatik/UtilitiesDotNet"),]
[assembly: AssemblyCompany("Roten Informatik"),]
[assembly: AssemblyCopyright("Copyright (c) 2011-2020 Roten Informatik"),]
[assembly: AssemblyTrademark(""),]
[assembly: AssemblyCulture(""),]
[assembly: CLSCompliant(true),]
[assembly: AssemblyVersion("1.1.0.0"),]
[assembly: AssemblyFileVersion("1.1.0.0"),]
[assembly: AssemblyInformationalVersion("1.1.0.0"),]

#if DEBUG
[assembly: AssemblyConfiguration("DEBUG"),]
#else
[assembly: AssemblyConfiguration("RELEASE")]
#if !RELEASE
#warning "RELEASE not specified"
#endif
#endif
