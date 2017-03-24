module App.Tests

open NUnit.Framework
open FsUnit

[<Test>]
let ``2 + 2 = 4`` () =
    2 + 2 |> should equal 4