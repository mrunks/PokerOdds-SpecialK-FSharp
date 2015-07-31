open PokerOdds.SpecialK

[<EntryPoint>]
let main argv = 
    
    //Get the rank of a five Card hand
    let fiveCardEval = new FiveCardEvaluator.FiveEval()
    fiveCardEval.Initialize() |> ignore
    let rank5 = fiveCardEval.GetRank(0, 4, 8, 12, 16);


    //Get the rank of a seven card hand
    let sevenEval = new SevenCardEvaluator.SevenEval()
    sevenEval.Initialize() 
    let rank = sevenEval.GetRank(0, 4, 8, 12, 16, 20, 24);
    printfn "%u" rank
    0
