// Hint: paket.exe generate-load-scripts
#I "./packages/"
#r "FSharp.Data/lib/net40/FSharp.Data.dll"
#r "Newtonsoft.Json/lib/net45/Newtonsoft.Json.dll"

open System
open System.IO
open FSharp.Data
open Newtonsoft.Json

let guid () = let g = Guid.NewGuid() in g.ToString()
let nextRandom = let rnd = Random() in fun () -> rnd.Next(1000)

let baseDir = __SOURCE_DIRECTORY__
let getJsonFileName id = sprintf "%s\\%s.json" baseDir id

type WorkItem = { Id: string; Value: int }

let writeStuff () =
    let id = guid ()
    { Id = id; Value = nextRandom () }
    |> JsonConvert.SerializeObject
    |> fun json -> File.WriteAllText (id |> getJsonFileName, json)
    id

type WorkItemJson = JsonProvider<"""{ "Id": "abcd", "Value": 1924 }""">

let readStuff id =
    let fileName = getJsonFileName id
    let item = WorkItemJson.Load(fileName)
    // clean up
    File.Delete fileName
    item

type WriterState = Idle | Writing
type WriterMsg = StartWriting | StopWriting
type ReaderMsg = Work of string | StopReading

type Agent<'a> = MailboxProcessor<'a>

let postToWriter post =
    let rec loop tryReceive state = async {
        let! msg = tryReceive ()
        match msg, state with
        | Some StartWriting, _ ->
            printfn "Writer starts writing..."
            return! loop tryReceive Writing
        | Some StopWriting, _ -> printfn "Writer is stopping..."
        | None, Writing ->
            do! Async.Sleep 1000
            let workItemId = writeStuff ()
            workItemId |> Work |> post
            return! loop tryReceive Writing
        | _, Idle -> return! loop tryReceive Idle
    }

    let agent = Agent.Start(fun inbox ->
        printfn "Writer started..."
        let tryReceive () = inbox.TryReceive(timeout = 0)
        loop tryReceive Idle)

    agent.Post

let postToReader log =
    let rec loop receive () = async {
        let! msg = receive ()
        match msg with
        | Work id ->
            let workItem = readStuff id
            // simulate processing
            do! Async.Sleep workItem.Value
            sprintf "Elapsed processing time: %d" workItem.Value |> log
            return! loop receive ()
        | StopReading -> printfn "Reader is stopping..."
    }

    let agent = Agent.Start(fun inbox ->
        printfn "Reader started..."
        loop inbox.Receive ())
    agent.Post

// see Watcher.fsx for what to do with this log file :)
let log (txt: string) =
    use sw = File.AppendText(baseDir + @"\log.txt")
    txt |> printfn "%s"
    txt |> sw.WriteLine

let postToReader' = postToReader log
let postToWriter' = postToWriter postToReader'

StartWriting |> postToWriter'
//StopReading |> postToReader'
//StopWriting |> postToWriter'