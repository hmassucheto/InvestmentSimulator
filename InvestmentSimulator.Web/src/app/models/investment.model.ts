export interface InvestmentRequest {
    initialValue: number;
    months: number;
  }
  
  export interface InvestmentResult {
    grossAmount: number;
    netAmount: number;
  }