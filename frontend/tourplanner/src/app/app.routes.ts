import { Routes } from '@angular/router';
import { authGuard } from './services/guards/auth.guard';

export const routes: Routes = [
    {
        path: '',
        redirectTo: 'login',
        pathMatch: 'full'
    },
    {
        path: 'register',
        loadComponent: () => import('./components/auth/register/register.component').then(m => m.RegisterComponent)
    },
    {
        path: 'login',
        loadComponent: () => import('./components/auth/login/login.component').then(m => m.LoginComponent)
    },
    {
        path: 'tours',
        canActivate: [authGuard],
        loadComponent: () => import('./components/layout/main-layout/main-layout.component').then(m => m.MainLayoutComponent),
        children: [
            {
                path: '',
                loadComponent: () => import('./components/tours/tour-list/tour-list.component').then(m => m.TourListComponent)
            },
            {
                path: 'dashboard',
                loadComponent: () => import('./components/dashboard/dashboard.component').then(m => m.DashboardComponent)
            },
            {
                path: 'search',
                loadComponent: () => import('./components/search/search-panel/search-panel.component').then(m => m.SearchPanelComponent)
            },
            {
                path: ':id',
                loadComponent: () => import('./components/tours/tour-detail/tour-detail.component').then(m => m.TourDetailComponent)
            }
        ]
    }
];
