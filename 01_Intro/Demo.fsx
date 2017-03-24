// let, basic types
let x = 5m
let str = "hello world"
let ``this is string`` = "hi!"
let xml = """
<tag attr="value'"></tag>
"""

// functions
let add x y = x + y
let add' (a: int) (b: int) : int = a + b    // same as above
let inline add'' x y = x + y    // generic addition
add 2 2
add' 2 2
add'' 2m 2m
add'' 2. 2.
add'' "a" "b"

// recursion
let rec coundown n =
    if n = 0
    then
        printfn "Finish!"
    else
        printfn "n = %d" n
        coundown (n - 1)

coundown 5

// partial application
let add2 = add 2
add2 6

// pipe (|>) & composition (>>)
5 |> add2 |> add2
let add4 = add2 >> add2
// f >> g = g(f)
add4 1

// tuples
let p = (1, 2, 3)   // Tuple<int, int, int>
p.GetType()

// records (with)
type Point3d = { X: int; Y: int; Z: int }
let p3d = { X = 1; Y = 2; Z = 3  }
let p3d' = { p3d with Z = -1 }
p3d = p3d'

// discriminated unions
type Color = Black | White
let c = Black
type DomainEvent = SpreadGroupChanged | CptySpreadGropChanded of cptyId: int
let evt = CptySpreadGropChanded 1241

// options
// type Option<'T> = None | Some of 'T
let some = Some 14
let none = None

// patern matching
let handleOption o =
    match o with
    | None -> "nothing"
    | Some _ -> "something"

let handleColor c =
    match c with
    | Black -> printfn "black"
    | White -> printfn "white"
handleColor White
// shortcut
let handleColor' = function White -> printfn "white" | Black -> printfn "black"

let handleEvt = function
    | SpreadGroupChanged -> printfn "bla"
    | CptySpreadGropChanded id -> printfn "cpty: %d" id
handleEvt evt

// collections: lists, arrays, seq
let lst = [1; 32; 34; 57]
let newLst = 5 :: lst
let arr = [| 1; 2 |]
let sq = seq [1; 2; 3]

// collections: pipe (|>) - "LINQ alike"
lst
|> List.rev
|> List.map (fun v -> v + 1)
|> List.filter (fun x -> x % 2 <> 0)

// same as:
List.filter (fun x -> x % 2 <> 0) (List.map (fun v -> v + 1) (List.rev lst))

// hardcore version
lst |> (List.rev >> List.map ((+) 1) >> List.filter (fun x -> x % 2 <> 0))

// collections: pattern matching
let listInfo = function
    | [] -> printfn "this is empty list"
    | [head] -> printfn "only one element on the list (%A)" head
    | _ -> printfn "2+ element list"

// collections: expressions & lazyness
let sq' = seq { for i in 1..10 do printfn "%d" i; yield i }
// lazy vs. eager
sq' |> Seq.truncate 5 |> List.ofSeq
[ for i in 1..10 do printfn "%d" i; yield i ] |> Seq.truncate 5 |> List.ofSeq

// higher order functions
let log f a b =
    let result = f a b
    printfn "Called f(%A, %A) = %A" a b result
    result

let addWithLogging = log add
addWithLogging 1 2

// closures
let nextrandom =
    let rnd = System.Random()
    (fun () -> rnd.Next())
let f = nextrandom
f()
f()     // not "clean" function

// EXAMPLE: Type Driven Development
type Order = { Id: int; Rate: decimal }

module Orders =
    // Look! I'm generic
    let processOrders getOrders persist recalc =
        getOrders ()
        |> Seq.map recalc
        |> Seq.iter persist

    let recalc o = { o with Rate = o.Rate * 2m }

Orders.recalc { Id = 1234; Rate = 45m }
let order = { Id = 1234; Rate = 45m }

module DataAccess =
    type OrderEntity = OrderEntity of int * decimal
    let private db = System.Collections.Generic.Dictionary<_, _>()

    let persist (order: Order) =
        printfn "saving %A to database" order
        db.[order.Id] <- OrderEntity (order.Id, order.Rate)

    let getOrders () =
        db.Values
        |> Seq.map (fun (OrderEntity (id, value)) -> { Id = id; Rate = value })
        |> List.ofSeq

module CompositionRoot =
    let processOrders () = Orders.processOrders DataAccess.getOrders DataAccess.persist Orders.recalc

DataAccess.persist { Id = 1; Rate = 2m }
DataAccess.persist { Id = 2; Rate = 3m }

CompositionRoot.processOrders ()

// Look at the order of "layers":
// * data structures (Order): persistence agnostic "POCOs"
// * logic (module Orders): no deps, generic code, defines its deps as simple functions
// * data access adapters: converts from app structures to persistence one's
// * compostion root aka Main: where everything is glued together

// [*] active patterns
// [*] classes
// [*] interop: class wrappers, functional core (port & adapters), "no null inside"