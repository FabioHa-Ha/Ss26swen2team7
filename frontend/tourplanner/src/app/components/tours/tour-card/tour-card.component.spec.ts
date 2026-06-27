import { ComponentFixture, TestBed } from '@angular/core/testing';
import { provideRouter } from '@angular/router';

import { TourCardComponent } from './tour-card.component';

describe('TourCardComponent', () => {
  let component: TourCardComponent;
  let fixture: ComponentFixture<TourCardComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TourCardComponent],
      providers: [provideRouter([])]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TourCardComponent);
    component = fixture.componentInstance;
    // tour is a required input, so it must be set before change detection.
    fixture.componentRef.setInput('tour', {
      id: 1,
      name: 'Radtour',
      description: '',
      transportTypeId: 1,
      fromLocation: 'Wien',
      toLocation: 'Graz',
      distance: 10,
      estimatedTime: 60
    });
    fixture.componentRef.setInput('stats', {
      popularity: 3,
      childFriendliness: 4,
      averageRating: 4.5,
      totalLogs: 2
    });
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
