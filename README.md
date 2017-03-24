# F# Workshop

## Prerequisites

* Visual Studio 2015 or VSCode+Ionide (if your are brave enough)
* Git (with your favourite command line)

Nice to haves:

* [Visual F# Power Tools](http://fsprojects.github.io/VisualFSharpPowerTools/)
* [NUnit Test Adapter](https://github.com/nunit/docs/wiki/Visual-Studio-Test-Adapter) (or other test runner like R#/NCrunch)

## Part I: Crash Course

Intro into F# wellness. You need nice editor + F# REPL. For example:

* Visual Studio 2015+ - for Addicts ;-)
* VIM + [vim-fsharp plugin](https://github.com/fsharp/vim-fsharp)
* [Visual Studio Code](https://code.visualstudio.com/) + [Ionide plugin](http://ionide.io/)
* or just REPL (fsi.exe)

Topics:

* functions
* higher order functions
* partial application & composition
* basic types: tuples, records, discriminated unions
* collections
* pattern matching
* quick look at Type Driven Development

## Part II: Unit Testing

Good place to start F# journey. Why:

* Unit tests (should) have simple structure (AAA) -> let you focus on learning syntax
* Concise & readable syntax of F# shines
* Really nice libraries
* Expert mode: easy way to write DSL's around your acceptance/functional tests

Topics:

* Howto create your first F# project
* [Paket](https://fsprojects.github.io/Paket/) - awesome NuGet replacement (and written in F#)
* [FAKE](http://fsharp.github.io/FAKE/) - F# for your automation scripts
* NUnit - just because it doesn't matter
* [FsUnit](http://fsprojects.github.io/FsUnit/) - for human readable assertions
* [Swensen.Unquote](http://www.swensensoftware.com/unquote) - for smth completely different
* Testing OOP code with & without mocks (e.g. [Moq](https://github.com/moq/moq4))

This part is doable outside of Visual Studio (even in .NET Core) but I will leave it as an exercise :).

## Part III: Application Development

## Part IV: Integration