import { Component, ViewChild } from '@angular/core';
import { Observable } from 'rxjs';

import { AgGridAngular } from 'ag-grid-angular';
import { ColDef, GridOptions, IGetRowsParams } from 'ag-grid-community';
import { ToastrService } from 'ngx-toastr';

import { AgGridOptionsService } from 'src/services/ag-grid-options.service';
import { DataService } from 'src/services/data.service';

@Component({
  selector: 'grid-base',
  template: ``,
})
export abstract class GridBaseComponent {
  @ViewChild('grid') grid: AgGridAngular | undefined;
  gridOptions: GridOptions;
  rowData: any;

  constructor(protected dataService: DataService, agGridOptionsService: AgGridOptionsService, private toastrService: ToastrService) {
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
            params.successCallback(result.Result, result.Count);

            if (result.count == 0)
              this.gridOptions.api?.showNoRowsOverlay();
            else
              this.gridOptions.api?.hideOverlay();
          },
          error: error => {
            this.gridOptions.api?.showNoRowsOverlay();

            console.error(error.error);
            this.toastrService.error(error.error.detail, error.error.title);
          }
        });
      }
    }

    params.api.setDatasource(datasource);
  }
}
