import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { NameBasicsGridComponent } from './name-basics-grid/name-basics-grid.component';
import { TitleAkasGridComponent } from './title-akas-grid/title-akas-grid.component';
import { TitleBasicsGridComponent } from './title-basics-grid/title-basics-grid.component';
import { TitleCrewGridComponent } from './title-crew-grid/title-crew-grid.component';
import { TitleEpisodesGridComponent } from './title-episodes-grid/title-episodes-grid.component';
import { TitlePrincipalsGridComponent } from './title-principals-grid/title-principals-grid.component';
import { TitleRatingsGridComponent } from './title-ratings-grid/title-ratings-grid.component';

const routes: Routes = [
  { path: 'name-basics', component: NameBasicsGridComponent },
  { path: 'title-akas', component: TitleAkasGridComponent },
  { path: 'title-basics', component: TitleBasicsGridComponent },
  { path: 'title-crew', component: TitleCrewGridComponent },
  { path: 'title-episodes', component: TitleEpisodesGridComponent },
  { path: 'title-principals', component: TitlePrincipalsGridComponent },
  { path: 'title-ratings', component: TitleRatingsGridComponent },
  { path: '', redirectTo: '/title-basics', pathMatch: 'full' },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
