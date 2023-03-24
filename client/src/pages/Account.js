import {useEffect, useState} from "react";
import * as CarsService from '../services/CarsService';
import {Card} from "../components/card/Card";
import styles from './Account.module.css';

export const Account = () => {
    const [cars, setCars] = useState([]);

    useEffect(() => {
        const token = sessionStorage.getItem('token');
        async function fetchData() {
            const result = await CarsService.GetCarsByUser();
            setCars(result.$values[0].cars.$values);
        }

        fetchData();
    }, []);

    return (
        <div className='container'>
            <h1>My Cars</h1>
            <div className={styles['cars-wrapper']}>
            {cars.map(car => <Card key={car.id} car={car}/>)}
            </div>
        </div>
    );
}