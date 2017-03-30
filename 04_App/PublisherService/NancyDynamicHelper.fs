module NancyDynamicHelper

open Nancy

let (?) (p: obj) prop =
    let ddv = (p :?> DynamicDictionary).[prop] :?> DynamicDictionaryValue
    match ddv.HasValue with
    | false -> None
    | _ -> ddv.TryParse<'a>() |> Some