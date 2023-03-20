const baseUrl = 'https://localhost:7299';

export const GetAll = async (token) => {
    const response = await fetch(baseUrl + '/AllCars', {
        headers: {
            'Authorization': `Bearer ${token}`
        },
    });

    if(!response.ok){
        throw new Error("error");
    }

    const data = await response.json();
    return data;
}

export const GetSingle = async (carId, token) => {
    const response = await fetch(baseUrl + `/Car?id=${carId}`, {
        headers:{
            'Authorization': `Bearer ${token}`
        },
    });
    return await response.json();
}

export const Add = async (carData, token) => {
    const response = await fetch(baseUrl + `/Add`, {
        method: 'POST',
        headers: {
            'content-type': 'application/json',
            'Authorization': `Bearer ${token}`
        },
        body: JSON.stringify(carData)
    });

    const result = await response.json();
    console.log(result);
    return result;
}

export const OrderBy = async (property, direction, token) => {
    const response = await fetch(`${baseUrl}/GetCarsOrdered?property=${property}&direction=${direction}`, {
        headers: {
            'Authorization': `Bearer ${token}`
        }
    });
    const result = response.json();
    return result;
}
