import { useEffect, useState } from "react";
import * as carsService from "../services/CarsService";
import styles from "./Cars.module.css";
import { Card } from "../components/card/Card";

export const Cars = () => {
    const [cars, setCars] = useState([]);
    const [property, setProperty] = useState(null);
    const [direction, setDirection] = useState(null);

    useEffect(() => {
        carsService.GetAll().then((cars) => setCars(cars));
    }, []);

    const propertyHandler = (e) => {
        setProperty(e.target.value);
    };

    const directionHandler = (e) => {
        setDirection(e.target.value);
    };

    const orderHandler = () => {
        carsService
            .OrderBy(property, direction)
            .then((orderedCars) => setCars(orderedCars));
    };

    return (
        <div className={styles.wrapper}>
            <div className={styles['order-form']}>
                <label>
                    OrderBy
                    <select value={property} onChange={propertyHandler}>
                        <option value="">Select a property</option>
                        <option value="brand">Make</option>
                        <option value="model">Model</option>
                        <option value="year">Year</option>
                        <option value="price">Price</option>
                    </select>
                </label>
                <label>
                    Direction:
                    <select value={direction} onChange={directionHandler}>
                        <option value="">Select a direction</option>
                        <option value="asc">Ascending</option>
                        <option value="desc">Descending</option>
                    </select>
                </label>
                <button onClick={orderHandler}>Order</button>
            </div>
            {cars.map((car) => (
                <Card key={car.id} car={car} />
            ))}
        </div>
    );
};