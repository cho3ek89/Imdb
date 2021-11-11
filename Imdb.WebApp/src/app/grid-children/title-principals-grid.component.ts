import { Component } from '@angular/core';
import { Observable } from 'rxjs';

import { ColDef, IGetRowsParams } from 'ag-grid-community';
import { ToastrService } from 'ngx-toastr';

import { AgGridOptionsService } from 'src/services/ag-grid-options.service';
import { DataService } from 'src/services/data.service';
import { GridBaseComponent } from '../grid-base/grid-base.component';

@Component({
  selector: 'title-principals-grid',
  templateUrl: './../grid-base/grid-base.component.html',
})
export class TitlePrincipalsGridComponent extends GridBaseComponent {
  constructor(dataService: DataService, agGridOptionsService: AgGridOptionsService, toastrService: ToastrService) {
    super(dataService, agGridOptionsService, toastrService);
  }

  protected getData(params: IGetRowsParams): Observable<any> {
    return this.dataService.getTitlePrincipals(params);
  }

  protected getGridColumnDefs(): ColDef[] {
    return [
      { headerName: 'Title Id', field: 'TitleId', type: 'number' },
      { headerName: 'Index', field: 'Index', type: 'number' },
      { headerName: 'Name Id', field: 'NameId', type: 'number' },
      { headerName: 'Category', field: 'Category', type: 'string' },
      { headerName: 'Job', field: 'Job', type: 'string' },
      { headerName: 'Characters', field: 'Characters', type: 'string' },
    ];
  }
}
