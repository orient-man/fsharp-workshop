module App.Tests

open NUnit.Framework

[<Test>]
let ``2 + 2 = 4`` () =
    Assert.AreEqual(4, 2 + 2)