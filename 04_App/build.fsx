#r "./packages/FAKE/tools/FakeLib.dll"

open Fake
open Fake.Testing

let buildDir = "./build/"
let appReferences = !! "**/*.csproj" ++ "**/*.fsproj"

Target "Clean" (fun _ -> CleanDirs [buildDir])

Target "Build" (fun _ ->
    appReferences
    |> MSBuildDebug buildDir "Build"
    |> Log "Build-output: "
)

Target "Test" (fun _ -> !! (buildDir + "*.Tests.dll") |> NUnit3 id)

"Clean" ==> "Build" ==> "Test"

RunTargetOrDefault "Build"