module App.Tests

open System
open System.Linq
open NUnit.Framework
open FsUnit
open Swensen.Unquote

// basics
[<Test>]
let ``2 + 2 = 4 in old school`` () =
    Assert.AreEqual(4, 2 + 2)

// FsUnit for human readable assertions
[<Test>]
let ``2 + 2 = 4`` () =
    2 + 2 |> should equal 4

[<TestCase(2, 2, ExpectedResult = 4)>]
[<TestCase(1, 2, ExpectedResult = 3)>]
let ``adds ints`` a b = a + b

// Unquote
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

// Exercise 1: Verify how List.fold works (hint: Just like LINQ's Aggregate)
[<Test>]
let ``List.fold works as expected`` () =
    ["work"; "job"; "mBank"]
    |> List.fold (fun acc word -> acc + word.Length) 0
    =! 12

[<Test>]
let ``Aggregate works just like List.fold!`` () =
    ["work"; "job"; "mBank"]
        .Aggregate(0, fun acc word -> acc + word.Length)
        =! 12

// Exercise 2: Verify how List.collet works (hint: Just like LINQ's SelectMany)
[<Test>]
let ``List.collect works as expected`` () =
    ["work"; "job"]
    |> List.collect List.ofSeq
    =! ['w'; 'o'; 'r'; 'k'; 'j'; 'o'; 'b']

[<Test>]
let ``SelectMany works just like List.collect`` () =
    ["work"; "job"].SelectMany(fun word -> word :> seq<_>)  // that's type casting :)
    |> List.ofSeq
    =! ['w'; 'o'; 'r'; 'k'; 'j'; 'o'; 'b']

// DSL 1: Easy way to write test cases
type TestData =
    { Desc: string; List: int list; Expected: int list }
    override this.ToString() = this.Desc

let getTestData () = [
    {   Desc = "Already sorted list"
        List = [1; 2; 3; 4]
        Expected = [1; 2; 3; 4] }
    {   Desc = "Reversed list"
        List = [4; 3; 2; 1]
        Expected = [1; 2; 3; 4] }
]

[<TestCaseSource("getTestData")>]
let ``List.sort works as expected`` (data: TestData) =
    data.List |> List.sort =! data.Expected