import { Component } from '@angular/core';
import { Observable } from 'rxjs';

import { IGetRowsParams, ColDef } from 'ag-grid-community';
import { ToastrService } from 'ngx-toastr';

import { GridBaseComponent } from '../grid-base/grid-base.component';
import { AgGridOptionsProvider } from '../services/ag-grid-options.provider';
import { DataService } from '../services/data.service';

@Component({
  selector: 'app-title-ratings-grid',
  templateUrl: './../grid-base/grid-base.component.html',
  styleUrls: ['./../grid-base/grid-base.component.scss']
})
export class TitleRatingsGridComponent extends GridBaseComponent {
  constructor(dataService: DataService, agGridOptionsProvider: AgGridOptionsProvider, toastrService: ToastrService) {
    super(dataService, agGridOptionsProvider, toastrService);
  }

  protected getData(params: IGetRowsParams): Observable<any> {
    return this.dataService.getTitleRatings(params);
  }

  protected getGridColumnDefs(): ColDef[] {
    return [
      { headerName: 'Title Id', field: 'titleId', type: 'number' },
      { headerName: 'Average Rating', field: 'averageRating', type: 'number' },
      { headerName: 'Number Of Votes', field: 'numberOfVotes', type: 'number' },
    ];
  }
}