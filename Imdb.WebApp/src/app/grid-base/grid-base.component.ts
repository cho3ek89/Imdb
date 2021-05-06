import { Component, OnInit, ViewChild } from '@angular/core';
import { Observable } from 'rxjs';

import { AgGridAngular } from 'ag-grid-angular';
import { ColDef, GridOptions, IGetRowsParams } from 'ag-grid-community';
import { ToastrService } from 'ngx-toastr';

import { AgGridOptionsProvider } from '../services/ag-grid-options.provider';
import { DataService } from '../services/data.service'

@Component({
  selector: 'app-grid-base',
  template: ''
})
export abstract class GridBaseComponent implements OnInit {
  @ViewChild('grid') grid: AgGridAngular | undefined;
  gridOptions: GridOptions;
  rowData: any;

  constructor(protected dataService: DataService, agGridOptionsProvider: AgGridOptionsProvider, private toastrService: ToastrService) {
    this.gridOptions = agGridOptionsProvider.getGridOptions();
    this.gridOptions.columnDefs = this.getGridColumnDefs();
  }

  ngOnInit(): void { }

  protected abstract getData(params: IGetRowsParams): Observable<any>;

  protected abstract getGridColumnDefs(): ColDef[];

  onGridReady(params: any) {
    var datasource = {
      getRows: (params: IGetRowsParams) => {
        this.gridOptions.api?.showLoadingOverlay();

        this.getData(params)
          .subscribe(result => {
            params.successCallback(result.result, result.count);

            if (result.count == 0)
              this.gridOptions.api?.showNoRowsOverlay();
            else
              this.gridOptions.api?.hideOverlay();
          }, error => {
            console.error(error);
            this.toastrService.error('Data loading failed!');
          });
      }
    }

    params.api.setDatasource(datasource);
  }
}
