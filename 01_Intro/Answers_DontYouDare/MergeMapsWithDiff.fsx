// generic version
let inline mergeWithDiff map1 map2 =
    let zero = LanguagePrimitives.GenericZero
    let seq1 = map1 |> Map.toSeq |> Seq.map (fun (k, v) -> k, (v, v))
    let seq2 = map2 |> Map.toSeq |> Seq.map (fun (k, v) -> k, (zero, -v))
    let sum = Seq.fold (fun (sum1, sum2) (v1, v2) -> sum1 + v1, sum2 + v2) (zero, zero)

    Seq.append seq1 seq2
    |> Seq.groupBy fst
    |> Seq.map (fun (k, values) -> k, values |> Seq.map snd |> sum)
    |> Map.ofSeq

let map1 = [("A", 5); ("B", 2); ("C", 7)] |> Map.ofList
let map2 = [("B", 2); ("C", 5); ("D", 3)] |> Map.ofList

mergeWithDiff map1 map2
// map [("A", (5, 5)); ("B", (2, 0)); ("C", (7, 2)); ("D", (0, -3))]