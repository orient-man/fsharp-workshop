module Classes

// F# classes/interfaces are little bit more concise...
type IEmailService =
    abstract member Send: address: string * subject: string * body: string -> bool

type WorkshopService (emailService: IEmailService) =
    member __.GiveFeedback(isPositive) =
        if isPositive then
            if not (emailService.Send("orientman{at}gmail-dot-com", "Workshop", "Awesome work! Thank You!")) then
                failwith "Notification failed!"