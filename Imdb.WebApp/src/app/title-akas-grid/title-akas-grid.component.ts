import { Component } from '@angular/core';
import { Observable } from 'rxjs';

import { IGetRowsParams, ColDef } from 'ag-grid-community';
import { ToastrService } from 'ngx-toastr';

import { GridBaseComponent } from '../grid-base/grid-base.component';
import { AgGridOptionsProvider } from '../services/ag-grid-options.provider';
import { DataService } from '../services/data.service';

@Component({
  selector: 'app-title-akas-grid',
  templateUrl: './../grid-base/grid-base.component.html',
  styleUrls: ['./../grid-base/grid-base.component.scss']
})
export class TitleAkasGridComponent extends GridBaseComponent {
  constructor(dataService: DataService, agGridOptionsProvider: AgGridOptionsProvider, toastrService: ToastrService) {
    super(dataService, agGridOptionsProvider, toastrService);
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
