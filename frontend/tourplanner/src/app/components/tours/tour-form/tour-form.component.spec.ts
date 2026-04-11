import { ComponentFixture, TestBed } from '@angular/core/testing';
import { TourFormComponent } from './tour-form.component';

describe('TourFormComponent', () => {
  let component: TourFormComponent;
  let fixture: ComponentFixture<TourFormComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TourFormComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TourFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should have bike as default transport type', () => {
    expect(component.formData.transportType).toBe('bike');
  });

  it('isValid() should return false when name is empty', () => {
    component.formData = { ...component.formData, name: '', from: 'Wien', to: 'Graz', distance: 10, estimatedTime: 60 };
    expect(component.isValid()).toBeFalse();
  });

  it('isValid() should return false when name is only whitespace', () => {
    component.formData = { ...component.formData, name: '   ', from: 'Wien', to: 'Graz', distance: 10, estimatedTime: 60 };
    expect(component.isValid()).toBeFalse();
  });

  it('isValid() should return false when from is empty', () => {
    component.formData = { ...component.formData, name: 'Radtour', from: '', to: 'Graz', distance: 10, estimatedTime: 60 };
    expect(component.isValid()).toBeFalse();
  });

  it('isValid() should return false when from is only whitespace', () => {
    component.formData = { ...component.formData, name: 'Radtour', from: '  ', to: 'Graz', distance: 10, estimatedTime: 60 };
    expect(component.isValid()).toBeFalse();
  });

  it('isValid() should return false when to is empty', () => {
    component.formData = { ...component.formData, name: 'Radtour', from: 'Wien', to: '', distance: 10, estimatedTime: 60 };
    expect(component.isValid()).toBeFalse();
  });

  it('isValid() should return false when to is only whitespace', () => {
    component.formData = { ...component.formData, name: 'Radtour', from: 'Wien', to: '   ', distance: 10, estimatedTime: 60 };
    expect(component.isValid()).toBeFalse();
  });

  it('isValid() should return false when distance is 0', () => {
    component.formData = { ...component.formData, name: 'Radtour', from: 'Wien', to: 'Graz', distance: 0, estimatedTime: 60 };
    expect(component.isValid()).toBeFalse();
  });

  it('isValid() should return false when distance is negative', () => {
    component.formData = { ...component.formData, name: 'Radtour', from: 'Wien', to: 'Graz', distance: -5, estimatedTime: 60 };
    expect(component.isValid()).toBeFalse();
  });

  it('isValid() should return false when estimatedTime is 0', () => {
    component.formData = { ...component.formData, name: 'Radtour', from: 'Wien', to: 'Graz', distance: 10, estimatedTime: 0 };
    expect(component.isValid()).toBeFalse();
  });

  it('isValid() should return false when estimatedTime is negative', () => {
    component.formData = { ...component.formData, name: 'Radtour', from: 'Wien', to: 'Graz', distance: 10, estimatedTime: -30 };
    expect(component.isValid()).toBeFalse();
  });

  it('isValid() should return true when all required fields are valid', () => {
    component.formData = { ...component.formData, name: 'Radtour', from: 'Wien', to: 'Graz', distance: 10, estimatedTime: 60 };
    expect(component.isValid()).toBeTrue();
  });

  it('onSubmit() should set submitted to true', () => {
    expect(component.submitted).toBeFalse();
    component.onSubmit();
    expect(component.submitted).toBeTrue();
  });

  it('onSubmit() should not emit save when form is invalid', () => {
    const emitSpy = spyOn(component.save, 'emit');
    component.formData = { ...component.formData, name: '', from: '', to: '', distance: 0, estimatedTime: 0 };
    component.onSubmit();
    expect(emitSpy).not.toHaveBeenCalled();
  });

  it('onSubmit() should emit formData when form is valid', () => {
    const emitSpy = spyOn(component.save, 'emit');
    component.formData = { ...component.formData, name: 'Radtour', from: 'Wien', to: 'Graz', distance: 10, estimatedTime: 60 };
    component.onSubmit();
    expect(emitSpy).toHaveBeenCalledWith(component.formData);
  });
});
