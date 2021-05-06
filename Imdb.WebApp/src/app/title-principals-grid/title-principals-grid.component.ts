import { Component } from '@angular/core';
import { Observable } from 'rxjs';

import { IGetRowsParams, ColDef } from 'ag-grid-community';
import { ToastrService } from 'ngx-toastr';

import { GridBaseComponent } from '../grid-base/grid-base.component';
import { AgGridOptionsProvider } from '../services/ag-grid-options.provider';
import { DataService } from '../services/data.service';

@Component({
  selector: 'app-title-principals-grid',
  templateUrl: './../grid-base/grid-base.component.html',
  styleUrls: ['./../grid-base/grid-base.component.scss']
})
export class TitlePrincipalsGridComponent extends GridBaseComponent {
  constructor(dataService: DataService, agGridOptionsProvider: AgGridOptionsProvider, toastrService: ToastrService) {
    super(dataService, agGridOptionsProvider, toastrService);
  }

  protected getData(params: IGetRowsParams): Observable<any> {
    return this.dataService.getTitlePrincipals(params);
  }

  protected getGridColumnDefs(): ColDef[] {
    return [
      { headerName: 'Title Id', field: 'titleId', type: 'number' },
      { headerName: 'Index', field: 'index', type: 'number' },
      { headerName: 'Name Id', field: 'nameId', type: 'number' },
      { headerName: 'Category', field: 'category', type: 'string' },
      { headerName: 'Job', field: 'job', type: 'string' },
      { headerName: 'Characters', field: 'characters', type: 'string' },
    ];
  }
}
