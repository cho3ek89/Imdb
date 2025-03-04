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
  selector: 'title-basics-grid',
  imports: [
    AgGridModule, 
    CommonModule, 
  ],
  templateUrl: './../grid-base/grid-base.component.html',
})
export class TitleBasicsGridComponent extends GridBaseComponent {
  constructor(dataService: DataService, agGridOptionsService: AgGridOptionsService, snackBar: MatSnackBar) {
    super(dataService, agGridOptionsService, snackBar);
  }

  protected getData(params: IGetRowsParams): Observable<any> {
    return this.dataService.getTitleBasics(params);
  }

  protected getGridColumnDefs(): ColDef[] {
    return [
      { headerName: 'Title Id', field: 'TitleId', type: 'number' },
      { headerName: 'Title Type', field: 'TitleType', type: 'string' },
      { headerName: 'Primary Title', field: 'PrimaryTitle', type: 'string' },
      { headerName: 'Original Title', field: 'OriginalTitle', type: 'string' },
      { headerName: 'Is Adult', field: 'IsAdult', type: 'boolean' },
      { headerName: 'Start Year', field: 'StartYear', type: 'number' },
      { headerName: 'End Year', field: 'EndYear', type: 'number' },
      { headerName: 'Runtime Minutes', field: 'RuntimeMinutes', type: 'number' },
      { headerName: 'Genres', field: 'Genres', type: 'string' },
    ];
  }
}
