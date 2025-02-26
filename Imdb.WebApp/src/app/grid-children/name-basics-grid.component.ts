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
  selector: 'name-basics-grid',
  imports: [
    AgGridModule, 
    CommonModule, 
  ],
  templateUrl: './../grid-base/grid-base.component.html',
})
export class NameBasicsGridComponent extends GridBaseComponent {
  constructor(dataService: DataService, agGridOptionsService: AgGridOptionsService, snackBar: MatSnackBar) {
    super(dataService, agGridOptionsService, snackBar);
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
