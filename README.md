# PokerOdds-SpecialK-FSharp
F# port of Kenneth Shackleton's  Texas Hold'em 5- and 7-card hand evaluator written in C++

Example as done in Test.fs

    
    let sevenEval = new SevenCardEvaluator.SevenEval()
    sevenEval.Initialize() 
    let rank = sevenEval.GetRank(0, 4, 8, 12, 16, 20, 24);
    
    Initialize Loads all of the necessary arrays
