import { Component } from '@angular/core';
import { Observable } from 'rxjs';

import { ColDef, IGetRowsParams } from 'ag-grid-community';
import { ToastrService } from 'ngx-toastr';

import { AgGridOptionsService } from '../services/ag-grid-options.service';
import { DataService } from '../services/data.service';
import { GridBaseComponent } from '../grid-base/grid-base.component';

@Component({
  selector: 'title-basics-grid',
  templateUrl: './../grid-base/grid-base.component.html',
})
export class TitleBasicsGridComponent extends GridBaseComponent {
  constructor(dataService: DataService, agGridOptionsService: AgGridOptionsService, toastrService: ToastrService) {
    super(dataService, agGridOptionsService, toastrService);
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
