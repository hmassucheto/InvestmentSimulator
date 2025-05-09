import { Component } from '@angular/core';
import { InvestmentFormComponent } from './components/investment-form/investment-form.component';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [InvestmentFormComponent],
  template: '<app-investment-form></app-investment-form>',
})
export class AppComponent {}
