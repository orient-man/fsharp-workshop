// let, basic types
// functions
// recursion
// partial application
// pipe (|>) & composition (>>)
// tuples
// records (with)
// discriminated unions
// options
// patern matching
// collections: lists, arrays, seq
// collections: processing with pipe
// collections: pattern matching
// collections: expressions & lazyness
// higher order functions
// closures
// EXERCISE 1: Find value closest to Zero
// [0.5; 0.6; -0.5; 15.; -20.; -0.1; 0.1; 2.] |> findClosestToZero // returns -0.1
// EXERCISE 2 (advanced): Merge 2 maps (dictionaries) and compute diff
// let map1 = [("A", 5); ("B", 2); ("C", 7)] |> Map.ofList
// let map2 = [("B", 2); ("C", 5); ("D", 3)] |> Map.ofList
// mergeWithDiff map1 map2
// returns: map [("A", (5, 5)); ("B", (2, 0)); ("C", (7, 2)); ("D", (0, -3))]
// EXAMPLE: Type Driven Development
// [*] active patterns
// [*] classes
// [*] interop: class wrappers, functional core (port & adapters), "no null inside"