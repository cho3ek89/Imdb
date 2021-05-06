import { NgModule, APP_INITIALIZER } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule } from '@angular/common/http';

import { AppRoutingModule } from './app-routing.module';

import { AgGridOptionsProvider } from './services/ag-grid-options.provider';
import { ConfigService } from './services/config.service';
import { DataService } from './services/data.service';

import { MainComponent } from './main/main.component';
import { NameBasicsGridComponent } from './name-basics-grid/name-basics-grid.component';
import { TitleAkasGridComponent } from './title-akas-grid/title-akas-grid.component';
import { TitleBasicsGridComponent } from './title-basics-grid/title-basics-grid.component';
import { TitleCrewGridComponent } from './title-crew-grid/title-crew-grid.component';
import { TitleEpisodesGridComponent } from './title-episodes-grid/title-episodes-grid.component';
import { TitlePrincipalsGridComponent } from './title-principals-grid/title-principals-grid.component';
import { TitleRatingsGridComponent } from './title-ratings-grid/title-ratings-grid.component';

import { AgGridModule } from 'ag-grid-angular';
import { ToastNoAnimationModule } from 'ngx-toastr';

function appInitializerFactory(configService: ConfigService) {
  return () => configService.loadConfiguration('./assets/appSettings.json');
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
    BrowserModule,
    HttpClientModule,
    AppRoutingModule,
    AgGridModule.withComponents([]),
    ToastNoAnimationModule.forRoot(),
  ],
  providers: [
    AgGridOptionsProvider,
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
