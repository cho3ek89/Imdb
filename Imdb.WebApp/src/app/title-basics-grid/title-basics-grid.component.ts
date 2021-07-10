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
