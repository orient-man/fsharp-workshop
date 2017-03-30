namespace App

type Item = { Id: string; Value: int }

module Subscriber =
    open System
    open FSharp.Control.Reactive
    open FSharp.Data
    open FSharp.Data.HttpRequestHeaders
    open Newtonsoft.Json

    let deserialize<'T> value = JsonConvert.DeserializeObject<'T>(value)

    let getItems from : Item list =
        try
            printfn "Pooling..."
            Http.RequestString(
               "http://localhost:50001/items",
               httpMethod = "GET",
               query = [ "from", from.ToString() ],
               headers = [ Accept HttpContentTypes.Json ])
            |> deserialize
        with ex ->
            printfn "ERROR: %s" ex.Message
            []

    let getItemsStream sleep getItems () =
        let evt = Event<_>()
        let rec loop from = async {
            do! sleep 500
            let items = getItems from
            items |> List.iter evt.Trigger
            return! loop (from + items.Length)
        }
        loop 0 |> Async.Start
        evt.Publish

    let getTimer () =
        Observable.timerPeriod DateTimeOffset.Now (TimeSpan.FromMilliseconds 500.)

    let getItemsStream2 getItems =
        let pool (_, from) _ =
            let items = getItems from
            items, from + (items |> List.length)

        Observable.scanInit ([], 0) pool
        >> Observable.map fst
        >> Observable.flatmapSeq Seq.ofList

module Program =
    open System

    //let getItemsStream = Subscriber.getItemsStream Async.Sleep Subscriber.getItems
    let getItemsStream =
        Subscriber.getTimer >> Subscriber.getItemsStream2 Subscriber.getItems

    let processItem { Id = id; Value = value } = printfn "%s:%d" id value

    [<EntryPoint>]
    let main _ =
        let stream = getItemsStream ()
        use subs = stream |> Observable.subscribe processItem
        Console.ReadLine() |> ignore
        0 // return an integer exit code