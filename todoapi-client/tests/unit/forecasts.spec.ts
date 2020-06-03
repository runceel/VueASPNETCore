import { shallowMount } from '@vue/test-utils';
import { api } from '@/api';
import store from '@/store';
import Forecasts from '@/views/Forecasts.vue';

jest.mock('@/store');

const mockStore = store as jest.Mocked<typeof store>;

describe('Forecasts.vue', () => {
    test('データが空のときはテーブルを描画しない', () => {
        mockStore.forecasts = [];
        const forecasts = shallowMount(Forecasts);

        expect(mockStore.loadWeatherForecasts).toHaveBeenCalled();
        expect(forecasts.text()).toMatch('データがありません');
        expect(forecasts.find('table').exists()).toBe(false);
    });

    test('データがあるときはテーブルが描画される', async () => {
        mockStore.loadWeatherForecasts.mockImplementation(() => {
            mockStore.forecasts = [ 
                new api.WeatherForecast({ date: new Date(2020, 5, 1, 0, 0, 0), temperatureC: 0, temperatureF: 10, summary: 's1' }),
                new api.WeatherForecast({ date: new Date(2020, 5, 2, 0, 0, 0), temperatureC: 1, temperatureF: 20, summary: 's2' }),
                new api.WeatherForecast({ date: new Date(2020, 5, 3, 0, 0, 0), temperatureC: 2, temperatureF: 30, summary: 's3' }),
            ];
            return Promise.resolve();
        });

        const forecasts = shallowMount(Forecasts);
        const tableRows = forecasts.findAll('table > tbody > tr');

        expect(mockStore.loadWeatherForecasts).toHaveBeenCalled();
        expect(tableRows.length).toBe(3);
        expect(tableRows.at(0).text()).toBe('Mon Jun 01 2020 00:00:00 GMT+0900 (GMT+09:00) 0 s1');
        expect(tableRows.at(1).text()).toBe('Tue Jun 02 2020 00:00:00 GMT+0900 (GMT+09:00) 1 s2');
        expect(tableRows.at(2).text()).toBe('Wed Jun 03 2020 00:00:00 GMT+0900 (GMT+09:00) 2 s3');
    });
});
