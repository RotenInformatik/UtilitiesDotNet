[Back](index.md)

# Collections

* [Extensions](#Extensions)
* [Concurrent](#Concurrent)
* [Generic](#Generic)
* [Specialized](#Specialized)
* [Virtualization](#Virtualization)

## Extensions

Namespace: `RI.Utilities.Collections`

Contains extension methods for existing collection types.

### `IEnumerableExtensions`

* AsEnumarable
* ForEach,
* SequenceEqual
* ToList, ToGenericList, ToDictionaryList, ToDictionarySet, ToArray

### `ICollectionExtensions`

* AsCollection
* AddRange, RemoveRange
* RemoveAll, RemoveAllRange
* RemoveWhere

### `ISetExtensions`

* AsSet
* AddRange, RemoveRange

### `IListExtensions`

* AsList
* GetIndexOrDefault, TryGetIndex
* InsertRange, RemoveAtRange
* Peek, PeekClamp, PeekRandom
* Pop, PopClamp, PopRandom
* Reverse, Shuffle, Sort
* SwapDefault, SwapInPlace, SwapInsert
* Transform

### `IDictionaryExtensions`

* AsDictionary
* GetValueOrDefault
* AddOrReplace, AddRange, AddRangeExact, TryAdd
* RemoveRange, RemoveWhere
* ContainsKey, ContainsValue
* GetKeys, GetValues
* Transform

### `IReadOnlyListExtensions`

* AsReadOnlyList
* RoGetIndexOrDefault, RoTryGetIndex
* RoPeek, RoPeekClamp, RoPeekRandom

### `IReadOnlyDictionaryExtensions`

* AsReadOnlyDictionary
* RoGetValueOrDefault
* RoContainsKey, RoContainsValue
* RoGetKeys, RoGetValues

### `LinkedListExtensions`

* AsItemsForward, AsItemsBackward
* AsNodesForward, AsNodesBackward
* ToItemsForward, ToItemsBackward
* ToNodesForward, ToNodesBackward

### `KeyedCollectionExtensions`

* GetValueOrDefault
* ContainsKey, ContainsValue
* RemoveRange

### `QueueExtensions`

* PeekAll
* EnqueueRange
* DequeueAll, DequeueInto

### `StackExtensions`

* PeekAll
* PushRange
* PopAll, PopInto

## Concurrent

Namespace: `RI.Utilities.Collections.Concurrent`

Contains new collection types for concurrent/multithreading and asynchronous scenarios.

### `RequestResponseQueue`

A queue which can have multiple asynchronous producers and consumers on different threads and which notifies the producer (through asynchronous continuation) when the consumer finished processing an item.

TODO: Code example

## Generic

Namespace: `RI.Utilities.Collections.Generic`

Contains new generic collections.

### Pools (`IPool`, `IPoolExtensions`, `IPoolAware`, `PoolBase`, `Pool`)

Contract and implementation of object pools to reuse object instances.

```c#
// create a pool
var pool = new Pool<MyObject>();
    
// get an item from the pool (needs to be created as the pool is empty)
var item1 = pool.Take();
    
// get another item from the pool (needs to be created as the pool is empty)
var item2 = pool.Take();
    
// ... do something ...
    
// return one of the items
pool.Return(item2);
    
// ... do something ...
    
// get another item (the former item2 is recycled)
var item3 = pool.Take();
```

### Priority queues (`IPriorityQueue`, `IPriorityQueueExtensions`, `PriorityQueue`)

Contract and implementation of priority queues.

```c#
// create a priority queue
var queue = new PriorityQueue<string>();
    
// add some items with different priorities
queue.Enqueue("queue", 0);
queue.Enqueue("this", 101);
queue.Enqueue("a", 10);
queue.Enqueue("is", 100);
    
// dequeue items, we get: this, is, a, queue
while(queue.Count > 0)
{
    string value = queue.Dequeue();
}
```

## Specialized

Namespace: `RI.Utilities.Collections.Specialized`

Contains derivations of generic collections for specific types.

### `ClonePool`

An object pool which uses a prototype instance from which items are created using `ICloneable`.

```c#
// create a pool with a cloneable prototype (which must implement ICloneable)
var pool = new ClonePool<MyObject>(new MyObject(some, constructor, parameters));
    
// get some cloned items
var item1 = pool.Take();
var item2 = pool.Take();
var item3 = pool.Take();
    
// ... do something ...
    
// return one of the items
pool.Return(item2);
    
// ... do something ...
    
// get another item (the former item2 is recycled)
var item4 = pool.Take();
```

## Virtualization

Namespace: `RI.Utilities.Collections.Virtualization`

Contains types for implementing collection virtualization.

### `VirtualizationCollection`

A list implementation (`IList`) which loads items on demand using pages from an items provider (`IItemsProvider`, `INotifyItemsProvider`).

TODO: Code example