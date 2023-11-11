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
  selector: 'title-principals-grid',
  standalone: true,
  imports: [
    AgGridModule, 
    CommonModule, 
  ],
  templateUrl: './../grid-base/grid-base.component.html',
})
export class TitlePrincipalsGridComponent extends GridBaseComponent {
  constructor(dataService: DataService, agGridOptionsService: AgGridOptionsService, snackBar: MatSnackBar) {
    super(dataService, agGridOptionsService, snackBar);
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
