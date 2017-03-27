F# 101
===

## Dlaczego w ogóle?

Kilkoro lepszych ode mnie programistów odpowiada na to pytanie: https://zombiecodekill.com/2016/04/30/the-case-for-f-sharp/

## tl;dr

- lepsze domyślne zachowania dla typów: niezmienność (immutability), niedozwolony null, działające strukturalne porównania (nie trzeba nadpisywać Equals/GetHashCode)
- type inference = czyli jak zjeść ciastko (lekkość języków dynamicznych) i mieć ciastko (bezpieczeństwo języków statycznie typowanych)
- większą ekspresywność (= precyzja w modelowaniu domeny):
  - algebraiczne typy danych (discriminated unions)
  - pattern matching
  - rekordy, krotki (tuple)
  - jednostki miary
  - automatyczne generalizacja (wszystko jest domyślnie generyczne)
- kompozycyjność = pipe operators (|>, <|), sklejanie funkcji (>>) - czyli fluent API bez konieczność pisania metod rozszerzających
- zwięzła składnia
- interpreter (REPL)
- i wiele więcej...

## Jak zacząć?

F# jest pełnoprawnym językiem .NET-owym:

* działa "z pudełka" w Visual Studio 2010+
* kompiluje się do normalnej DLL-ki .NET-owej
* pozwala na 100% współpracę z innym kodem .NET-owym (np.: konsumpcja API napisanych w C# i vice versa)

Do budowania projektów bez Visual Studio (np. na własnym build serwerze w chmurze) potrzebne są [Visual F# Build Tools](https://www.microsoft.com/en-us/download/details.aspx?id=48179).

_Uwaga_: Bibliotekę standardową ([FSharp.Core](https://www.nuget.org/packages/FSharp.Core)) należy linkować z paczki NuGet, a nie z GAC (co domyślnie robi VS).

## Narzędzia

[Visual F# Power Tools](http://fsprojects.github.io/VisualFSharpPowerTools/) - Resharper dla F# :).

Ponadto polecam [Trailing Whitespace Visualizer](https://visualstudiogallery.msdn.microsoft.com/a204e29b-1778-4dae-affd-209bea658a59) - nie tylko dla F#. Niestety automatyczny reformat dla F# nie usuwa wiszących spacji, ale ten plugin to naprawia.

Ciekawe paczki:

* https://github.com/fsprojects/FsUnit
* https://github.com/SwensenSoftware/unquote
* http://fsprojects.github.io/FSharp.Control.Reactive/
* ...

## Życie poza Visual Studio

Czyli coś czego wielu backendowców od dawna zazdrości frontendowcom. F# pozwala uwolnić się od Visual Studio. To ostatnie oczywiście działa, ale jest to - IMHO - ciężka kobyła ucząca złych nawyków. Składnia F# jest zwięzła, a funkcyjna natura w praktyce prowadzi do kodu składającego się z wielu małych, niezależnych modułów, która łatwo jest uruchamiać w interpreterze, a pisać bez wsparcia IDE. Osobiście polecam [VIM-a](https://github.com/fsharp/vim-fsharp), ale wybór jest szeroki np.: [Ionide](http://ionide.io/), [Visual Studio Code](https://code.visualstudio.com/) itd. Każdy z nich udostępnia podstawowe sprawdzanie składni i uruchamianie interpretera, co w zupełności wystarcza do codziennej pracy.

## Co warto przeczytać / obejrzeć?

### Książki

* [Real-World Functional Programming: With Examples in F# and C#](http://www.amazon.com/Real-World-Functional-Programming-Tomas-Petricek/dp/1933988924) - rewelacyjne wprowadzenie w programowanie funkcyjne
* [F# Deep Dives](http://functional-programming.net/deepdives/) - świetne kolekcja rzeczywistych przykładów zastosowań F# w różnych domenach
* [Expert F#](http://www.amazon.com/Expert-F-4-0-Don-Syme/dp/1484207416) - dla twardzieli, bo nie tyle trudna, co miejscami nudnawa
* [Machine Learning Projects for .NET Developers](http://www.apress.com/9781430267676) - to już naprawdę dla twardzieli ;)
* więcej: http://fsharp.org/learn.html

### Kursy

Rewelacyjne kursy Marka Seemanna:
* [Pluralsight: A Functional Architecture with F#](https://app.pluralsight.com/library/courses/functional-architecture-fsharp/table-of-contents) - budowanie aplikacji end-to-end (backend-frontend) w F#
* [Pluralsight: Type-Driven Development with F#](https://app.pluralsight.com/library/courses/fsharp-type-driven-development/table-of-contents) - o potędze systemu typów F#
* [Pluralsight: Test-Driven Development with F#](http://app.pluralsight.com/courses/fsharp-test-driven-development)

### Linki

* [F# Software Foundation](http://fsharp.org/)
* [F# for fun and profit](https://fsharpforfunandprofit.com/)
* [Mark Seemann's blog](http://blog.ploeh.dk/)
* [Tomas Petricek's blog](http://tomasp.net/)

## Pytania graniczne

### Czy przepisywać wszystko z C# na F#?

Nie.

C# jeszcze nie umarł :) i ma się dobrze. Nie ma sensu przepisywać dla samego przepisywania. F#, podobnie jak C#, jest językiem ogólnego zastosowania tzn. nie tylko do ciężkiej algorytmiki (taki jest stereotyp dot. języków funkcyjnych), ale także dla GUI. To napisawszy, dodam, że również C++ jest takim językiem... Czysty kod i dobra architektura są możliwe zarówno w C++, jak i w C#, niemniej w praktyce - [z jakichś powodów](http://blog.ploeh.dk/2016/03/18/functional-architecture-is-ports-and-adapters/) - rzadko występują w przyrodzie. IMHO F# delikatnie nakierowuje programistę we właściwą stronę. Porównałbym to - dla wtajemniczonych (okaleczonych) - do WebForms i ASP.NET MVC. Ten pierwszy w praktyce kierował programistę w stronę kultu potwora spaghetti, a ten drugi pozwalał rozwinąć skrzydła.

Zalecałbym następującą kolejność adaptacji:

1. Unit testy - to na ogół prosty kod, a pozwala na oswojenie się z językiem
1. Logika biznesowa - szczególnie algorytmy, obliczenia - korzyści będą widoczne od razu
1. Kod współbieżny - tutaj zalety F# mocno się uwydatniają (immutability, async, agenci), ale że jest to trudne same w sobie, to dopiero jako pkt. 3
4. ...
5. GUI - jeśli to Web to C# też nie ma zwykle za wiele do roboty (klient w JavaScript + chudy kontroler w czymkolwiek + logika biznesowa w F#). Natomiast w WebForms/WPF pewnym problemem jest framework (designer). Jeśli tylko GUI jest wystarczająco skomplikowany, to IMHO warto.

### Jakie są zalecane Coding Standards?

Na [GitHubie](https://github.com/trending/fsharp) jest mnóstwo dobrego kodu do poczytania (uśmiech). A poważnie, to [Visual F# Power Tools](http://fsprojects.github.io/VisualFSharpPowerTools/) (odpowiednik R#) zawierają sensowne domyślne ustawienia formatowania (które są konfigurowalne) i łapie też sporo "naiwnych" błędów (znów jak R#).