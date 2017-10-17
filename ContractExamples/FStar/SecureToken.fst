module ZenModule

module V = Zen.Vector
module O = Zen.Option

open Zen.Types
open Zen.Cost

val parse_outpoint: n:nat & inputData n -> option outpoint
let parse_outpoint = function
  | (| _ , Outpoint o |) -> Some o
  | _ -> None

val cost_fn: inputMsg -> cost nat 0
let cost_fn _ = ret 0

val secureToken: inputMsg -> cost (result transactionSkeleton) 0
let secureToken i =
  let open O in

  let resTx = match parse_outpoint i.data with
    | Some outpoint ->
        begin match i.utxo outpoint with
          | Some output ->
            let outpoints = V.VCons outpoint V.VNil in

            let lock = PKLock (Zen.Util.hashFromBase64 "AAEECRAZJDFAUWR5kKnE4QAhRGmQueQRQHGk2RBJhME=") in

            let connotativeOutput = {
              lock = lock;
              spend = output.spend
            } in

            let tokenOutput = {
              lock = lock;
              spend = {
                asset = i.contractHash;
                amount = 1000UL
              }
            } in

            V (Tx outpoints [| tokenOutput; connotativeOutput |] None)
          | None -> Err "Cannot resolve outpoint"
        end
    | None -> Err "Cannot parse outpoint" in

  ret resTx


val main: mainFunction
let main = MainFunc (CostFunc cost_fn) secureToken
