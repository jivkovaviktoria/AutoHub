const baseUrl = 'https://localhost:7299';

export const GetAll = async () => {
    const response = await fetch(baseUrl + `/AllCars`);
    return await response.json();
}

export const Add = async (carData) => {
    const response = await fetch(baseUrl + `/Add`, {
        method: 'POST',
        headers: {
            'content-type': 'application/json'
        },
        body: JSON.stringify(carData)
    });

    const result = await response.json();
    console.log(result);
    return result;
}
