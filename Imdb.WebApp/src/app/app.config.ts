import { ApplicationConfig, inject, provideAppInitializer, provideZoneChangeDetection } from '@angular/core';
import { provideAnimationsAsync } from '@angular/platform-browser/animations/async';
import { provideHttpClient } from '@angular/common/http';
import { provideRouter } from '@angular/router';

import { routes } from './app.routes';

import { ConfigService } from './services/config.service';

export const appConfig: ApplicationConfig = {
  providers: [
    provideAnimationsAsync(),
    provideAppInitializer(() => {
      let configService = inject(ConfigService);
      return configService.loadConfiguration('./assets/appsettings.json');
    }),
    provideHttpClient(),
    provideRouter(routes), 
    provideZoneChangeDetection({ eventCoalescing: true }), 
  ]
};
