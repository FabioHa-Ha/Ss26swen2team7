import { Component } from '@angular/core';
import { Router, RouterModule } from '@angular/router';
import { AuthService } from '../../../services/auth.service';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-register',
  imports: [RouterModule, FormsModule],
  templateUrl: './register.component.html',
  styleUrl: './register.component.css',
})
export class RegisterComponent {
  username = '';
  password = '';

  constructor (private authService: AuthService, private router: Router) {}


  onSubmit(): void {
    this.authService.register({ username: this.username, password: this.password }).subscribe({
      next: () => this.router.navigate(['/login']),
      error: (err) => console.error('Registration failed', err)
    });
  }
}
