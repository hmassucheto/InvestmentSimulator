import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ReactiveFormsModule, FormBuilder } from '@angular/forms';
import { of, throwError } from 'rxjs';
import { NO_ERRORS_SCHEMA } from '@angular/core';

import { InvestmentFormComponent } from './investment-form.component';
import { InvestmentService } from '../../services/investment.service';
import { InvestmentResult } from '../../models/investment.model';

describe('InvestmentFormComponent', () => {
  let component: InvestmentFormComponent;
  let fixture: ComponentFixture<InvestmentFormComponent>;
  let serviceSpy: jasmine.SpyObj<InvestmentService>;

  beforeEach(async () => {
    const spy = jasmine.createSpyObj('InvestmentService', ['calculate']);

    await TestBed.configureTestingModule({
      declarations: [ InvestmentFormComponent ],
      imports: [ ReactiveFormsModule ],
      providers: [
        FormBuilder,
        { provide: InvestmentService, useValue: spy }
      ],
      schemas: [ NO_ERRORS_SCHEMA ]
    })
    .compileComponents();

    serviceSpy = TestBed.inject(InvestmentService) as jasmine.SpyObj<InvestmentService>;
    fixture = TestBed.createComponent(InvestmentFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create the component', () => {
    expect(component).toBeTruthy();
  });

  it('should not call the service if the form is invalid', () => {
    component.form.controls['initialValue'].setValue(null);
    component.form.controls['months'].setValue(null);

    component.onSubmit();

    expect(serviceSpy.calculate).not.toHaveBeenCalled();
  });

  it('should convert the formatted value, call calculate, and assign the result on success', () => {
    const mockResult: InvestmentResult = {
      grossAmount: 1100,
      netAmount: 1050
    };
    serviceSpy.calculate.and.returnValue(of(mockResult));

    component.form.controls['initialValue'].setValue('1.234,56');
    component.form.controls['months'].setValue(12);

    component.onSubmit();

    expect(serviceSpy.calculate).toHaveBeenCalledWith({
      initialValue: 1234.56,
      months: 12
    });
    expect(component.result).toEqual(mockResult);
    expect(component.error).toBeUndefined();
  });

  it('should handle the error and assign a failure message', () => {
    serviceSpy.calculate.and.returnValue(throwError(() => new Error('API down')));

    component.form.controls['initialValue'].setValue('1000');
    component.form.controls['months'].setValue(6);

    component.onSubmit();

    expect(component.result).toBeUndefined();
    expect(component.error).toBe(
      'Error simulating investment. Please try again later.'
    );
  });
});
