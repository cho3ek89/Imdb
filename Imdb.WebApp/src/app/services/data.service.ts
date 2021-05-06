import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ConfigService } from '././config.service';
import { IGetRowsParams } from 'ag-grid-community';
import buildQuery from 'odata-query';
import f from 'odata-filter-builder';

@Injectable()
export class DataService {
    private url: string;

    constructor(private http: HttpClient, configService: ConfigService) { 
        let config = configService.getConfiguration();
        this.url = config.apiUrl;
    }

    getNameBasics = (params: IGetRowsParams) => this.getData(params, 'name-basics');

    getTitleAkas = (params: IGetRowsParams) => this.getData(params, 'title-akas');

    getTitleBasics = (params: IGetRowsParams) => this.getData(params, 'title-basics');

    getTitleCrew = (params: IGetRowsParams) => this.getData(params, 'title-crew');

    getTitleEpisodes = (params: IGetRowsParams) => this.getData(params, 'title-episodes');

    getTitlePrincipals = (params: IGetRowsParams) => this.getData(params, 'title-principals');

    getTitleRatings = (params: IGetRowsParams) => this.getData(params, 'title-ratings');

    private getData(params: IGetRowsParams, route: string) {
        let query = this.getQuery(params);
        return this.http.get<any>(this.url + route + query);
    }

    private getQuery(params: IGetRowsParams, count: boolean = true): string {
        let top = params.endRow - params.startRow;
        let skip = params.startRow;
        let orderBy = params.sortModel.map((m: { colId: string; sort: string; }) => `${m.colId} ${m.sort}`);
        let filter = this.getFilter(params.filterModel);

        return buildQuery({ top, skip, orderBy, filter, count });
    }

    private getFilter(filterModel: any): string {
        var filter = f('and');

        Object.keys(filterModel).forEach(property => {
            let filterParam = Reflect.get(filterModel, property);

            console.warn(filterParam);

            let value = filterParam.filterType == 'date'
                ? filterParam.dateFrom
                : filterParam.filter;
            let valueTo = filterParam.filterType == 'date'
                ? filterParam.dateTo
                : filterParam.filterTo;
            let operator = filterParam.type;

            this.addConditionToFilter(filter, property, operator, value, valueTo);
        });

        return filter.toString();
    }

    private addConditionToFilter(filter: f, property: string, operator: string, value: any, valueTo?: any) {
        switch (operator) {
            case 'equals':
                filter.eq(property, value);
                break;
            case 'notEqual':
                filter.ne(property, value);
                break;
            case 'contains':
                filter.contains(property, value);
                break;
            case 'notContains':
                filter.not(f => filter.contains(property, value));
                break;
            case 'startsWith':
                filter.startsWith(property, value);
                break;
            case 'endsWith':
                filter.endsWith(property, value);
                break;
            case 'lessThan':
                filter.lt(property, value);
                break;
            case 'lessThanOrEqual':
                filter.le(property, value);
                break;
            case 'greaterThan':
                filter.gt(property, value);
                break;
            case 'greaterThanOrEqual':
                filter.ge(property, value);
                break;
            case 'inRange':
                let rangeFilter = f('and')
                    .ge(property, value)
                    .le(property, valueTo);
                filter.and(rangeFilter);
                break;
            case 'empty':
                throw new Error(`Unsupported operator (${operator})!`);
            case 'isTrue':
                filter.eq(property, true);
                break;
            case 'isFalse':
                filter.eq(property, false);
                break;
            default:
                throw new Error(`Unknown operator (${operator})!`);
        }
    }
}