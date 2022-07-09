import { NgModule, APP_INITIALIZER } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule } from '@angular/common/http';

import { AppRoutingModule } from './app-routing.module';

import { AgGridModule } from 'ag-grid-angular';
import { ToastNoAnimationModule } from 'ngx-toastr';

import { MainComponent } from './main/main.component';
import { NameBasicsGridComponent } from './grid-children/name-basics-grid.component';
import { TitleAkasGridComponent } from './grid-children/title-akas-grid.component';
import { TitleBasicsGridComponent } from './grid-children/title-basics-grid.component';
import { TitleCrewGridComponent } from './grid-children/title-crew-grid.component';
import { TitleEpisodesGridComponent } from './grid-children/title-episodes-grid.component';
import { TitlePrincipalsGridComponent } from './grid-children/title-principals-grid.component';
import { TitleRatingsGridComponent } from './grid-children/title-ratings-grid.component';

import { AgGridOptionsService } from 'src/services/ag-grid-options.service';
import { ConfigService } from 'src/services/config.service';
import { DataService } from 'src/services/data.service';

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
    BrowserModule,
    HttpClientModule,
    ToastNoAnimationModule.forRoot(),
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
