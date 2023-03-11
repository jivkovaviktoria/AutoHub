const baseUrl = 'https://localhost:7299/AllCars';

export const GetAll = async () => {
    const response = await fetch(baseUrl);
    return await response.json();
}