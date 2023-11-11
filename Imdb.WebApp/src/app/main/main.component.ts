import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink, RouterLinkActive, RouterOutlet } from '@angular/router';

import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatMenuModule } from '@angular/material/menu';
import { MatToolbarModule } from '@angular/material/toolbar';

@Component({
  selector: 'main',
  standalone: true,
  imports: [
    CommonModule, 
    MatButtonModule, 
    MatIconModule, 
    MatMenuModule, 
    MatToolbarModule, 
    RouterLink, 
    RouterLinkActive, 
    RouterOutlet, 
  ],
  templateUrl: 'main.component.html',
  styleUrls: ['main.component.scss'],
})
export class MainComponent {

}
