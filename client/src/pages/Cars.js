import {useEffect, useState} from 'react';
import * as carsService from "../services/CarsService";
import styles from './Cars.module.css';
import {Card} from "../components/card/Card";

export const Cars = () => {
    const [cars, setCars] = useState([]);

    useEffect(() => {
        carsService.GetAll()
            .then(cars => setCars(cars));
    }, []);
    console.log(cars);

    return (
        <div className={styles.wrapper}>
            {cars.map(car => <Card car={car}/>)}
        </div>
    );
}