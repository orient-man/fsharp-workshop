open System

let selectClosest prev value =
    match Math.Abs(float value) - Math.Abs(float prev) with
    | diff when diff < 0. -> value
    | 0. when value < prev -> value
    | _ -> prev

let findClosestToZero =
    function [] -> 0. | list -> List.fold selectClosest Double.MaxValue list

findClosestToZero [ 0.5; 0.6; -0.5; 15.; -20.; -0.1; 0.1; 2. ]
findClosestToZero []