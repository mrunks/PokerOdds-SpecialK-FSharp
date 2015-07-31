namespace PokerOdds.SpecialK
    (****************************************************
    All code below is created based on the C++ version of 
    Kenneth J. Shackleton's SevenEval.h and SevenEval.cpp files located at
    https://github.com/kennethshackleton/SpecialKEval/tree/develop/src
    ************************************************)
    open FiveCardEvaluator

    module SevenCardEvaluator = 
        
        type SevenEval() =

            let mRankPtr = Array.init (CIRCUMFERENCE_SEVEN) (fun f -> 0us )
            let mFlushRankPtr = Array.init (((MAX_SEVEN_FLUSH_KEY_INT + 1u) |> int32))  (fun f-> 0us)

            let  mDeckcardsKey  = Array.init 52 (fun (f)-> 0UL);
            let  mDeckcardsFlush  = Array.init 52 (fun (f)-> 0us);
            let  mDeckcardsSuit  = Array.init 52 (fun (f)-> 0us);
            let  mFlushCheck  = Array.init (MAX_FLUSH_CHECK_SUM+1) (fun (f)-> UNVERIFIED |> int16);

            let face = Common.seven_face
            let face_flush = Common.seven_face_flush
                       
            let suits = [|SPADE; HEART; DIAMOND; CLUB |]
            let five_card_evaluator = FiveEval()
                
            let SetDeckCards() =
                [0..12] |> Seq.iter( fun n -> 
                    let N = n <<< 2
                    let start = face.[n] <<< NON_FLUSH_BIT_SHIFT
                    mDeckcardsKey.[N] <-    (start + SPADE) |> uint64
                    mDeckcardsKey.[N+1] <-  (start + HEART) |> uint64
                    mDeckcardsKey.[N+2] <-  (start + DIAMOND) |> uint64
                    mDeckcardsKey.[N+3] <-  (start + CLUB) |> uint64 

                    let flushCard = (uint16) face_flush.[n]

                    mDeckcardsFlush.[N]   <- flushCard
                    mDeckcardsFlush.[N+1] <- flushCard
                    mDeckcardsFlush.[N+2] <- flushCard
                    mDeckcardsFlush.[N+3] <- flushCard 

                    mDeckcardsSuit.[N] <- (uint16)SPADE
                    mDeckcardsSuit.[N+1] <- (uint16)HEART
                    mDeckcardsSuit.[N+2] <- (uint16)DIAMOND
                    mDeckcardsSuit.[N+3] <- uint16 <| CLUB
                )

            let SetNonFlushRanks() = 
                let mutable tempCounter = 0
                for i in 1..12 do
                    for j in 1..i do
                    for k in 1..j do
                    for l in 0..k do
                    for m in 0..l do
                    for n in 0..m do
                    for p in 0..n do
                        let key = face.[i] + face.[j] + face.[k] + face.[l] + face.[m] + face.[n] + face.[p]
                        let rank =  five_card_evaluator.GetRankFromSeven((i<<<2), (j<<<2), k<<<2, l<<<2,  ((m<<<2))+1, (n<<<2)+1, (p<<<2)+1)
                        let condition = key < CIRCUMFERENCE_SEVEN
                        tempCounter <- tempCounter + 1
                        match condition with 
                        | true -> mRankPtr.[key] <- rank 
                        | false -> mRankPtr.[key - CIRCUMFERENCE_SEVEN] <- rank
                tempCounter

            let SetSevenCardFlushRanks() =
                let mutable tempCounter = 0
                for i in 6..12 do
                    let I = (i<<<2)
                    for j in 5..(i-1) do
                    let J = (j<<<2)
                    for k in 4..(j-1) do
                    let K = (k<<<2)
                    for l in 3..(k-1) do
                    let L = (l<<<2)
                    for m in 2..(l-1) do
                    let M = (m<<<2)
                    for n in 1..(m-1) do
                    let N = (n<<<2)
                    for p in 0..(n-1) do
                        tempCounter <- tempCounter + 1
                        let key =   
                            face_flush.[i] + face_flush.[j] + face_flush.[k] + 
                            face_flush.[l] + face_flush.[m] + face_flush.[n] + face_flush.[p]
                        mFlushRankPtr.[key |> int] <- five_card_evaluator.GetRankFromSeven(I, J, K, L, M, N, p<<<2)
                tempCounter
            
            let SetSixCardFlushRanks() =
                let mutable tempCounter = 0
                for i in 5..12 do
                    let I = (i<<<2)
                    for j in 4..(i-1) do
                    let J = (j<<<2)
                    for k in 3..(j-1) do
                    let K = (k<<<2)
                    for l in 2..(k-1) do
                    let L = (l<<<2)
                    for m in 1..(l-1) do
                    let M = (m<<<2)
                    for n in 0..(m-1) do
                        tempCounter <- tempCounter + 1
                        let key = 
                            face_flush.[i] + face_flush.[j] + face_flush.[k] + 
                            face_flush.[l] + face_flush.[m] + face_flush.[n]
                        mFlushRankPtr.[key |> int] <- five_card_evaluator.GetRankFromSeven(I, J, K, L, M, (n<<<2), 51)
                tempCounter
            
            let SetFiveCardFlushRanks() =
                for i in 4..12 do
                    let I = (i<<<2)
                    for j in 3..(i-1) do
                    let J = (j<<<2)
                    for k in 2..(j-1) do
                    let K = (k<<<2)
                    for l in 1..(k-1) do
                    let L = (l<<<2)
                    for m in 0..(l-1) do
                        let key = face_flush.[i] + face_flush.[j] + face_flush.[k] + face_flush.[l] + face_flush.[m]
                        mFlushRankPtr.[key |> int] <- five_card_evaluator.GetRank(I, J, K, L, (m<<<2))


            let GetSuitCount(letterArray:int array, suits:int array , flush_index) = 
                letterArray |> Array.fold(fun (acc:int) f ->
                        match (suits.[f] = suits.[flush_index]) with
                        | true -> 1 + acc
                        | false -> acc
                    ) 0 
                |> int16

            let rec SetSuitsArray(cardArray: int array, cards_matched_so_far, flush_suit_index, suit_count) = 
                                
                match (cards_matched_so_far < 3 && flush_suit_index < 4) with
                | true ->
                    let local_flush_suit_index = flush_suit_index + 1
                    let local_suit_count = GetSuitCount( cardArray, suits, local_flush_suit_index)   
                    let local_cards_matched_so_far =  cards_matched_so_far + (local_suit_count  |>int ) 
                                         
                    SetSuitsArray( cardArray, (local_cards_matched_so_far),(local_flush_suit_index), local_suit_count)

                | false ->
                    (suit_count, flush_suit_index)
            
            let SetSevenCardFlush() =
                //let mutable flush_suit_index = 0
                let mutable suit_count = 0s
                for i in 0..3 do
                    for j in 0..i do
                    for k in 0..j do
                    for l in 0..k do
                    for m in 0..l do
                    for n in 0..m do
                    for p in 0..n do
                        //let flush_suit_index = 0
                        let cards_matched_so_far = 0
                        let suit_key = suits.[i] + suits.[j] + suits.[k] + suits.[l] + suits.[m] + suits.[n] + suits.[p]
                        let unverifiedCard =  (UNVERIFIED |> int16)
                        match ((unverifiedCard = mFlushCheck.[suit_key])) with
                        | true ->
                            //flush_suit_index <- flush_suit_index + 1
                            let tuple =  SetSuitsArray( [|i;j;k;l;m;n;p|], cards_matched_so_far, -1, suit_count )
                            let suit_count = fst tuple
                            let flush_suit_index = snd tuple
                            match ((suit_count |> int) > 4) with
                            | true -> mFlushCheck.[suit_key] <-  (suits.[flush_suit_index] |> int16)
                            | false -> mFlushCheck.[suit_key] <- int16 <| (NOT_A_FLUSH)
                        | false -> ()
          
            
            member this.Initialize() =
                five_card_evaluator.Initialize() |> ignore
                SetDeckCards()
                SetNonFlushRanks() |> ignore
                SetSevenCardFlushRanks() |> ignore
                SetSixCardFlushRanks()  |> ignore
                SetFiveCardFlushRanks() |> ignore
                SetSevenCardFlush()
            
            member private this.GetRankFromArray(letterArray:int array, flush_suit) = 
                letterArray |> 
                Array.fold(fun (acc:uint16) f ->
                    match (mDeckcardsSuit.[f] = flush_suit) with
                    | true -> mDeckcardsFlush.[f] + acc
                    | false -> acc
                ) 0us
                

            member this.GetRank(card1, card2, card3, card4, card5, card6, card7) = 
                let key = 
                    mDeckcardsKey.[card1] + mDeckcardsKey.[card2] + mDeckcardsKey.[card3] + 
                    mDeckcardsKey.[card4] + mDeckcardsKey.[card5] + mDeckcardsKey.[card6] + mDeckcardsKey.[card7]

                let flush_check_key = int32 <| (key &&& SUIT_BIT_MASK) 
                let flush_suit = int32 <| mFlushCheck.[flush_check_key];

                match (flush_suit = NOT_A_FLUSH) with 
                | true ->
                    let newKey = int32 <| (key >>> NON_FLUSH_BIT_SHIFT) 
                    match (newKey < CIRCUMFERENCE_SEVEN) with
                    | true ->  mRankPtr.[newKey]
                    | false -> mRankPtr.[(newKey - CIRCUMFERENCE_SEVEN)]
                | false ->
                    let flush_key = int32 <| this.GetRankFromArray([|card1;card2;card3;card4;card5;card6;card7|], (flush_suit |> uint16))
                    mFlushRankPtr.[flush_key]       
                    
                    
