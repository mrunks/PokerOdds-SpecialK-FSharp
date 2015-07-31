namespace PokerOdds.SpecialK
    (******************
    All code below is created based on the C++ version of 
    Kenneth J. Shackleton's Constants.h file located at
    https://github.com/kennethshackleton/SpecialKEval/blob/develop/src/Constants.h
    **********************)

    [<AutoOpen>]
    module Common =
        [<Literal>]
        let DECK_SIZE = 52
        [<Literal>]
        let NUMBER_OF_SUITS = 4
        [<Literal>]
        let NUMBER_OF_FACES = 13
        [<Literal>]
        let SPADE = 0
        [<Literal>]
        let HEART = 1
        [<Literal>]
        let DIAMOND = 8
        [<Literal>]
        let CLUB = 57
        [<Literal>]
        let TWO_FIVE = 0u
        [<Literal>]
        let THREE_FIVE = 1u
        [<Literal>]
        let FOUR_FIVE = 5u
        [<Literal>]
        let FIVE_FIVE = 22u
        [<Literal>]
        let SIX_FIVE = 94u
        [<Literal>]
        let SEVEN_FIVE = 312u
        [<Literal>]
        let EIGHT_FIVE = 992u
        [<Literal>]
        let NINE_FIVE = 2422u
        [<Literal>]
        let TEN_FIVE = 5624u
        [<Literal>]
        let JACK_FIVE =12522u
        [<Literal>]
        let QUEEN_FIVE =19998u
        [<Literal>]
        let KING_FIVE =43258u
        [<Literal>]
        let ACE_FIVE =79415u
        [<Literal>]
        let TWO_FLUSH =1u
        [<Literal>]
        let THREE_FLUSH =2u
        [<Literal>]
        let FOUR_FLUSH =4u
        [<Literal>]
        let FIVE_FLUSH =8u
        [<Literal>]
        let SIX_FLUSH = 16u
        [<Literal>]
        let SEVEN_FLUSH =32u
        [<Literal>]
        let EIGHT_FLUSH = 64u

        let NINE_FLUSH = (EIGHT_FLUSH + SEVEN_FLUSH + SIX_FLUSH + FIVE_FLUSH + FOUR_FLUSH + THREE_FLUSH + TWO_FLUSH + 1u)
        let TEN_FLUSH = (NINE_FLUSH + EIGHT_FLUSH + SEVEN_FLUSH + SIX_FLUSH + FIVE_FLUSH + FOUR_FLUSH + THREE_FLUSH + 1u)
        let JACK_FLUSH = (TEN_FLUSH + NINE_FLUSH + EIGHT_FLUSH + SEVEN_FLUSH + SIX_FLUSH + FIVE_FLUSH + FOUR_FLUSH + 1u)
        let QUEEN_FLUSH = (JACK_FLUSH + TEN_FLUSH + NINE_FLUSH + EIGHT_FLUSH + SEVEN_FLUSH + SIX_FLUSH + FIVE_FLUSH + 1u)
        let KING_FLUSH = (QUEEN_FLUSH + JACK_FLUSH + TEN_FLUSH + NINE_FLUSH + EIGHT_FLUSH + SEVEN_FLUSH + SIX_FLUSH + 1u)
        let ACE_FLUSH = (KING_FLUSH + QUEEN_FLUSH + JACK_FLUSH + TEN_FLUSH + NINE_FLUSH + EIGHT_FLUSH + SEVEN_FLUSH + 1u)
        
        [<Literal>]
        let TWO = 0
        [<Literal>]
        let THREE = 1
        [<Literal>]
        let FOUR = 5
        [<Literal>]
        let FIVE =22
        [<Literal>]
        let SIX =98
        [<Literal>]
        let SEVEN =453
        [<Literal>]
        let EIGHT =2031
        [<Literal>]
        let NINE =8698
        [<Literal>]
        let TEN =22854
        [<Literal>]
        let JACK =83661
        [<Literal>]
        let QUEEN =262349
        [<Literal>]
        let KING =636345
        [<Literal>]
        let ACE =1479181

        let MAX_FIVE_NONFLUSH_KEY_INT = ((4u * ACE_FIVE) + KING_FIVE)

        let MAX_FIVE_FLUSH_KEY_INT = (ACE_FLUSH + KING_FLUSH + QUEEN_FLUSH + JACK_FLUSH + TEN_FLUSH)
        let MAX_SEVEN_FLUSH_KEY_INT = (ACE_FLUSH+KING_FLUSH+QUEEN_FLUSH+JACK_FLUSH+ TEN_FLUSH + NINE_FLUSH+EIGHT_FLUSH)

        [<Literal>]
        let MAX_FLUSH_CHECK_SUM = 399
        [<Literal>]
        let CIRCUMFERENCE_SEVEN = 4565145
        // Used in flush checking. These must be distinct from each of the suits.
        [<Literal>]
        let UNVERIFIED = -2
        [<Literal>]
        let NOT_A_FLUSH = -1
        // Bit masks
        [<Literal>]
        let SUIT_BIT_MASK = 511UL
        [<Literal>]
        let NON_FLUSH_BIT_SHIFT = 9

        let public five_face  = [| TWO_FIVE; THREE_FIVE; FOUR_FIVE; FIVE_FIVE; SIX_FIVE;SEVEN_FIVE;EIGHT_FIVE;NINE_FIVE;TEN_FIVE;JACK_FIVE;QUEEN_FIVE;KING_FIVE;ACE_FIVE |]
        let five_face_flush = [|TWO_FLUSH; THREE_FLUSH; FOUR_FLUSH; FIVE_FLUSH;SIX_FLUSH;SEVEN_FLUSH;EIGHT_FLUSH;NINE_FLUSH;TEN_FLUSH; JACK_FLUSH;QUEEN_FLUSH;KING_FLUSH;ACE_FLUSH|]

        let seven_face = [|ACE; KING; QUEEN; JACK; TEN; NINE; EIGHT; SEVEN; SIX; FIVE; FOUR; THREE; TWO |]
        let seven_face_flush = Array.rev five_face_flush
        

