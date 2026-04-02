import { Component } from '@angular/core';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-login',
  imports: [RouterModule, FormsModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css',
})
export class LoginComponent {
  email = '';
  password = '';

  onSubmit(): void {
    console.log(this.email, this.password);
  }
}
