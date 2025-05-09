import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { InvestmentResult } from '../models/investment.model';

@Injectable({ providedIn: 'root' })
export class InvestmentService {
  private readonly baseUrl = 'http://localhost:5000/api/investments';

  constructor(private http: HttpClient) {}

  calculate(input: { initialValue: number; months: number }): Observable<InvestmentResult> {
    return this.http.post<InvestmentResult>(this.baseUrl, input);
  }
}
