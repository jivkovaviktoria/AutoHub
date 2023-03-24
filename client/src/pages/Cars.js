    import { useEffect, useState } from "react";
    import * as carsService from "../services/CarsService";
    import styles from "./Cars.module.css";
    import { Card } from "../components/card/Card";
    import {Info} from "./Info";

    export const Cars = () => {
        const [cars, setCars] = useState([]);
        const [selectedCar, setSelectedCar] = useState(null);
        const [property, setProperty] = useState(null);
        const [direction, setDirection] = useState("asc");

        useEffect(() => {
            const token = sessionStorage.getItem('token');
            async function fetchData() {
                const result = await carsService.GetAll();
                setCars(result.$values);
            }
            fetchData();
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

        const selectCarHandler = (carId) => {
            carsService.GetSingle(carId)
                .then(car => setSelectedCar(car));
        }

        const closeInfoHandler = () => {
            setSelectedCar(null);
        }
        return (
            <>
            {sessionStorage.getItem('token') ? (
            <div className={styles.wrapper}>
                <div className={styles["order-form"]}>
                    <label>
                        OrderBy
                        <select className={styles.select} value={property} onChange={propertyHandler}>
                            <option value="">Select a property</option>
                            <option value="brand">Make</option>
                            <option value="model">Model</option>
                            <option value="year">Year</option>
                            <option value="price">Price</option>
                        </select>
                    </label>
                    <label>
                        Direction:
                        <select className={styles.select} value={direction} onChange={directionHandler}>
                            <option value="asc">Select a direction</option>
                            <option value="asc">Ascending</option>
                            <option value="desc">Descending</option>
                        </select>
                    </label>
                    <button onClick={orderHandler}>Order</button>
                </div>
                <div className={styles["cars-wrapper"]}>
                    {cars.length > 0 ? cars.map((car) => (
                        <Card key={car.id} car={car} onInfoClick={selectCarHandler} />
                    )) : <div>No cars to display</div>}
                </div>
                {selectedCar && <Info car={selectedCar} onClose={closeInfoHandler}/>}
            </div>
                    ) : (
                    <div>You are not authorized to access this page. Please login or register.</div>
                )}
                </>
        );
    }
