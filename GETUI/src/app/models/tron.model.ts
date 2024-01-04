export class TronTransactionModel {
  public data!: TransactionDetails[];
  public success!: boolean;
}

export class TransactionDetails {
  public transaction_id!: string;
  public token_info!: TokenInfo;
  public block_timestamp!: number;
  public from!: string;
  public to!: string;
  public type!: string;
  public value!: string;
}

export class TokenInfo {
  public symbol!: string;
  public address!: string;
  public decimals!: number;
  public name!: string;
}
