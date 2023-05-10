import axios from "axios";

const config = {headers: { Authorization: `Bearer ${sessionStorage.getItem('token')}` }};

const baseUrl = 'https://localhost:7299';

export const GetUser = async () => {
    const response = await axios.get(`${baseUrl}/User`, config);
    return response.data;
}

export const SignIn = async (loginRequest) => {
    const response = await axios.post(`${baseUrl}/Auth/login`, loginRequest);
    return response.data;
}

export const SignUp = async (registerRequest) => {
    const response = await axios.post(`${baseUrl}/Auth/register`, registerRequest);
    return response.data;
}

export const GetUserInfo = async (userId) => {
    const response = await axios.get(`${baseUrl}/UserById?id=${userId}`, config);
    console.log(response.data);
    return response.data;
}