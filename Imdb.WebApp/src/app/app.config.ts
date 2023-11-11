import { APP_INITIALIZER, ApplicationConfig } from '@angular/core';
import { provideAnimations } from '@angular/platform-browser/animations';
import { provideHttpClient } from '@angular/common/http';
import { provideRouter } from '@angular/router';

import { routes } from './app.routes';

import { ConfigService } from './services/config.service';

function appInitializerFactory(configService: ConfigService) {
  return () => configService.loadConfiguration('./assets/appsettings.json');
};

export const appConfig: ApplicationConfig = {
  providers: [
    provideAnimations(),
    provideHttpClient(),
    provideRouter(routes), 
    {
      provide: APP_INITIALIZER,
      multi: true,
      useFactory: appInitializerFactory,
      deps: [ConfigService],
    },
  ],
};
