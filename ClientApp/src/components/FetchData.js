import React, { Component } from 'react';
import 'bootstrap/dist/css/bootstrap.min.css';
import 'bootstrap-icons/font/bootstrap-icons.css';


export class FetchData extends Component {
  static displayName = FetchData.name;

  constructor(props) {
    super(props);
    this.state = { forecasts: [], loading: true };
  }

  componentDidMount() {
    this.populateWeatherData();
  }

  static renderForecastsTable(forecasts) {
    return (
      <table className='table table-striped' aria-labelledby="tabelLabel">
        <thead>
          <tr>
            <th>Miasto</th>
            <th>Kraj</th>
            <th>Pogoda</th>
          </tr>
        </thead>
        <tbody>
              {forecasts.map(forecast => 
                    <tr key={"" + forecast.city + forecast.country}>
                        <td>{forecast.city}</td>
              <td>{forecast.country}</td>
                      {/*<td>{forecast.weather}</td>*/}
                      <td>
                          {
                              forecast.weather === "Clear" ?
                                  <i class="bi bi-brightness-high-fill text-warning" style={{ fontSize: 30 }}></i>
                                  : forecast.weather === "Clouds" ?
                                      <i class="bi bi-cloud-fill text-primary" style={{ fontSize: 30 }}></i>
                                      : forecast.weather === "Rain" ?
                                          <i class="bi bi-cloud-rain-heavy-fill text-primary" style={{ fontSize: 30 }}></i>
                                          :
                                          <i class="bi bi-x text-muted" style={{ fontSize: 30 }}></i>
                          }
                        
                      </td>         
            </tr>
          )}
        </tbody>
      </table>
    );
  }

  render() {
    let contents = this.state.loading
      ? <p><em>Loading...</em></p>
      : FetchData.renderForecastsTable(this.state.forecasts);



    return (
      <div>
        <h1 id="tabelLabel" >Pogoda dla miast</h1>
        {contents}
      </div>
    );
  }

  async populateWeatherData() {
    const response = await fetch('weatherforecast');
      const data = await response.json();
      console.log(data);
    this.setState({ forecasts: data, loading: false });
  }
}
