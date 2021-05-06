export class Config {
    private _apiUrl: string;

    public get apiUrl() {
        return this._apiUrl;
    }

    constructor(apiUrl: string) {
        this._apiUrl = apiUrl;
    }
}