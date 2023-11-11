import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { Observable } from 'rxjs';

import { AgGridModule } from 'ag-grid-angular';
import { ColDef, IGetRowsParams } from 'ag-grid-community';
import { MatSnackBar } from '@angular/material/snack-bar';

import { AgGridOptionsService } from '../services/ag-grid-options.service';
import { DataService } from '../services/data.service';
import { GridBaseComponent } from '../grid-base/grid-base.component';

@Component({
  selector: 'title-episodes-grid',
  standalone: true,
  imports: [
     AgGridModule, 
    CommonModule, 
  ],
  templateUrl: './../grid-base/grid-base.component.html',
})
export class TitleEpisodesGridComponent extends GridBaseComponent {
  constructor(dataService: DataService, agGridOptionsService: AgGridOptionsService, snackBar: MatSnackBar) {
    super(dataService, agGridOptionsService, snackBar);
  }

  protected getData(params: IGetRowsParams): Observable<any> {
    return this.dataService.getTitleEpisodes(params);
  }

  protected getGridColumnDefs(): ColDef[] {
    return [
      { headerName: 'Title Id', field: 'TitleId', type: 'number' },
      { headerName: 'Parent Title Id', field: 'ParentTitleId', type: 'number' },
      { headerName: 'Season Number', field: 'SeasonNumber', type: 'number' },
      { headerName: 'Episode Number', field: 'EpisodeNumber', type: 'number' },
    ];
  }
}
