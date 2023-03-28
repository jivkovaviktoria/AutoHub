import axios from "axios";

const config = {headers: { Authorization: `Bearer ${sessionStorage.getItem('token')}` }};

const baseUrl = 'https://localhost:7299';

export const GetUser = async () => {
    const response = await axios.get(`${baseUrl}/User`, config);
    return response.data;
}