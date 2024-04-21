# SpaceCore/Types
The contents of this directory pertain to globally used types.

## Contents

#### [ExpoNumber](./ExpoNumber.cs)
A simple wrapper for scientificly noted numerical values (both positive and negative). All mathmatical and comparision operations have been implemented. As such, 
objects from this class can be treated as numbers.

#### [MultiTask](./MultiTask.cs)
Very useful for asynchronous operations. A handler that can collect multiple asynchronous operations into a single package, then you wait for all to be completed.
Both the `+` and `-` are used for adding and removing `Task` values from an internal collection. 

#### [ResourceExpoDictionary](./ResourceExpoDictionary.cs)
A simple implementation from the generic `Dictionary<K, V>` class that uses `Resource, ExpoNumber` Key,Value pairs. Also comes with the ability to lock the dictionary, 
ensuring the key, value pair associates remain immutable. Cannot unlock.
