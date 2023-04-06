import axios from "axios";

const baseUrl = 'https://localhost:7299';

export const UploadManyImages = async (files) => {
    const data = new FormData();
    for (let i = 0; i < files.length; i++) {
        data.append('files', files[i]);
    }
    const response = await axios.post(`${baseUrl}/UploadMany`, data);
    return response;
}