namespace PokerOdds.SpecialK
    (**************************
    All code below is created based on the C++ version of 
    Kenneth J. Shackleton's FiveEval.h and FiveEval.cpp files located at
    https://github.com/kennethshackleton/SpecialKEval/tree/develop/src
    ********************************)
    module FiveCardEvaluator =

        type FiveEval() =
            let mRankPtr = Array.init (MAX_FIVE_NONFLUSH_KEY_INT + 1u |> int32) (fun f -> 0us )
            let mFlushRankPtr = Array.init (((MAX_FIVE_FLUSH_KEY_INT + 1u) |> int32))  (fun f-> 0us)
            let mDeckcardsFace = Array.init 52 (fun (f)-> 0u)
            let mDeckcardsFlush  = Array.init 52 (fun (f)-> 0us);
            let mDeckcardsSuit  = Array.init 52 (fun (f)-> 0us);

            let face = Common.five_face
            let face_flush = five_face_flush
        
            let rec SetHighCardRank(i, j, k, l, m, counter:uint16) =
                let falseCondition = ((i-m) = 4) ||  (i = 12 && j = 3)
                match m with 
                | m when (m < l && (falseCondition = false)) ->
                    let ordinal = (int)(face.[i] + face.[j] + face.[k] + face.[l] + face.[m])
                    mRankPtr.[ordinal] <- counter
                    SetHighCardRank(i, j, k,l, m+1,  (counter + 1us))
                | _ -> counter

            let SetDeckCards() =
                [0..12] |> Seq.iter( fun n -> 
                    let N = n <<< 2
                    let faceCard = face.[12-n];
                    let faceFlush = face_flush.[12-n] |> uint16

                    mDeckcardsSuit.[N] <-  SPADE |> uint16
                    mDeckcardsSuit.[N+1] <-  HEART |> uint16
                    mDeckcardsSuit.[N+2] <-  DIAMOND |> uint16
                    mDeckcardsSuit.[N+3] <-  CLUB |> uint16
                
                    mDeckcardsFace.[N]   <- faceCard
                    mDeckcardsFace.[N+1] <- faceCard
                    mDeckcardsFace.[N+2] <- faceCard
                    mDeckcardsFace.[N+3] <- faceCard 
                
                    mDeckcardsFlush.[N] <- faceFlush
                    mDeckcardsFlush.[N+1] <- faceFlush
                    mDeckcardsFlush.[N+2] <- faceFlush
                    mDeckcardsFlush.[N+3] <- faceFlush
                )
            
            let SetHighCardRanks(counter) = 
                let mutable tempCounter = counter
                for i in 5..12 do
                    for j in 3..(i-1) do
                    for k in 2..(j-1) do
                    for l in 1..(k-1) do
                    let m = 0
                    tempCounter <- SetHighCardRank(i, j, k, l, m, tempCounter)
                tempCounter   
           
            let SetPairRanks(counter) = 
                let mutable tempCounter = counter
                for i in 0..(12) do
                    for j in 2..(12) do
                    for k in 1..(j-1) do
                    for l in 0..(k-1) do
                    match  ((i <> j) && (i <> k) && (i <> l)) with 
                    | true ->
                        let ordinal = ((face.[i] <<< 1) + face.[j] + face.[k] + face.[l]) |> int
                        mRankPtr.[ordinal] <- tempCounter
                        tempCounter <- tempCounter + 1us
                    | _ -> ()
                tempCounter
            
            let SetTwoPairRanks(counter) = 
                let mutable tempCounter = counter
                for i in 1..12 do
                    for j in 0..(i-1) do
                    for k in 0..12 do
                    match k with 
                    | k when (k <> i && k <> j) ->
                        let ordinal = ((face.[i] <<< 1) + (face.[j]<<<1) + (uint32)(face.[k])) |> int
                        mRankPtr.[ordinal] <- tempCounter
                        tempCounter <- tempCounter + 1us
                    | _ -> ()
                tempCounter
               
            let SetThreeOfAKindRanks(counter) = 
                let mutable tempCounter = counter
                for i in 0..12 do
                    for j in 1..12 do
                    for k in 0..(j-1) do
                    match i with 
                    | i when (i <> j && i <> k) ->
                        let rankOrdinal = ((3u * face.[i]) + face.[j] + face.[k]) |> int
                        mRankPtr.[rankOrdinal] <- counter
                        tempCounter <- tempCounter + 1us
                    | _ -> ()
                tempCounter

            let SetLowStraightNonFlushRanks(counter) =
                let ordinal = (Array.sum face.[0..3] + face.[12]) |> int
                mRankPtr.[ordinal] <- counter
                counter + 1us
        
            let SetStraightNonFlushRanks(counter) =
                [0..8] |> Seq.fold(fun (acc:uint16)  i -> 
                    let ordinal = Array.sum face.[i..(i + 4)] |> int
                    mRankPtr.[ordinal] <- acc
                    acc + 1us
                ) counter
            
            let SetFlushNotStraightRanks(counter) =
                let mutable tempCounter = counter
                for i in 5..12 do
                    for j in 3..(i-1) do 
                    for k in 2..(j-1) do
                    for l in 1..(k-1) do
                    for m in 0..(l-1) do
                    match ((i-m = 4)  || (i = 12 && j = 3)) with 
                    | false ->
                        let ordinal = (face_flush.[i] + face_flush.[j] + face_flush.[k] + face_flush.[l] + face_flush.[m]) |> int
                        mFlushRankPtr.[ordinal] <- tempCounter
                        tempCounter <- (tempCounter + 1us)
                    | true -> ()
                tempCounter

            let SetFullHouseRanks(counter) =
                let mutable tempCounter = counter 
                for i in 0..12 do
                    for j in 0..12 do
                    match i <> j with 
                    | true ->
                        let ordinal = ((3u * face.[i]) + (face.[j]<<<1)) |> int32
                        mRankPtr.[ordinal] <- tempCounter
                        tempCounter <- (tempCounter + 1us)
                    | false -> ()
                tempCounter

            let SetFourOfAKindRanks(counter) =
                let mutable tempCounter = counter 
                for i in 0..12 do
                    for j in 0..12 do
                    match i <> j with 
                    | true ->
                        let ordinal = ((face.[i] <<<2) + (uint32)face.[j]) |> int
                        mRankPtr.[ordinal] <- counter
                        tempCounter <- tempCounter + 1us
                    | false -> ()
                tempCounter

            let SetLowStraightFlushRanks(counter) =
                let ordinal = (face_flush.[0] + face_flush.[1] + face_flush.[2] + face_flush.[3] + face_flush.[12]) |> int
                mFlushRankPtr.[ordinal] <- counter
                (counter + 1us)

            let SetUsualStraightFlushRanks(counter) =
                let mutable tempCounter = counter
                for i in 0..8 do
                    let ordinal = Array.sum face_flush.[i..(i+4)] |> int
                    mFlushRankPtr.[ordinal] <- tempCounter
                    tempCounter <- (tempCounter + 1us)
                tempCounter

            member this.Initialize() =
                let totalCount =
                    SetDeckCards()
                    SetHighCardRanks(1us) |> 
                    SetPairRanks |> 
                    SetTwoPairRanks |> 
                    SetThreeOfAKindRanks |>
                    SetLowStraightNonFlushRanks |>
                    SetStraightNonFlushRanks |>
                    SetFlushNotStraightRanks |>
                    SetFullHouseRanks |>
                    SetFourOfAKindRanks |>
                    SetLowStraightFlushRanks |>
                    SetUsualStraightFlushRanks
                totalCount


            member this.GetRank(card_one, card_two, card_three, card_four, card_five) =
                match card_one with 
                | card_one when (mDeckcardsSuit.[card_one] = mDeckcardsSuit.[card_two] && 
                                    mDeckcardsSuit.[card_one] = mDeckcardsSuit.[card_three] && 
                                    mDeckcardsSuit.[card_one] = mDeckcardsSuit.[card_four] && 
                                    mDeckcardsSuit.[card_one] = mDeckcardsSuit.[card_five]) -> 

                    let ordinal = (mDeckcardsFlush.[card_one] + 
                                            mDeckcardsFlush.[card_two] + 
                                            mDeckcardsFlush.[card_three] + 
                                            mDeckcardsFlush.[card_four]  + 
                                            mDeckcardsFlush.[card_five]) |> int
                    mFlushRankPtr.[ordinal]
                | _ -> 
                    mRankPtr.[  (mDeckcardsFace.[card_one] + 
                                 mDeckcardsFace.[card_two] +  
                                 mDeckcardsFace.[card_three] +  
                                 mDeckcardsFace.[card_four] + 
                                 mDeckcardsFace.[card_five]) |> int32];

            
            member this.GetRankFromSeven(card_one, card_two, card_three, card_four, card_five, card_six, card_seven) =
                let seven_cards : int array = [|card_one; card_two; card_three; card_four; card_five; card_six; card_seven|]
            
                let temp = Array.init 5 (fun f-> 0)
                let mutable best_rank_so_far = 0us
         
                let mutable m = 0
                for i in 1..6 do
                    for j in 0..(i-1) do
                    m <- 0
                    for k in 0..6 do
                        match k with
                        | k when (k <> i && k <> j) -> 
                            temp.[m] <- seven_cards.[k]
                            m <- m+1                           
                        | _-> ()
                
                    let current_rank = this.GetRank(temp.[0], temp.[1], temp.[2], temp.[3], temp.[4])
                
                    match best_rank_so_far < current_rank with
                    | true ->
                        best_rank_so_far <- current_rank
                    | false -> ()
                best_rank_so_far
     
           