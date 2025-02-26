import { bootstrapApplication } from '@angular/platform-browser';

import { AllCommunityModule, ModuleRegistry } from 'ag-grid-community';

import { appConfig } from './app/app.config';
import { MainComponent } from './app/main/main.component';

ModuleRegistry.registerModules([
  AllCommunityModule,
]);

bootstrapApplication(MainComponent, appConfig)
  .catch((err) => console.error(err));
