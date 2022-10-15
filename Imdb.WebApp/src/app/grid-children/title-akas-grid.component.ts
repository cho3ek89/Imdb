import { Component } from '@angular/core';
import { Observable } from 'rxjs';

import { ColDef, IGetRowsParams } from 'ag-grid-community';
import { ToastrService } from 'ngx-toastr';

import { AgGridOptionsService } from '../services/ag-grid-options.service';
import { DataService } from '../services/data.service';
import { GridBaseComponent } from '../grid-base/grid-base.component';

@Component({
  selector: 'title-akas-grid',
  templateUrl: './../grid-base/grid-base.component.html',
})
export class TitleAkasGridComponent extends GridBaseComponent {
  constructor(dataService: DataService, agGridOptionsService: AgGridOptionsService, toastrService: ToastrService) {
    super(dataService, agGridOptionsService, toastrService);
  }

  protected getData(params: IGetRowsParams): Observable<any> {
    return this.dataService.getTitleAkas(params);
  }

  protected getGridColumnDefs(): ColDef[] {
    return [
      { headerName: 'Title Id', field: 'TitleId', type: 'number' },
      { headerName: 'Index', field: 'Index', type: 'number' },
      { headerName: 'Title', field: 'Title', type: 'string' },
      { headerName: 'Region', field: 'Region', type: 'string' },
      { headerName: 'Language', field: 'Language', type: 'string' },
      { headerName: 'Types', field: 'Types', type: 'string' },
      { headerName: 'Attributes', field: 'Attributes', type: 'string' },
      { headerName: 'Is Original Title', field: 'IsOriginalTitle', type: 'boolean' },
    ];
  }
}
