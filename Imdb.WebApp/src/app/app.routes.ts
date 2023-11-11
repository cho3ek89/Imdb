import { Routes } from '@angular/router';

import { NameBasicsGridComponent } from './grid-children/name-basics-grid.component';
import { TitleAkasGridComponent } from './grid-children/title-akas-grid.component';
import { TitleBasicsGridComponent } from './grid-children/title-basics-grid.component';
import { TitleCrewGridComponent } from './grid-children/title-crew-grid.component';
import { TitleEpisodesGridComponent } from './grid-children/title-episodes-grid.component';
import { TitlePrincipalsGridComponent } from './grid-children/title-principals-grid.component';
import { TitleRatingsGridComponent } from './grid-children/title-ratings-grid.component';

export const routes: Routes = [
  { path: 'name-basics', component: NameBasicsGridComponent },
  { path: 'title-akas', component: TitleAkasGridComponent },
  { path: 'title-basics', component: TitleBasicsGridComponent },
  { path: 'title-crew', component: TitleCrewGridComponent },
  { path: 'title-episodes', component: TitleEpisodesGridComponent },
  { path: 'title-principals', component: TitlePrincipalsGridComponent },
  { path: 'title-ratings', component: TitleRatingsGridComponent },
  { path: '', redirectTo: '/title-basics', pathMatch: 'full' },
];
