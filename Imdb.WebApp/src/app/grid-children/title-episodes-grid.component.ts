import { Component } from '@angular/core';
import { Observable } from 'rxjs';

import { ColDef, IGetRowsParams } from 'ag-grid-community';
import { ToastrService } from 'ngx-toastr';

import { AgGridOptionsService } from '../services/ag-grid-options.service';
import { DataService } from '../services/data.service';
import { GridBaseComponent } from '../grid-base/grid-base.component';

@Component({
  selector: 'title-episodes-grid',
  templateUrl: './../grid-base/grid-base.component.html',
})
export class TitleEpisodesGridComponent extends GridBaseComponent {
  constructor(dataService: DataService, agGridOptionsService: AgGridOptionsService, toastrService: ToastrService) {
    super(dataService, agGridOptionsService, toastrService);
  }

  protected getData(params: IGetRowsParams): Observable<any> {
    return this.dataService.getTitleEpisodes(params);
  }

  protected getGridColumnDefs(): ColDef[] {
    return [
      { headerName: 'Title Id', field: 'TitleId', type: 'number' },
      { headerName: 'Parent Title Id', field: 'ParentTitleId', type: 'number' },
      { headerName: 'Season Number', field: 'SeasonNumber', type: 'number' },
      { headerName: 'Episode Number', field: 'EpisodeNumber', type: 'number' },
    ];
  }
}
