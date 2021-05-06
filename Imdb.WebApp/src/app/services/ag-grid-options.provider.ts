import { Injectable } from '@angular/core';
import { GridOptions } from 'ag-grid-community';

@Injectable()
export class AgGridOptionsProvider {
    getGridOptions(): GridOptions {
        return {
            cacheBlockSize: 90,
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
                    alwaysShowBothConditions: false, //important
                    buttons: ['clear', 'apply'],
                    closeOnApply: true,
                    suppressAndOrCondition: true, //important
                },
                resizable: true,
                sortable: true,
            },
            infiniteInitialRowCount: 0,
            maxConcurrentDatasourceRequests: 2,
            pagination: true,
            paginationPageSize: 90,
            rowModelType: 'infinite',
        };
    }
}
