module App.Tests

open System
open NUnit.Framework
open FsUnit
open Swensen.Unquote

[<Test>]
let ``2 + 2 = 4`` () =
    2 + 2 |> should equal 4

[<TestCase(2, 2, ExpectedResult = 4)>]
[<TestCase(1, 2, ExpectedResult = 3)>]
let ``adds ints`` a b = a + b

let [<Test>] ``11 - 9 = 2`` () = test <@ 11 - 9 = 2 @>

[<Test>]
let ``divide by 0`` () =
    raises<DivideByZeroException> <@ 5 / 0 @>

let failMe msg = failwithf "%s" msg

[<Test>]
let ``assertion on ex.Message`` () =
    raisesWith<Exception> <@ failMe "44" @> (fun ex -> <@ ex.Message = "44" @>)

[<Test>]
let ``List.map & structural equality`` () =
    [1; 2; 3] |> List.map (fun x -> x * 2) =! [2; 4; 6]