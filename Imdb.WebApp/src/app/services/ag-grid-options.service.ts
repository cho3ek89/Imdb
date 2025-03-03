import { Injectable } from '@angular/core';

import { GridOptions, themeAlpine } from 'ag-grid-community';

@Injectable({
  providedIn: 'root'
})
export class AgGridOptionsService {
  getGridOptions(): GridOptions {
    return {
      cacheBlockSize: 100,
      cacheOverflowSize: 2,
      columnTypes: {
        boolean: {
          filter: 'agTextColumnFilter',
          filterParams: {
            defaultOption: 'empty',
            filterOptions: [
              'empty',
              {
                displayKey: 'isTrue',
                displayName: 'Is True',
                hideFilterInput: true,
                test: (_filterValue: any, cellValue: any) => cellValue == true,
              },
              {
                displayKey: 'isFalse',
                displayName: 'is False',
                hideFilterInput: true,
                test: (_filterValue: any, cellValue: any) => cellValue == false,
              }
            ]
          }
        },
        date: {
          filter: 'agDateColumnFilter',
        },
        number: {
          filter: 'agNumberColumnFilter',
        },
        string: {
          filter: 'agTextColumnFilter',
        }
      },
      defaultColDef: {
        filter: true,
        filterParams: {
          buttons: ['clear', 'apply' ],
          closeOnApply: true,
          maxNumConditions: 1, //important
          numAlwaysVisibleConditions: 1, //important
        },
        resizable: true,
        sortable: true,
      },
      infiniteInitialRowCount: 1,
      maxConcurrentDatasourceRequests: 2,
      pagination: true,
      paginationPageSize: 100,
      paginationPageSizeSelector: [100, 200, 300],
      rowModelType: 'infinite',
      theme: themeAlpine,
    };
  }
}
