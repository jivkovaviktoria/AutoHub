import axios from "axios";

const config = {headers: { Authorization: `Bearer ${sessionStorage.getItem('token')}` }};

const baseUrl = 'https://localhost:7299';

export const GetAll = async () => {
    const response = await axios.get(`${baseUrl}/AllCars`, config);
    return response.data;
}

export const GetSingle = async (carId) => {
    const response = await axios.get(`${baseUrl}/Car?id=${carId}`, config);
    return response.data;
}

export const Add = async (carData) => {
    const response = await axios.post(`${baseUrl}/Add`, carData, config);
    return response.data;
}

export const OrderBy = async (property, direction) => {
    const response = await axios.get(`${baseUrl}/GetCarsOrdered?property=${property}&direction=${direction}`, config);
    return response.data;
}

export const GetCarsByUser = async () => {
    const response = await axios.get(`${baseUrl}/GetCarsByUser`, config);
    return response.data;
}

export const SaveCar = async (carId) => {
    const response = await axios.post(`${baseUrl}/AddToFavourite?carId=${carId}`, carId, config);
    return response.data;
}

export const GetSavedCars = async () => {
    const response = await axios.get(`${baseUrl}/GetFavourite`, config);
    return response.data;
}

export const OrderCars = async (order) => {
    const response = await axios.get(`${baseUrl}/OrderCars`, {params: order, headers: {"Content-Type": "application/json"}});
    return response.data;
}

export const FilterByPrice = async (filter) => {
    const response = await axios.get(`${baseUrl}/FilterByPrice?Min=${filter.Min}&Max=${filter.Max}`, config);
    return response.data;
}