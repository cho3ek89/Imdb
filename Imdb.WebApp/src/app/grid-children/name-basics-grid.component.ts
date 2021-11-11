import { Component } from '@angular/core';
import { Observable } from 'rxjs';

import { ColDef, IGetRowsParams } from 'ag-grid-community';
import { ToastrService } from 'ngx-toastr';

import { AgGridOptionsService } from 'src/services/ag-grid-options.service';
import { DataService } from 'src/services/data.service';
import { GridBaseComponent } from '../grid-base/grid-base.component';

@Component({
  selector: 'name-basics-grid',
  templateUrl: './../grid-base/grid-base.component.html',
})
export class NameBasicsGridComponent extends GridBaseComponent {
  constructor(dataService: DataService, agGridOptionsService: AgGridOptionsService, toastrService: ToastrService) {
    super(dataService, agGridOptionsService, toastrService);
  }

  protected getData(params: IGetRowsParams): Observable<any> {
    return this.dataService.getNameBasics(params);
  }

  protected getGridColumnDefs(): ColDef[] {
    return [
      { headerName: 'Name Id', field: 'NameId', type: 'number' },
      { headerName: 'Name', field: 'Name', type: 'string' },
      { headerName: 'Birth Year', field: 'BirthYear', type: 'number' },
      { headerName: 'Death Year', field: 'DeathYear', type: 'number' },
      { headerName: 'Professions', field: 'Professions', type: 'string' },
      { headerName: 'Known For Title Ids', field: 'KnownForTitleIds', type: 'string' },
    ];
  }
}
