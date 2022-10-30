import { HttpErrorResponse } from '@angular/common/http';
import { Component, ViewChild } from '@angular/core';
import { Observable } from 'rxjs';

import { AgGridAngular } from 'ag-grid-angular';
import { ColDef, GridOptions, IGetRowsParams } from 'ag-grid-community';
import { MatSnackBar } from '@angular/material/snack-bar';

import { AgGridOptionsService } from '../services/ag-grid-options.service';
import { DataService } from '../services/data.service';

@Component({
  selector: 'grid-base',
  template: ``,
})
export abstract class GridBaseComponent {
  @ViewChild('grid') grid: AgGridAngular | undefined;
  gridOptions: GridOptions;
  rowData: any;

  constructor(protected dataService: DataService, agGridOptionsService: AgGridOptionsService, private snackBar: MatSnackBar) {
    this.gridOptions = agGridOptionsService.getGridOptions();
    this.gridOptions.columnDefs = this.getGridColumnDefs();
  }

  protected abstract getData(params: IGetRowsParams): Observable<any>;

  protected abstract getGridColumnDefs(): ColDef[];

  onGridReady(params: any) {
    var datasource = {
      getRows: (params: IGetRowsParams) => {
        this.gridOptions.api?.showLoadingOverlay();

        this.getData(params).subscribe({
          next: result => {
            params.successCallback(result['value'], result['@odata.count']);

            if (result.count == 0)
              this.gridOptions.api?.showNoRowsOverlay();
            else
              this.gridOptions.api?.hideOverlay();
          },
          error: (error: HttpErrorResponse) => {
            this.gridOptions.api?.showNoRowsOverlay();

            console.error(error.error);
            this.snackBar.open(
              `${error.error.title}: ${error.error.detail}`, 
              'CLOSE', 
              { duration: 4000, horizontalPosition: 'end' });
          }
        });
      }
    }

    params.api.setDatasource(datasource);
  }
}
