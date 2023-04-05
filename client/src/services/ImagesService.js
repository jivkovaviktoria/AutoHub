import axios from "axios";

const baseUrl = 'https://localhost:7299';

export const UploadImage = async (file) => {
    const data = new FormData();
    data.append('file', file);

    const response = await axios.post(`${baseUrl}/Upload`, data);
    return response;
}