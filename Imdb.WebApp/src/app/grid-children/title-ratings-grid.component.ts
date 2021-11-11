import { Component } from '@angular/core';
import { Observable } from 'rxjs';

import { ColDef, IGetRowsParams } from 'ag-grid-community';
import { ToastrService } from 'ngx-toastr';

import { AgGridOptionsService } from 'src/services/ag-grid-options.service';
import { DataService } from 'src/services/data.service';
import { GridBaseComponent } from '../grid-base/grid-base.component';

@Component({
  selector: 'title-ratings-grid',
  templateUrl: './../grid-base/grid-base.component.html',
})
export class TitleRatingsGridComponent extends GridBaseComponent {
  constructor(dataService: DataService, agGridOptionsService: AgGridOptionsService, toastrService: ToastrService) {
    super(dataService, agGridOptionsService, toastrService);
  }

  protected getData(params: IGetRowsParams): Observable<any> {
    return this.dataService.getTitleRatings(params);
  }

  protected getGridColumnDefs(): ColDef[] {
    return [
      { headerName: 'Title Id', field: 'TitleId', type: 'number' },
      { headerName: 'Average Rating', field: 'AverageRating', type: 'number' },
      { headerName: 'Number Of Votes', field: 'NumberOfVotes', type: 'number' },
    ];
  }
}
