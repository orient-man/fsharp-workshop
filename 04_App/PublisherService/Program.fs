namespace PublisherService

type Msg = Start | Stop
type Item = { Id: string; Value: int }

module Publisher =
    open System

    type State = Idle | Writing

    let guid () = let g = Guid.NewGuid() in g.ToString()

    let nextRandom = let rnd = Random() in fun () -> rnd.Next(1000)

    let loop tryReceive persist =
        let rec loop' state = async {
            let! msg = tryReceive ()
            match msg, state with
            | Some Start, _ ->
                printfn "Writer starts writing..."
                return! loop' Writing
            | Some Stop, Writing ->
                printfn "Writer is stopping..."
                return! loop' Idle
            | None, Writing ->
                do! Async.Sleep 1000
                { Id = guid (); Value = nextRandom () } |> persist
                return! loop' Writing
            | _, Idle ->
                return! loop' Idle
        }
        loop'

    let createAgent persist =
        let agent = MailboxProcessor.Start(fun inbox ->
            printfn "Agent started..."
            let tryReceive () = inbox.TryReceive(timeout = 0)
            loop tryReceive persist Idle)

        agent.Post

module ItemStore =
    open System.Collections.Generic

    let db = List<Item>()

    let persist item =
        printfn "Storing %A" item
        db.Add item

    let get from = db |> Seq.skip from |> List.ofSeq


module CompositionRoot =
    let compose () =
        let post = Publisher.createAgent ItemStore.persist
        post, ItemStore.get

open Nancy
open NancyDynamicHelper

type MainEndpoint () as this =
    inherit NancyModule()

    static let post, getItems = CompositionRoot.compose ()

    do
        this.Get.["/start"] <- fun _ ->
            Start |> post
            HttpStatusCode.OK |> box

        this.Get.["/stop"] <- fun _ ->
            Stop |> post
            HttpStatusCode.OK |> box

        this.Get.["/items"] <- fun _ ->
            this.Request.Query?from
            |> defaultArg <| 0
            |> getItems
            |> this.Response.AsJson
            |> box

module Program =
    open System
    open Nancy
    open Nancy.Hosting.Self

    [<EntryPoint>]
    let main _ =
        let cfg = HostConfiguration(RewriteLocalhost = true)
        cfg.UrlReservations.CreateAutomatically <- true

        use bootstrapper = new DefaultNancyBootstrapper()
        use host = new NancyHost(Uri("http://localhost:50001"), bootstrapper, cfg)

        host.Start()
        printfn "WriterService started..."
        Console.ReadLine() |> ignore
        0 // return an integer exit code