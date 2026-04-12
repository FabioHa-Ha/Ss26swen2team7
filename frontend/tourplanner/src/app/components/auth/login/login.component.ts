import { Component } from '@angular/core';
import { Router, RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { AuthService } from '../../../services/auth.service';

@Component({
  selector: 'app-login',
  imports: [RouterModule, FormsModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css',
})
export class LoginComponent {
  username = '';
  password = '';

  constructor(private authService: AuthService, private router: Router) {}

  onLogin(username: string, password: string): void {
    this.authService.login( {username, password }).subscribe({
      next: () => this.router.navigate(['/tours/dashboard']),
      error: (err) => console.error('Login failed', err)
    });
  }

  onSubmit(): void {
    this.onLogin(this.username, this.password);
  }
}
