import { NgModule, APP_INITIALIZER } from '@angular/core';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule } from '@angular/common/http';

import { AppRoutingModule } from './app-routing.module';

import { AgGridModule } from 'ag-grid-angular';

import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatMenuModule } from '@angular/material/menu';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { MatToolbarModule } from '@angular/material/toolbar';

import { MainComponent } from './main/main.component';
import { NameBasicsGridComponent } from './grid-children/name-basics-grid.component';
import { TitleAkasGridComponent } from './grid-children/title-akas-grid.component';
import { TitleBasicsGridComponent } from './grid-children/title-basics-grid.component';
import { TitleCrewGridComponent } from './grid-children/title-crew-grid.component';
import { TitleEpisodesGridComponent } from './grid-children/title-episodes-grid.component';
import { TitlePrincipalsGridComponent } from './grid-children/title-principals-grid.component';
import { TitleRatingsGridComponent } from './grid-children/title-ratings-grid.component';

import { AgGridOptionsService } from './services/ag-grid-options.service';
import { ConfigService } from './services/config.service';
import { DataService } from './services/data.service';

function appInitializerFactory(configService: ConfigService) {
  return () => configService.loadConfiguration('./assets/appsettings.json');
};

@NgModule({
  declarations: [
    MainComponent,
    NameBasicsGridComponent,
    TitleAkasGridComponent,
    TitleBasicsGridComponent,
    TitleCrewGridComponent,
    TitleEpisodesGridComponent,
    TitlePrincipalsGridComponent,
    TitleRatingsGridComponent,
  ],
  imports: [
    AgGridModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    BrowserModule,
    HttpClientModule,
    MatButtonModule,
    MatIconModule,
    MatMenuModule,
    MatSnackBarModule,
    MatToolbarModule,
  ],
  providers: [
    AgGridOptionsService,
    ConfigService,
    DataService,
    {
      provide: APP_INITIALIZER,
      multi: true,
      useFactory: appInitializerFactory,
      deps: [ConfigService]
    }
  ],
  bootstrap: [MainComponent]
})
export class AppModule { }
