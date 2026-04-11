import { ComponentFixture, TestBed } from '@angular/core/testing';
import { TourLogFormComponent } from './tour-log-form.component';

describe('TourLogFormComponent', () => {
  let component: TourLogFormComponent;
  let fixture: ComponentFixture<TourLogFormComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TourLogFormComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TourLogFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should have 5 difficulty options', () => {
    expect(component.difficultyOptions.length).toBe(5);
  });

  it('isValid() should return false when totalDistance is 0', () => {
    component.formData = { ...component.formData, totalDistance: 0, totalTime: 60, rating: 3 };
    expect(component.isValid()).toBeFalse();
  });

  it('isValid() should return false when totalDistance is negative', () => {
    component.formData = { ...component.formData, totalDistance: -10, totalTime: 60, rating: 3 };
    expect(component.isValid()).toBeFalse();
  });

  it('isValid() should return false when totalTime is 0', () => {
    component.formData = { ...component.formData, totalDistance: 10, totalTime: 0, rating: 3 };
    expect(component.isValid()).toBeFalse();
  });

  it('isValid() should return false when totalTime is negative', () => {
    component.formData = { ...component.formData, totalDistance: 10, totalTime: -30, rating: 3 };
    expect(component.isValid()).toBeFalse();
  });

  it('isValid() should return false when rating is 0', () => {
    component.formData = { ...component.formData, totalDistance: 10, totalTime: 60, rating: 0 };
    expect(component.isValid()).toBeFalse();
  });

  it('isValid() should return false when rating is 6', () => {
    component.formData = { ...component.formData, totalDistance: 10, totalTime: 60, rating: 6 };
    expect(component.isValid()).toBeFalse();
  });

  it('isValid() should return false when difficulty is 0', () => {
    component.formData = { ...component.formData, totalDistance: 10, totalTime: 60, rating: 3, difficulty: 0 };
    expect(component.isValid()).toBeFalse();
  });

  it('isValid() should return false when difficulty is 6', () => {
    component.formData = { ...component.formData, totalDistance: 10, totalTime: 60, rating: 3, difficulty: 6 };
    expect(component.isValid()).toBeFalse();
  });

  it('isValid() should return false when date is empty', () => {
    component.formData = { ...component.formData, date: '', totalDistance: 10, totalTime: 60, rating: 3 };
    expect(component.isValid()).toBeFalse();
  });

  it('isValid() should return true when all fields are valid', () => {
    component.formData = {
      date: '2026-04-11T10:00',
      comment: 'Schöne Tour',
      difficulty: 3,
      totalDistance: 15,
      totalTime: 90,
      rating: 4,
    };
    expect(component.isValid()).toBeTrue();
  });

  it('isValid() should return true with boundary rating of 1', () => {
    component.formData = { ...component.formData, totalDistance: 10, totalTime: 60, rating: 1, difficulty: 1 };
    expect(component.isValid()).toBeTrue();
  });

  it('isValid() should return true with boundary rating of 5', () => {
    component.formData = { ...component.formData, totalDistance: 10, totalTime: 60, rating: 5, difficulty: 5 };
    expect(component.isValid()).toBeTrue();
  });

  it('onSubmit() should set submitted to true', () => {
    expect(component.submitted).toBeFalse();
    component.onSubmit();
    expect(component.submitted).toBeTrue();
  });

  it('onSubmit() should not emit save when form is invalid', () => {
    const emitSpy = spyOn(component.save, 'emit');
    component.formData = { ...component.formData, totalDistance: 0, totalTime: 0, rating: 0 };
    component.onSubmit();
    expect(emitSpy).not.toHaveBeenCalled();
  });

  it('onSubmit() should emit data when form is valid', () => {
    const emitSpy = spyOn(component.save, 'emit');
    component.formData = {
      date: '2026-04-11T10:00',
      comment: 'Schöne Tour',
      difficulty: 3,
      totalDistance: 15,
      totalTime: 90,
      rating: 4,
    };
    component.onSubmit();
    expect(emitSpy).toHaveBeenCalled();
  });
});
