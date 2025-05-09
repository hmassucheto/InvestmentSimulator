import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { NgxMaskDirective, provideNgxMask } from 'ngx-mask';
import { InvestmentService } from '../../services/investment.service';
import { InvestmentResult } from '../../models/investment.model';

@Component({
  selector: 'app-investment-form',
  standalone: true,
  imports: [ CommonModule, ReactiveFormsModule, NgxMaskDirective ],
  providers: [ provideNgxMask() ],
  templateUrl: './investment-form.component.html',
})
export class InvestmentFormComponent {
  form: FormGroup;
  result?: InvestmentResult;
  error?: string;

  constructor(
    private fb: FormBuilder,
    private service: InvestmentService
  ) {
    this.form = this.fb.group({
      initialValue: [
        null,
        [
          Validators.required,
          Validators.pattern(/^(?!0+(?:[.,]0+)?$)\d+(?:[.,]\d+)?$/)
        ]
      ],
      months: [
        null,
        [
          Validators.required,
          Validators.min(2),
          Validators.max(1200)
        ]
      ],
    });
  }

  ngOnInit() {
    this.form.valueChanges.subscribe(() => {
      this.error = undefined;
      Object.values(this.form.controls).forEach(ctrl => {
        if (ctrl.hasError('server')) {
          const { server, ...others } = ctrl.errors || {};
          ctrl.setErrors(Object.keys(others).length ? others : null);
        }
      });
    });
  }

  onSubmit() {
    if (this.form.invalid) return;

    const raw = String(this.form.value.initialValue)
      .replace(/[^0-9.,]/g, '');

    let normalized = raw;
    if (normalized.includes('.') && normalized.includes(',')) {
      normalized = normalized.replace(/\./g, '').replace(',', '.');
    } else {
      normalized = normalized.replace(',', '.');
    }
    const numericValue = parseFloat(normalized);

    const payload = {
      initialValue: numericValue,
      months: this.form.value.months
    };

    this.service.calculate(payload).subscribe({
      next: res => this.result = res,
      error: err => {
        console.error('Erro ao chamar API:', err);
        this.error = 'Erro ao simular investimento. Tente novamente mais tarde.';
      }
    });
  }
}
