import { Component } from '@angular/core';
import { Observable } from 'rxjs';

import { IGetRowsParams, ColDef } from 'ag-grid-community';
import { ToastrService } from 'ngx-toastr';

import { GridBaseComponent } from '../grid-base/grid-base.component';
import { AgGridOptionsProvider } from '../services/ag-grid-options.provider';
import { DataService } from '../services/data.service';

@Component({
  selector: 'app-name-basics-grid',
  templateUrl: './../grid-base/grid-base.component.html',
  styleUrls: ['./../grid-base/grid-base.component.scss']
})
export class NameBasicsGridComponent extends GridBaseComponent {
  constructor(dataService: DataService, agGridOptionsProvider: AgGridOptionsProvider, toastrService: ToastrService) {
    super(dataService, agGridOptionsProvider, toastrService);
  }

  protected getData(params: IGetRowsParams): Observable<any> {
    return this.dataService.getNameBasics(params);
  }

  protected getGridColumnDefs(): ColDef[] {
    return [
      { headerName: 'Name Id', field: 'nameId', type: 'number' },
      { headerName: 'Name', field: 'name', type: 'string' },
      { headerName: 'Birth Year', field: 'birthYear', type: 'number' },
      { headerName: 'Death Year', field: 'deathYear', type: 'number' },
      { headerName: 'Professions', field: 'professions', type: 'string' },
      { headerName: 'Known For Title Ids', field: 'knownForTitleIds', type: 'string' },
    ];
  }
}
