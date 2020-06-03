import authService, { AuthorizeService, SignInState } from '@/components/api-authorization/AuthorizeService';
import { Profile } from 'oidc-client';
import config from '@/utils/appSettings';
import { api } from '@/api';
import axios from '@/plugins/axios';

class Auth {
    user: Profile | null = null;
    accessToken: string | null = null;

    get isAuthenticated() {
        return !!this.user;
    }

    constructor(private _authService: AuthorizeService){
        _authService.subscribe(async () => {
            await this.getUser();
        });
    }

    async getUser() {
        this.user = await this._authService.getUser();
        this.accessToken = await this._authService.getAccessToken();
    }

    async signIn(signInState: SignInState) {
        return await this._authService.signIn(signInState);
    }

    async completeSignIn(url: string) {
        await this._authService.completeSignIn(url);
        this.user = await this._authService.getUser();
        this.accessToken = await this._authService.getAccessToken();
    }
}

class AppStore {
    private _auth: Auth;
    get auth() {
        return this._auth;
    }

    forecasts: api.WeatherForecast[] = [];

    constructor(authService: AuthorizeService, private _forecastClient: api.WeatherForecastClient) {
        this._auth = new Auth(authService);
    }

    async loadWeatherForecasts() {
        this.forecasts = await this._forecastClient.get();
    }
}

const _default = new AppStore(
    authService, 
    new api.WeatherForecastClient(config.apiRoot, axios));
export default _default;
