# RI.Utilities

[![Nuget](https://img.shields.io/nuget/v/RI.Utilities)](https://www.nuget.org/packages/RI.Utilities/) [![License](https://img.shields.io/github/license/RotenInformatik/UtilitiesDotNet)](LICENSE) [![Repository](https://img.shields.io/badge/repo-UtilitiesDotNet-lightgrey)](https://github.com/RotenInformatik/UtilitiesDotNet) [![Documentation](https://img.shields.io/badge/docs-Readme-yellowgreen)](README.md) [![Documentation](https://img.shields.io/badge/docs-History-yellowgreen)](HISTORY.md) [![Documentation](https://img.shields.io/badge/docs-API-yellowgreen)](https://roteninformatik.github.io/UtilitiesDotNet/api/)

---

Assorted utilities and extensions for .NET.

Single assembly, based on .NET Standard 2.1, with no additional dependencies.

---

## New types

* Data formats
  * **CSV** (CsvReader, CsvWriter, CsvDocument)
  * **INI** (IniReader, IniWriter, IniDocument)
* Collections
  * **Pool** (stores and recycles object instances)
  * **PriorityQueue** (queues items according to their priorities)
  * **RequestResponseQueue** (thread-safe, asynchronous, bidirectional, multi-producer/multi-consumer queue)
  * **VirtualizationCollection** (loads items page-wise from a source)
* Paths
  * **DirectoryPath**
  * **FilePath**
* Text
  * **CommandLine**
  * **IndentedTextWriter**
* Streams
  * **BinaryStream**
  * **LoopbackStream**
  * **RandomStream**
  * **ReadOnlyStream**
  * **SynchronizedStream**
  * **UncloseableStream**
* Mathematics
  * **RomanNumber**
  * **RunningValues**
  * **StatisticValues**
* Object model interfaces
  * **ICloneable**
  * **ICopyable**
  * **ISynchronizable**

## Utilities

* Comparison
  * **AlphanumComparer** (compares strings based on natural language perception)
  * **CollectionComparer** (compares collections and their elements)
  * **EqualityComparison**, **OrderComparison** (wraps delegates as comparers)
* Threading
  * **HeavyThread** (base class for managing and using threads)
* Exceptions
  * **EmptyStringArgumentException**
  * **InvalidPathArgumentException**
  * **. . .**

## Extensions

* Integral types
  * **string**
  * **sbyte**, **short**, **int**, **long**
  * **byte**, **ushort**, **uint**, **ulong**
  * **float**, **double**, **decimal**
* Commonly used types
  * **DateTime**
  * **TimeSpan**
  * **Exception**
  * **Stream**
  * **TextReader**
* Collection types
  * **IEnumerable**
  * **ICollection**
  * **ISet**
  * **IList**
  * **IDictionary**
  * **IReadOnlyList**
  * **IReadOnlyDictionary**
  * **LinkedList**
  * **Queue**
  * **Stack**
  * **KeyedCollection**
* Reflection types
  * **Assembly**
  * **Delegate**
  * **Type**
* Other types
  * **BinaryReader**
  * **CultureInfo**
  * **Random**
  * **XDocument**
  * **XmlDocument**