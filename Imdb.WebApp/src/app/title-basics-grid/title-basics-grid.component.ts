import { Component } from '@angular/core';
import { Observable } from 'rxjs';

import { IGetRowsParams, ColDef } from 'ag-grid-community';
import { ToastrService } from 'ngx-toastr';

import { GridBaseComponent } from '../grid-base/grid-base.component';
import { AgGridOptionsProvider } from '../services/ag-grid-options.provider';
import { DataService } from '../services/data.service';

@Component({
  selector: 'app-title-basics-grid',
  templateUrl: './../grid-base/grid-base.component.html',
  styleUrls: ['./../grid-base/grid-base.component.scss']
})
export class TitleBasicsGridComponent extends GridBaseComponent {
  constructor(dataService: DataService, agGridOptionsProvider: AgGridOptionsProvider, toastrService: ToastrService) {
    super(dataService, agGridOptionsProvider, toastrService);
  }

  protected getData(params: IGetRowsParams): Observable<any> {
    return this.dataService.getTitleBasics(params);
  }

  protected getGridColumnDefs(): ColDef[] {
    return [
      { headerName: 'Title Id', field: 'titleId', type: 'number' },
      { headerName: 'Title Type', field: 'titleType', type: 'string' },
      { headerName: 'Primary Title', field: 'primaryTitle', type: 'string' },
      { headerName: 'Original Title', field: 'originalTitle', type: 'string' },
      { headerName: 'Is Adult', field: 'isAdult', type: 'boolean' },
      { headerName: 'Start Year', field: 'startYear', type: 'number' },
      { headerName: 'End Year', field: 'endYear', type: 'number' },
      { headerName: 'Runtime Minutes', field: 'runtimeMinutes', type: 'number' },
      { headerName: 'Genres', field: 'genres', type: 'string' },
    ];
  }
}
