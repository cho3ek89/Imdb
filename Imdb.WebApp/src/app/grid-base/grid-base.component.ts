import { CommonModule } from '@angular/common';
import { HttpErrorResponse } from '@angular/common/http';
import { Component, ViewChild } from '@angular/core';
import { Observable } from 'rxjs';

import { AgGridAngular } from 'ag-grid-angular';
import { ColDef, GridOptions, GridReadyEvent, IGetRowsParams } from 'ag-grid-community';
import { MatSnackBar } from '@angular/material/snack-bar';

import { AgGridOptionsService } from '../services/ag-grid-options.service';
import { DataService } from '../services/data.service';

@Component({
  selector: 'grid-base',
  imports: [
    CommonModule, 
  ],
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

  onGridReady(onGridReady: GridReadyEvent) {
    var datasource = {
      getRows: (params: IGetRowsParams) => {
        onGridReady.api.setGridOption('loading', true);

        this.getData(params).subscribe({
          next: result => {
            params.successCallback(result['value'], result['@odata.count']);

            if (result['@odata.count'] == 0)
              onGridReady.api.showNoRowsOverlay();
            else
            onGridReady.api.setGridOption('loading', false);
          },
          error: (error: HttpErrorResponse) => {
            onGridReady.api.setGridOption('loading', false);
            params.failCallback();

            console.error(error.error);
            this.snackBar.open(
              `${error.error.title}: ${error.error.detail}`,
              'CLOSE',
              { duration: 4000, horizontalPosition: 'end' });
          }
        });
      }
    }

    onGridReady.api.setGridOption('datasource', datasource);
  }
}
